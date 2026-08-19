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

            /// <summary>
            /// Reads a placement off a sprite somebody positioned by hand in a
            /// scene.
            /// </summary>
            /// <param name="marker">The sprite standing where the character should.</param>
            /// <param name="camera">The scene's camera. Must be orthographic.</param>
            /// <param name="measured">The placement, if it could be measured.</param>
            /// <returns>False when there was nothing to measure.</returns>
            /// <remarks>
            /// <para>
            /// Measured from the sprite and the transform rather than from
            /// <c>Renderer.bounds</c>. A renderer that has been switched off is
            /// not in the culling system and does not have to report anything
            /// useful — and switched off is exactly what these markers become
            /// once somebody realises they are only there to be measured.
            /// </para>
            /// <para>
            /// Lives here rather than in the stage so that the Story Editor can
            /// run the same arithmetic and write the answer into this asset,
            /// which is how a placement stops being something every act has to
            /// be told again.
            /// </para>
            /// </remarks>
            public bool TryMeasureFrom(SpriteRenderer marker, Camera camera, out Placement measured)
            {
                measured = this;

                if (marker == null || marker.sprite == null)
                {
                    return false;
                }

                if (camera == null || !camera.orthographic || camera.orthographicSize <= 0f)
                {
                    return false;
                }

                // The canvas is 1080 units tall and the camera sees twice its
                // orthographic size, so this is the whole of the conversion.
                float unitsToCanvas = 1080f / (2f * camera.orthographicSize);

                Transform pose = marker.transform;
                Bounds local = marker.sprite.bounds;

                Vector3 centre = pose.TransformPoint(local.center);
                float height = local.size.y * Mathf.Abs(pose.lossyScale.y);

                Vector3 eye = camera.transform.position;

                measured.OffsetX = (centre.x - eye.x) * unitsToCanvas;
                measured.FeetY = (centre.y - height * 0.5f - (eye.y - camera.orthographicSize)) * unitsToCanvas;
                measured.Height = height * unitsToCanvas;

                return true;
            }

            /// <summary>
            /// Where a character stands, written as the world position their
            /// object sits at in a scene.
            /// </summary>
            /// <remarks>
            /// <para>
            /// These are the numbers the placement is actually judged by, so
            /// they are the numbers kept. Yua stands at world x −4.55 and Haru
            /// at +4.70, both with their centre at y −1.50, against the
            /// orthographic camera of size 5 every act scene uses.
            /// </para>
            /// <para>
            /// Everything below converts them into the canvas measurements the
            /// stage draws with, rather than the other way round. Doing it in
            /// that direction is the whole point: the canvas figures used to be
            /// typed in by hand and drifted from the scene they came from, which
            /// left both characters sitting two thirds of a world unit too high
            /// in every act that had no marker sprites in it — that is, in every
            /// act after the first.
            /// </para>
            /// </remarks>
            public const float YuaAnchorX = -4.55f;

            /// <summary>Haru's world x. See <see cref="YuaAnchorX"/>.</summary>
            public const float HaruAnchorX = 4.70f;

            /// <summary>The world y both characters' centres sit at.</summary>
            public const float AnchorY = -1.50f;

            /// <summary>Orthographic size of every act scene's camera.</summary>
            public const float CameraHalfHeight = 5f;

            /// <summary>
            /// Canvas units per world unit: a 1080-tall canvas over a camera
            /// that sees ten world units.
            /// </summary>
            public const float CanvasPerWorld = 1080f / (2f * CameraHalfHeight);

            /// <summary>
            /// How tall a character is drawn, in canvas units.
            /// </summary>
            /// <remarks>
            /// Taller than the 1080-unit screen on purpose, so a full-body
            /// sprite is cropped at the shin rather than standing in mid-air
            /// with a margin of empty canvas underneath it.
            /// </remarks>
            public const float CharacterHeight = 1500f;

            /// <summary>The world x a character's object sits at.</summary>
            public static float AnchorXFor(Speaker speaker)
            {
                return speaker == Speaker.Haru ? HaruAnchorX : YuaAnchorX;
            }

            /// <summary>
            /// The canvas-space foot height that puts a sprite of
            /// <paramref name="heightInCanvasUnits"/> with its centre on
            /// <see cref="AnchorY"/>.
            /// </summary>
            /// <remarks>
            /// The stage anchors a character by the bottom of their rect, and
            /// the scene anchors them by the middle of their sprite. This is the
            /// one line that reconciles the two, and it is the line that was
            /// missing.
            /// </remarks>
            public static float FeetYFor(float heightInCanvasUnits)
            {
                float centreAboveBottom = (AnchorY + CameraHalfHeight) * CanvasPerWorld;
                return centreAboveBottom - heightInCanvasUnits * 0.5f;
            }

            /// <summary>Where a character stands, unless a scene says otherwise.</summary>
            public static Placement Default(Speaker speaker)
            {
                bool left = CharacterArt.HomeSide(speaker) == StageSide.Left;

                return new Placement
                {
                    Speaker = speaker,
                    Side = left ? StageSide.Left : StageSide.Right,
                    OffsetX = AnchorXFor(speaker) * CanvasPerWorld,
                    Height = CharacterHeight,
                    FeetY = FeetYFor(CharacterHeight),
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
        [Tooltip("Frame the picture during acts. The menus are never framed.")]
        public bool FrameEnabled = true;

        /// <summary>
        /// Thickness of the left and right bars, in canvas units.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The frame was described by an aspect ratio at first — "hold the
        /// picture at 16:10" — and derived the four bar widths from it. That is
        /// the wrong handle to hold it by. A ratio says nothing about how thick
        /// the border looks, which is the only thing anybody judges it on, and
        /// nudging it a hundredth moved the sides by fourteen pixels while the
        /// top and bottom did something else entirely.
        /// </para>
        /// <para>
        /// So the border is two thicknesses now, in the same 1920 × 1080 canvas
        /// units as everything else here. What aspect the picture ends up at is
        /// whatever falls out, which is the right way round: the border is the
        /// thing being designed.
        /// </para>
        /// </remarks>
        [Tooltip("Thickness of the left and right bars, in canvas units. The screen is 1920 wide.")]
        [Range(0f, 400f)] public float FrameBorderX = 149f;

        /// <summary>Thickness of the top and bottom bars, in canvas units.</summary>
        [Tooltip("Thickness of the top and bottom bars, in canvas units. The screen is 1080 tall.")]
        [Range(0f, 300f)] public float FrameBorderY = 69f;

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

        /// <summary>
        /// Puts both characters back on the anchors the game is designed around.
        /// </summary>
        /// <remarks>
        /// The asset on disk keeps whatever was last written into it, so fixing
        /// the built-in default does not reach a project that already has one.
        /// This is how the fix reaches it, and it is a menu item rather than a
        /// migration because overwriting somebody's deliberate placement
        /// silently is the same class of mistake as overwriting their save.
        /// </remarks>
        public void ResetPlacementsToDefault()
        {
            _placements = new[]
            {
                Placement.Default(Speaker.Yua),
                Placement.Default(Speaker.Haru)
            };
        }

        /// <summary>
        /// Records where a character stands, for every act from now on.
        /// </summary>
        /// <remarks>
        /// The point of this asset: a placement is a property of the game, not
        /// of one scene. Writing it once here is what makes a new act a scene
        /// with a camera in it rather than a scene with a camera and two sprites
        /// that have to be dragged to the same place they were dragged to last
        /// time.
        /// </remarks>
        public void SetPlacement(Placement placement)
        {
            if (_placements == null)
            {
                _placements = new Placement[0];
            }

            for (int i = 0; i < _placements.Length; i++)
            {
                if (_placements[i].Speaker == placement.Speaker)
                {
                    _placements[i] = placement;
                    return;
                }
            }

            Placement[] grown = new Placement[_placements.Length + 1];
            System.Array.Copy(_placements, grown, _placements.Length);
            grown[_placements.Length] = placement;

            _placements = grown;
        }
    }
}
