// -----------------------------------------------------------------------------
//  The Frayed Red String
//  AmbientMotionProfile.cs
//
//  Tuning presets for idle motion. The story document asks for a menu that
//  feels alive rather than paused, so nothing on screen is ever perfectly
//  still — but the motion has to stay under the threshold where the player
//  consciously notices it, or the menu reads as unstable instead of breathing.
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Motion
{
    /// <summary>
    /// Describes one idle-motion feel: how much an element scales, drifts and
    /// sways, and how slowly it does so.
    /// </summary>
    [Serializable]
    public struct AmbientMotionProfile
    {
        [Header("Scale pulse")]
        [Tooltip("Peak scale deviation as a fraction, e.g. 0.02 = ±2%.")]
        public float ScaleAmplitude;

        [Tooltip("Per-axis multiplier. (0.5, 1, 1) makes an element swell more vertically, which reads as breathing.")]
        public Vector3 ScaleAxisWeights;

        [Tooltip("Seconds for one full scale cycle.")]
        public float ScalePeriod;

        [Tooltip("Constant scale-up applied on top of the base scale. Full-screen art uses this to keep a safety margin so drift never exposes an edge.")]
        public float ConstantScale;

        [Tooltip("Remaps the scale wave from -1..1 to 0..1, so the element only ever grows. Required for stretched backgrounds.")]
        public bool ScaleGrowsOnly;

        [Header("Positional drift")]
        [Tooltip("Peak drift, authored in UI units. World-space transforms convert it to world units automatically.")]
        public Vector2 DriftAmplitude;

        [Tooltip("Seconds for one full drift cycle.")]
        public float DriftPeriod;

        [Tooltip("Remaps the drift wave from -1..1 to 0..1, so the element only ever moves in the amplitude's own direction. Use a negative amplitude to pick the other way.")]
        public bool DriftOneDirection;

        [Header("Rotational sway")]
        [Tooltip("Peak rotation around Z, in degrees.")]
        public float SwayDegrees;

        [Tooltip("Seconds for one full sway cycle.")]
        public float SwayPeriod;

        /// <summary>True when this profile would produce no visible motion.</summary>
        public bool IsSilent =>
            Mathf.Approximately(ScaleAmplitude, 0f)
            && DriftAmplitude.sqrMagnitude <= Mathf.Epsilon
            && Mathf.Approximately(SwayDegrees, 0f);

        // ---------------------------------------------------------------------
        //  Presets
        // ---------------------------------------------------------------------

        /// <summary>
        /// Full-screen artwork: a very slow Ken Burns push.
        /// </summary>
        /// <remarks>
        /// <see cref="ConstantScale"/> is what makes this safe. A stretched
        /// background scaled to exactly 1.0 has no spare pixels, so any drift
        /// would pull a transparent edge into frame. Holding it 3% oversized
        /// buys roughly 28 horizontal and 16 vertical pixels of margin at
        /// 1920×1080 — comfortably more than the drift ever uses.
        /// </remarks>
        public static AmbientMotionProfile Background => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.012f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 17.0f,
            ConstantScale = 1.03f,
            ScaleGrowsOnly = true,
            DriftAmplitude = new Vector2(5f, 3f),
            DriftPeriod = 23.0f,
            SwayDegrees = 0f,
            SwayPeriod = 1f
        };

        /// <summary>
        /// Standing characters: a resting respiratory rhythm of roughly
        /// fourteen breaths per minute, weighted towards the vertical axis.
        /// </summary>
        /// <remarks>
        /// The menu characters are a bust that is cropped exactly at the bottom
        /// of the screen — there is no more artwork below the cut. So this
        /// profile may never shrink the sprite or lift it: either would pull the
        /// cut edge up into frame and reveal that the figures have no legs.
        /// Hence grow-only scale, no drift and no sway. The breath still reads,
        /// because breathing is a swell rather than a wobble.
        /// </remarks>
        public static AmbientMotionProfile Character => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.012f,
            ScaleAxisWeights = new Vector3(0.5f, 1f, 1f),
            ScalePeriod = 4.3f,
            ConstantScale = 1.004f,
            ScaleGrowsOnly = true,
            DriftAmplitude = Vector2.zero,
            DriftPeriod = 4.3f,
            DriftOneDirection = false,
            SwayDegrees = 0f,
            SwayPeriod = 7.1f
        };

        /// <summary>
        /// Artwork that fills its frame exactly, with no margin to spare.
        /// </summary>
        /// <remarks>
        /// Same constraint as <see cref="Character"/>, applied to panel
        /// backdrops: the load panel's backing image is sized to its frame, so
        /// any shrink or slide exposes the edge behind it. Grow-only, and
        /// nothing else.
        /// </remarks>
        public static AmbientMotionProfile FittedArt => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.008f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 7.5f,
            ConstantScale = 1.003f,
            ScaleGrowsOnly = true,
            DriftAmplitude = Vector2.zero,
            DriftPeriod = 8f,
            DriftOneDirection = false,
            SwayDegrees = 0f,
            SwayPeriod = 9f
        };

        /// <summary>Logos and title art: a light, weightless float.</summary>
        public static AmbientMotionProfile Title => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.014f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 4.6f,
            ConstantScale = 1f,
            ScaleGrowsOnly = false,
            DriftAmplitude = new Vector2(0f, 6f),
            DriftPeriod = 5.2f,
            SwayDegrees = 0.7f,
            SwayPeriod = 6.8f
        };

        /// <summary>Interactive controls: a soft, inviting pulse.</summary>
        public static AmbientMotionProfile Button => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.018f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 3.1f,
            ConstantScale = 1f,
            ScaleGrowsOnly = false,
            DriftAmplitude = new Vector2(0f, 2.5f),
            DriftPeriod = 3.6f,
            SwayDegrees = 0.25f,
            SwayPeriod = 5.0f
        };

        /// <summary>Cards, panels backdrops and save slots: a slow hover.</summary>
        public static AmbientMotionProfile Card => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.010f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 6.0f,
            ConstantScale = 1f,
            ScaleGrowsOnly = false,
            DriftAmplitude = new Vector2(1.5f, 4f),
            DriftPeriod = 5.6f,
            SwayDegrees = 0.5f,
            SwayPeriod = 7.4f
        };

        /// <summary>
        /// Body copy: barely perceptible. Text must never wobble enough to make
        /// reading uncomfortable.
        /// </summary>
        public static AmbientMotionProfile Text => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.005f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 5.4f,
            ConstantScale = 1f,
            ScaleGrowsOnly = false,
            DriftAmplitude = new Vector2(0f, 1.5f),
            DriftPeriod = 6.2f,
            SwayDegrees = 0f,
            SwayPeriod = 1f
        };

        /// <summary>
        /// The "press Enter" prompt: a deliberate heartbeat, the one element on
        /// screen that is meant to be noticed.
        /// </summary>
        public static AmbientMotionProfile Prompt => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.055f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 1.15f,
            ConstantScale = 1f,
            ScaleGrowsOnly = false,
            DriftAmplitude = new Vector2(0f, 5f),
            DriftPeriod = 2.3f,
            SwayDegrees = 0f,
            SwayPeriod = 1f
        };

        /// <summary>Container panels: almost still, just enough to avoid deadness.</summary>
        public static AmbientMotionProfile Panel => new AmbientMotionProfile
        {
            ScaleAmplitude = 0.006f,
            ScaleAxisWeights = Vector3.one,
            ScalePeriod = 7.0f,
            ConstantScale = 1f,
            ScaleGrowsOnly = false,
            DriftAmplitude = new Vector2(0f, 2f),
            DriftPeriod = 8.0f,
            SwayDegrees = 0.15f,
            SwayPeriod = 9.0f
        };
    }
}
