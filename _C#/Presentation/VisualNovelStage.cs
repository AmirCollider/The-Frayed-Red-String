// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VisualNovelStage.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Core;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// The world the story is played against: one background and two character
    /// positions, with the transitions between them.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Built on top of whatever the scene already contains. Act01.unity ships
    /// with a <c>GameBackGrundImage</c> inside a background canvas; the stage
    /// adopts it as its first background layer and creates the second one
    /// itself, because a crossfade needs two.
    /// </para>
    /// <para>
    /// Every transform this class animates goes through
    /// <see cref="AmbientMotion"/>'s offset and scale channels rather than being
    /// written directly, so a character can walk on, breathe, and lean forward
    /// to speak all at once without the three effects overwriting one another.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class VisualNovelStage : MonoBehaviour
    {
        /// <summary>Native aspect of every character sprite: 1200 × 2400.</summary>
        private const float CharacterAspect = 0.5f;

        /// <summary>Scale the incoming background is pushed in from.</summary>
        private const float BackgroundPushFrom = 1.035f;

        /// <summary>How far a character rises as they arrive, in canvas units.</summary>
        private const float EntranceRise = 26f;

        /// <summary>One character position on the stage.</summary>
        private sealed class Slot
        {
            public Image Image;
            public AmbientMotion Motion;
            public Speaker Occupant;
            public Portrait Pose;
            public bool Present;
            public TweenHandle Fade;
            public TweenHandle Focus;
            public float SlideFrom;
            public StageSettings.Placement Placement;
        }

        private StageSpriteLibrary _library;
        private StageSettings _settings;
        private RectTransform _characterRoot;

        private Image _backgroundFront;
        private Image _backgroundBack;
        private AmbientMotion _frontMotion;
        private AmbientMotion _backMotion;
        private TweenHandle _backgroundFade;

        private Slot _left;
        private Slot _right;

        /// <summary>The background currently displayed, by asset name.</summary>
        public string CurrentBackground { get; private set; }

        /// <summary>
        /// Prepares the stage.
        /// </summary>
        /// <param name="authoredBackground">
        /// The background image already in the scene, adopted as the first
        /// layer. May be null, in which case the stage creates both layers.
        /// </param>
        /// <param name="backgroundParent">
        /// Canvas the scenery is drawn into — the lower of the two, so the
        /// characters are always in front of it.
        /// </param>
        /// <param name="characterParent">Canvas the characters are drawn into.</param>
        public void Initialize(
            Image authoredBackground,
            RectTransform backgroundParent,
            RectTransform characterParent)
        {
            _library = StageSpriteLibrary.Load();
            _settings = StageSettings.Load();

            SetUpBackgrounds(authoredBackground, backgroundParent);
            SetUpCharacters(characterParent);
        }

        // ---------------------------------------------------------------------
        //  Backgrounds
        // ---------------------------------------------------------------------

        /// <summary>
        /// Crossfades to a new background.
        /// </summary>
        /// <param name="backgroundName">Asset name, from <see cref="Backgrounds"/>.</param>
        /// <param name="duration">Length of the crossfade.</param>
        /// <param name="onComplete">Invoked once the incoming layer is fully opaque.</param>
        public void ShowBackground(string backgroundName, float duration, Action onComplete = null)
        {
            if (string.Equals(CurrentBackground, backgroundName, StringComparison.Ordinal))
            {
                onComplete?.Invoke();
                return;
            }

            Sprite sprite = _library != null ? _library.FindBackground(backgroundName) : null;

            if (sprite == null)
            {
                // A place the art has not been drawn for yet gets a stand-in
                // generated from its own name, so a scene change reads as a
                // scene change and an unfinished act can be played through and
                // judged. See ProceduralBackgrounds.
                sprite = ProceduralBackgrounds.For(backgroundName);
            }

            if (sprite == null)
            {
                // Nothing to show and nothing to invent from — an empty name.
                // Keeping the previous picture reads as "this scene did not
                // change", which is better than cutting to black.
                onComplete?.Invoke();
                return;
            }

            CurrentBackground = backgroundName;

            // The save cards show where a save was taken, and this is the only
            // place that knows.
            StorySession.BackgroundName = backgroundName;

            _backgroundFade.CancelIfRunning();

            _backgroundBack.sprite = sprite;
            _backgroundBack.SetAlpha(0f);

            Image incoming = _backgroundBack;
            Image outgoing = _backgroundFront;
            AmbientMotion incomingMotion = _backMotion;

            if (duration <= 0f)
            {
                incoming.SetAlpha(1f);
                outgoing.SetAlpha(0f);

                if (incomingMotion != null)
                {
                    incomingMotion.ExtraScale = Vector3.one;
                }

                SwapBackgroundLayers();
                onComplete?.Invoke();
                return;
            }

            if (incomingMotion != null)
            {
                incomingMotion.ExtraScale = new Vector3(BackgroundPushFrom, BackgroundPushFrom, 1f);
            }

            _backgroundFade = TweenRunner.Play(
                duration,
                t =>
                {
                    incoming.SetAlpha(t);

                    // The outgoing layer is left fully opaque underneath rather
                    // than faded out in step. Cross-dissolving both at once dips
                    // the combined opacity in the middle and shows the black
                    // canvas through the seam.

                    // The new place also settles into frame rather than simply
                    // appearing in it. A crossfade alone tells the player the
                    // picture changed; a picture that is still moving as it
                    // arrives tells them they moved.
                    if (incomingMotion != null)
                    {
                        float push = Mathf.LerpUnclamped(BackgroundPushFrom, 1f, t);
                        incomingMotion.ExtraScale = new Vector3(push, push, 1f);
                    }
                },
                EaseType.InOutSine,
                0f,
                () =>
                {
                    incoming.SetAlpha(1f);
                    outgoing.SetAlpha(0f);

                    if (incomingMotion != null)
                    {
                        incomingMotion.ExtraScale = Vector3.one;
                    }

                    SwapBackgroundLayers();
                    onComplete?.Invoke();
                },
                this,
                true,
                true);
        }

        private void SwapBackgroundLayers()
        {
            (_backgroundFront, _backgroundBack) = (_backgroundBack, _backgroundFront);
            (_frontMotion, _backMotion) = (_backMotion, _frontMotion);

            // Whichever layer is now showing has to be the one drawn on top, or
            // the next crossfade will happen behind the old picture.
            _backgroundFront.transform.SetAsLastSibling();
        }

        private void SetUpBackgrounds(Image authored, RectTransform backgroundParent)
        {
            RectTransform parent = authored != null
                ? authored.transform.parent as RectTransform
                : backgroundParent;

            if (authored != null)
            {
                _backgroundFront = authored;
                StretchFull(_backgroundFront.rectTransform);

                // The scene's own image already has idle motion from the installer
                // sweep, but its captured rest pose predates the stretch above.
                //
                // Safe to re-capture only because Initialize runs from a scene
                // controller's Start, which is before the first LateUpdate of
                // the frame: the transform still holds the authored pose rather
                // than an authored pose with a frame of drift baked on top.
                AmbientMotion motion = _backgroundFront.GetComponent<AmbientMotion>();
                if (motion != null)
                {
                    motion.RecaptureBase();
                }
            }
            else
            {
                _backgroundFront = CreateBackgroundLayer(parent, "StageBackgroundA");
            }

            _backgroundBack = CreateBackgroundLayer(parent, "StageBackgroundB");
            _backgroundBack.SetAlpha(0f);

            _frontMotion = AmbientMotion.GetOrAdd(_backgroundFront.gameObject);
            _backMotion = AmbientMotion.GetOrAdd(_backgroundBack.gameObject);

            _backgroundFront.transform.SetAsFirstSibling();
            _backgroundBack.transform.SetSiblingIndex(_backgroundFront.transform.GetSiblingIndex() + 1);
        }

        private static Image CreateBackgroundLayer(RectTransform parent, string layerName)
        {
            GameObject host = new GameObject(layerName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            Image image = host.AddComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = false;
            image.color = Color.white;

            StretchFull(image.rectTransform);

            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Background, UnityUtility.StableHash01(layerName));

            return image;
        }

        // ---------------------------------------------------------------------
        //  Characters
        // ---------------------------------------------------------------------

        /// <summary>
        /// Brings a character on stage, or changes the expression of one who is
        /// already there.
        /// </summary>
        public void ShowCharacter(Speaker speaker, Portrait portrait, float duration)
        {
            Slot slot = SlotFor(speaker);
            if (slot == null || portrait == Portrait.Unchanged)
            {
                return;
            }

            Sprite sprite = _library != null ? _library.FindCharacter(speaker, portrait) : null;

            if (sprite == null)
            {
                // Art that has not been drawn yet — act six's two nine-year-olds,
                // today — gets a silhouette, so the act can be played through and
                // its staging and pacing judged before anybody picks up a pen.
                sprite = ProceduralBackgrounds.SilhouetteFor(
                    CharacterArt.SpriteName(speaker, portrait), CharacterArt.IsYua(speaker));
            }

            if (sprite == null)
            {
                return;
            }

            bool entering = !slot.Present || slot.Occupant != speaker;

            // A nine-year-old is not the same size as their seventeen-year-old
            // self, and the slot was built for whichever one the act happened to
            // ask for first. Re-reading the placement here is what lets one
            // stage position hold both, and costs nothing on the ninety-nine
            // beats out of a hundred where the age has not changed.
            if (slot.Placement.Speaker != speaker)
            {
                ApplyPlacement(slot, ResolvePlacement(speaker));
            }

            slot.Image.sprite = sprite;
            slot.Occupant = speaker;
            slot.Pose = portrait;

            ResizeToSprite(slot, sprite);

            if (!entering)
            {
                // An expression change on someone already standing there is a
                // cut, not an entrance. Fading it would read as the character
                // leaving and a different one arriving.
                return;
            }

            slot.Present = true;
            slot.Fade.CancelIfRunning();

            slot.Image.SetAlpha(0f);
            slot.Image.enabled = true;

            float from = slot.SlideFrom;

            slot.Fade = TweenRunner.Play(
                duration,
                t =>
                {
                    if (slot.Image == null)
                    {
                        return;
                    }

                    slot.Image.SetAlpha(t);

                    if (slot.Motion != null)
                    {
                        // Sideways and upward at once: a character who only
                        // slides in reads as a sprite being dragged onto the
                        // screen, where the same slide with a little lift under
                        // it reads as somebody stepping into the shot.
                        slot.Motion.ExtraOffset = new Vector3(
                            Mathf.LerpUnclamped(from, 0f, t),
                            Mathf.LerpUnclamped(-EntranceRise, 0f, t),
                            0f);
                    }
                },
                EaseType.OutCubic,
                0f,
                () =>
                {
                    if (slot.Image != null)
                    {
                        slot.Image.SetAlpha(1f);
                    }

                    if (slot.Motion != null)
                    {
                        slot.Motion.ExtraOffset = Vector3.zero;
                    }
                },
                this,
                true,
                true);
        }

        /// <summary>Takes a character off stage.</summary>
        public void HideCharacter(Speaker speaker, float duration)
        {
            Slot slot = SlotFor(speaker);
            if (slot == null || !slot.Present)
            {
                return;
            }

            slot.Present = false;
            slot.Fade.CancelIfRunning();

            float to = slot.SlideFrom;
            float fromAlpha = slot.Image != null ? slot.Image.color.a : 0f;

            slot.Fade = TweenRunner.Play(
                duration,
                t =>
                {
                    if (slot.Image == null)
                    {
                        return;
                    }

                    slot.Image.SetAlpha(Mathf.LerpUnclamped(fromAlpha, 0f, t));

                    if (slot.Motion != null)
                    {
                        slot.Motion.ExtraOffset = new Vector3(
                            Mathf.LerpUnclamped(0f, to, t),
                            Mathf.LerpUnclamped(0f, -EntranceRise * 0.5f, t),
                            0f);
                    }
                },
                EaseType.InCubic,
                0f,
                () =>
                {
                    if (slot.Image != null)
                    {
                        slot.Image.SetAlpha(0f);
                        slot.Image.enabled = false;
                    }

                    if (slot.Motion != null)
                    {
                        slot.Motion.ExtraOffset = Vector3.zero;
                    }
                },
                this,
                true,
                true);
        }

        /// <summary>Clears the stage.</summary>
        public void HideAll(float duration)
        {
            HideCharacter(Speaker.Yua, duration);
            HideCharacter(Speaker.Haru, duration);
        }

        /// <summary>
        /// Lights whoever is talking and steps everyone else back.
        /// </summary>
        /// <remarks>
        /// The two cues — brightness and a very small scale change — are both
        /// under the threshold of conscious notice on their own. Together they
        /// answer "who said that?" before the player has finished reading the
        /// name plate, which is the whole job.
        /// </remarks>
        public void SetFocus(Speaker speaker, float duration)
        {
            ApplyFocus(_left, _left.Occupant == speaker && _left.Present, duration);
            ApplyFocus(_right, _right.Occupant == speaker && _right.Present, duration);
        }

        /// <summary>
        /// Lights everybody on stage, because nobody is talking.
        /// </summary>
        /// <remarks>
        /// <see cref="SetFocus"/> cannot say this: it lights the slot holding a
        /// given speaker and steps the other one back, and there is no speaker
        /// that means "both". Passing it <see cref="Speaker.Narrator"/> dims the
        /// pair of them, which is right under a paragraph of description and
        /// wrong on a held picture — a wordless scene is nobody's line, and
        /// leaving whoever spoke last lit through twenty seconds of it makes the
        /// other one read as having wandered out of the shot.
        /// </remarks>
        public void LightEveryone(float duration)
        {
            ApplyFocus(_left, _left != null && _left.Present, duration);
            ApplyFocus(_right, _right != null && _right.Present, duration);
        }

        private void ApplyFocus(Slot slot, bool active, float duration)
        {
            if (slot?.Image == null)
            {
                return;
            }

            float targetTint = active ? 1f : _settings.InactiveTint;
            float targetScale = active ? 1f : _settings.InactiveScale;

            float fromTint = slot.Image.color.r;
            float fromScale = slot.Motion != null ? slot.Motion.ExtraScale.x : 1f;

            if (Mathf.Approximately(fromTint, targetTint) && Mathf.Approximately(fromScale, targetScale))
            {
                return;
            }

            slot.Focus.CancelIfRunning();

            slot.Focus = TweenRunner.Play(
                duration,
                t =>
                {
                    if (slot.Image == null)
                    {
                        return;
                    }

                    float tint = Mathf.LerpUnclamped(fromTint, targetTint, t);

                    // Alpha is owned by the entrance fade; only the colour
                    // channels belong to focus.
                    Color colour = slot.Image.color;
                    slot.Image.color = new Color(tint, tint, tint, colour.a);

                    if (slot.Motion != null)
                    {
                        float scale = Mathf.LerpUnclamped(fromScale, targetScale, t);
                        slot.Motion.ExtraScale = new Vector3(scale, scale, 1f);
                    }
                },
                EaseType.OutCubic,
                0f,
                null,
                this,
                true,
                true);
        }

        /// <summary>
        /// The stage position a character occupies.
        /// </summary>
        /// <remarks>
        /// Matched on the placement each slot was built from rather than on the
        /// speaker, so putting both characters on the same side in the Stage
        /// settings is merely a strange-looking scene and not a crash.
        /// </remarks>
        private Slot SlotFor(Speaker speaker)
        {
            if (_left != null && SamePerson(_left.Placement.Speaker, speaker))
            {
                return _left;
            }

            return _right != null && SamePerson(_right.Placement.Speaker, speaker) ? _right : null;
        }

        /// <summary>
        /// True when two speakers are the same person, whatever age they are.
        /// </summary>
        /// <remarks>
        /// Act six is Yua and Haru at nine, and they stand exactly where their
        /// older selves stand — the whole point of the flashback is that it is
        /// the same two people in the same places. So the child occupies the
        /// adult's slot rather than needing one of its own, and the stage stays
        /// two positions wide.
        /// </remarks>
        private static bool SamePerson(Speaker a, Speaker b)
        {
            if (a == b)
            {
                return true;
            }

            return (CharacterArt.IsYua(a) && CharacterArt.IsYua(b))
                   || (CharacterArt.IsHaru(a) && CharacterArt.IsHaru(b));
        }

        private void SetUpCharacters(RectTransform parent)
        {
            GameObject root = new GameObject("StageCharacters", typeof(RectTransform));
            root.transform.SetParent(parent, false);

            _characterRoot = (RectTransform)root.transform;
            StretchFull(_characterRoot);

            _left = CreateSlot("StageCharacterLeft", ResolvePlacement(Speaker.Yua));
            _right = CreateSlot("StageCharacterRight", ResolvePlacement(Speaker.Haru));
        }

        // ---------------------------------------------------------------------
        //  Where the characters stand
        // ---------------------------------------------------------------------

        /// <summary>
        /// Where a character stands: taken from the scene when the scene says,
        /// and from the stage settings when it does not.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Act01.unity has a <c>YuaNeutralGentleSmile</c> and a
        /// <c>HaruNeutralGentleSmile</c> placed in it by hand, at the positions
        /// the two characters are meant to occupy. Those objects were being
        /// ignored: the stage built its own pair from numbers in an asset, and
        /// the two disagreed, so the placement that had been chosen by looking
        /// at the screen lost to a placement nobody had looked at.
        /// </para>
        /// <para>
        /// So the authored object is read instead — its position, and its size —
        /// and then switched off, because it has done its job. Dragging the
        /// sprite in the scene view is now how a character is placed, and what
        /// the scene view shows is what the game draws. A scene with no
        /// character objects in it falls back to the settings asset exactly as
        /// before.
        /// </para>
        /// <para>
        /// The marker does not have to be switched on. Once it is clear that it
        /// is only there to be measured, the natural thing is to disable it and
        /// leave it in the scene as a guide, and that keeps working.
        /// </para>
        /// <para>
        /// Only orthographic cameras are converted. The arithmetic below is
        /// specific to one, and a perspective camera would need the distance to
        /// each sprite as well — which a visual novel does not have and does not
        /// want.
        /// </para>
        /// </remarks>
        private StageSettings.Placement ResolvePlacement(Speaker speaker)
        {
            StageSettings.Placement placement = _settings.PlacementFor(speaker);

            SpriteRenderer authored = FindAuthoredAnchor(speaker);
            if (authored == null)
            {
                return placement;
            }

            // The marker is still switched off so it cannot draw a second,
            // permanently neutral copy of the character behind the real one —
            // that part was always right and is nothing to do with whether its
            // position is believed.
            if (authored.gameObject.activeSelf)
            {
                authored.gameObject.SetActive(false);
            }

            if (!_settings.ReadPlacementFromScene)
            {
                // The asset is the placement. See StageSettings.ReadPlacementFromScene
                // for why this is the default.
                return placement;
            }

            if (!placement.TryMeasureFrom(authored, Camera.main, out StageSettings.Placement measured))
            {
                Debug.LogWarning(
                    $"[Stage] '{authored.name}' is in this scene but could not be measured — it needs a " +
                    "sprite and an orthographic main camera — so the Stage settings are used instead.");

                return placement;
            }

            if (!StageSettings.Placement.IsPlausible(speaker, measured, out string complaint))
            {
                Debug.LogWarning(
                    $"[Stage] '{authored.name}' was ignored because {complaint}. {speaker} is drawn on the " +
                    $"anchor instead — x {StageSettings.Placement.AnchorXFor(speaker):0.00}, " +
                    $"y {StageSettings.Placement.AnchorY:0.00}, scale " +
                    $"{StageSettings.Placement.ScaleFor(speaker):0.00}. Put the marker back on those three " +
                    "numbers, or run The Frayed Red String ▸ Reset Character Placement, and it will be read again.");

                return placement;
            }

            return measured;
        }

        /// <summary>
        /// The sprite in this scene that marks where a character stands, if
        /// there is one.
        /// </summary>
        /// <remarks>
        /// Matched on the name starting with the character's own — every pose
        /// file is <c>Yua…</c> or <c>Haru…</c>, so whichever expression happened
        /// to be dropped into the scene is found without the scene having to use
        /// one particular one.
        /// </remarks>
        public static SpriteRenderer FindAuthoredAnchor(Speaker speaker)
        {
            string prefix = speaker == Speaker.Haru ? "Haru" : speaker == Speaker.Yua ? "Yua" : null;

            if (prefix == null)
            {
                return null;
            }

            UnityEngine.SceneManagement.Scene scene =
                UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (!scene.IsValid())
            {
                return null;
            }

            System.Collections.Generic.List<GameObject> roots =
                new System.Collections.Generic.List<GameObject>(scene.rootCount);

            scene.GetRootGameObjects(roots);

            for (int i = 0; i < roots.Count; i++)
            {
                SpriteRenderer[] candidates = roots[i].GetComponentsInChildren<SpriteRenderer>(true);

                for (int j = 0; j < candidates.Length; j++)
                {
                    if (candidates[j].name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return candidates[j];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Builds one character position from its configured placement.
        /// </summary>
        /// <remarks>
        /// The rect is anchored to the bottom edge with its pivot at the bottom,
        /// so <c>FeetY</c> means exactly what it says: where the bottom of the
        /// sprite sits relative to the bottom of the screen. A negative value
        /// pushes the figure down past the edge, which is how a full-body sprite
        /// is cropped at the shin instead of standing in mid-air with a margin
        /// of empty canvas underneath it.
        /// </remarks>
        private Slot CreateSlot(string slotName, StageSettings.Placement placement)
        {
            GameObject host = new GameObject(slotName, typeof(RectTransform));
            host.transform.SetParent(_characterRoot, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 0f);
            rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);

            Image image = host.AddComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = true;
            image.color = new Color(1f, 1f, 1f, 0f);
            image.enabled = false;

            Slot slot = new Slot
            {
                Image = image,
                Occupant = Speaker.Narrator,
                Pose = Portrait.Unchanged,
                Present = false
            };

            // Placed BEFORE the motion component exists, and that order is the
            // whole of this method.
            //
            // AmbientMotion reads the transform as its rest pose the moment it
            // is enabled, and then writes that pose back every LateUpdate with
            // the breathing added on top. Adding it first meant it captured a
            // rect that had not been positioned yet — anchoredPosition (0, 0) —
            // and then dragged the character back to the bottom centre of the
            // screen on the very next frame, at the full 1244-unit height,
            // every act, for both of them at once. The placement asset was
            // correct the entire time and was being overwritten sixty times a
            // second.
            ApplyPlacement(slot, placement);

            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Character, UnityUtility.StableHash01(slotName));

            slot.Motion = motion;

            return slot;
        }

        /// <summary>
        /// Moves and sizes one stage position to a placement.
        /// </summary>
        /// <remarks>
        /// Split out of <see cref="CreateSlot"/> so a slot can be re-placed
        /// after it exists, which is what an act with two ages of the same
        /// character needs. Deliberately does not touch the sprite, the alpha or
        /// anything the entrance animation owns.
        /// </remarks>
        private void ApplyPlacement(Slot slot, StageSettings.Placement placement)
        {
            slot.Placement = placement;
            slot.SlideFrom = _settings.EntranceSlide * (placement.Side == StageSide.Left ? -1f : 1f);

            if (slot.Image == null)
            {
                return;
            }

            RectTransform rect = slot.Image.rectTransform;
            rect.sizeDelta = new Vector2(placement.Height * CharacterAspect, placement.Height);
            rect.anchoredPosition = new Vector2(placement.OffsetX, placement.FeetY);
            rect.localScale = placement.Flip ? new Vector3(-1f, 1f, 1f) : Vector3.one;

            // And when a slot is re-placed after it already exists — which is
            // what act six's two ages of the same character need — the motion
            // has to be told, or it pulls the character back to wherever it was
            // standing when it started breathing. RecapturePosition rather than
            // RecaptureBase: the position is what was just written and is safe
            // to read back, where scale and rotation would fold the current
            // frame of the animation into the rest pose.
            if (slot.Motion != null)
            {
                slot.Motion.RecapturePosition();
            }
        }

        /// <summary>
        /// Fits the slot to its sprite's own proportions.
        /// </summary>
        /// <remarks>
        /// Every character sprite in the project shares a 1200×2400 canvas, so
        /// in practice this always produces the same size. It is computed rather
        /// than assumed because the day one sprite is exported at a different
        /// aspect, the alternative is a character silently stretched thin with
        /// no obvious cause.
        /// </remarks>
        private void ResizeToSprite(Slot slot, Sprite sprite)
        {
            if (slot.Image == null || sprite == null)
            {
                return;
            }

            Rect bounds = sprite.rect;
            if (bounds.height <= 0f)
            {
                return;
            }

            float height = slot.Placement.Height;
            float width = height * (bounds.width / bounds.height);

            // sizeDelta only. Deliberately no RecaptureBase: idle motion's rest
            // pose is position, rotation and scale, none of which this touches —
            // and re-capturing mid-animation would fold the current drift and
            // pulse into the rest pose, so every expression change would nudge
            // the character a little further from where they started.
            slot.Image.rectTransform.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// Where the two stage positions actually are, against where they were
        /// told to be.
        /// </summary>
        /// <remarks>
        /// Written after the placement went wrong four separate times for four
        /// separate reasons, each of which looked identical from the player's
        /// chair. Reading the rect back is the only way to tell "the asset holds
        /// the wrong numbers" apart from "the asset is right and something is
        /// overwriting it", and those two have completely different fixes.
        /// </remarks>
        public string DescribeSlots()
        {
            return "[Stage] " + Describe("left", _left) + "\n        " + Describe("right", _right);
        }

        private static string Describe(string side, Slot slot)
        {
            if (slot?.Image == null)
            {
                return $"{side}: not built";
            }

            RectTransform rect = slot.Image.rectTransform;
            Vector2 want = new Vector2(slot.Placement.OffsetX, slot.Placement.FeetY);
            Vector2 have = rect.anchoredPosition;

            string verdict = Vector2.Distance(want, have) < 1.5f
                ? "matches"
                : "DOES NOT MATCH — something is writing over the placement";

            return
                $"{side}: {slot.Placement.Speaker}, wants x {want.x:0} feet {want.y:0} " +
                $"height {slot.Placement.Height:0}; rect is at x {have.x:0} y {have.y:0} " +
                $"size {rect.sizeDelta.x:0}×{rect.sizeDelta.y:0} — {verdict}";
        }

        private static void StretchFull(RectTransform rect)
        {
            if (rect == null)
            {
                return;
            }

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.anchoredPosition3D = Vector3.zero;
        }
    }
}
