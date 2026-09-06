// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ChildVoiceReport.cs  (Editor only)
//
//  Reads the built assets — not the builders — and says, for every moment the
//  game measures the player's patience, whether the sign that it is one of them
//  is actually in front of it.
//
//  It exists because the two can disagree. A builder can be rewritten and the
//  .asset file on disk can stay exactly as it was, and then a whole playthrough
//  happens with none of the new writing in it and nothing anywhere says so.
//  This looks at what will really be played.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Where the game waits, and whether the player is told.</summary>
    public static class ChildVoiceReport
    {
        [MenuItem("The Frayed Red String/Where Does The Game Wait?", priority = 40)]
        public static void Report()
        {
            List<ActAsset> acts = LoadAll();

            StringBuilder log = new StringBuilder();
            log.AppendLine("THE FRAYED RED STRING — the waiting moments, as they are on disk");
            log.AppendLine();

            int episodes = 0;
            int lines = 0;
            int unsigned = 0;

            for (int a = 0; a < acts.Count; a++)
            {
                ActAsset act = acts[a];
                List<BeatData> beats = act.Beats;

                if (beats == null || beats.Count == 0)
                {
                    continue;
                }

                bool headed = false;

                int i = 0;

                while (i < beats.Count)
                {
                    if (!Measured(beats, i))
                    {
                        i++;
                        continue;
                    }

                    // One episode: everything from here to the first line that
                    // stops measuring. Beats that are not lines at all — a hold,
                    // a sound — sit inside it and do not end it.
                    int first = i;
                    int last = i;
                    int spoken = 0;
                    int plated = 0;

                    while (i < beats.Count)
                    {
                        BeatData beat = beats[i];

                        if (beat != null && beat.Kind == StoryBeatKind.Line)
                        {
                            if (!beat.MeasurePatience)
                            {
                                break;
                            }

                            last = i;
                            spoken++;

                            if (beat.Speaker == Speaker.YuaChild || beat.Speaker == Speaker.HaruChild)
                            {
                                plated++;
                            }
                        }

                        i++;
                    }

                    episodes++;
                    lines += spoken;

                    if (!headed)
                    {
                        log.AppendLine($"  {act.name}");
                        headed = true;
                    }

                    if (plated > 0)
                    {
                        log.AppendLine(
                            $"     beats {first,4}–{last,-4} {spoken,3} line(s), {plated} of them under a child name plate");
                    }
                    else
                    {
                        unsigned++;
                        log.AppendLine(
                            $"     beats {first,4}–{last,-4} {spoken,3} line(s), NO CHILD NAME PLATE ANYWHERE IN IT");
                    }
                }
            }

            log.AppendLine();

            if (episodes == 0)
            {
                log.AppendLine("  No waiting moments at all. Either no act has been built, or the beats that");
                log.AppendLine("  measure patience have lost their tick. Run Rebuild Every Act From The Story");
                log.AppendLine("  Document.");
            }
            else
            {
                log.AppendLine(
                    $"  {episodes} episode(s), {lines} line(s) in total. Stopping on any one of those lines for " +
                    "the full wait offers the ending, if the run has earned one.");

                if (unsigned > 0)
                {
                    log.AppendLine(
                        $"  {unsigned} of them carry no child name plate, so the player has no way of knowing " +
                        "they are episodes.");
                }
            }

            Debug.Log(log.ToString());
        }

        /// <summary>True when the beat is a line that measures patience.</summary>
        private static bool Measured(List<BeatData> beats, int index)
        {
            BeatData beat = beats[index];

            return beat != null && beat.Kind == StoryBeatKind.Line && beat.MeasurePatience;
        }

        private static List<ActAsset> LoadAll()
        {
            string[] guids = AssetDatabase.FindAssets("t:ActAsset");
            List<ActAsset> acts = new List<ActAsset>(guids.Length);

            for (int i = 0; i < guids.Length; i++)
            {
                ActAsset act = AssetDatabase.LoadAssetAtPath<ActAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));

                if (act != null)
                {
                    acts.Add(act);
                }
            }

            acts.Sort((x, y) => string.CompareOrdinal(x.name, y.name));

            return acts;
        }
    }
}
