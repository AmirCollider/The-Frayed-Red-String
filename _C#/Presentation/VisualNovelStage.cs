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
                // Keep the previous background rather than cutting to nothing: a
                // missing asset should read as "this scene did not change", not
                // as a rendering failure.
                onComplete?.Invoke();
                return;
            }

            CurrentBackground = backgroundName;
            _backgroundFade.CancelIfRunning();

            _backgroundBack.sprite = sprite;
            _backgroundBack.SetAlpha(0f);

            Image incoming = _backgroundBack;
            Image outgoing = _backgroundFront;

            if (duration <= 0f)
            {
                incoming.SetAlpha(1f);
                outgoing.SetAlpha(0f);
                SwapBackgroundLayers();
                onComplete?.Invoke();
                return;
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
                },
                EaseType.InOutSine,
                0f,
                () =>
                {
                    incoming.SetAlpha(1f);
                    outgoing.SetAlpha(0f);
                    SwapBackgroundLayers();
                    onComplete?.Invoke();
                },
                this);
        }

        private void SwapBackgroundLayers()
        {
            (_backgroundFront, _backgroundBack) = (_backgroundBack, _backgroundFront);

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
                return;
            }

            bool entering = !slot.Present || slot.Occupant != speaker;

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
                        slot.Motion.ExtraOffset = new Vector3(Mathf.LerpUnclamped(from, 0f, t), 0f, 0f);
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
                this);
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
                        slot.Motion.ExtraOffset = new Vector3(Mathf.LerpUnclamped(0f, to, t), 0f, 0f);
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
                this);
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
                this);
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
            if (_left != null && _left.Placement.Speaker == speaker)
            {
                return _left;
            }

            return _right != null && _right.Placement.Speaker == speaker ? _right : null;
        }

        private void SetUpCharacters(RectTransform parent)
        {
            GameObject root = new GameObject("StageCharacters", typeof(RectTransform));
            root.transform.SetParent(parent, false);

            _characterRoot = (RectTransform)root.transform;
            StretchFull(_characterRoot);

            _left = CreateSlot("StageCharacterLeft", _settings.PlacementFor(Speaker.Yua));
            _right = CreateSlot("StageCharacterRight", _settings.PlacementFor(Speaker.Haru));
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
            rect.sizeDelta = new Vector2(placement.Height * CharacterAspect, placement.Height);
            rect.anchoredPosition = new Vector2(placement.OffsetX, placement.FeetY);

            if (placement.Flip)
            {
                rect.localScale = new Vector3(-1f, 1f, 1f);
            }

            Image image = host.AddComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = true;
            image.color = new Color(1f, 1f, 1f, 0f);
            image.enabled = false;

            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Character, UnityUtility.StableHash01(slotName));

            float slide = _settings.EntranceSlide * (placement.Side == StageSide.Left ? -1f : 1f);

            return new Slot
            {
                Image = image,
                Motion = motion,
                Occupant = Speaker.Narrator,
                Pose = Portrait.Unchanged,
                Present = false,
                SlideFrom = slide,
                Placement = placement
            };
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
