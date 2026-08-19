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
