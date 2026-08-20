// -----------------------------------------------------------------------------
//  The Frayed Red String
//  DialogueBoxView.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// The dialogue box: a scrim, a panel, a name plate, the line itself, and
    /// the marker that says the game is waiting for the player.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Built entirely from code and from shapes drawn at runtime, so Act01.unity
    /// stays as small as it was authored and every act added later gets the same
    /// box without anything being copied between scenes.
    /// </para>
    /// <para>
    /// The whole layout mirrors for Persian — name plate, text alignment and the
    /// continue marker all move to the other side. A right-to-left line reading
    /// out from under a name plate pinned to the left looks broken even to
    /// someone who cannot read the language.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class DialogueBoxView : MonoBehaviour
    {
        // --- Geometry, in the canvas' 1920 × 1080 reference space -------------

        /// <summary>
        /// Gap between the panel and the edge of the frame, left and right.
        /// </summary>
        /// <remarks>
        /// A margin rather than a width. The panel lives inside the frame's
        /// content rect, which is narrower while the frame is closed and grows
        /// as it opens; a fixed 1720 was wider than the closed frame, so the box
        /// ran underneath the bars and every line lost a word off each end.
        /// </remarks>
        private const float PanelSideMargin = 40f;

        /// <summary>
        /// How tall the box is, in canvas units.
        /// </summary>
        /// <remarks>
        /// Three lines of dialogue and the insets around them, and no more. It
        /// was three hundred, which is over a quarter of the screen for a box
        /// that is usually holding two lines — so most of what it covered was
        /// empty panel sitting across the characters' waists.
        /// </remarks>
        private const float PanelHeight = 235f;

        private const float PanelBottomMargin = 30f;
        private const int PanelCornerRadius = 28;

        /// <summary>How far the panel rises as it fades in, in canvas units.</summary>
        private const float PanelRise = 26f;

        private const float ScrimHeight = 400f;
        private const float TextInsetSide = 52f;
        private const float TextInsetTop = 34f;
        private const float TextInsetBottom = 30f;

        private const float NamePlateHeight = 62f;
        private const float NamePlateInsetX = 46f;

        /// <summary>How far the plate dips behind the panel's top edge.</summary>
        private const float NamePlateOverlap = 12f;

        private const int NamePlateCornerRadius = 22;

        private const float MarkerSize = 26f;
        private const float MarkerInset = 34f;

        // --- Palette ----------------------------------------------------------

        private static readonly Color PanelFill = new Color(1f, 0.976f, 0.984f, 0.92f);
        private static readonly Color PanelBorder = new Color(0.98f, 0.71f, 0.80f, 1f);
        private static readonly Color BodyColour = new Color(0.20f, 0.16f, 0.19f, 1f);
        private static readonly Color ScrimTop = new Color(0f, 0f, 0f, 0f);
        private static readonly Color ScrimBottom = new Color(0.10f, 0.04f, 0.08f, 0.52f);

        private static readonly Color YuaPlate = new Color(0.96f, 0.53f, 0.68f, 0.96f);
        private static readonly Color HaruPlate = new Color(0.50f, 0.71f, 0.93f, 0.96f);
        private static readonly Color PlayerPlate = new Color(0.72f, 0.62f, 0.90f, 0.96f);

        private RectTransform _root;
        private CanvasGroup _group;
        private Image _scrimImage;
        private Image _panelImage;
        private RectTransform _panel;
        private AmbientMotion _panelMotion;
        private RectTransform _namePlate;
        private Image _namePlateImage;
        private TMP_Text _nameLabel;
        private TMP_Text _bodyLabel;
        private Image _markerImage;
        private RectTransform _marker;
        private AmbientMotion _markerMotion;
        private TypewriterLabel _typewriter;

        private TweenHandle _visibility;
        private Speaker _speaker;
        private bool _visible;

        /// <summary>
        /// True once Haru has taken the box away and the lines are bare.
        /// </summary>
        /// <remarks>
        /// The state act five leaves the game in. It is not reset by
        /// <see cref="Show"/> — the box does not come back because the next line
        /// needs somewhere to go, which would undo the gesture two seconds after
        /// it happened. It goes away when the scene does, because the view is
        /// rebuilt for every act.
        /// </remarks>
        private bool _bare;

        /// <summary>
        /// True while the story is playing itself, so the continue arrow is
        /// withheld.
        /// </summary>
        /// <remarks>
        /// The arrow means "the game is waiting for you". During act six it is
        /// not, and leaving it blinking would have the player clicking at a
        /// screen that has stopped listening — which reads as the game being
        /// broken rather than as the game being a film.
        /// </remarks>
        private bool _promptSuppressed;

        /// <summary>Colour the lines take once the panel behind them is gone.</summary>
        private static readonly Color BareBodyColour = new Color(0.97f, 0.96f, 0.96f, 1f);

        /// <summary>The scrim, once it is the only thing making the text readable.</summary>
        private static readonly Color BareScrimColour = new Color(0.05f, 0.03f, 0.04f, 0.80f);

        /// <summary>The typewriter driving the body text.</summary>
        public TypewriterLabel Typewriter => _typewriter;

        /// <summary>True when the box is on screen.</summary>
        public bool IsVisible => _visible;

        /// <summary>True while the current line is still appearing.</summary>
        public bool IsTyping => _typewriter != null && _typewriter.IsTyping;

        /// <summary>Builds the box as a full-screen layer under <paramref name="parent"/>.</summary>
        public void Initialize(RectTransform parent)
        {
            _root = (RectTransform)transform;
            _root.SetParent(parent, false);
            StretchFull(_root);

            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);

            // Nothing in the box is clickable. Advancing is handled by a
            // full-screen catcher behind it, and a raycast target here would
            // carve a dead zone out of the bottom third of the screen — exactly
            // where a player's cursor already is.
            _group.blocksRaycasts = false;
            _group.interactable = false;

            BuildScrim();
            BuildPanel();
            BuildNamePlate();
            BuildBody();
            BuildMarker();

            _typewriter = UnityUtility.GetOrAdd<TypewriterLabel>(_bodyLabel.gameObject);
            _typewriter.Initialize(_bodyLabel);
            _typewriter.Completed += OnLineCompleted;

            ApplyDirection();

            _group.alpha = 0f;
            _visible = false;

            if (_panelMotion != null)
            {
                _panelMotion.ExtraOffset = new Vector3(0f, -PanelRise, 0f);
            }

            SetMarkerVisible(false);
        }

        /// <summary>Shows one line, typing it out in the speaker's voice.</summary>
        /// <param name="speaker">Who is talking.</param>
        /// <param name="text">The line, already in the player's language.</param>
        public void ShowLine(Speaker speaker, string text, float charactersPerSecond = 0f)
        {
            _speaker = speaker;

            ApplyNamePlate(speaker);
            SetMarkerVisible(false);

            _typewriter.Play(text, speaker, charactersPerSecond);
        }

        /// <summary>
        /// Replaces the line on screen without typing it, for a language change.
        /// </summary>
        public void ShowLineInstantly(Speaker speaker, string text)
        {
            _speaker = speaker;

            ApplyDirection();
            ApplyNamePlate(speaker);

            _typewriter.ShowInstantly(text, speaker);
            SetMarkerVisible(true);
        }

        /// <summary>Reveals the rest of the current line immediately.</summary>
        public void CompleteLine()
        {
            _typewriter.CompleteNow();
        }

        /// <summary>Fades the box in.</summary>
        public void Show(float duration)
        {
            if (_visible)
            {
                return;
            }

            _visible = true;
            _visibility.CancelIfRunning();

            AudioService.Play(SfxId.PageTurn, 0.7f);
            _visibility = AnimateVisibility(1f, duration, EaseType.OutCubic);
        }

        /// <summary>Fades the box out, e.g. while the location changes.</summary>
        public void Hide(float duration)
        {
            if (!_visible)
            {
                return;
            }

            _visible = false;
            _visibility.CancelIfRunning();

            SetMarkerVisible(false);
            _visibility = AnimateVisibility(0f, duration, EaseType.InCubic);
        }

        /// <summary>
        /// Haru takes hold of the box and drags it off the bottom of the screen.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Act five's gesture, from the design document. It is deliberately not
        /// a fade: the box travels, all the way down and past the edge, and the
        /// alpha only starts going once it is most of the way there. A box that
        /// dissolves has been switched off by the game; a box that is dragged
        /// out of frame has been moved by somebody standing in it.
        /// </para>
        /// <para>
        /// Driven through the panel's ambient-motion offset channel, like every
        /// other movement in this class, so the panel's idle breathing carries
        /// on underneath rather than fighting it for the same property.
        /// </para>
        /// <para>
        /// The box is left hidden afterwards. Anything that calls
        /// <see cref="Show"/> again — the next line, a title card, the act after
        /// this one — puts it back at its proper place, because the offset is
        /// cleared here on the way out.
        /// </para>
        /// </remarks>
        public void PullDown(float duration)
        {
            _visibility.CancelIfRunning();
            SetMarkerVisible(false);

            _visible = false;

            if (duration <= 0f)
            {
                FinishPullDown();
                return;
            }

            float fromAlpha = _group != null ? _group.alpha : 0f;

            // Far enough that the panel and its name plate are both clear of the
            // screen before the travel ends.
            const float TravelDown = 520f;

            _visibility = TweenRunner.Play(
                duration,
                t =>
                {
                    if (_group == null)
                    {
                        return;
                    }

                    if (_panelMotion != null)
                    {
                        _panelMotion.ExtraOffset = new Vector3(0f, -TravelDown * t, 0f);
                    }

                    // Opaque for the first two thirds. He is not fading it out,
                    // he is pulling it, and it should still be a solid object
                    // while it is in shot.
                    float vanish = Mathf.InverseLerp(0.66f, 1f, t);
                    _group.alpha = Mathf.LerpUnclamped(fromAlpha, 0f, vanish);
                },

                // Heavy to start and quicker as it goes, which is what dragging
                // something that does not want to move looks like.
                EaseType.InCubic,
                0f,
                FinishPullDown,
                this,
                true,
                true);
        }

        private void FinishPullDown()
        {
            if (_group != null)
            {
                _group.alpha = 0f;
            }

            if (_panelMotion != null)
            {
                _panelMotion.ExtraOffset = new Vector3(0f, -PanelRise, 0f);
            }

            EnterBareMode();
        }

        /// <summary>
        /// What the box is after he has taken it: the words, and nothing behind
        /// them but a little more dark at the bottom of the picture.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The alternative was to let the panel come back on the next line,
        /// which is what happens if this does not exist — the box is dragged
        /// off, Haru says one sentence, and the pastel panel fades politely back
        /// in underneath it. Two seconds of the best gesture in the game,
        /// undone by the line after it.
        /// </para>
        /// <para>
        /// So the panel and its name plate are switched off for good and the
        /// text is recoloured for a dark ground. The scrim stays and gets
        /// heavier: it is not decoration, it is the only reason a light line is
        /// readable over an arbitrary background, and every voiced game in the
        /// genre has one for exactly that reason.
        /// </para>
        /// <para>
        /// The rect is untouched, so the lines stay exactly where the player has
        /// been reading them for four acts. Everything about where to look
        /// carries over; only the box is gone.
        /// </para>
        /// </remarks>
        private void EnterBareMode()
        {
            if (_bare)
            {
                return;
            }

            _bare = true;

            if (_panelImage != null)
            {
                _panelImage.enabled = false;
            }

            if (_namePlate != null)
            {
                _namePlate.gameObject.SetActive(false);
            }

            if (_scrimImage != null)
            {
                _scrimImage.sprite = ProceduralUiSprites.VerticalGradient(ScrimTop, BareScrimColour);
            }

            if (_bodyLabel != null)
            {
                _bodyLabel.color = BareBodyColour;
            }

            if (_markerImage != null)
            {
                _markerImage.sprite = ProceduralUiSprites.DownArrow(24, BareBodyColour);
            }

            // The panel travelled to get here; put the rect back where it was so
            // the next line arrives in the place the player has been reading.
            if (_panelMotion != null)
            {
                _panelMotion.ExtraOffset = new Vector3(0f, -PanelRise, 0f);
            }
        }

        /// <summary>
        /// Fades the box and slides it the last few pixels into place.
        /// </summary>
        /// <remarks>
        /// A box that only fades reads as a layer being switched on. The same
        /// fade with a short rise under it reads as the box arriving — which is
        /// what it is doing, several times per scene, every time the story steps
        /// away from the dialogue and comes back to it.
        /// </remarks>
        private TweenHandle AnimateVisibility(float target, float duration, EaseType ease)
        {
            float fromAlpha = _group.alpha;

            return TweenRunner.Play(
                duration,
                t =>
                {
                    if (_group == null)
                    {
                        return;
                    }

                    float alpha = Mathf.LerpUnclamped(fromAlpha, target, t);
                    _group.alpha = alpha;

                    if (_panelMotion != null)
                    {
                        _panelMotion.ExtraOffset = new Vector3(0f, (1f - alpha) * -PanelRise, 0f);
                    }
                },
                ease,
                0f,
                () =>
                {
                    if (_group != null)
                    {
                        _group.alpha = target;
                    }

                    if (_panelMotion != null)
                    {
                        _panelMotion.ExtraOffset = new Vector3(0f, (1f - target) * -PanelRise, 0f);
                    }
                },
                this,
                true,
                true);
        }

        /// <summary>
        /// Mirrors the layout for the active language's reading direction.
        /// </summary>
        /// <remarks>
        /// The line itself is re-supplied by the director, which is the only
        /// thing that knows which beat is on screen. This handles the parts that
        /// belong to the box: which side the name plate sits on, which way the
        /// text is aligned, and where the continue marker waits.
        /// </remarks>
        public void RefreshLanguage()
        {
            ApplyDirection();
            ApplyNamePlate(_speaker);
        }

        // ---------------------------------------------------------------------
        //  Construction
        // ---------------------------------------------------------------------

        private void BuildScrim()
        {
            GameObject host = new GameObject("DialogueScrim", typeof(RectTransform));
            host.transform.SetParent(_root, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = new Vector2(0f, ScrimHeight);

            _scrimImage = host.AddComponent<Image>();

            // A one-pixel-wide gradient drawn Simple stretches to fill in both
            // axes, which is the whole of what a vertical scrim needs.
            _scrimImage.sprite = ProceduralUiSprites.VerticalGradient(ScrimTop, ScrimBottom);
            _scrimImage.type = Image.Type.Simple;
            _scrimImage.raycastTarget = false;
        }

        private void BuildPanel()
        {
            GameObject host = new GameObject("DialoguePanel", typeof(RectTransform));
            host.transform.SetParent(_root, false);

            // Stretched between the two sides of whatever it is parented to,
            // rather than given a width of its own, so the box is always inside
            // the frame and grows with it when the frame opens.
            _panel = (RectTransform)host.transform;
            _panel.anchorMin = new Vector2(0f, 0f);
            _panel.anchorMax = new Vector2(1f, 0f);
            _panel.pivot = new Vector2(0.5f, 0f);
            _panel.offsetMin = new Vector2(PanelSideMargin, PanelBottomMargin);
            _panel.offsetMax = new Vector2(-PanelSideMargin, PanelBottomMargin + PanelHeight);

            _panelImage = host.AddComponent<Image>();
            _panelImage.sprite = ProceduralUiSprites.RoundedRect(PanelCornerRadius, PanelFill, PanelBorder, 3f);
            _panelImage.type = Image.Type.Sliced;
            _panelImage.raycastTarget = false;

            _panelMotion = AmbientMotion.GetOrAdd(host);
            _panelMotion.Configure(AmbientMotionProfile.Panel, UnityUtility.StableHash01("DialoguePanel"));
        }

        private void BuildNamePlate()
        {
            GameObject host = new GameObject("NamePlate", typeof(RectTransform));
            host.transform.SetParent(_panel, false);

            _namePlate = (RectTransform)host.transform;
            _namePlate.sizeDelta = new Vector2(0f, NamePlateHeight);

            _namePlateImage = host.AddComponent<Image>();
            _namePlateImage.sprite = ProceduralUiSprites.RoundedRect(NamePlateCornerRadius, Color.white);
            _namePlateImage.type = Image.Type.Sliced;
            _namePlateImage.raycastTarget = false;

            HorizontalLayoutGroup layout = host.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(28, 28, 2, 8);
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            // The plate is exactly as wide as the name inside it. Sizing it by
            // hand would mean picking a width that fits "Haru" and hoping it
            // also fits every translation of every speaker added later.
            ContentSizeFitter fitter = host.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            GameObject labelHost = new GameObject("NameLabel", typeof(RectTransform));
            labelHost.transform.SetParent(_namePlate, false);

            _nameLabel = labelHost.AddComponent<TextMeshProUGUI>();
            _nameLabel.fontSize = GameConfig.SpeakerFontSize;
            _nameLabel.color = Color.white;
            _nameLabel.alignment = TextAlignmentOptions.Midline;
            _nameLabel.textWrappingMode = TextWrappingModes.NoWrap;
            _nameLabel.raycastTarget = false;

            StoryFonts.Apply(_nameLabel);
        }

        private void BuildBody()
        {
            GameObject host = new GameObject("DialogueText", typeof(RectTransform));
            host.transform.SetParent(_panel, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = new Vector2(TextInsetSide, TextInsetBottom);
            rect.offsetMax = new Vector2(-TextInsetSide, -TextInsetTop);

            _bodyLabel = host.AddComponent<TextMeshProUGUI>();
            _bodyLabel.fontSize = GameConfig.DialogueFontSize;
            _bodyLabel.color = BodyColour;
            _bodyLabel.alignment = TextAlignmentOptions.TopLeft;
            _bodyLabel.lineSpacing = 12f;
            _bodyLabel.overflowMode = TextOverflowModes.Overflow;
            _bodyLabel.raycastTarget = false;

            StoryFonts.Apply(_bodyLabel);
        }

        private void BuildMarker()
        {
            GameObject host = new GameObject("ContinueMarker", typeof(RectTransform));
            host.transform.SetParent(_panel, false);

            _marker = (RectTransform)host.transform;
            _marker.sizeDelta = new Vector2(MarkerSize, MarkerSize);

            _markerImage = host.AddComponent<Image>();
            _markerImage.sprite = ProceduralUiSprites.DownArrow(24, PanelBorder);
            _markerImage.raycastTarget = false;

            _markerMotion = AmbientMotion.GetOrAdd(host);
            _markerMotion.Configure(AmbientMotionProfile.Prompt, 0.25f);
        }

        // ---------------------------------------------------------------------
        //  Direction and state
        // ---------------------------------------------------------------------

        /// <summary>
        /// Places the mirrored pieces for the active language's reading
        /// direction.
        /// </summary>
        private void ApplyDirection()
        {
            bool rightToLeft = LocalizationService.Current.IsRightToLeft();

            // Name plate: above the edge the line starts from.
            Vector2 plateAnchor = new Vector2(rightToLeft ? 1f : 0f, 1f);
            _namePlate.anchorMin = plateAnchor;
            _namePlate.anchorMax = plateAnchor;
            _namePlate.pivot = plateAnchor;
            _namePlate.anchoredPosition = new Vector2(
                rightToLeft ? -NamePlateInsetX : NamePlateInsetX,
                NamePlateHeight - NamePlateOverlap);

            // Continue marker: at the far end of the last line.
            Vector2 markerAnchor = new Vector2(rightToLeft ? 0f : 1f, 0f);
            _marker.anchorMin = markerAnchor;
            _marker.anchorMax = markerAnchor;
            _marker.pivot = markerAnchor;
            _marker.anchoredPosition = new Vector2(
                rightToLeft ? MarkerInset : -MarkerInset,
                MarkerInset);

            // Position only. The marker pulses, so re-capturing its full pose
            // here would bake the current beat of that pulse into its rest scale
            // and grow it a little on every language change.
            _markerMotion?.RecapturePosition();

            _bodyLabel.alignment = rightToLeft
                ? TextAlignmentOptions.TopRight
                : TextAlignmentOptions.TopLeft;

            StoryText.ApplyWeight(_bodyLabel);
            StoryText.ApplyWeight(_nameLabel);
        }

        private void ApplyNamePlate(Speaker speaker)
        {
            string nameKey = CharacterArt.NameKey(speaker);

            // Once the panel is gone the plate has nothing to sit on, and a
            // coloured tab floating over the scenery is the one piece of
            // interface act five would look wrong keeping.
            bool named = !string.IsNullOrEmpty(nameKey) && !_bare;

            _namePlate.gameObject.SetActive(named);

            if (!named)
            {
                return;
            }

            _namePlateImage.color = PlateColour(speaker);
            StoryText.Set(_nameLabel, LocalizationService.Get(nameKey));
        }

        private static Color PlateColour(Speaker speaker)
        {
            switch (speaker)
            {
                case Speaker.Yua: return YuaPlate;
                case Speaker.Haru: return HaruPlate;
                default: return PlayerPlate;
            }
        }

        private void OnLineCompleted()
        {
            SetMarkerVisible(true);
        }

        /// <summary>Shows or withholds the "waiting for you" arrow.</summary>
        public void SetPromptSuppressed(bool suppressed)
        {
            _promptSuppressed = suppressed;

            if (suppressed)
            {
                SetMarkerVisible(false);
            }
        }

        private void SetMarkerVisible(bool visible)
        {
            if (_markerImage != null)
            {
                _markerImage.enabled = visible && !_promptSuppressed;
            }
        }

        private static void StretchFull(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void OnDestroy()
        {
            if (_typewriter != null)
            {
                _typewriter.Completed -= OnLineCompleted;
            }
        }
    }
}
