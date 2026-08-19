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

        private const float PanelWidth = 1720f;
        private const float PanelHeight = 300f;
        private const float PanelBottomMargin = 46f;
        private const int PanelCornerRadius = 28;

        private const float ScrimHeight = 520f;
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
        private RectTransform _panel;
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
            SetMarkerVisible(false);
        }

        /// <summary>Shows one line, typing it out in the speaker's voice.</summary>
        /// <param name="speaker">Who is talking.</param>
        /// <param name="text">The line, already in the player's language.</param>
        public void ShowLine(Speaker speaker, string text)
        {
            _speaker = speaker;

            ApplyNamePlate(speaker);
            SetMarkerVisible(false);

            _typewriter.Play(text, speaker);
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
            _visibility = _group.FadeTo(1f, duration, EaseType.OutCubic);
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
            _visibility = _group.FadeTo(0f, duration, EaseType.InCubic);
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

            Image image = host.AddComponent<Image>();

            // A one-pixel-wide gradient drawn Simple stretches to fill in both
            // axes, which is the whole of what a vertical scrim needs.
            image.sprite = ProceduralUiSprites.VerticalGradient(ScrimTop, ScrimBottom);
            image.type = Image.Type.Simple;
            image.raycastTarget = false;
        }

        private void BuildPanel()
        {
            GameObject host = new GameObject("DialoguePanel", typeof(RectTransform));
            host.transform.SetParent(_root, false);

            _panel = (RectTransform)host.transform;
            _panel.anchorMin = new Vector2(0.5f, 0f);
            _panel.anchorMax = new Vector2(0.5f, 0f);
            _panel.pivot = new Vector2(0.5f, 0f);
            _panel.sizeDelta = new Vector2(PanelWidth, PanelHeight);
            _panel.anchoredPosition = new Vector2(0f, PanelBottomMargin);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.RoundedRect(PanelCornerRadius, PanelFill, PanelBorder, 3f);
            image.type = Image.Type.Sliced;
            image.raycastTarget = false;

            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Panel, UnityUtility.StableHash01("DialoguePanel"));
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
        }

        private void ApplyNamePlate(Speaker speaker)
        {
            string nameKey = CharacterArt.NameKey(speaker);
            bool named = !string.IsNullOrEmpty(nameKey);

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

        private void SetMarkerVisible(bool visible)
        {
            if (_markerImage != null)
            {
                _markerImage.enabled = visible;
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
