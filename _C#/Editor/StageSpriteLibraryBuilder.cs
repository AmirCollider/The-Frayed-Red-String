// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StageSpriteLibraryBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Presentation;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Bakes the background and character art into a Resources asset the story
    /// can load by name.
    /// </summary>
    /// <remarks>
    /// Scans the art folders rather than taking a hand-maintained list, so a new
    /// background is usable from a script the moment the file lands in
    /// <c>Assets/Images/Backgrounds</c>. Textures that were not imported as
    /// sprites are converted on the way through — see
    /// <see cref="AssetPaths.CollectSprites"/>.
    /// </remarks>
    [InitializeOnLoad]
    public static class StageSpriteLibraryBuilder
    {
        private const string BackgroundFolder = "Assets/Images/Backgrounds";
        private const string CharacterFolder = "Assets/Images/Characters";

        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string LibraryPath = LibraryFolder + "/" + StageSpriteLibrary.ResourceName + ".asset";

        static StageSpriteLibraryBuilder()
        {
            EditorApplication.delayCall += Rebuild;
        }

        /// <summary>Creates or refreshes the stage sprite library.</summary>
        [MenuItem("The Frayed Red String/Rebuild Stage Sprite Library")]
        public static void Rebuild()
        {
            if (!AssetDatabase.IsValidFolder(BackgroundFolder) && !AssetDatabase.IsValidFolder(CharacterFolder))
            {
                // Not an error: a project with no art folders simply has no art.
                return;
            }

            StageSpriteLibrary.Entry[] backgrounds = Collect(BackgroundFolder);
            StageSpriteLibrary.Entry[] characters = Collect(CharacterFolder);

            StageSpriteLibrary library = AssetDatabase.LoadAssetAtPath<StageSpriteLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                AssetPaths.EnsureFolder(ResourcesFolder);
                AssetPaths.EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<StageSpriteLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }
            else if (IsUpToDate(library, backgrounds, characters))
            {
                return;
            }

            library.SetEntries(backgrounds, characters);

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            Debug.Log(
                $"[Stage] {(created ? "Generated" : "Refreshed")} the stage sprite library with " +
                $"{backgrounds.Length} background(s) and {characters.Length} character pose(s).");
        }

        private static StageSpriteLibrary.Entry[] Collect(string folder)
        {
            Dictionary<string, Sprite> sprites = AssetPaths.CollectSprites(folder);

            List<StageSpriteLibrary.Entry> entries = new List<StageSpriteLibrary.Entry>(sprites.Count);

            foreach (KeyValuePair<string, Sprite> pair in sprites)
            {
                entries.Add(new StageSpriteLibrary.Entry
                {
                    Name = pair.Key,
                    Sprite = pair.Value
                });
            }

            // Sorted so the generated asset has a stable order and does not
            // produce a spurious diff every time the asset database happens to
            // enumerate the folder differently.
            entries.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));

            return entries.ToArray();
        }

        /// <summary>
        /// True when the stored tables already match the folders, so the asset is
        /// not rewritten — and version control not dirtied — on every reload.
        /// </summary>
        private static bool IsUpToDate(
            StageSpriteLibrary library,
            StageSpriteLibrary.Entry[] backgrounds,
            StageSpriteLibrary.Entry[] characters)
        {
            return Matches(library.BackgroundEntries, backgrounds)
                   && Matches(library.CharacterEntries, characters);
        }

        private static bool Matches(StageSpriteLibrary.Entry[] stored, StageSpriteLibrary.Entry[] fresh)
        {
            if (stored == null || stored.Length != fresh.Length)
            {
                return false;
            }

            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i].Sprite == null || stored[i].Name != fresh[i].Name)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
