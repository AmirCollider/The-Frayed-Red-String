// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryDiagnostics.cs  (Editor only)
//
//  Two questions this project cannot answer by looking at the screen.
//
//  "Why is there no sound when I press play on a beat" and "why is this label
//  drawing boxes" both have the same shape: everything involved is internal to
//  somebody else's package, the failure is silent, and the only evidence is a
//  picture of the wrong thing. These print what is actually there.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using TMPro;
using UnityDirectTMP;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Console reports for the two subsystems that fail quietly.</summary>
    public static class StoryDiagnostics
    {
        /// <summary>
        /// Where the characters actually are, while the game is running.
        /// </summary>
        /// <remarks>
        /// The placement has now been wrong for four different reasons, and from
        /// the player's chair all four looked the same: two people in the wrong
        /// place. This prints the numbers the stage was given and the numbers
        /// the rect ended up with, which separates "the asset is wrong" from
        /// "the asset is right and something is overwriting it".
        /// </remarks>
        [MenuItem("The Frayed Red String/Diagnose Character Placement")]
        public static void DiagnosePlacement()
        {
            StringBuilder report = new StringBuilder();

            StageSettings settings = StageSettings.Load();

            report.AppendLine("[Stage] What the settings hold:");

            foreach (Speaker speaker in new[]
                     {
                         Speaker.Yua, Speaker.Haru, Speaker.YuaChild, Speaker.HaruChild
                     })
            {
                StageSettings.Placement placement = settings.PlacementFor(speaker);

                report.AppendLine(
                    $"  {speaker,-10} x {placement.OffsetX,7:0} feet {placement.FeetY,7:0} " +
                    $"height {placement.Height,6:0}   (design: world x " +
                    $"{StageSettings.Placement.AnchorXFor(speaker):0.00}, y " +
                    $"{StageSettings.Placement.AnchorY:0.00}, scale " +
                    $"{StageSettings.Placement.ScaleFor(speaker):0.000})");
            }

            report.AppendLine(
                settings.PlacementsAreStale
                    ? "  The asset is from an older version and is being ignored in favour of the built-in " +
                      "numbers. Run The Frayed Red String ▸ Prepare The Whole Game."
                    : "  The asset is current.");

            report.AppendLine(
                settings.ReadPlacementFromScene
                    ? "  Read Placement From Scene is ON — a Yua… or Haru… sprite in the scene will " +
                      "override all of the above. This is the usual cause of a broken placement."
                    : "  Read Placement From Scene is off, so scene sprites cannot affect any of this.");

            if (!Application.isPlaying)
            {
                report.AppendLine();
                report.AppendLine(
                    "  Press Play and run this again to see where the characters actually ended up.");

                Debug.Log(report.ToString());
                return;
            }

            VisualNovelStage stage = Object.FindAnyObjectByType<VisualNovelStage>();

            report.AppendLine();
            report.AppendLine(
                stage != null
                    ? stage.DescribeSlots()
                    : "  No stage in this scene — is an act scene running?");

            Debug.Log(report.ToString());
        }

        [MenuItem("The Frayed Red String/Diagnose Sound Preview")]
        public static void DiagnoseSoundPreview()
        {
            StringBuilder report = new StringBuilder();

            report.AppendLine(SoundPreview.Describe());
            report.AppendLine();

            AudioClip clip = TheFrayedRedString.Audio.ProceduralSfxLibrary.Get(
                TheFrayedRedString.Audio.SfxId.Confirm);

            report.AppendLine(clip == null
                ? "  clip   : Confirm could not be synthesised"
                : $"  clip   : Confirm is {clip.length:0.00}s, {clip.channels} channel(s), {clip.frequency} Hz");

            if (clip != null)
            {
                report.AppendLine(SoundPreview.Play(clip)
                    ? "  result : played — if you heard nothing, the Editor is muted or its volume is down"
                    : "  result : the Editor offers no way to play it");
            }

            Debug.Log(report.ToString());
        }

        /// <summary>
        /// What every label in the open scenes is actually drawing, and with
        /// what.
        /// </summary>
        /// <remarks>
        /// The useful column is the last one. A Direct Font hands TextMeshPro a
        /// shaped, reordered string, and comparing that against the text it was
        /// given says immediately whether the shaper ran at all — which is the
        /// difference between "this font has no Persian in it" and "the Persian
        /// never got joined".
        /// </remarks>
        [MenuItem("The Frayed Red String/Diagnose Fonts")]
        public static void DiagnoseFonts()
        {
            List<DirectFont> labels = new List<DirectFont>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (!scene.isLoaded)
                {
                    continue;
                }

                List<GameObject> roots = new List<GameObject>(scene.rootCount);
                scene.GetRootGameObjects(roots);

                for (int r = 0; r < roots.Count; r++)
                {
                    labels.AddRange(roots[r].GetComponentsInChildren<DirectFont>(true));
                }
            }

            if (labels.Count == 0)
            {
                Debug.LogWarning(
                    "[Fonts] No Direct Font components in the open scenes. Every label the game draws its " +
                    "story with is built at runtime, so run this while the game is playing.");
                return;
            }

            StringBuilder report = new StringBuilder();
            report.AppendLine($"[Fonts] {labels.Count} label(s) drawn from font files:");

            for (int i = 0; i < labels.Count; i++)
            {
                DirectFont direct = labels[i];
                TMP_Text label = direct.GetComponent<TMP_Text>();

                string asset = direct.FontAsset != null ? direct.FontAsset.name : "NONE";
                string text = label != null ? Shorten(label.text) : "no label";
                string prepared = Shorten(direct.PreparedText());

                report.AppendLine();
                report.AppendLine($"  {Path(direct.transform)}");
                report.AppendLine($"    script {direct.Script}, asset '{asset}', font '{Name(direct.Font)}'");
                report.AppendLine($"    text     {text}");
                report.AppendLine($"    prepared {prepared}");
            }

            Debug.Log(report.ToString());
        }

        private static string Name(Font font)
        {
            return font != null ? font.name : "NONE";
        }

        private static string Shorten(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "(empty)";
            }

            text = text.Replace("\n", "⏎").Replace("\r", string.Empty);
            return text.Length <= 60 ? text : text.Substring(0, 60) + "…";
        }

        private static string Path(Transform target)
        {
            string path = target.name;
            Transform cursor = target.parent;

            while (cursor != null)
            {
                path = cursor.name + "/" + path;
                cursor = cursor.parent;
            }

            return path;
        }
    }
}
