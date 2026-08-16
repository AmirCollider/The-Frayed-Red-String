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

        /// <summary>
        /// Pre-generates every clip. Called once at boot so the first click of a
        /// session does not pay the synthesis cost mid-frame.
        /// </summary>
        public static void WarmUp()
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
                            new SparkleSpec
                            {
                                Count = 3,
                                StartTime = 0.005f,
                                Spread = 0.07f,
                                BaseFrequency = C6,
                                Octaves = 2,
                                Amplitude = 0.10f,
                                StereoWidth = 0.5f,
                                GrainDuration = 0.20f
                            }
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
                            new SparkleSpec
                            {
                                Count = 7,
                                StartTime = 0.01f,
                                Spread = 0.16f,
                                BaseFrequency = C6,
                                Octaves = 2,
                                Amplitude = 0.17f,
                                StereoWidth = 0.75f,
                                GrainDuration = 0.28f
                            }
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
                            new SparkleSpec
                            {
                                Count = 3,
                                StartTime = 0f,
                                Spread = 0.06f,
                                BaseFrequency = E6,
                                Octaves = 2,
                                Amplitude = 0.09f,
                                StereoWidth = 0.85f,
                                GrainDuration = 0.18f
                            }
                        },
                        reverbMix: 0.30f,
                        reverbSeconds: 0.9f,
                        seed: 5533);

                // Accepting, advancing, pressing Enter on the warning screen.
                // A rising pentatonic run answered by a wide scatter of stars —
                // the most generous sound in the game, and the only one allowed
                // to take a full second and a half.
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
                            new SparkleSpec
                            {
                                Count = 18,
                                StartTime = 0.16f,
                                Spread = 0.60f,
                                BaseFrequency = C6,
                                Octaves = 3,
                                Amplitude = 0.15f,
                                StereoWidth = 1f,
                                GrainDuration = 0.45f
                            }
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
                            new SparkleSpec
                            {
                                Count = 5,
                                StartTime = 0.02f,
                                Spread = 0.22f,
                                BaseFrequency = A5,
                                Octaves = 2,
                                Amplitude = 0.10f,
                                StereoWidth = 0.7f,
                                GrainDuration = 0.30f
                            }
                        },
                        reverbMix: 0.30f,
                        reverbSeconds: 1.2f,
                        seed: 7712);

                // The language flip. Two bright notes a sixth apart and a
                // scatter that sweeps left to right, like a page turning.
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
                            new SparkleSpec
                            {
                                Count = 9,
                                StartTime = 0.01f,
                                Spread = 0.26f,
                                BaseFrequency = D6,
                                Octaves = 2,
                                Amplitude = 0.13f,
                                StereoWidth = 1f,
                                GrainDuration = 0.30f
                            }
                        },
                        reverbMix: 0.32f,
                        reverbSeconds: 1.2f,
                        seed: 3344);

                // Something that cannot be done. The one sound with no sparkle
                // and almost no tail: dry, low and over immediately, so it reads
                // as a closed door next to everything else's openness.
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
            float pan = 0f)
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
                NoiseAmount = 0.035f
            };
        }
    }
}
