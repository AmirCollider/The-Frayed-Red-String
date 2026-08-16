// -----------------------------------------------------------------------------
//  The Frayed Red String
//  UiSpriteLibraryBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.IO;
using TheFrayedRedString.UI;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Generates and maintains the runtime sprite library.
    /// </summary>
    /// <remarks>
    /// The language button holds one flag at a time, so the other flag is not
    /// reachable from anything in the loaded scene. Rather than ask for two
    /// Inspector references to be dragged in by hand — the sort of wiring that
    /// silently breaks when an asset moves — this locates the flags by name and
    /// bakes them into a Resources asset the runtime can load.
    /// </remarks>
    [InitializeOnLoad]
    public static class UiSpriteLibraryBuilder
    {
        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string LibraryPath = LibraryFolder + "/" + UiSpriteLibrary.ResourceName + ".asset";

        private const string EnglishFlagAsset = "LanguageFlagEnglish";
        private const string JapaneseFlagAsset = "LanguageFlagJapanese";

        static UiSpriteLibraryBuilder()
        {
            EditorApplication.delayCall += EnsureLibrary;
        }

        /// <summary>Creates the library if missing and fills in any empty flag slots.</summary>
        [MenuItem("The Frayed Red String/Rebuild UI Sprite Library")]
        public static void EnsureLibrary()
        {
            UiSpriteLibrary library = AssetDatabase.LoadAssetAtPath<UiSpriteLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                EnsureFolder(ResourcesFolder);
                EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<UiSpriteLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }

            // Only fill gaps. If someone has deliberately assigned different
            // artwork, regenerating must not overwrite that choice.
            if (library.HasFlags && !created)
            {
                return;
            }

            Sprite english = library.EnglishFlag != null ? library.EnglishFlag : FindSprite(EnglishFlagAsset);
            Sprite japanese = library.JapaneseFlag != null ? library.JapaneseFlag : FindSprite(JapaneseFlagAsset);

            library.SetFlags(english, japanese);

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            if (created)
            {
                Debug.Log($"[UI] Generated the sprite library at {LibraryPath}.");
            }

            if (english == null || japanese == null)
            {
                Debug.LogWarning(
                    "[UI] One or both language flags could not be found as Sprites. " +
                    $"Check that '{EnglishFlagAsset}' and '{JapaneseFlagAsset}' exist under Assets/Images/UI " +
                    "and that their Texture Type is set to 'Sprite (2D and UI)'.");
            }
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string leaf = Path.GetFileName(path);

            if (!string.IsNullOrEmpty(parent) && !string.IsNullOrEmpty(leaf))
            {
                AssetDatabase.CreateFolder(parent, leaf);
            }
        }

        private static Sprite FindSprite(string assetName)
        {
            string[] guids = AssetDatabase.FindAssets($"t:Sprite {assetName}");

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                if (Path.GetFileNameWithoutExtension(path) != assetName)
                {
                    continue;
                }

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprite != null)
                {
                    return sprite;
                }
            }

            return null;
        }
    }
}
