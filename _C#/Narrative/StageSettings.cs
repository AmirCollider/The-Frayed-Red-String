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

            /// <summary>
            /// The uniform scale Yua's marker sprite is placed at.
            /// </summary>
            /// <remarks>
            /// <para>
            /// The other half of the placement, and the half that was missing.
            /// The anchors said where the two of them stand and nothing said how
            /// big they are, so the size came from a canvas height typed in by
            /// hand — one number, for both characters, that matched neither of
            /// them. Both were drawn about a sixth too large in every act after
            /// the first, and identically large, which threw away the eight
            /// centimetres between them as well.
            /// </para>
            /// <para>
            /// So the scale is written down here beside the anchors, and every
            /// canvas measurement below is derived from it. These are the exact
            /// numbers the marker sprites in Act01.unity carry: x, y and scale
            /// on three lines, which is the whole of what "where does this
            /// character stand" means.
            /// </para>
            /// </remarks>
            public const float YuaScale = 0.48f;

            /// <summary>The uniform scale Haru's marker sprite is placed at.</summary>
            /// <remarks>
            /// Very slightly larger than Yua's, on top of the difference already
            /// baked into the artwork. See <see cref="YuaScale"/>.
            /// </remarks>
            public const float HaruScale = 0.50f;

            /// <summary>
            /// The scale the nine-year-olds are drawn at.
            /// </summary>
            /// <remarks>
            /// <para>
            /// Roughly seven tenths of their adult selves, which is about right
            /// for nine against seventeen and — more to the point — is small
            /// enough to read as a child in silhouette from across a room. The
            /// exact anatomy is the artist's problem; what this number has to
            /// get right is that a player who looks away and back knows
            /// instantly which half of the story they are in.
            /// </para>
            /// <para>
            /// Their x is their adult anchor, unchanged. Act six is the same two
            /// people standing in the same places, which is the whole of what it
            /// is about.
            /// </para>
            /// </remarks>
            public const float YuaChildScale = 0.335f;

            /// <summary>Haru at nine. See <see cref="YuaChildScale"/>.</summary>
            public const float HaruChildScale = 0.350f;

            /// <summary>Orthographic size of every act scene's camera.</summary>
            public const float CameraHalfHeight = 5f;

            /// <summary>
            /// Canvas units per world unit: a 1080-tall canvas over a camera
            /// that sees ten world units.
            /// </summary>
            public const float CanvasPerWorld = 1080f / (2f * CameraHalfHeight);

            /// <summary>
            /// Height of every character sprite's canvas, in pixels.
            /// </summary>
            /// <remarks>
            /// All sixteen poses share a 1200 × 2400 canvas — Yua's carries a
            /// margin at the top so that she comes out eight centimetres
            /// shorter without anything in the engine knowing about it.
            /// </remarks>
            public const float SpritePixelHeight = 2400f;

            /// <summary>Pixels per unit the character art is imported at.</summary>
            /// <remarks>
            /// Unity's default, and nothing in the project changes it. It is
            /// named here because the scale above only means a size in company
            /// with it, and a silent change to the import setting would move
            /// both characters with nothing on screen to say why.
            /// </remarks>
            public const float SpritePixelsPerUnit = 100f;

            /// <summary>A character sprite's height at a scale of one, in world units.</summary>
            public const float SpriteWorldHeight = SpritePixelHeight / SpritePixelsPerUnit;

            /// <summary>The world x a character's object sits at.</summary>
            public static float AnchorXFor(Speaker speaker)
            {
                return CharacterArt.IsHaru(speaker) ? HaruAnchorX : YuaAnchorX;
            }

            /// <summary>The uniform scale a character's object is set to.</summary>
            public static float ScaleFor(Speaker speaker)
            {
                switch (speaker)
                {
                    case Speaker.Haru: return HaruScale;
                    case Speaker.YuaChild: return YuaChildScale;
                    case Speaker.HaruChild: return HaruChildScale;
                    default: return YuaScale;
                }
            }

            /// <summary>
            /// How tall a character is drawn, in canvas units.
            /// </summary>
            /// <remarks>
            /// Taller than the 1080-unit screen on purpose, so a full-body
            /// sprite is cropped at the shin rather than standing in mid-air
            /// with a margin of empty canvas underneath it. Yua comes out at
            /// 1244 and Haru at 1296.
            /// </remarks>
            public static float HeightFor(Speaker speaker)
            {
                return SpriteWorldHeight * ScaleFor(speaker) * CanvasPerWorld;
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
                float height = HeightFor(speaker);

                return new Placement
                {
                    Speaker = speaker,
                    Side = left ? StageSide.Left : StageSide.Right,
                    OffsetX = AnchorXFor(speaker) * CanvasPerWorld,
                    Height = height,
                    FeetY = FeetYFor(height),
                    Flip = false
                };
            }

            /// <summary>
            /// How far a marker's size is from the size the game is designed
            /// around, as a multiple of it.
            /// </summary>
            /// <remarks>
            /// One is exactly right, two is twice as tall as it should be. A
            /// marker left at a scale of one measures 2.08 for Yua and 2.00 for
            /// Haru, which is the single most common way this goes wrong and the
            /// reason the number is worth having.
            /// </remarks>
            public static float SizeErrorAgainstDesign(Speaker speaker, float heightInCanvasUnits)
            {
                float designed = HeightFor(speaker);

                return designed > 0f ? heightInCanvasUnits / designed : 0f;
            }

            /// <summary>
            /// True when a measured placement is close enough to the design to
            /// be believed.
            /// </summary>
            /// <param name="speaker">Who was measured.</param>
            /// <param name="complaint">What is wrong with it, when it is not.</param>
            /// <remarks>
            /// <para>
            /// A wide band on purpose. This is not here to enforce the anchors —
            /// dragging a character somewhere else in a scene is a thing the
            /// stage is built to let you do, and a thirty per cent resize passes
            /// without comment. It is here to catch the one failure that looks
            /// like nothing in the scene view and like a bug in the game: a
            /// marker whose scale has been reset to one, which draws a character
            /// twice their proper size with their head out of frame.
            /// </para>
            /// <para>
            /// Rejecting it and saying so is better than drawing it, because the
            /// fallback — the numbers in this file — is known to be right.
            /// </para>
            /// </remarks>
            public static bool IsPlausible(Speaker speaker, Placement candidate, out string complaint)
            {
                float error = SizeErrorAgainstDesign(speaker, candidate.Height);

                if (error < 0.6f || error > 1.5f)
                {
                    complaint =
                        $"it comes out {error:0.00}× the designed height ({candidate.Height:0} canvas units " +
                        $"against {HeightFor(speaker):0}), which usually means its scale is not " +
                        $"{ScaleFor(speaker):0.00}";

                    return false;
                }

                float offsetDrift = Mathf.Abs(candidate.OffsetX - AnchorXFor(speaker) * CanvasPerWorld);

                if (offsetDrift > 3f * CanvasPerWorld)
                {
                    complaint =
                        $"it stands {offsetDrift / CanvasPerWorld:0.0} world units from the anchor at " +
                        $"x {AnchorXFor(speaker):0.00}, which is off the side of the picture";

                    return false;
                }

                complaint = null;
                return true;
            }
        }

        /// <summary>
        /// The revision of the placement numbers this file understands.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Bumped whenever the built-in placement changes in a way that makes
        /// what is already saved in somebody's asset wrong rather than merely
        /// different. An asset stamped with an older revision is treated as
        /// holding stale numbers: the game reads the built-in placement instead,
        /// and the Editor rewrites the asset and stamps it the next time it
        /// loads.
        /// </para>
        /// <para>
        /// Needed because this asset is where a placement is kept, and fixing
        /// the default in code does not reach a project that already has one.
        /// The alternative was a menu item nobody would know to press, and a bug
        /// that stays fixed only in a fresh checkout.
        /// </para>
        /// <para>
        /// Revision 1 is the first to carry a per-character size: Yua at a scale
        /// of 0.48 and Haru at 0.50, rather than one canvas height for both.
        /// Revision 2 adds the two nine-year-olds act six is played by.
        /// Revision 3 stops scene markers overriding any of it by default.
        /// </para>
        /// </remarks>
        public const int CurrentPlacementRevision = 3;

        [Header("Characters")]
        [Tooltip("One entry per character. Anything not listed uses the built-in default.")]
        [SerializeField] private Placement[] _placements =
        {
            Placement.Default(Speaker.Yua),
            Placement.Default(Speaker.Haru),
            Placement.Default(Speaker.YuaChild),
            Placement.Default(Speaker.HaruChild)
        };

        /// <summary>
        /// Which revision of the placement numbers this asset was written
        /// against.
        /// </summary>
        /// <remarks>
        /// Starts at zero, and has to. Unity gives a field that is absent from
        /// the file its initializer's value, so an asset saved before this field
        /// existed loads with whatever is written here — initialising it to the
        /// current revision would declare every old file up to date and the
        /// migration would never run on the one project it exists for.
        /// </remarks>
        [HideInInspector]
        [SerializeField] private int _placementRevision;

        /// <summary>
        /// Whether a character sprite dropped into a scene overrides these
        /// numbers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Off, and off by default, after the placement broke three separate
        /// times in three separate ways. The idea was good — drag the character
        /// where you want them and the game draws them there — and the practice
        /// was that the placement then lived in two places at once, and the one
        /// nobody was looking at kept winning. A marker at the wrong scale, a
        /// marker somebody nudged, a marker left over from a scene that got
        /// duplicated: each of those is invisible in the scene view and moves
        /// the whole cast in one act.
        /// </para>
        /// <para>
        /// So the asset is the placement now, full stop, and the Story Editor's
        /// Stage tab is where it is judged — it draws the real 16:9 frame with
        /// the real arithmetic, which is a better way to look at it than the
        /// scene view ever was. The markers in Act01 stay where they are as a
        /// reference and are simply not read.
        /// </para>
        /// <para>
        /// Tick this to have them read again. It is here because the feature
        /// works and somebody may want it; it is off because it is the single
        /// most reliable way to break this game.
        /// </para>
        /// </remarks>
        [Header("Characters — advanced")]
        [Tooltip("Let a Yua… or Haru… sprite placed in a scene override the numbers above, for that scene.")]
        public bool ReadPlacementFromScene;

        // ---------------------------------------------------------------------
        //  The veil
        // ---------------------------------------------------------------------

        /// <summary>
        /// Whether the picture is seen through a soft wash of colour.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document's pane of frosted glass: the first four acts are
        /// meant to be very slightly softened and warmed, and act five takes it
        /// away, which is what "the graphics become realistic" means in a game
        /// that is never going to become three-dimensional. The same art, with
        /// nothing in front of it.
        /// </para>
        /// <para>
        /// It lives here rather than in code because it is a number that has to
        /// be looked at to be judged, and the Story Editor's Stage tab draws it
        /// live. Switch it off and the game is sharp from the first frame; act
        /// five's Grade beats then have nothing to remove, which is a legitimate
        /// way to play the whole thing and costs one moment in one act.
        /// </para>
        /// </remarks>
        [Header("Veil")]
        [Tooltip("The soft wash the first four acts are seen through. Act five removes it.")]
        public bool VeilEnabled = true;

        /// <summary>
        /// How strong the veil is at full strength, 0..1.
        /// </summary>
        /// <remarks>
        /// Multiplied by the colour's own alpha, so this is the number to reach
        /// for first. It was fixed in code at full strength and read as the
        /// whole game being slightly out of focus, which is more than the effect
        /// is supposed to be doing.
        /// </remarks>
        [Tooltip("0 is no veil at all. 1 is the full strength of the colour below.")]
        [Range(0f, 1f)] public float VeilStrength = 0.55f;

        /// <summary>The colour of the veil, alpha included.</summary>
        [Tooltip("Warm and pale, matching the dialogue panel. The alpha here is the maximum.")]
        public Color VeilColour = new Color(1f, 0.87f, 0.91f, 0.17f);

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

        /// <summary>
        /// True when this asset was written before the current placement
        /// numbers existed, so what it holds should not be believed.
        /// </summary>
        public bool PlacementsAreStale => _placementRevision < CurrentPlacementRevision;

        /// <summary>The placement for a character.</summary>
        /// <remarks>
        /// An asset from before <see cref="CurrentPlacementRevision"/> is
        /// ignored in favour of the built-in numbers. Reading it would put both
        /// characters back at the wrong size in exactly the acts the fix was for,
        /// and the file is repaired by the Editor on its next load anyway — this
        /// is only what happens in the seconds, or the build, before that.
        /// </remarks>
        public Placement PlacementFor(Speaker speaker)
        {
            if (_placements != null && !PlacementsAreStale)
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
                Placement.Default(Speaker.Haru),
                Placement.Default(Speaker.YuaChild),
                Placement.Default(Speaker.HaruChild)
            };

            _placementRevision = CurrentPlacementRevision;
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

            // Somebody has now looked at this character and decided where they
            // stand, so the file is no longer a leftover from an older idea of
            // the stage and stops being treated as one.
            _placementRevision = CurrentPlacementRevision;

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
