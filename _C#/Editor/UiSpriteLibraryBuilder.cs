// -----------------------------------------------------------------------------
//  The Frayed Red String
//  UiSpriteLibraryBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using TheFrayedRedString.UI;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Generates and maintains the runtime sprite library.
    /// </summary>
    /// <remarks>
    /// The language button holds one flag at a time, so the other flags are not
    /// reachable from anything in the loaded scene; the same is true of the
    /// choice-button and pause-menu art, which the story UI builds from code.
    /// Rather than ask for a dozen Inspector references to be dragged in by
    /// hand — the sort of wiring that silently breaks when an asset moves — this
    /// locates them by name and bakes them into a Resources asset.
    /// </remarks>
    [InitializeOnLoad]
    public static class UiSpriteLibraryBuilder
    {
        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string LibraryPath = LibraryFolder + "/" + UiSpriteLibrary.ResourceName + ".asset";

        private const string EnglishFlagAsset = "LanguageFlagEnglish";
        private const string JapaneseFlagAsset = "LanguageFlagJapanese";
        private const string PersianFlagAsset = "LanguageFlagIran";

        private const string ChoiceBlueAsset = "ChoiceButtonBackgroundBlue";
        private const string ChoiceGreenAsset = "ChoiceButtonBackgroundGreen";
        private const string ChoiceWhiteAsset = "ChoiceButtonBackgroundWhite";

        private const string PauseResumeAsset = "PauseMenuButtonResume";
        private const string PauseSaveAsset = "PauseMenuButtonSave";
        private const string PauseLoadAsset = "PauseMenuButtonLoad";
        private const string PauseMenuAsset = "PauseMenuButtonMenu";
        private const string BackToMenuAsset = "MenuButtonBackToMenu";

        static UiSpriteLibraryBuilder()
        {
            EditorApplication.delayCall += EnsureLibrary;
        }

        /// <summary>Creates the library if missing and fills in any empty slots.</summary>
        [MenuItem("The Frayed Red String/Rebuild UI Sprite Library")]
        public static void EnsureLibrary()
        {
            UiSpriteLibrary library = AssetDatabase.LoadAssetAtPath<UiSpriteLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                AssetPaths.EnsureFolder(ResourcesFolder);
                AssetPaths.EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<UiSpriteLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }

            // Only fill gaps. If someone has deliberately assigned different
            // artwork, regenerating must not overwrite that choice.
            Sprite english = library.EnglishFlag != null ? library.EnglishFlag : AssetPaths.FindSprite(EnglishFlagAsset);
            Sprite japanese = library.JapaneseFlag != null ? library.JapaneseFlag : AssetPaths.FindSprite(JapaneseFlagAsset);
            Sprite persian = library.PersianFlag != null ? library.PersianFlag : AssetPaths.FindSprite(PersianFlagAsset);

            library.SetFlags(english, japanese, persian);

            library.SetChoiceBackgrounds(
                library.ChoiceBlue != null ? library.ChoiceBlue : AssetPaths.FindSprite(ChoiceBlueAsset),
                library.ChoiceGreen != null ? library.ChoiceGreen : AssetPaths.FindSprite(ChoiceGreenAsset),
                library.ChoiceWhite != null ? library.ChoiceWhite : AssetPaths.FindSprite(ChoiceWhiteAsset));

            library.SetPauseButtons(
                library.PauseResume != null ? library.PauseResume : AssetPaths.FindSprite(PauseResumeAsset),
                library.PauseSave != null ? library.PauseSave : AssetPaths.FindSprite(PauseSaveAsset),
                library.PauseLoad != null ? library.PauseLoad : AssetPaths.FindSprite(PauseLoadAsset),
                library.PauseMenu != null ? library.PauseMenu : AssetPaths.FindSprite(PauseMenuAsset),
                library.BackToMenu != null ? library.BackToMenu : AssetPaths.FindSprite(BackToMenuAsset));

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            if (created)
            {
                Debug.Log($"[UI] Generated the sprite library at {LibraryPath}.");
            }

            if (!library.HasFlags)
            {
                Debug.LogWarning(
                    "[UI] One or more language flags could not be found as Sprites. " +
                    $"Check that '{EnglishFlagAsset}', '{JapaneseFlagAsset}' and '{PersianFlagAsset}' " +
                    "exist under Assets/Images/UI.");
            }
        }
    }
}
