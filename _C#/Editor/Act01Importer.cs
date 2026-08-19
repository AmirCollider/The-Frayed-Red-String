// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act01Importer.cs  (Editor only)
//
//  A one-off. Act one was written in C# before the acts became assets, and this
//  walks that code and writes it out as an ActAsset — all one hundred and
//  thirty-odd lines, all three languages, in the same order.
//
//  Once it has run and the result looks right, four files can be deleted and
//  nothing will miss them:
//
//      Assets/_C#/Narrative/Act01Script.cs
//      Assets/_C#/Narrative/StoryInterludes.cs
//      Assets/_C#/Narrative/StoryScript.cs
//      Assets/_C#/Localization/Act01Strings.cs
//
//  ...along with this importer itself.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Converts the code-written act one into an asset.</summary>
    public static class Act01Importer
    {
        private const string MachineRoomName = "Interlude_MachineRoom";
        private const string LegAcheName = "Interlude_LegAche";

        /// <summary>True when act one has already been imported.</summary>
        public static bool AlreadyImported => ActLibrary.Find(1) != null;

        /// <summary>Imports act one and its two interludes, replacing any previous import.</summary>
        [MenuItem("The Frayed Red String/Import Act 01 From Code")]
        public static void Import()
        {
            ActAsset machineRoom = ImportScript(StoryInterludes.MachineRoom(), MachineRoomName, 0);
            ActAsset legAche = ImportScript(StoryInterludes.LegAche(), LegAcheName, 0);

            ActAsset act = ImportScript(Act01Script.Build(), "Act01", 1);

            if (act == null)
            {
                return;
            }

            act.Title = TitleFor(LocKeys.ActOne);
            act.MusicTrack = Audio.MusicTracks.ActOne;
            act.ShowTitleCard = true;

            LinkInterludes(act, machineRoom, legAche);

            EditorUtility.SetDirty(act);
            AssetDatabase.SaveAssets();

            StoryAssetBuilder.Rebuild();

            Debug.Log(
                $"[Story] Imported act one: {act.Count} beats, {act.SpokenCount()} of them spoken. " +
                "Open The Frayed Red String > Story Editor to read it.");

            Selection.activeObject = act;
        }

        /// <summary>
        /// Points the act's interlude beats at the assets just written.
        /// </summary>
        /// <remarks>
        /// The code version builds a fresh sub-script for every interlude beat,
        /// so there is no object identity to follow. They are matched on the
        /// first line's key instead, which is unambiguous — the machine-room
        /// scene and the leg scene share no text.
        /// </remarks>
        private static void LinkInterludes(ActAsset act, ActAsset machineRoom, ActAsset legAche)
        {
            StoryScript source = Act01Script.Build();

            for (int i = 0; i < act.Count && i < source.Count; i++)
            {
                if (act.Beats[i].Kind != StoryBeatKind.Interlude)
                {
                    continue;
                }

                StoryScript inner = source.At(i)?.Interlude;
                string firstKey = FirstTextKey(inner);

                act.Beats[i].Interlude =
                    firstKey != null && firstKey.StartsWith("act01.leg", System.StringComparison.Ordinal)
                        ? legAche
                        : machineRoom;
            }
        }

        private static string FirstTextKey(StoryScript script)
        {
            if (script == null)
            {
                return null;
            }

            for (int i = 0; i < script.Count; i++)
            {
                string key = script.At(i)?.TextKey;
                if (!string.IsNullOrEmpty(key))
                {
                    return key;
                }
            }

            return null;
        }

        private static ActAsset ImportScript(StoryScript script, string assetName, int actNumber)
        {
            if (script == null)
            {
                return null;
            }

            ActAsset act = FindExisting(assetName) ?? StoryAssetBuilder.CreateAct(actNumber, assetName);

            act.ActNumber = actNumber;
            act.ShowTitleCard = actNumber > 0;
            act.Beats = new List<BeatData>(script.Count);

            for (int i = 0; i < script.Count; i++)
            {
                BeatData beat = Convert(script.At(i));

                if (beat != null)
                {
                    act.Beats.Add(beat);
                }
            }

            EditorUtility.SetDirty(act);
            return act;
        }

        private static BeatData Convert(StoryBeat source)
        {
            if (source == null)
            {
                return null;
            }

            BeatData beat = new BeatData
            {
                Kind = source.Kind,
                Speaker = source.Speaker,
                Portrait = source.Portrait,
                Seconds = source.Seconds,
                Chance = source.Chance <= 0f ? 1f : source.Chance
            };

            switch (source.Kind)
            {
                case StoryBeatKind.Line:
                    beat.Text = Resolve(source.TextKey);
                    break;

                case StoryBeatKind.Background:
                    beat.Background = source.AssetName;
                    beat.Caption = Resolve(source.TextKey);
                    break;

                case StoryBeatKind.Caption:
                case StoryBeatKind.TitleCard:
                    beat.Caption = Resolve(source.TextKey);
                    break;

                case StoryBeatKind.Music:
                    beat.MusicTrack = source.AssetName;
                    break;

                case StoryBeatKind.Sound:
                    beat.PlaySound = true;
                    beat.Sound = source.Sound ?? Audio.SfxId.Petal;

                    // The code builder smuggled the volume through the Seconds
                    // field, which the asset format keeps for holds only.
                    beat.SoundVolume = source.Seconds <= 0f ? 1f : source.Seconds;
                    beat.Seconds = 0f;
                    break;

                case StoryBeatKind.Choice:
                    beat.Choices = ConvertChoices(source.Choices);
                    break;
            }

            return beat;
        }

        private static ChoiceData[] ConvertChoices(StoryChoice[] choices)
        {
            if (choices == null || choices.Length == 0)
            {
                return System.Array.Empty<ChoiceData>();
            }

            ChoiceData[] converted = new ChoiceData[choices.Length];

            for (int i = 0; i < choices.Length; i++)
            {
                converted[i] = new ChoiceData
                {
                    Text = Resolve(choices[i].TextKey),
                    Tone = choices[i].Tone
                };
            }

            return converted;
        }

        /// <summary>Pulls all three translations of a key out of the old table.</summary>
        private static LocalizedLine Resolve(string key)
        {
            if (string.IsNullOrEmpty(key) || !LocalizationDatabase.TryGetEntry(key, out LocEntry entry))
            {
                return default;
            }

            return new LocalizedLine(entry.English, entry.Japanese, entry.Persian);
        }

        private static LocalizedLine TitleFor(string key)
        {
            return Resolve(key);
        }

        private static ActAsset FindExisting(string assetName)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(ActAsset)} {assetName}");

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                if (System.IO.Path.GetFileNameWithoutExtension(path) == assetName)
                {
                    return AssetDatabase.LoadAssetAtPath<ActAsset>(path);
                }
            }

            return null;
        }
    }
}
