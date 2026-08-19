// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryInterludes.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Localization;

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// The two beats that recur across every act.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The design document asks for both of these to turn up at random through
    /// the whole game: the machine-room sound that frightens Yua, and the pain
    /// in Haru's leg that he never explains. They are written once here and
    /// dropped into acts as interludes, so a later act gets them by adding one
    /// line to its script.
    /// </para>
    /// <para>
    /// Both are built around a refusal. Yua is told something she should
    /// immediately question and lets it pass; Haru shows something he should be
    /// asked about and is not. The scenes are short on purpose — long enough for
    /// the player to feel the gap, too short for them to be sure there was one.
    /// </para>
    /// </remarks>
    public static class StoryInterludes
    {
        /// <summary>
        /// The machine room: a sound from under the floor, and the answer Yua
        /// does not follow up on.
        /// </summary>
        public static StoryScript MachineRoom()
        {
            return new StoryScript()
                .Sound(SfxId.BoilerRoom, 0.75f)
                .Narrate(Act01Strings.Boiler01)
                .Beat(1.4f)
                .Narrate(Act01Strings.Boiler02)
                .Yua(Act01Strings.Boiler03, Portrait.Sad)
                .Sound(SfxId.Heartbeat, 0.8f)
                .Yua(Act01Strings.Boiler04)
                .Haru(Act01Strings.Boiler05, Portrait.Neutral)
                .Narrate(Act01Strings.Boiler06)

                // The silence is doing the work here. Given a line of narration
                // instead, the player reads a statement; given two seconds of
                // nothing, they notice the question themselves.
                .Beat(2.0f)
                .Narrate(Act01Strings.Boiler07)
                .Yua(Act01Strings.Boiler08, Portrait.Neutral);
        }

        /// <summary>
        /// Haru's leg: he stops walking, gives no reason, and is not asked for
        /// one.
        /// </summary>
        public static StoryScript LegAche()
        {
            return new StoryScript()
                .Sound(SfxId.LegAche, 0.85f)
                .Narrate(Act01Strings.Leg01)
                .Haru(Act01Strings.Leg02, Portrait.Sad)
                .Narrate(Act01Strings.Leg03)
                .Beat(1.6f)
                .Narrate(Act01Strings.Leg04)
                .Haru(Act01Strings.Leg05, Portrait.Neutral);
        }
    }
}
