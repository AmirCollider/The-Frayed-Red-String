// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act01Script.cs
//
//  Act one: "Cherry Blossom Mirage".
//
//  A note on what is deliberately absent:
//  There are no blue-and-green choices in this act. The design document puts
//  that mechanic in act two, where Yua turns to the player, explains that the
//  buttons exist, and asks them to press the green ones. The reveal only works
//  if the player has spent the whole first act with no buttons at all and no
//  reason to suspect they were ever going to get any. ChoicePanelView is built
//  and the director handles choice beats; act one simply does not use them.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Localization;

namespace TheFrayedRedString.Narrative
{
    /// <summary>Builds the script for act one.</summary>
    public static class Act01Script
    {
        /// <summary>1-based act number, matching the save slots and title card.</summary>
        public const int ActNumber = 1;

        /// <summary>
        /// Chance that a repeat of one of the recurring beats fires.
        /// </summary>
        /// <remarks>
        /// The first machine-room scene and the first leg scene are guaranteed —
        /// they are how the act plants both threads, and an act that happened to
        /// skip them would be missing its spine. The later slots are the random
        /// ones the document asks for, so a second playthrough is not beat for
        /// beat identical to the first.
        /// </remarks>
        private const float RepeatChance = 0.35f;

        /// <summary>Assembles the act.</summary>
        public static StoryScript Build()
        {
            StoryScript script = new StoryScript(LocKeys.ActOne, ActNumber);

            script
                .TitleCard()
                .Music(MusicTracks.ActOne)

                // -------------------------------------------------------------
                //  The path to the gate
                // -------------------------------------------------------------
                .Place(Backgrounds.SchoolAlleyDay, Act01Strings.PlaceAlley)
                .Narrate(Act01Strings.Alley01)
                .Sound(SfxId.Petal, 0.6f)
                .Enter(Speaker.Yua, Portrait.Neutral)
                .Yua(Act01Strings.Alley02, Portrait.Neutral)
                .Yua(Act01Strings.Alley03)
                .Narrate(Act01Strings.Alley04)
                .Enter(Speaker.Haru, Portrait.Neutral)
                .Haru(Act01Strings.Alley05, Portrait.Neutral)
                .Yua(Act01Strings.Alley06, Portrait.Shy)

                // The one time the narration acknowledges the "-pi" names. After
                // this the game behaves as though nothing was said, which is
                // what makes it sit badly.
                .Narrate(Act01Strings.Alley07)
                .Haru(Act01Strings.Alley08, Portrait.Joyful)
                .Yua(Act01Strings.Alley09, Portrait.Neutral)
                .Haru(Act01Strings.Alley10, Portrait.Neutral)
                .Yua(Act01Strings.Alley11, Portrait.Joyful)
                .Yua(Act01Strings.Alley12, Portrait.Neutral)

                // -------------------------------------------------------------
                //  Homeroom
                // -------------------------------------------------------------
                .Place(Backgrounds.ClassroomDay, Act01Strings.PlaceClassroom)
                .Narrate(Act01Strings.Class01)
                .Haru(Act01Strings.Class02, Portrait.Joyful)
                .Yua(Act01Strings.Class03, Portrait.Joyful)
                .Haru(Act01Strings.Class04, Portrait.Shy)
                .Yua(Act01Strings.Class05, Portrait.Neutral)
                .Haru(Act01Strings.Class06, Portrait.Neutral)
                .Narrate(Act01Strings.Class07)
                .Haru(Act01Strings.Class08, Portrait.Sad)
                .Narrate(Act01Strings.Class09)

                // The first crack. Everything Yua says here is kind; only the
                // timing is wrong, so the beat is carried by the pause before it
                // and the narration after it rather than by her line.
                .Narrate(Act01Strings.Class10)
                .Yua(Act01Strings.Class11, Portrait.Neutral)
                .Narrate(Act01Strings.Class12)
                .Haru(Act01Strings.Class13, Portrait.Shy)
                .Yua(Act01Strings.Class14, Portrait.Neutral)
                .Narrate(Act01Strings.Class15)

                // -------------------------------------------------------------
                //  The roof
                // -------------------------------------------------------------
                .Place(Backgrounds.RooftopDay, Act01Strings.PlaceRooftop)
                .Narrate(Act01Strings.Roof01)
                .Haru(Act01Strings.Roof02, Portrait.Neutral)
                .Yua(Act01Strings.Roof03, Portrait.Joyful)
                .Haru(Act01Strings.Roof04, Portrait.Shy)
                .Yua(Act01Strings.Roof05, Portrait.Neutral)
                .Haru(Act01Strings.Roof06, Portrait.Neutral)
                .Yua(Act01Strings.Roof07, Portrait.Neutral)
                .Haru(Act01Strings.Roof08, Portrait.Shy)
                .Beat(1.5f)
                .Narrate(Act01Strings.Roof09)
                .Yua(Act01Strings.Roof10, Portrait.Neutral)
                .Narrate(Act01Strings.Roof11)
                .Haru(Act01Strings.Roof12, Portrait.Joyful)
                .Yua(Act01Strings.Roof13, Portrait.Joyful)
                .Haru(Act01Strings.Roof14, Portrait.Neutral)
                .Yua(Act01Strings.Roof15, Portrait.Neutral)
                .Haru(Act01Strings.Roof16, Portrait.Sad)
                .Yua(Act01Strings.Roof17, Portrait.Neutral)

                // -------------------------------------------------------------
                //  The west corridor
                // -------------------------------------------------------------
                .Place(Backgrounds.CorridorSunset, Act01Strings.PlaceCorridor)
                .Narrate(Act01Strings.Hall01)
                .Haru(Act01Strings.Hall02, Portrait.Neutral)
                .Yua(Act01Strings.Hall03, Portrait.Joyful)
                .Interlude(StoryInterludes.MachineRoom)
                .Narrate(Act01Strings.Hall04)

                // -------------------------------------------------------------
                //  The café
                // -------------------------------------------------------------
                .Place(Backgrounds.CafeDay, Act01Strings.PlaceCafe)
                .Narrate(Act01Strings.Cafe01)
                .Haru(Act01Strings.Cafe02, Portrait.Shy)
                .Yua(Act01Strings.Cafe03, Portrait.Joyful)
                .Haru(Act01Strings.Cafe04, Portrait.Shy)
                .Yua(Act01Strings.Cafe05, Portrait.Joyful)
                .Narrate(Act01Strings.Cafe06)
                .Narrate(Act01Strings.Cafe07)
                .Narrate(Act01Strings.Cafe08)
                .Haru(Act01Strings.Cafe09, Portrait.Joyful)
                .Yua(Act01Strings.Cafe10, Portrait.Neutral)
                .Haru(Act01Strings.Cafe11, Portrait.Neutral)
                .Narrate(Act01Strings.Cafe12)
                .Yua(Act01Strings.Cafe13, Portrait.Neutral)
                .Yua(Act01Strings.Cafe14)
                .Haru(Act01Strings.Cafe15, Portrait.Neutral)
                .Yua(Act01Strings.Cafe16, Portrait.Shy)
                .Haru(Act01Strings.Cafe17, Portrait.Joyful)
                .Yua(Act01Strings.Cafe18, Portrait.Shy)
                .Narrate(Act01Strings.Cafe19)

                // -------------------------------------------------------------
                //  The road home
                // -------------------------------------------------------------
                .Place(Backgrounds.VendingStreetDay, Act01Strings.PlaceStreet)
                .Narrate(Act01Strings.Street01)
                .Narrate(Act01Strings.Street02)
                .Yua(Act01Strings.Street03, Portrait.Neutral)
                .Haru(Act01Strings.Street04, Portrait.Shy)
                .Narrate(Act01Strings.Street05)
                .Interlude(StoryInterludes.LegAche)

                .Place(Backgrounds.TrainPlatformSunset, Act01Strings.PlacePlatform)
                .Narrate(Act01Strings.Platform01)
                .Haru(Act01Strings.Platform02, Portrait.Neutral)
                .Yua(Act01Strings.Platform03, Portrait.Joyful)
                .Exit(Speaker.Haru)
                .Narrate(Act01Strings.Platform04)
                .Yua(Act01Strings.Platform05, Portrait.Neutral)
                .Yua(Act01Strings.Platform06)

                // -------------------------------------------------------------
                //  The rain, a few days later
                // -------------------------------------------------------------
                .ClearStage()
                .Place(Backgrounds.ClassroomRainy, Act01Strings.PlaceClassroomRain)
                .Narrate(Act01Strings.Rain01)
                .Narrate(Act01Strings.Rain02)
                .Enter(Speaker.Yua, Portrait.Neutral)
                .Enter(Speaker.Haru, Portrait.Joyful)
                .Haru(Act01Strings.Rain03, Portrait.Joyful)
                .Yua(Act01Strings.Rain04, Portrait.Neutral)
                .Haru(Act01Strings.Rain05, Portrait.Shy)
                .Yua(Act01Strings.Rain06, Portrait.Joyful)
                .Haru(Act01Strings.Rain07, Portrait.Joyful)
                .Narrate(Act01Strings.Rain08)
                .Interlude(StoryInterludes.MachineRoom, RepeatChance)

                // -------------------------------------------------------------
                //  The café, wet
                // -------------------------------------------------------------
                .Place(Backgrounds.CafeRainy, Act01Strings.PlaceCafeRain)
                .Narrate(Act01Strings.Wet01)
                .Haru(Act01Strings.Wet02, Portrait.Neutral)
                .Yua(Act01Strings.Wet03, Portrait.Joyful)
                .Haru(Act01Strings.Wet04, Portrait.Shy)

                // The slip, and the beat in which both of them decide not to
                // have heard it.
                .Beat(1.6f)
                .Haru(Act01Strings.Wet05, Portrait.Neutral)
                .Narrate(Act01Strings.Wet06)
                .Yua(Act01Strings.Wet07, Portrait.Neutral)
                .Narrate(Act01Strings.Wet08)

                // -------------------------------------------------------------
                //  The last lamp
                // -------------------------------------------------------------
                .Place(Backgrounds.VendingStreetNight, Act01Strings.PlaceStreetNight)
                .Narrate(Act01Strings.Night01)
                .Haru(Act01Strings.Night02, Portrait.Neutral)
                .Yua(Act01Strings.Night03, Portrait.Joyful)
                .Haru(Act01Strings.Night04, Portrait.Joyful)
                .Haru(Act01Strings.Night05, Portrait.Neutral)
                .Yua(Act01Strings.Night06, Portrait.Shy)
                .Haru(Act01Strings.Night07, Portrait.Shy)
                .Yua(Act01Strings.Night08, Portrait.Neutral)
                .Interlude(StoryInterludes.LegAche, RepeatChance)
                .Exit(Speaker.Haru)
                .Narrate(Act01Strings.Night09)
                .Narrate(Act01Strings.Night10)
                .Yua(Act01Strings.Night11, Portrait.Neutral)
                .Yua(Act01Strings.Night12)

                // One frame of the mask off, on a single word, and then straight
                // back. Long enough to be seen, short enough that a player who
                // blinked will not be sure they saw it — which is the whole
                // instruction the document gives for act one.
                .Yua(Act01Strings.Night13, Portrait.DeadEyes)
                .Beat(2.2f)
                .Yua(Act01Strings.Night14, Portrait.Neutral)
                .ClearStage()
                .Narrate(Act01Strings.Night15)
                .End();

            return script;
        }
    }
}
