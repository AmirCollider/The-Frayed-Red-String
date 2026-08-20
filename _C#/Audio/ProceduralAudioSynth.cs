// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ProceduralAudioSynth.cs
//
//  A small offline synthesiser. Every UI sound in the game is rendered into a
//  float buffer here and handed to Unity as an AudioClip, which is why the
//  project needs no .wav / .mp3 assets for its interface.
//
//  Design note — why this is not just a sine blip:
//  A single decaying sine reads as a machine beep. Three things turn it into
//  something that sounds alive:
//    1. Inharmonic partials. Real struck objects (glockenspiel, music box)
//       ring at ratios like 2.76 and 5.40, not at whole-number harmonics.
//       That inharmonicity is the whole difference between "chime" and "beep".
//    2. Sparkle grains. A scatter of tiny high notes, pitched to a pentatonic
//       scale and spread across the stereo field, so the sound arrives as a
//       handful of light rather than a single point.
//    3. Reverb. Without a tail a sound exists nowhere. With one it exists in
//       a room.
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>Instrument colours available to <see cref="VoiceSpec"/>.</summary>
    public enum Timbre
    {
        /// <summary>Bright, glassy, strongly inharmonic — a glockenspiel.</summary>
        Bell,

        /// <summary>Sweeter and rounder, with a hint of inharmonicity — a music box.</summary>
        MusicBox,

        /// <summary>Warm and quick, mostly odd harmonics — a marimba bar.</summary>
        Wood,

        /// <summary>Almost pure, for grains that should be felt more than heard.</summary>
        Soft
    }

    /// <summary>One struck note inside a sound.</summary>
    public struct VoiceSpec
    {
        /// <summary>Pitch of the fundamental, in Hz.</summary>
        public float Frequency;

        /// <summary>Offset from the start of the clip, in seconds.</summary>
        public float StartTime;

        /// <summary>How long the note is rendered for, in seconds.</summary>
        public float Duration;

        /// <summary>Peak linear amplitude, 0..1.</summary>
        public float Amplitude;

        /// <summary>Exponential decay rate of the fundamental. Larger dies faster.</summary>
        public float Decay;

        /// <summary>Instrument colour.</summary>
        public Timbre Timbre;

        /// <summary>Stereo placement, -1 hard left to +1 hard right.</summary>
        public float Pan;

        /// <summary>Level of the noise transient at the attack, 0..1.</summary>
        public float NoiseAmount;

        /// <summary>Creates a note with sensible defaults for a UI chime.</summary>
        public static VoiceSpec Note(
            float frequency,
            float startTime,
            float duration,
            float amplitude,
            Timbre timbre = Timbre.MusicBox,
            float pan = 0f)
        {
            return new VoiceSpec
            {
                Frequency = frequency,
                StartTime = startTime,
                Duration = duration,
                Amplitude = amplitude,
                Decay = 6.5f,
                Timbre = timbre,
                Pan = pan,
                NoiseAmount = 0.05f
            };
        }
    }

    /// <summary>
    /// A scatter of tiny high notes — the "handful of stars" layer.
    /// </summary>
    public struct SparkleSpec
    {
        /// <summary>How many grains to scatter.</summary>
        public int Count;

        /// <summary>When the scatter begins, in seconds.</summary>
        public float StartTime;

        /// <summary>How long the grains are spread over, in seconds.</summary>
        public float Spread;

        /// <summary>Lowest pitch a grain can take, in Hz.</summary>
        public float BaseFrequency;

        /// <summary>How many octaves above <see cref="BaseFrequency"/> grains may reach.</summary>
        public int Octaves;

        /// <summary>Peak amplitude of a single grain.</summary>
        public float Amplitude;

        /// <summary>How far grains spread across the stereo field, 0..1.</summary>
        public float StereoWidth;

        /// <summary>Length of one grain, in seconds.</summary>
        public float GrainDuration;
    }

    /// <summary>Renders voices and sparkles into playable stereo clips.</summary>
    public static class ProceduralAudioSynth
    {
        /// <summary>Sample rate of every generated clip.</summary>
        public const int SampleRate = 44100;

        private const int Channels = 2;

        /// <summary>
        /// Ramp applied to the head and tail of a clip. Without it the buffer
        /// starts and ends on a discontinuity, audible as a dry pop on top of
        /// the intended sound.
        /// </summary>
        private const float EdgeFadeSeconds = 0.003f;

        /// <summary>
        /// Semitone offsets of a major pentatonic scale. Sparkle grains are
        /// quantised to these so a random scatter still lands in key — random
        /// pitches sound like interference, pentatonic ones sound like music.
        /// </summary>
        private static readonly int[] PentatonicSemitones = { 0, 2, 4, 7, 9 };

        /// <summary>
        /// Partial ratios and relative levels per timbre.
        /// Index 0 of each pair is the frequency ratio, index 1 the amplitude.
        /// </summary>
        private static readonly float[][] BellPartials =
        {
            new[] { 1.00f, 1.00f },
            new[] { 2.76f, 0.42f },
            new[] { 5.40f, 0.18f },
            new[] { 8.93f, 0.08f }
        };

        private static readonly float[][] MusicBoxPartials =
        {
            new[] { 1.00f, 1.00f },
            new[] { 2.00f, 0.30f },
            new[] { 3.01f, 0.16f },
            new[] { 4.17f, 0.07f },
            new[] { 5.43f, 0.04f }
        };

        private static readonly float[][] WoodPartials =
        {
            new[] { 1.00f, 1.00f },
            new[] { 3.01f, 0.22f },
            new[] { 6.05f, 0.06f }
        };

        private static readonly float[][] SoftPartials =
        {
            new[] { 1.00f, 1.00f },
            new[] { 2.00f, 0.12f }
        };

        /// <summary>
        /// Renders a complete sound.
        /// </summary>
        /// <param name="name">Debug name shown in the Unity audio profiler.</param>
        /// <param name="totalDuration">
        /// Clip length in seconds. Leave room for the reverb tail — a clip that
        /// ends before its tail does will cut the tail off mid-decay.
        /// </param>
        /// <param name="voices">Struck notes. May be null for a sparkle-only sound.</param>
        /// <param name="sparkles">Grain scatters layered on top. May be null.</param>
        /// <param name="reverbMix">Wet level, 0..1. Around 0.3 suits UI sounds.</param>
        /// <param name="reverbSeconds">Approximate RT60 of the tail.</param>
        /// <param name="seed">Seed for grains and noise, so clips are reproducible.</param>
        public static AudioClip Render(
            string name,
            float totalDuration,
            VoiceSpec[] voices,
            SparkleSpec[] sparkles = null,
            float reverbMix = 0.28f,
            float reverbSeconds = 1.1f,
            int seed = 12345)
        {
            totalDuration = Mathf.Max(0.02f, totalDuration);

            int frameCount = Mathf.CeilToInt(totalDuration * SampleRate);
            float[] left = new float[frameCount];
            float[] right = new float[frameCount];

            System.Random random = new System.Random(seed);

            if (voices != null)
            {
                for (int i = 0; i < voices.Length; i++)
                {
                    RenderVoice(left, right, voices[i], random);
                }
            }

            if (sparkles != null)
            {
                for (int i = 0; i < sparkles.Length; i++)
                {
                    RenderSparkle(left, right, sparkles[i], random);
                }
            }

            if (reverbMix > 0.001f)
            {
                ApplyReverb(left, right, reverbMix, reverbSeconds);
            }

            float[] interleaved = Interleave(left, right);
            Normalise(interleaved);
            ApplyEdgeFades(interleaved, frameCount);

            AudioClip clip = AudioClip.Create(name, frameCount, Channels, SampleRate, false);
            clip.SetData(interleaved, 0);
            return clip;
        }

        // ---------------------------------------------------------------------
        //  Voices
        // ---------------------------------------------------------------------

        private static void RenderVoice(float[] left, float[] right, VoiceSpec voice, System.Random random)
        {
            float[][] partials = GetPartials(voice.Timbre);

            int startFrame = Mathf.Clamp(Mathf.RoundToInt(voice.StartTime * SampleRate), 0, left.Length);
            int voiceFrames = Mathf.RoundToInt(Mathf.Max(0.005f, voice.Duration) * SampleRate);
            int endFrame = Mathf.Min(left.Length, startFrame + voiceFrames);

            GetPanGains(voice.Pan, out float gainLeft, out float gainRight);

            // A short attack ramp. Struck instruments are not instantaneous, and
            // a hard onset is exactly what makes a synthesised tone sound
            // digital.
            int attackFrames = Mathf.Max(1, Mathf.RoundToInt(0.004f * SampleRate));

            for (int frame = startFrame; frame < endFrame; frame++)
            {
                int local = frame - startFrame;
                float seconds = local / (float)SampleRate;

                float sample = 0f;

                for (int p = 0; p < partials.Length; p++)
                {
                    float ratio = partials[p][0];
                    float level = partials[p][1];

                    // Higher partials fade faster, as they do on a real bar or
                    // bell. This is what makes the tone mellow as it rings out
                    // instead of holding one static colour.
                    float partialDecay = voice.Decay * (1f + (ratio - 1f) * 0.55f);
                    float envelope = Mathf.Exp(-seconds * partialDecay);

                    if (envelope < 0.0002f)
                    {
                        continue;
                    }

                    double phase = 2d * Math.PI * voice.Frequency * ratio * seconds;
                    sample += (float)Math.Sin(phase) * level * envelope;
                }

                if (voice.NoiseAmount > 0f)
                {
                    float white = (float)(random.NextDouble() * 2d - 1d);
                    sample += white * voice.NoiseAmount * Mathf.Exp(-seconds * 700f);
                }

                float attack = local < attackFrames ? local / (float)attackFrames : 1f;
                sample *= voice.Amplitude * attack;

                left[frame] += sample * gainLeft;
                right[frame] += sample * gainRight;
            }
        }

        private static float[][] GetPartials(Timbre timbre)
        {
            switch (timbre)
            {
                case Timbre.Bell: return BellPartials;
                case Timbre.Wood: return WoodPartials;
                case Timbre.Soft: return SoftPartials;
                default: return MusicBoxPartials;
            }
        }

        // ---------------------------------------------------------------------
        //  Sparkles
        // ---------------------------------------------------------------------

        private static void RenderSparkle(float[] left, float[] right, SparkleSpec spec, System.Random random)
        {
            int count = Mathf.Max(0, spec.Count);
            int octaves = Mathf.Max(1, spec.Octaves);

            for (int i = 0; i < count; i++)
            {
                // Grains cluster towards the beginning of the spread rather than
                // spacing out evenly: a scatter should feel thrown, not metered.
                float bias = (float)random.NextDouble();
                bias *= bias;

                float offset = spec.StartTime + bias * spec.Spread;

                int semitone = PentatonicSemitones[random.Next(PentatonicSemitones.Length)]
                               + 12 * random.Next(octaves);

                float frequency = spec.BaseFrequency * Mathf.Pow(2f, semitone / 12f);

                // Quieter the higher it goes, so the top octave shimmers rather
                // than stabs.
                float heightFalloff = Mathf.Lerp(1f, 0.45f, semitone / (12f * octaves));

                VoiceSpec grain = new VoiceSpec
                {
                    Frequency = frequency,
                    StartTime = offset,
                    Duration = spec.GrainDuration,
                    Amplitude = spec.Amplitude * heightFalloff * Mathf.Lerp(0.6f, 1f, (float)random.NextDouble()),
                    Decay = 11f,
                    Timbre = Timbre.Bell,
                    Pan = (float)(random.NextDouble() * 2d - 1d) * spec.StereoWidth,
                    NoiseAmount = 0f
                };

                RenderVoice(left, right, grain, random);
            }
        }

        // ---------------------------------------------------------------------
        //  Reverb
        // ---------------------------------------------------------------------

        /// <summary>
        /// A Schroeder reverb: parallel comb filters for density, series
        /// all-pass filters to smear the result so the combs do not ring.
        /// </summary>
        /// <remarks>
        /// The two channels use slightly different delay lengths. That mismatch
        /// is what gives the tail its width — identical channels would collapse
        /// to a mono point in the middle of the image.
        /// </remarks>
        private static void ApplyReverb(float[] left, float[] right, float mix, float rt60Seconds)
        {
            float[] combDelaysLeft = { 0.0297f, 0.0371f, 0.0411f, 0.0437f };
            float[] combDelaysRight = { 0.0307f, 0.0380f, 0.0421f, 0.0448f };
            float[] allPassDelays = { 0.0050f, 0.0017f };

            ProcessChannel(left, combDelaysLeft, allPassDelays, mix, rt60Seconds);
            ProcessChannel(right, combDelaysRight, allPassDelays, mix, rt60Seconds);
        }

        private static void ProcessChannel(
            float[] channel,
            float[] combDelays,
            float[] allPassDelays,
            float mix,
            float rt60Seconds)
        {
            float[] wet = new float[channel.Length];

            for (int c = 0; c < combDelays.Length; c++)
            {
                int delay = Mathf.Max(1, Mathf.RoundToInt(combDelays[c] * SampleRate));

                // Feedback chosen so the comb decays by 60 dB in rt60 seconds.
                float feedback = Mathf.Pow(10f, -3f * combDelays[c] / Mathf.Max(0.05f, rt60Seconds));
                feedback = Mathf.Clamp(feedback, 0f, 0.97f);

                float[] buffer = new float[delay];
                int index = 0;
                float lowPassState = 0f;

                for (int i = 0; i < channel.Length; i++)
                {
                    float delayed = buffer[index];
                    wet[i] += delayed;

                    // One-pole low pass inside the loop: real rooms lose their
                    // highs first, and without this the tail hisses.
                    lowPassState = delayed * 0.72f + lowPassState * 0.28f;

                    buffer[index] = channel[i] + lowPassState * feedback;

                    index++;
                    if (index >= delay)
                    {
                        index = 0;
                    }
                }
            }

            float combScale = 1f / combDelays.Length;
            for (int i = 0; i < wet.Length; i++)
            {
                wet[i] *= combScale;
            }

            for (int a = 0; a < allPassDelays.Length; a++)
            {
                int delay = Mathf.Max(1, Mathf.RoundToInt(allPassDelays[a] * SampleRate));
                const float coefficient = 0.5f;

                float[] buffer = new float[delay];
                int index = 0;

                for (int i = 0; i < wet.Length; i++)
                {
                    float delayed = buffer[index];
                    float input = wet[i];
                    float output = -input * coefficient + delayed;

                    buffer[index] = input + delayed * coefficient;
                    wet[i] = output;

                    index++;
                    if (index >= delay)
                    {
                        index = 0;
                    }
                }
            }

            for (int i = 0; i < channel.Length; i++)
            {
                channel[i] = channel[i] * (1f - mix * 0.5f) + wet[i] * mix;
            }
        }

        // ---------------------------------------------------------------------
        //  Mixdown helpers
        // ---------------------------------------------------------------------

        /// <summary>
        /// Constant-power panning. Linear panning dips in level through the
        /// centre; the sine/cosine pair keeps perceived loudness even as a grain
        /// moves across the field.
        /// </summary>
        private static void GetPanGains(float pan, out float gainLeft, out float gainRight)
        {
            pan = Mathf.Clamp(pan, -1f, 1f);
            float angle = (pan + 1f) * 0.25f * Mathf.PI;

            gainLeft = Mathf.Cos(angle);
            gainRight = Mathf.Sin(angle);
        }

        private static float[] Interleave(float[] left, float[] right)
        {
            float[] interleaved = new float[left.Length * Channels];

            for (int i = 0; i < left.Length; i++)
            {
                interleaved[i * 2] = left[i];
                interleaved[i * 2 + 1] = right[i];
            }

            return interleaved;
        }

        /// <summary>
        /// Scales the buffer so summed layers cannot clip, leaving a little
        /// headroom below full scale.
        /// </summary>
        private static void Normalise(float[] buffer)
        {
            float peak = 0f;
            for (int i = 0; i < buffer.Length; i++)
            {
                float magnitude = Mathf.Abs(buffer[i]);
                if (magnitude > peak)
                {
                    peak = magnitude;
                }
            }

            if (peak <= 0.0001f || peak <= 0.95f)
            {
                return;
            }

            float scale = 0.95f / peak;
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] *= scale;
            }
        }

        private static void ApplyEdgeFades(float[] interleaved, int frameCount)
        {
            int fadeFrames = Mathf.Min(Mathf.RoundToInt(EdgeFadeSeconds * SampleRate), frameCount / 2);
            if (fadeFrames <= 0)
            {
                return;
            }

            for (int i = 0; i < fadeFrames; i++)
            {
                float gain = i / (float)fadeFrames;

                interleaved[i * 2] *= gain;
                interleaved[i * 2 + 1] *= gain;

                int tail = frameCount - 1 - i;
                interleaved[tail * 2] *= gain;
                interleaved[tail * 2 + 1] *= gain;
            }
        }
    }
}
