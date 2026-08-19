// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StageSettings.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// Where the characters stand, and how the picture is framed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These were constants in the code until it turned out that constants
    /// chosen without the art in front of you put both characters in mid-air.
    /// Numbers that have to be looked at to be judged belong in an asset, next
    /// to a preview of what they do — which is what the Story Editor's Stage tab
    /// is.
    /// </para>
    /// <para>
    /// All measurements are in the canvas' 1920 × 1080 reference space, so they
    /// mean the same thing on every monitor.
    /// </para>
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/Stage Settings",
        order = 12)]
    public sealed class StageSettings : ScriptableObject
    {
        /// <summary>File name of the asset.</summary>
        public const string ResourceName = "StageSettings";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        /// <summary>Where one character stands.</summary>
        [Serializable]
        public struct Placement
        {
            public Speaker Speaker;

            [Tooltip("Which side of the screen they stand on.")]
            public StageSide Side;

            [Tooltip("Distance from the centre line, in canvas units. Larger pushes them outward.")]
            public float OffsetX;

            [Tooltip("How tall the sprite is drawn, in canvas units. The screen is 1080 tall.")]
            public float Height;

            [Tooltip("Height of their feet above the bottom edge. Negative crops the feet off.")]
            public float FeetY;

            [Tooltip("Mirror the sprite, so a character drawn facing one way can stand on either side.")]
            public bool Flip;

            /// <summary>A sensible starting point for a full-body sprite.</summary>
            public static Placement Default(Speaker speaker)
            {
                bool left = CharacterArt.HomeSide(speaker) == StageSide.Left;

                return new Placement
                {
                    Speaker = speaker,
                    Side = left ? StageSide.Left : StageSide.Right,
                    OffsetX = left ? -380f : 380f,

                    // Taller than the screen and sunk below it, so the figure is
                    // cropped at the shin rather than floating with its whole
                    // body and a margin of empty canvas visible.
                    Height = 1500f,
                    FeetY = -300f,
                    Flip = false
                };
            }
        }

        [Header("Characters")]
        [Tooltip("One entry per character. Anything not listed uses the built-in default.")]
        [SerializeField] private Placement[] _placements =
        {
            Placement.Default(Speaker.Yua),
            Placement.Default(Speaker.Haru)
        };

        [Header("Focus")]
        [Tooltip("How much the character who is not speaking is dimmed. 1 is no dimming.")]
        [Range(0.2f, 1f)] public float InactiveTint = 0.62f;

        [Tooltip("How much the character who is not speaking shrinks back.")]
        [Range(0.9f, 1f)] public float InactiveScale = 0.972f;

        [Tooltip("How far a character slides in from when they arrive.")]
        public float EntranceSlide = 70f;

        [Header("Frame")]
        [Tooltip("Letterbox the picture during acts. The menus are never framed.")]
        public bool FrameEnabled = true;

        [Tooltip("The aspect the picture is cropped to while the frame is closed. 1.333 is 4:3.")]
        public float FrameAspect = 4f / 3f;

        [Tooltip("Colour of the bars.")]
        public Color FrameColour = Color.black;

        [Tooltip("How long the frame takes to open when the story opens it.")]
        public float FrameOpenSeconds = 2.4f;

        private static StageSettings _cached;
        private static bool _lookupAttempted;

        /// <summary>Loads the asset, falling back to built-in defaults.</summary>
        /// <remarks>
        /// A missing asset is not an error. The defaults are the same ones the
        /// asset is created with, so a project without one behaves identically
        /// until somebody wants to change something.
        /// </remarks>
        public static StageSettings Load()
        {
            if (_cached != null)
            {
                return _cached;
            }

            if (!_lookupAttempted)
            {
                _lookupAttempted = true;
                _cached = Resources.Load<StageSettings>(ResourcePath);
            }

            if (_cached == null)
            {
                _cached = CreateInstance<StageSettings>();
                _cached.name = ResourceName + " (defaults)";
            }

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
        }

        /// <summary>The placement for a character.</summary>
        public Placement PlacementFor(Speaker speaker)
        {
            if (_placements != null)
            {
                for (int i = 0; i < _placements.Length; i++)
                {
                    if (_placements[i].Speaker == speaker)
                    {
                        return _placements[i];
                    }
                }
            }

            return Placement.Default(speaker);
        }

        /// <summary>Every placement, for the editor.</summary>
        public Placement[] Placements => _placements;
    }
}
