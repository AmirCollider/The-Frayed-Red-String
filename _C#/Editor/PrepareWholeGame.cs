// -----------------------------------------------------------------------------
//  The Frayed Red String
//  PrepareWholeGame.cs  (Editor only)
//
//  One menu item that takes a fresh checkout to a game you can press Play on.
//
//  Everything it does is already available separately, and that is the problem
//  it solves: there are eleven menu items, they have to be run in an order
//  nobody has written down, and missing one of them produces a game that starts
//  and is quietly wrong. This runs all of them, in the right order, and then
//  says what is left.
//
//  It is safe to run at any time and safe to run twice. The only destructive
//  thing it can do — replacing act scripts that already have beats in them — it
//  asks about once, up front, and defaults to leaving them alone.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Builds and repairs the entire game in one press.</summary>
    public static class PrepareWholeGame
    {
        /// <summary>Acts that have a builder.</summary>
        private static readonly int[] ScriptedActs = { 1, 2, 3, 4, 5, 6, 7 };

        [MenuItem("The Frayed Red String/Prepare The Whole Game", priority = -200)]
        public static void Prepare()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            ActScriptWriter.RebuildPolicy policy = AskAboutExistingScripts();

            if (policy == ActScriptWriter.RebuildPolicy.Ask)
            {
                // The dialog's third button.
                return;
            }

            StringBuilder log = new StringBuilder();
            log.AppendLine("THE FRAYED RED STRING — preparing the whole game");
            log.AppendLine();

            try
            {
                Step(log, 1, "Folders", EnsureFolders);
                Step(log, 2, "Art, sound and film libraries", RebuildLibraries);
                Step(log, 3, "Character placement", RepairPlacement);
                Step(log, 4, "Act scenes", RepairScenes);
                Step(log, 5, "Act scripts", () => BuildScripts(policy));
                Step(log, 6, "Act library", StoryAssetBuilder.Rebuild);
                Step(log, 7, "Build Settings", BuildSettingsAutoConfigurator.EnsureScenesRegistered);
                Step(log, 8, "Player Settings", () => RepairPlayerSettings(log));
                Step(log, 9, "Video module", VideoModuleGuard.Sync);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            log.AppendLine();
            log.AppendLine("Done. Press Play, or run The Frayed Red String ▸ Is The Game Ready? for the");
            log.AppendLine("list of files that are still missing — none of which stop the game running.");

            Debug.Log(log.ToString());

            GameReadinessReport.Report();
        }

        // ---------------------------------------------------------------------

        /// <summary>
        /// The one destructive decision, put once.
        /// </summary>
        /// <returns>
        /// The policy chosen, or <see cref="ActScriptWriter.RebuildPolicy.Ask"/>
        /// to mean the whole thing was cancelled.
        /// </returns>
        private static ActScriptWriter.RebuildPolicy AskAboutExistingScripts()
        {
            int edited = 0;

            for (int i = 0; i < ScriptedActs.Length; i++)
            {
                ActAsset act = ActLibrary.Find(ScriptedActs[i]);

                if (act != null && act.Count > 0 && !ActScriptWriter.IsUntouchedSinceGenerated(act))
                {
                    edited++;
                }
            }

            // Nothing has been touched by hand, so there is no destructive
            // decision to put to anybody: rebuild everything that is merely
            // older than its builder, which is the whole point of the command.
            if (edited == 0)
            {
                return ActScriptWriter.RebuildPolicy.IfUnedited;
            }

            int choice = EditorUtility.DisplayDialogComplex(
                "Prepare the whole game",
                $"{edited} act(s) have been edited since they were last generated.\n\n" +
                "Rewriting replaces every line in them with the version from the design document. " +
                "Acts you have not touched are rebuilt either way, so the game always matches the " +
                "current script.\n\n" +
                "Everything else — libraries, placement, scenes, Build Settings — is repaired either way.",
                "Keep my edits",
                "Cancel",
                "Rewrite every act from the document");

            switch (choice)
            {
                case 0: return ActScriptWriter.RebuildPolicy.IfUnedited;
                case 2: return ActScriptWriter.RebuildPolicy.Always;
                default: return ActScriptWriter.RebuildPolicy.Ask;
            }
        }

        /// <summary>
        /// Rewrites every code-written act from its builder, with no questions.
        /// </summary>
        /// <remarks>
        /// The remedy for the state this whole mechanism exists to prevent:
        /// assets on disk that were built from an older version of the script,
        /// so changes made to the builders never reach the game.
        /// </remarks>
        [MenuItem("The Frayed Red String/Rebuild Every Act From The Story Document", priority = -199)]
        public static void RebuildEveryAct()
        {
            if (!EditorUtility.DisplayDialog(
                    "Rebuild every act",
                    "Every act, and both endings, is replaced with the script from the design document. " +
                    "Anything edited in the Story Editor since the last build is lost.",
                    "Rewrite them",
                    "Cancel"))
            {
                return;
            }

            BuildScripts(ActScriptWriter.RebuildPolicy.Always);
            StoryAssetBuilder.Rebuild();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[Story] Every act was rebuilt from the story document.");

            ChildVoiceReport.Report();
        }

        private static void Step(StringBuilder log, int number, string title, System.Action work)
        {
            EditorUtility.DisplayProgressBar("Preparing the whole game", title, number / 9f);

            work();

            log.AppendLine($"  {number}. {title} — done");
        }

        // ---------------------------------------------------------------------

        private static void EnsureFolders()
        {
            AssetPaths.EnsureFolder("Assets/Story");
            AssetPaths.EnsureFolder("Assets/Story/Acts");
            AssetPaths.EnsureFolder("Assets/Resources");
            AssetPaths.EnsureFolder("Assets/Resources/TFRS");
            AssetPaths.EnsureFolder("Assets/Audio");
            AssetPaths.EnsureFolder("Assets/Audio/Voice");
            AssetPaths.EnsureFolder("Assets/Video");
            AssetPaths.EnsureFolder("Assets/StreamingAssets");
            AssetPaths.EnsureFolder("Assets/StreamingAssets/Video");
        }

        private static void RebuildLibraries()
        {
            UiSpriteLibraryBuilder.EnsureLibrary();
            StageSpriteLibraryBuilder.Rebuild();
            AudioLibraryBuilder.Rebuild();
            VoiceLibraryBuilder.Rebuild();
            VideoLibraryBuilder.Rebuild();
        }

        /// <summary>
        /// Puts the placement back on the design numbers and stamps the asset.
        /// </summary>
        /// <remarks>
        /// Unconditional, unlike the migration in StoryAssetBuilder, because
        /// somebody who pressed "prepare the whole game" is asking for exactly
        /// this. Yua at x −4.55 scale 0.48, Haru at x +4.70 scale 0.50, both at
        /// y −1.50.
        /// </remarks>
        private static void RepairPlacement()
        {
            StageSettings settings = StoryAssetBuilder.EnsureStageSettings();

            if (settings == null)
            {
                return;
            }

            settings.ResetPlacementsToDefault();

            // The single most reliable way this game breaks. See the field's
            // own remarks.
            settings.ReadPlacementFromScene = false;

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Makes sure every act scene exists, has what it needs, and has no
        /// character markers left in it that could move the cast.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Opens each act scene in turn, which is heavy and is the only way to
        /// do it: a scene that is not open is a YAML file, and editing one of
        /// those by hand is how you corrupt a project.
        /// </para>
        /// <para>
        /// Markers are moved onto the anchors and switched off rather than
        /// deleted. They are somebody's reference for what the placement looks
        /// like in the scene view, they cost nothing switched off, and deleting
        /// another person's objects is not what a repair command should do.
        /// </para>
        /// </remarks>
        private static void RepairScenes()
        {
            string openScene = EditorSceneManager.GetActiveScene().path;
            int repaired = 0;

            for (int number = 1; number <= 7; number++)
            {
                string sceneName = SceneNames.ForAct(number);
                string path = AssetPaths.FindPathByExactName("t:Scene " + sceneName, sceneName);

                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogWarning(
                        $"[Prepare] There is no scene called '{sceneName}'. Create it in Assets/Scenes and " +
                        "run The Frayed Red String ▸ Prepare This Scene As An Act, or run this again " +
                        "afterwards.");

                    continue;
                }

                UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                ActSceneSetup.PrepareScene();

                if (TidyMarkers())
                {
                    repaired++;
                }

                EditorSceneManager.SaveScene(scene);
            }

            if (!string.IsNullOrEmpty(openScene))
            {
                EditorSceneManager.OpenScene(openScene, OpenSceneMode.Single);
            }

            if (repaired > 0)
            {
                Debug.Log(
                    $"[Prepare] Character markers in {repaired} scene(s) were moved onto the anchors and " +
                    "switched off. They are not read at runtime any more — the Stage tab's numbers are.");
            }
        }

        /// <summary>
        /// Moves any character marker in the open scene onto its anchor and
        /// switches it off.
        /// </summary>
        /// <returns>True when this scene had one.</returns>
        private static bool TidyMarkers()
        {
            bool found = false;

            foreach (Speaker speaker in new[] { Speaker.Yua, Speaker.Haru })
            {
                SpriteRenderer marker = VisualNovelStage.FindAuthoredAnchor(speaker);

                if (marker == null)
                {
                    continue;
                }

                found = true;

                Transform pose = marker.transform;

                pose.position = new Vector3(
                    StageSettings.Placement.AnchorXFor(speaker),
                    StageSettings.Placement.AnchorY,
                    pose.position.z);

                pose.localScale = Vector3.one * StageSettings.Placement.ScaleFor(speaker);

                marker.gameObject.SetActive(false);
            }

            return found;
        }

        private static void BuildScripts(ActScriptWriter.RebuildPolicy policy)
        {
            Act01Builder.Build(policy);
            Act02Builder.Build(policy);
            Act03Builder.Build(policy);
            Act04Builder.Build(policy);
            Act05Builder.Build(policy);
            Act06Builder.Build(policy);
            Act07Builder.Build(policy);

            EndingsBuilder.BuildBoth(policy);
        }

        /// <summary>
        /// The handful of project settings a build needs to be right.
        /// </summary>
        /// <remarks>
        /// Deliberately few. Everything here is either empty-or-default (so
        /// setting it takes nothing away) or is required for the game to look
        /// like itself, and each one is reported rather than done silently.
        /// </remarks>
        private static void RepairPlayerSettings(StringBuilder log)
        {
            const string productName = "The Frayed Red String";

            if (PlayerSettings.productName != productName)
            {
                log.AppendLine($"     product name set to \"{productName}\" (was \"{PlayerSettings.productName}\")");
                PlayerSettings.productName = productName;
            }

            if (PlayerSettings.defaultScreenWidth != 1920 || PlayerSettings.defaultScreenHeight != 1080)
            {
                log.AppendLine("     default resolution set to 1920 × 1080, which every background is drawn at");
                PlayerSettings.defaultScreenWidth = 1920;
                PlayerSettings.defaultScreenHeight = 1080;
            }

            // A visual novel is read, so it must survive being resized and must
            // not be pinned to one aspect.
            if (!PlayerSettings.resizableWindow)
            {
                log.AppendLine("     the window is resizable now");
                PlayerSettings.resizableWindow = true;
            }

            // The story runs on unscaled time and keeps breathing behind a
            // menu; a build that pauses when it loses focus stops mid-line.
            if (!PlayerSettings.runInBackground)
            {
                log.AppendLine("     the game keeps running when the window loses focus");
                PlayerSettings.runInBackground = true;
            }
        }
    }
}