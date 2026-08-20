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
        /// <summary>
        /// How far back a child line may sit and still count as the warning.
        /// </summary>
        /// <remarks>
        /// A few staging beats — a change of expression, a sound — routinely sit
        /// between the line and the silence it belongs to, and none of them are
        /// visible to the player as anything other than the same moment.
        /// </remarks>
        private const int Reach = 6;

        [MenuItem("The Frayed Red String/Where Does The Game Wait?", priority = 40)]
        public static void Report()
        {
            List<ActAsset> acts = LoadAll();

            StringBuilder log = new StringBuilder();
            log.AppendLine("THE FRAYED RED STRING — the waiting moments, as they are on disk");
            log.AppendLine();

            int total = 0;
            int told = 0;

            for (int a = 0; a < acts.Count; a++)
            {
                ActAsset act = acts[a];
                List<BeatData> beats = act.Beats;

                if (beats == null || beats.Count == 0)
                {
                    continue;
                }

                bool headed = false;

                for (int i = 0; i < beats.Count; i++)
                {
                    BeatData beat = beats[i];

                    if (beat == null || !beat.MeasurePatience)
                    {
                        continue;
                    }

                    total++;

                    if (!headed)
                    {
                        log.AppendLine($"  {act.name}");
                        headed = true;
                    }

                    Speaker sign = ChildSpeakerBefore(beats, i);

                    if (sign == Speaker.YuaChild || sign == Speaker.HaruChild)
                    {
                        told++;
                        log.AppendLine($"     beat {i,4}  told — the plate reads {sign}");
                    }
                    else
                    {
                        log.AppendLine($"     beat {i,4}  NOT TOLD — no child line in the {Reach} beats before it");
                    }
                }
            }

            log.AppendLine();

            if (total == 0)
            {
                log.AppendLine("  No waiting moments at all. Either no act has been built, or the beats that");
                log.AppendLine("  measure patience have lost their tick. Run Rebuild Every Act From The Story");
                log.AppendLine("  Document.");
            }
            else
            {
                log.AppendLine($"  {told} of {total} waiting moments carry the child name plate.");

                if (told < total)
                {
                    log.AppendLine("  The ones marked NOT TOLD are silences the player has no way of noticing.");
                }
            }

            Debug.Log(log.ToString());
        }

        /// <summary>
        /// The child speaker of the nearest line before <paramref name="index"/>,
        /// or <see cref="Speaker.Narrator"/> for none.
        /// </summary>
        private static Speaker ChildSpeakerBefore(List<BeatData> beats, int index)
        {
            int seen = 0;

            for (int i = index; i >= 0 && seen < Reach; i--)
            {
                BeatData beat = beats[i];

                if (beat == null || beat.Kind != StoryBeatKind.Line)
                {
                    continue;
                }

                seen++;

                if (beat.Speaker == Speaker.YuaChild || beat.Speaker == Speaker.HaruChild)
                {
                    return beat.Speaker;
                }
            }

            return Speaker.Narrator;
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
