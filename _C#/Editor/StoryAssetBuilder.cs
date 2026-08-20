// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryAssetBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Keeps the act library and the stage settings in step with the project.
    /// </summary>
    /// <remarks>
    /// Acts are ordinary assets that can live anywhere, so the runtime cannot
    /// find them by folder. This scans for them and records the result where
    /// <see cref="ActLibrary"/> can load it — the same arrangement the music and
    /// sprite libraries use, for the same reason.
    /// </remarks>
    [InitializeOnLoad]
    public static class StoryAssetBuilder
    {
        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string ActsFolder = "Assets/Story/Acts";

        private const string LibraryPath = LibraryFolder + "/" + ActLibrary.ResourceName + ".asset";
        private const string SettingsPath = LibraryFolder + "/" + StageSettings.ResourceName + ".asset";

        static StoryAssetBuilder()
        {
            EditorApplication.delayCall += Rebuild;
        }

        /// <summary>The folder new acts are created in.</summary>
        public static string ActFolder => ActsFolder;

        /// <summary>Creates or refreshes the act library and the stage settings.</summary>
        [MenuItem("The Frayed Red String/Rebuild Act Library")]
        public static void Rebuild()
        {
            EnsureStageSettings();

            List<ActAsset> acts = FindActs();

            ActLibrary library = AssetDatabase.LoadAssetAtPath<ActLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                AssetPaths.EnsureFolder(ResourcesFolder);
                AssetPaths.EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<ActLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }
            else if (IsUpToDate(library, acts))
            {
                return;
            }

            library.SetActs(acts.ToArray());

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Story] {(created ? "Generated" : "Refreshed")} the act library with {acts.Count} act(s).");
        }

        /// <summary>The stage settings asset, created on first use.</summary>
        public static StageSettings EnsureStageSettings()
        {
            StageSettings settings = AssetDatabase.LoadAssetAtPath<StageSettings>(SettingsPath);

            if (settings != null)
            {
                MigratePlacements(settings);
                MigrateFrame(settings);
                return settings;
            }

            AssetPaths.EnsureFolder(ResourcesFolder);
            AssetPaths.EnsureFolder(LibraryFolder);

            settings = ScriptableObject.CreateInstance<StageSettings>();

            // Stamped as it is created, so a brand-new file is not immediately
            // reported as a stale one on the next reload.
            settings.ResetPlacementsToDefault();

            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Story] Created stage settings at {SettingsPath}.");
            return settings;
        }

        /// <summary>
        /// Every act in the project, numbered ones first and in order.
        /// </summary>
        /// <remarks>
        /// Interludes have no act number and sort to the end. They are still
        /// listed so the editor can offer them, and so an act that references one
        /// keeps working if it is moved.
        /// </remarks>
        public static List<ActAsset> FindActs()
        {
            List<ActAsset> acts = new List<ActAsset>();
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(ActAsset));

            for (int i = 0; i < guids.Length; i++)
            {
                ActAsset act = AssetDatabase.LoadAssetAtPath<ActAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));

                if (act != null)
                {
                    acts.Add(act);
                }
            }

            acts.Sort((a, b) =>
            {
                bool aNumbered = a.ActNumber > 0;
                bool bNumbered = b.ActNumber > 0;

                if (aNumbered != bNumbered)
                {
                    return aNumbered ? -1 : 1;
                }

                int byNumber = a.ActNumber.CompareTo(b.ActNumber);
                return byNumber != 0 ? byNumber : string.CompareOrdinal(a.name, b.name);
            });

            return acts;
        }

        /// <summary>Creates a new, empty act asset and returns it.</summary>
        public static ActAsset CreateAct(int actNumber, string assetName)
        {
            AssetPaths.EnsureFolder("Assets/Story");
            AssetPaths.EnsureFolder(ActsFolder);

            ActAsset act = ScriptableObject.CreateInstance<ActAsset>();
            act.ActNumber = actNumber;
            act.ShowTitleCard = actNumber > 0;

            string path = AssetDatabase.GenerateUniqueAssetPath($"{ActsFolder}/{assetName}.asset");
            AssetDatabase.CreateAsset(act, path);
            AssetDatabase.SaveAssets();

            Rebuild();
            return act;
        }

        /// <summary>
        /// Puts the character placement back on the anchors when the asset was
        /// written before those anchors carried a size.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The awkward half of keeping a design number in an asset: fixing the
        /// built-in default fixes a fresh checkout and reaches nobody who
        /// already has the file. This project's file holds a placement adopted
        /// back when both characters shared one canvas height, and every act
        /// that reads it draws them about a sixth too large — which is exactly
        /// the bug the new numbers are for.
        /// </para>
        /// <para>
        /// So the asset is stamped with the revision of the numbers it was
        /// written against, and an older stamp is rewritten once, here, with a
        /// line in the console saying so. Overwriting a deliberate placement
        /// silently would be the wrong thing; overwriting one that predates the
        /// idea of a per-character size is not a placement anybody chose.
        /// Anything set after this — by hand, or by taking it from a scene —
        /// carries the current stamp and is left alone.
        /// </para>
        /// </remarks>
        private static void MigratePlacements(StageSettings settings)
        {
            if (!settings.PlacementsAreStale)
            {
                return;
            }

            settings.ResetPlacementsToDefault();

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            Debug.Log(
                "[Stage] The stage settings held a character placement from before the two characters had " +
                $"sizes of their own, so both were being drawn too large. They are back on the anchors: Yua " +
                $"x {StageSettings.Placement.YuaAnchorX} at scale {StageSettings.Placement.YuaScale}, Haru " +
                $"x {StageSettings.Placement.HaruAnchorX} at scale {StageSettings.Placement.HaruScale}, both " +
                $"y {StageSettings.Placement.AnchorY}. The Story Editor's Stage tab shows what that looks like.");
        }

        /// <summary>
        /// Gives a stage settings asset a frame, if it was saved before the
        /// frame had a thickness.
        /// </summary>
        /// <remarks>
        /// Unity gives a field that is not in the file its initializer's value,
        /// so an asset written before these two existed already loads with the
        /// right numbers and nothing has to happen. The check is for the other
        /// case: an asset where somebody dragged both sliders to zero, or one
        /// written by a version that had a zero default, which produces a frame
        /// that is enabled and invisible — the kind of thing that gets debugged
        /// for an hour.
        /// </remarks>
        private static void MigrateFrame(StageSettings settings)
        {
            if (!settings.FrameEnabled || settings.FrameBorderX > 0f || settings.FrameBorderY > 0f)
            {
                return;
            }

            settings.FrameBorderX = 149f;
            settings.FrameBorderY = 69f;

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            Debug.Log(
                "[Story] The frame was switched on with no thickness on either axis, so it was drawing " +
                "nothing. It has been set to 149 × 69 canvas units. Both numbers are on the Story " +
                "Editor's Stage tab.");
        }

        private static bool IsUpToDate(ActLibrary library, List<ActAsset> acts)
        {
            ActAsset[] stored = library.Acts;

            if (stored == null || stored.Length != acts.Count)
            {
                return false;
            }

            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i] != acts[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
