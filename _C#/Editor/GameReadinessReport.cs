// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameReadinessReport.cs  (Editor only)
//
//  One question, answered in one place: what is left to do.
//
//  Every subsystem in this project is built to survive its assets not existing —
//  a missing background becomes a generated stand-in, a missing recording plays
//  as the typewriter, a missing film is skipped — which is what makes the game
//  playable end to end today. It is also what makes it impossible to tell, by
//  playing it, how much of it is finished.
//
//  So this prints the list.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>What is written, what is missing, and what still plays anyway.</summary>
    public static class GameReadinessReport
    {
        [MenuItem("The Frayed Red String/Is The Game Ready?", priority = -100)]
        public static void Report()
        {
            StringBuilder report = new StringBuilder();

            report.AppendLine("THE FRAYED RED STRING — what is finished and what is not");
            report.AppendLine();

            ReportActs(report);
            ReportEndings(report);
            ReportArt(report);
            ReportAudio(report);
            ReportVideo(report);
            ReportScenes(report);

            report.AppendLine();
            report.AppendLine(
                "Anything marked MISSING is a file, not a bug. The game plays from the first frame to " +
                "the last without any of them — with stand-in pictures and the typewriter voice — so " +
                "everything below can be dropped in whenever it exists, in any order.");

            Debug.Log(report.ToString());
        }

        // ---------------------------------------------------------------------

        private static void ReportActs(StringBuilder report)
        {
            report.AppendLine("ACTS");

            for (int number = 1; number <= 7; number++)
            {
                ActAsset act = ActLibrary.Find(number);

                if (act == null || act.Count == 0)
                {
                    report.AppendLine(
                        $"  Act {number:00}  MISSING — build it from The Frayed Red String ▸ Build Act " +
                        $"{number:00} From The Story Document");

                    continue;
                }

                int silences = 0;
                int voiced = 0;
                int childPlates = 0;

                for (int i = 0; i < act.Count; i++)
                {
                    BeatData beat = act.At(i);

                    if (beat == null || beat.Kind != StoryBeatKind.Line)
                    {
                        continue;
                    }

                    if (beat.MeasurePatience)
                    {
                        silences++;
                    }

                    if (!string.IsNullOrWhiteSpace(beat.VoiceClip))
                    {
                        voiced++;
                    }

                    if (beat.Speaker == Speaker.YuaChild || beat.Speaker == Speaker.HaruChild)
                    {
                        childPlates++;
                    }
                }

                report.AppendLine(
                    $"  Act {number:00}  {act.Count,4} beats, {act.SpokenCount(),3} spoken" +
                    $"{(silences > 0 ? $", {silences} silence(s)" : string.Empty)}" +
                    $"{(voiced > 0 ? $", {voiced} voiced line(s)" : string.Empty)}" +
                    $"{(childPlates > 0 ? $", {childPlates} child name plate(s)" : string.Empty)}" +
                    $"{(silences > 0 && childPlates == 0 ? "  — SILENCES WITH NO SIGN: rebuild this act" : string.Empty)}");
            }

            report.AppendLine();
        }

        private static void ReportEndings(StringBuilder report)
        {
            report.AppendLine("ENDINGS");
            report.AppendLine("  Bitter — act seven. No condition. This is what most players will finish on.");

            ReportEnding(report, Endings.GoodAssetName, "Secret — every counted press blue, never green");
            ReportEnding(report, Endings.NormalAssetName, "Open   — every counted press green, never blue");

            report.AppendLine("  Mixed runs earn neither, and waiting buys nothing. That is deliberate.");

            int total = 0;

            for (int number = 1; number <= 7; number++)
            {
                ActAsset act = ActLibrary.Find(number);

                if (act == null)
                {
                    continue;
                }

                for (int i = 0; i < act.Count; i++)
                {
                    BeatData beat = act.At(i);

                    if (beat != null && beat.Kind == StoryBeatKind.Line && beat.MeasurePatience)
                    {
                        total++;
                    }
                }
            }

            report.AppendLine(
                total == 0
                    ? "  NO SILENCES ANYWHERE — neither ending can be reached. Mark some lines as " +
                      "'One of the silences' in the Story Editor."
                    : $"  {total} silence(s) across the game, each one a place an ending can be redeemed.");

            report.AppendLine(
                StoryTesting.IsShortened
                    ? $"  NOTE: the five minutes are shortened to {GameConfig.PatienceSeconds:0.#}s for " +
                      "this Editor session."
                    : "  The wait is the full five minutes. Endings ▸ Shorten The Five Minutes to test.");

            report.AppendLine();
        }

        private static void ReportEnding(StringBuilder report, string assetName, string condition)
        {
            ActAsset act = ActLibrary.FindByName(assetName);

            report.AppendLine(
                act != null && act.Count > 0
                    ? $"  {condition} — {assetName}, {act.Count} beats"
                    : $"  {condition} — {assetName} MISSING, build it from Endings ▸ Build The Two Endings");
        }

        private static void ReportArt(StringBuilder report)
        {
            report.AppendLine("PICTURES");

            StageSpriteLibrary library = StageSpriteLibrary.Load();

            List<string> missingBackgrounds = new List<string>();

            foreach (string name in ExpectedBackgrounds())
            {
                if (library == null || library.FindBackground(name) == null)
                {
                    missingBackgrounds.Add(name);
                }
            }

            report.AppendLine(
                missingBackgrounds.Count == 0
                    ? "  Every background the script asks for exists."
                    : $"  {missingBackgrounds.Count} background(s) MISSING — a stand-in is generated from " +
                      "the name until each arrives:");

            for (int i = 0; i < missingBackgrounds.Count; i++)
            {
                report.AppendLine($"      {missingBackgrounds[i]}");
            }

            // Counted by the name each pose resolves to rather than by every
            // speaker-and-expression pair there is, because plenty of those
            // pairs are the same drawing: Yua has no wince of her own and Haru
            // has no picture of an empty-handed finished lunch, and both fall
            // through to a neutral face that has already been counted once. The
            // total was written into the sentence as "32" and had been wrong
            // since the day the wince was added.
            HashSet<string> expectedPoses = new HashSet<string>();
            int missingPoses = 0;

            foreach (Speaker speaker in new[] { Speaker.Yua, Speaker.Haru, Speaker.YuaChild, Speaker.HaruChild })
            {
                foreach (Portrait portrait in System.Enum.GetValues(typeof(Portrait)))
                {
                    if (portrait == Portrait.Unchanged)
                    {
                        continue;
                    }

                    string pose = CharacterArt.SpriteName(speaker, portrait);

                    if (pose == null || !expectedPoses.Add(pose))
                    {
                        continue;
                    }

                    if (library == null || library.FindCharacter(pose) == null)
                    {
                        missingPoses++;
                    }
                }
            }

            report.AppendLine(
                missingPoses == 0
                    ? $"  All {expectedPoses.Count} character poses exist."
                    : $"  {missingPoses} of {expectedPoses.Count} character pose(s) MISSING — a silhouette " +
                      "stands in. The children are YuaChild… and HaruChild…, spelled exactly like their " +
                      "older selves.");

            report.AppendLine();
        }

        private static void ReportAudio(StringBuilder report)
        {
            report.AppendLine("SOUND");
            report.AppendLine("  Every sound effect is synthesised. Nothing to supply.");

            MusicLibrary music = MusicLibrary.Load();
            List<string> missingTracks = new List<string>();

            if (music == null || music.Find(MusicTracks.MainMenu) == null)
            {
                missingTracks.Add(MusicTracks.MainMenu);
            }

            for (int number = 1; number <= 7; number++)
            {
                string track = MusicTracks.ForAct(number);

                if (music == null || music.Find(track) == null)
                {
                    missingTracks.Add(track);
                }
            }

            report.AppendLine(
                missingTracks.Count == 0
                    ? "  Every music track exists."
                    : $"  {missingTracks.Count} music track(s) MISSING from Assets/Audio/BackgroundMusics " +
                      "— those acts play silent:");

            for (int i = 0; i < missingTracks.Count; i++)
            {
                report.AppendLine($"      {missingTracks[i]}");
            }

            VoiceLibrary voice = VoiceLibrary.Load();
            int namedLines = 0;
            int haveRecording = 0;

            for (int number = 1; number <= 7; number++)
            {
                ActAsset act = ActLibrary.Find(number);

                if (act == null)
                {
                    continue;
                }

                for (int i = 0; i < act.Count; i++)
                {
                    BeatData beat = act.At(i);

                    if (beat == null || string.IsNullOrWhiteSpace(beat.VoiceClip))
                    {
                        continue;
                    }

                    namedLines++;

                    if (voice != null && voice.Find(beat.VoiceClip) != null)
                    {
                        haveRecording++;
                    }
                }
            }

            report.AppendLine(
                namedLines == 0
                    ? "  No line names a recording."
                    : $"  {haveRecording} of {namedLines} voiced line(s) have a file in Assets/Audio/Voice. " +
                      "The rest are read by the typewriter.");

            report.AppendLine();
        }

        private static void ReportVideo(StringBuilder report)
        {
            report.AppendLine("VIDEO");

#if TFRS_VIDEO
            report.AppendLine("  Unity's Video module is enabled.");
#else
            report.AppendLine(
                "  Unity's Video module is NOT in this project, so act seven plays without its credits " +
                "film. Everything else is unaffected and the game still builds. " +
                "The Frayed Red String ▸ Check The Video Module says how to turn it on.");
#endif

            report.AppendLine(
                $"  The credits film is called '{Act07Builder.CreditsFilm}'. Put it in " +
                $"Assets/StreamingAssets/{VideoScreenView.StreamingFolder} — a file there stays a file and " +
                "can be recut without reimporting the project.");

            report.AppendLine();
        }

        private static void ReportScenes(StringBuilder report)
        {
            report.AppendLine("SCENES");

            List<string> missing = new List<string>();

            foreach (string sceneName in new[]
                     {
                         SceneNames.Warning, SceneNames.MainMenu,
                         "Act01", "Act02", "Act03", "Act04", "Act05", "Act06", "Act07"
                     })
            {
                if (string.IsNullOrEmpty(AssetPaths.FindPathByExactName("t:Scene " + sceneName, sceneName)))
                {
                    missing.Add(sceneName);
                }
            }

            report.AppendLine(
                missing.Count == 0
                    ? "  All nine scenes exist. Build Settings are repaired automatically on import."
                    : $"  MISSING scene(s): {string.Join(", ", missing)}");
        }

        /// <summary>Every background name the constants offer.</summary>
        private static IEnumerable<string> ExpectedBackgrounds()
        {
            foreach (System.Reflection.FieldInfo field in typeof(Backgrounds).GetFields(
                         System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.IsLiteral && field.FieldType == typeof(string))
                {
                    yield return (string)field.GetRawConstantValue();
                }
            }
        }
    }
}
