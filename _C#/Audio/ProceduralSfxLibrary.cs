// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ProceduralSfxLibrary.cs
//
//  The game's sound palette, written as recipes rather than files.
//
//  Everything here is tuned to C major pentatonic and voiced on music-box and
//  glockenspiel timbres. That choice is the story's: the game opens wearing the
//  costume of a bright pastel dating sim, and its interface has to sound like
//  something warm and alive rather than like a piece of software. Each sound is
//  a small chord plus a scatter of high grains, sitting in a short reverb — a
//  handful of light thrown across the stereo field.
//
//  Two sounds break that rule on purpose. BoilerRoom and LegAche are not in the
//  key and carry no sparkle at all, because they are the places where the
//  costume slips.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Builds and caches one <see cref="AudioClip"/> per <see cref="SfxId"/>.
    /// </summary>
    public static class ProceduralSfxLibrary
    {
        // Pitches of the C major pentatonic scale the whole interface sings in.
        private const float G3 = 196.00f;
        private const float C4 = 261.63f;
        private const float G4 = 392.00f;
        private const float A4 = 440.00f;
        private const float C5 = 523.25f;
        private const float D5 = 587.33f;
        private const float E5 = 659.25f;
        private const float G5 = 783.99f;
        private const float A5 = 880.00f;
        private const float C6 = 1046.50f;
        private const float D6 = 1174.66f;
        private const float E6 = 1318.51f;
        private const float G6 = 1567.98f;
        private const float A6 = 1760.00f;
        private const float C7 = 2093.00f;

        /// <summary>
        /// Sounds the player can trigger before any act has loaded. Warmed up at
        /// boot so the very first click does not pay for synthesis mid-frame.
        /// </summary>
        private static readonly SfxId[] CoreSounds =
        {
            SfxId.Tap,
            SfxId.Click,
            SfxId.Hover,
            SfxId.Confirm,
            SfxId.Cancel,
            SfxId.Toggle,
            SfxId.Denied
        };

        private static readonly Dictionary<SfxId, AudioClip> Cache = new Dictionary<SfxId, AudioClip>();

        /// <summary>Returns the clip for <paramref name="id"/>, synthesising it if needed.</summary>
        public static AudioClip Get(SfxId id)
        {
            if (Cache.TryGetValue(id, out AudioClip cached) && cached != null)
            {
                return cached;
            }

            AudioClip clip = Build(id);
            Cache[id] = clip;
            return clip;
        }

        /// <summary>Pre-generates the interface sounds. Called once at boot.</summary>
        public static void WarmUpCore()
        {
            for (int i = 0; i < CoreSounds.Length; i++)
            {
                Get(CoreSounds[i]);
            }
        }

        /// <summary>
        /// Pre-generates everything else.
        /// </summary>
        /// <remarks>
        /// Called by a story scene during its load, while the fade curtain is
        /// still down. The story palette is both larger and longer than the
        /// interface one — the machine-room drone alone is four seconds of
        /// reverberated stereo — and rendering it at boot would add a visible
        /// pause to the splash screen in exchange for nothing, since none of it
        /// can be triggered from a menu.
        /// </remarks>
        public static void WarmUpStory()
        {
            foreach (SfxId id in System.Enum.GetValues(typeof(SfxId)))
            {
                Get(id);
            }
        }

        /// <summary>Drops cached clips; used when statics are reset between play sessions.</summary>
        public static void Clear()
        {
            Cache.Clear();
        }

        private static AudioClip Build(SfxId id)
        {
            switch (id)
            {
                // -------------------------------------------------------------
                //  Interface
                // -------------------------------------------------------------

                // Clicking empty space. Soft, low, barely there — it must never
                // compete with a real button, but silence would make the screen
                // feel dead under the cursor.
                case SfxId.Tap:
                    return ProceduralAudioSynth.Render(
                        "SFX_Tap",
                        totalDuration: 0.55f,
                        voices: new[]
                        {
                            Note(C5, 0f, 0.30f, 0.34f, Timbre.Wood, decay: 13f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(3, 0.005f, 0.07f, C6, 2, 0.10f, 0.5f, 0.20f)
                        },
                        reverbMix: 0.22f,
                        reverbSeconds: 0.8f,
                        seed: 4021);

                // The main button press: a bright major third, struck like a
                // music box, with a small burst of stars behind it.
                case SfxId.Click:
                    return ProceduralAudioSynth.Render(
                        "SFX_Click",
                        totalDuration: 0.85f,
                        voices: new[]
                        {
                            Note(C6, 0.000f, 0.55f, 0.55f, Timbre.MusicBox, decay: 7.5f, pan: -0.12f),
                            Note(E6, 0.012f, 0.50f, 0.38f, Timbre.MusicBox, decay: 8.5f, pan: 0.14f),
                            Note(C5, 0.000f, 0.30f, 0.20f, Timbre.Wood, decay: 14f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(7, 0.01f, 0.16f, C6, 2, 0.17f, 0.75f, 0.28f)
                        },
                        reverbMix: 0.30f,
                        reverbSeconds: 1.1f,
                        seed: 9137);

                // Pointer-enter. One high grain and a whisper of scatter: enough
                // to acknowledge the cursor, quiet enough to survive being
                // triggered dozens of times a minute.
                case SfxId.Hover:
                    return ProceduralAudioSynth.Render(
                        "SFX_Hover",
                        totalDuration: 0.45f,
                        voices: new[]
                        {
                            Note(G6, 0f, 0.22f, 0.16f, Timbre.Soft, decay: 15f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(3, 0f, 0.06f, E6, 2, 0.09f, 0.85f, 0.18f)
                        },
                        reverbMix: 0.30f,
                        reverbSeconds: 0.9f,
                        seed: 5533);

                // Accepting, advancing, pressing Enter on the warning screen.
                // A rising pentatonic run answered by a wide scatter of stars —
                // the most generous sound in the interface.
                case SfxId.Confirm:
                    return ProceduralAudioSynth.Render(
                        "SFX_Confirm",
                        totalDuration: 1.90f,
                        voices: new[]
                        {
                            Note(C5, 0.000f, 0.70f, 0.42f, Timbre.MusicBox, decay: 5.0f, pan: -0.25f),
                            Note(E5, 0.070f, 0.70f, 0.40f, Timbre.MusicBox, decay: 5.0f, pan: -0.08f),
                            Note(G5, 0.140f, 0.75f, 0.40f, Timbre.MusicBox, decay: 4.6f, pan: 0.10f),
                            Note(C6, 0.210f, 0.90f, 0.44f, Timbre.Bell, decay: 4.0f, pan: 0.26f),
                            Note(E6, 0.230f, 0.85f, 0.20f, Timbre.Bell, decay: 4.4f, pan: 0.05f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(18, 0.16f, 0.60f, C6, 3, 0.15f, 1f, 0.45f)
                        },
                        reverbMix: 0.38f,
                        reverbSeconds: 1.9f,
                        seed: 2281);

                // Closing a panel. The same gesture inverted: a gentle fall,
                // fewer stars, still warm rather than negative.
                case SfxId.Cancel:
                    return ProceduralAudioSynth.Render(
                        "SFX_Cancel",
                        totalDuration: 1.10f,
                        voices: new[]
                        {
                            Note(G5, 0.000f, 0.45f, 0.40f, Timbre.MusicBox, decay: 7f, pan: 0.15f),
                            Note(E5, 0.075f, 0.50f, 0.36f, Timbre.MusicBox, decay: 7f, pan: -0.05f),
                            Note(C5, 0.150f, 0.60f, 0.34f, Timbre.Wood, decay: 7f, pan: -0.18f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(5, 0.02f, 0.22f, A5, 2, 0.10f, 0.7f, 0.30f)
                        },
                        reverbMix: 0.30f,
                        reverbSeconds: 1.2f,
                        seed: 7712);

                // The language flip. Two bright notes a sixth apart and a
                // scatter that sweeps across the field, like a page turning.
                case SfxId.Toggle:
                    return ProceduralAudioSynth.Render(
                        "SFX_Toggle",
                        totalDuration: 1.00f,
                        voices: new[]
                        {
                            Note(D6, 0.000f, 0.40f, 0.40f, Timbre.Bell, decay: 8f, pan: -0.4f),
                            Note(A6, 0.080f, 0.45f, 0.30f, Timbre.Bell, decay: 8f, pan: 0.4f),
                            Note(D5, 0.000f, 0.30f, 0.16f, Timbre.Wood, decay: 13f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(9, 0.01f, 0.26f, D6, 2, 0.13f, 1f, 0.30f)
                        },
                        reverbMix: 0.32f,
                        reverbSeconds: 1.2f,
                        seed: 3344);

                // Something that cannot be done. The one interface sound with no
                // sparkle and almost no tail: dry, low and over immediately, so
                // it reads as a closed door next to everything else's openness.
                case SfxId.Denied:
                    return ProceduralAudioSynth.Render(
                        "SFX_Denied",
                        totalDuration: 0.45f,
                        voices: new[]
                        {
                            Note(C5 * 0.25f, 0f, 0.26f, 0.45f, Timbre.Wood, decay: 16f),
                            Note(C5 * 0.375f, 0.008f, 0.20f, 0.22f, Timbre.Wood, decay: 20f)
                        },
                        sparkles: null,
                        reverbMix: 0.12f,
                        reverbSeconds: 0.5f,
                        seed: 6180);

                // -------------------------------------------------------------
                //  Typewriter voices
                //
                //  These are single grains, not phrases. The typewriter plays one
                //  every few characters at a pitch walking a pentatonic line, so
                //  what the player hears over a sentence is a little melody
                //  rather than the same blip forty times. That is the whole
                //  difference between "a character is speaking" and "a machine is
                //  printing".
                //
                //  Each grain is therefore kept short, quiet and free of reverb
                //  tail: at eight or ten per second, anything longer smears into
                //  a drone.
                // -------------------------------------------------------------

                // Yua: high, glassy, sweet. The prettiest sound in the game.
                case SfxId.TypeYua:
                    return ProceduralAudioSynth.Render(
                        "SFX_TypeYua",
                        totalDuration: 0.24f,
                        voices: new[]
                        {
                            Note(C6, 0f, 0.16f, 0.42f, Timbre.MusicBox, decay: 26f),
                            Note(G6, 0.004f, 0.11f, 0.14f, Timbre.Bell, decay: 34f, pan: 0.18f)
                        },
                        sparkles: null,
                        reverbMix: 0.16f,
                        reverbSeconds: 0.45f,
                        seed: 1201);

                // Haru: a fifth lower, wooden, rounder, with a slower fall. He
                // takes up less space in a room than she does, and the sound
                // should agree.
                case SfxId.TypeHaru:
                    return ProceduralAudioSynth.Render(
                        "SFX_TypeHaru",
                        totalDuration: 0.28f,
                        voices: new[]
                        {
                            Note(G4, 0f, 0.20f, 0.44f, Timbre.Wood, decay: 19f, pan: -0.10f),
                            Note(D5, 0.006f, 0.14f, 0.13f, Timbre.MusicBox, decay: 26f)
                        },
                        sparkles: null,
                        reverbMix: 0.18f,
                        reverbSeconds: 0.55f,
                        seed: 1303);

                // Narration: felt more than heard. Nearly a sine, very quiet, so
                // a long descriptive passage does not tire the ear.
                case SfxId.TypeNarrator:
                    return ProceduralAudioSynth.Render(
                        "SFX_TypeNarrator",
                        totalDuration: 0.22f,
                        voices: new[]
                        {
                            Note(A4, 0f, 0.15f, 0.20f, Timbre.Soft, decay: 24f, noise: 0.015f)
                        },
                        sparkles: null,
                        reverbMix: 0.20f,
                        reverbSeconds: 0.50f,
                        seed: 1409);

                // -------------------------------------------------------------
                //  Story punctuation
                // -------------------------------------------------------------

                // Opening a gift box. Three gestures in sequence: the ribbon
                // slipping (a noise transient with almost no pitch), a beat of
                // nothing while the lid comes off, then the chord blooming out.
                // The pause is what sells it — a present that opens instantly is
                // just a chord.
                case SfxId.GiftBox:
                    return ProceduralAudioSynth.Render(
                        "SFX_GiftBox",
                        totalDuration: 2.80f,
                        voices: new[]
                        {
                            // Ribbon: mostly the noise burst, pitched low enough
                            // to read as paper rather than as a note.
                            Note(C4, 0.000f, 0.18f, 0.30f, Timbre.Soft, decay: 30f, noise: 0.55f, pan: -0.2f),
                            Note(G3, 0.055f, 0.16f, 0.20f, Timbre.Wood, decay: 34f, noise: 0.40f, pan: 0.22f),

                            // Lid off, chord out.
                            Note(C5, 0.240f, 1.10f, 0.40f, Timbre.MusicBox, decay: 3.2f, pan: -0.30f),
                            Note(E5, 0.262f, 1.10f, 0.36f, Timbre.MusicBox, decay: 3.2f, pan: -0.08f),
                            Note(G5, 0.284f, 1.15f, 0.34f, Timbre.MusicBox, decay: 3.0f, pan: 0.14f),
                            Note(C6, 0.306f, 1.30f, 0.42f, Timbre.Bell, decay: 2.6f, pan: 0.32f),
                            Note(E6, 0.340f, 1.20f, 0.20f, Timbre.Bell, decay: 3.0f, pan: 0.05f),
                            Note(C4, 0.240f, 0.60f, 0.18f, Timbre.Wood, decay: 6f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(22, 0.30f, 0.95f, C6, 3, 0.14f, 1f, 0.55f)
                        },
                        reverbMix: 0.42f,
                        reverbSeconds: 2.4f,
                        seed: 8801);

                // A wish going up and bursting. Four bells climbing quickly, then
                // everything at once, thrown as wide as the stereo field goes.
                case SfxId.WishStar:
                    return ProceduralAudioSynth.Render(
                        "SFX_WishStar",
                        totalDuration: 2.20f,
                        voices: new[]
                        {
                            Note(G5, 0.000f, 0.30f, 0.26f, Timbre.Bell, decay: 12f, pan: -0.35f),
                            Note(C6, 0.045f, 0.30f, 0.28f, Timbre.Bell, decay: 12f, pan: -0.12f),
                            Note(E6, 0.090f, 0.30f, 0.30f, Timbre.Bell, decay: 12f, pan: 0.14f),
                            Note(G6, 0.135f, 0.35f, 0.32f, Timbre.Bell, decay: 10f, pan: 0.36f),

                            // The burst itself: a high strike plus a soft low
                            // body so it has weight as well as brightness.
                            Note(C7, 0.190f, 1.10f, 0.34f, Timbre.Bell, decay: 3.4f),
                            Note(C4, 0.190f, 0.55f, 0.22f, Timbre.Wood, decay: 7f, noise: 0.18f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(28, 0.19f, 0.80f, C6, 3, 0.13f, 1f, 0.60f),
                            Sparkle(10, 0.55f, 0.75f, G6, 2, 0.07f, 1f, 0.50f)
                        },
                        reverbMix: 0.44f,
                        reverbSeconds: 2.1f,
                        seed: 7305);

                // Paper. Two soft noise brushes a hair apart, panned across, with
                // just enough pitch to keep them from sounding like static.
                case SfxId.PageTurn:
                    return ProceduralAudioSynth.Render(
                        "SFX_PageTurn",
                        totalDuration: 0.60f,
                        voices: new[]
                        {
                            Note(D5, 0.000f, 0.14f, 0.22f, Timbre.Soft, decay: 30f, noise: 0.50f, pan: -0.35f),
                            Note(A4, 0.070f, 0.16f, 0.18f, Timbre.Soft, decay: 26f, noise: 0.42f, pan: 0.32f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(2, 0.02f, 0.10f, C6, 1, 0.05f, 0.8f, 0.16f)
                        },
                        reverbMix: 0.24f,
                        reverbSeconds: 0.8f,
                        seed: 4417);

                // Petals. No fundamental worth naming — a handful of very quiet
                // high grains drifting across the field.
                case SfxId.Petal:
                    return ProceduralAudioSynth.Render(
                        "SFX_Petal",
                        totalDuration: 1.60f,
                        voices: null,
                        sparkles: new[]
                        {
                            Sparkle(9, 0.00f, 0.85f, A5, 3, 0.055f, 1f, 0.55f)
                        },
                        reverbMix: 0.40f,
                        reverbSeconds: 1.6f,
                        seed: 2907);

                // Lub-dub. Two low thuds with the second softer and close behind,
                // which is what makes it read as a heart rather than a drum.
                case SfxId.Heartbeat:
                    return ProceduralAudioSynth.Render(
                        "SFX_Heartbeat",
                        totalDuration: 1.15f,
                        voices: new[]
                        {
                            Note(58f, 0.000f, 0.34f, 0.85f, Timbre.Soft, decay: 11f, noise: 0.10f),
                            Note(86f, 0.000f, 0.20f, 0.25f, Timbre.Wood, decay: 18f),
                            Note(54f, 0.235f, 0.32f, 0.55f, Timbre.Soft, decay: 12f, noise: 0.08f),
                            Note(80f, 0.235f, 0.18f, 0.16f, Timbre.Wood, decay: 20f)
                        },
                        sparkles: null,
                        reverbMix: 0.16f,
                        reverbSeconds: 1.0f,
                        seed: 6602);

                // The machine room, heard through a floor.
                //
                // Built from four voices detuned by a couple of Hz against each
                // other. That mistuning is the entire trick: the beating between
                // them never settles, so the drone will not resolve into a note
                // and the ear keeps waiting for it to. Bell partials on top put
                // metal in the room.
                case SfxId.BoilerRoom:
                    return ProceduralAudioSynth.Render(
                        "SFX_BoilerRoom",
                        totalDuration: 4.20f,
                        voices: new[]
                        {
                            Note(47.0f, 0f, 4.00f, 0.60f, Timbre.Soft, decay: 0.42f, noise: 0.05f, pan: -0.15f),
                            Note(49.3f, 0f, 4.00f, 0.48f, Timbre.Soft, decay: 0.40f, pan: 0.18f),
                            Note(94.6f, 0.05f, 3.70f, 0.22f, Timbre.Bell, decay: 0.55f, pan: 0.30f),
                            Note(141.0f, 0.12f, 3.40f, 0.10f, Timbre.Bell, decay: 0.75f, pan: -0.34f),

                            // A single knock somewhere in the dark, so the drone
                            // is a place rather than a texture.
                            Note(120f, 1.35f, 0.40f, 0.26f, Timbre.Wood, decay: 9f, noise: 0.22f, pan: 0.4f)
                        },
                        sparkles: null,
                        reverbMix: 0.52f,
                        reverbSeconds: 3.2f,
                        seed: 1666);

                // Haru's leg. One low swell that arrives and goes, no attack to
                // speak of — pain that has been there long enough to be ordinary.
                case SfxId.LegAche:
                    return ProceduralAudioSynth.Render(
                        "SFX_LegAche",
                        totalDuration: 1.60f,
                        voices: new[]
                        {
                            Note(72f, 0f, 1.20f, 0.50f, Timbre.Soft, decay: 2.2f, noise: 0.04f),
                            Note(108f, 0.03f, 1.00f, 0.16f, Timbre.Wood, decay: 3.0f, pan: -0.2f)
                        },
                        sparkles: null,
                        reverbMix: 0.30f,
                        reverbSeconds: 1.6f,
                        seed: 5150);

                // -------------------------------------------------------------
                //  Choices
                // -------------------------------------------------------------

                // Two options arriving. A soft open fifth and a light scatter:
                // an invitation, not a demand.
                case SfxId.ChoiceAppear:
                    return ProceduralAudioSynth.Render(
                        "SFX_ChoiceAppear",
                        totalDuration: 1.30f,
                        voices: new[]
                        {
                            Note(D5, 0.000f, 0.55f, 0.30f, Timbre.MusicBox, decay: 6f, pan: -0.28f),
                            Note(A5, 0.060f, 0.60f, 0.28f, Timbre.MusicBox, decay: 5.6f, pan: 0.26f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(8, 0.05f, 0.40f, D6, 2, 0.10f, 0.95f, 0.34f)
                        },
                        reverbMix: 0.34f,
                        reverbSeconds: 1.4f,
                        seed: 3111);

                // The kind answer. A plain major third, nothing hidden in it.
                case SfxId.ChoiceKind:
                    return ProceduralAudioSynth.Render(
                        "SFX_ChoiceKind",
                        totalDuration: 1.40f,
                        voices: new[]
                        {
                            Note(C6, 0.000f, 0.80f, 0.42f, Timbre.MusicBox, decay: 4.4f, pan: -0.16f),
                            Note(E6, 0.020f, 0.80f, 0.34f, Timbre.MusicBox, decay: 4.6f, pan: 0.18f),
                            Note(C5, 0.000f, 0.45f, 0.22f, Timbre.Wood, decay: 8f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(10, 0.02f, 0.35f, C6, 2, 0.12f, 0.9f, 0.38f)
                        },
                        reverbMix: 0.36f,
                        reverbSeconds: 1.5f,
                        seed: 3222);

                // The cruel answer.
                //
                // Deliberately built to pass for the kind one. Same instrument,
                // same length, same sparkle count — but the interval is a major
                // second instead of a third, so the two notes grind very gently
                // against each other. Most players will not be able to say what
                // is different. That is the point: in act one the game is still
                // pretending, and the sound pretends with it.
                case SfxId.ChoiceCruel:
                    return ProceduralAudioSynth.Render(
                        "SFX_ChoiceCruel",
                        totalDuration: 1.40f,
                        voices: new[]
                        {
                            Note(C6, 0.000f, 0.80f, 0.42f, Timbre.MusicBox, decay: 4.4f, pan: -0.16f),
                            Note(D6, 0.020f, 0.80f, 0.30f, Timbre.MusicBox, decay: 4.6f, pan: 0.18f),
                            Note(C5 * 0.5f, 0.000f, 0.50f, 0.24f, Timbre.Wood, decay: 7f)
                        },
                        sparkles: new[]
                        {
                            Sparkle(10, 0.02f, 0.35f, C6, 2, 0.12f, 0.9f, 0.38f)
                        },
                        reverbMix: 0.36f,
                        reverbSeconds: 1.5f,
                        seed: 3333);

                // Yua taking a blue choice back out of the player's hands.
                //
                // A fifth bent a quarter-tone flat, so it arrives on the same
                // interval the choice sounds do and lands just outside it — near
                // enough to be the same game, wrong enough to be heard as a
                // correction rather than as a note. No sparkle: this is the one
                // cue in act two that is not decorated.
                case SfxId.StringPull:
                    return ProceduralAudioSynth.Render(
                        "SFX_StringPull",
                        totalDuration: 1.50f,
                        voices: new[]
                        {
                            Note(247.0f, 0.000f, 0.95f, 0.34f, Timbre.Soft, decay: 3.2f, noise: 0.06f, pan: -0.12f),
                            Note(370.5f, 0.045f, 0.85f, 0.24f, Timbre.Soft, decay: 3.6f, pan: 0.16f),
                            Note(740.0f, 0.110f, 0.50f, 0.13f, Timbre.Bell, decay: 6.5f, pan: 0.26f),

                            // The knock at the end: the button put back where it
                            // was, by someone who was never going to let go of it.
                            Note(96.0f, 0.560f, 0.30f, 0.30f, Timbre.Wood, decay: 13f, noise: 0.20f)
                        },
                        sparkles: null,
                        reverbMix: 0.34f,
                        reverbSeconds: 1.4f,
                        seed: 2049);

                default:
                    return Get(SfxId.Click);
            }
        }

        /// <summary>Shorthand for a struck note, since every recipe needs several.</summary>
        private static VoiceSpec Note(
            float frequency,
            float startTime,
            float duration,
            float amplitude,
            Timbre timbre,
            float decay,
            float pan = 0f,
            float noise = 0.035f)
        {
            return new VoiceSpec
            {
                Frequency = frequency,
                StartTime = startTime,
                Duration = duration,
                Amplitude = amplitude,
                Decay = decay,
                Timbre = timbre,
                Pan = pan,
                NoiseAmount = noise
            };
        }

        /// <summary>Shorthand for a grain scatter.</summary>
        private static SparkleSpec Sparkle(
            int count,
            float startTime,
            float spread,
            float baseFrequency,
            int octaves,
            float amplitude,
            float stereoWidth,
            float grainDuration)
        {
            return new SparkleSpec
            {
                Count = count,
                StartTime = startTime,
                Spread = spread,
                BaseFrequency = baseFrequency,
                Octaves = octaves,
                Amplitude = amplitude,
                StereoWidth = stereoWidth,
                GrainDuration = grainDuration
            };
        }
    }
}
