// -----------------------------------------------------------------------------
//  The Frayed Red String
//  PauseMenuView.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// The in-act menu: resume, save, load, and back to the title.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Built into a canvas the act scene creates for it, with a known sorting
    /// order, scaler and render mode. An earlier version borrowed the scene's
    /// leftover MenuCanvas instead and the menu came out behind the characters
    /// with its buttons piled on top of one another — inheriting a canvas means
    /// inheriting four settings that are invisible when they are wrong.
    /// </para>
    /// <para>
    /// The buttons are drawn rather than taken from the PauseMenuButton* art on
    /// purpose: that art has its words baked into the image, so a Persian or
    /// Japanese player would be reading English in the middle of a translated
    /// game. Set <see cref="_useAuthoredArt"/> if the artwork matters more than
    /// the translation does — the labels then still sit on top, so the choice is
    /// visible rather than silent.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class PauseMenuView : MonoBehaviour
    {
        private const float ButtonWidth = 460f;
        private const float ButtonHeight = 96f;
        private const float ButtonGap = 20f;
        private const float FadeDuration = 0.28f;

        private static readonly Color VeilColour = new Color(0.06f, 0.03f, 0.06f, 0.55f);
        private static readonly Color ButtonFill = new Color(1f, 0.972f, 0.982f, 0.95f);
        private static readonly Color ButtonBorder = new Color(0.98f, 0.71f, 0.80f, 1f);
        private static readonly Color ButtonText = new Color(0.20f, 0.16f, 0.19f, 1f);
        private static readonly Color TitleText = new Color(1f, 0.94f, 0.96f, 1f);

        [Tooltip("Use the PauseMenuButton* artwork instead of drawn buttons. The art has English words baked in.")]
        [SerializeField] private bool _useAuthoredArt;

        private CanvasGroup _group;
        private RectTransform _column;
        private TweenHandle _fade;
        private UiSpriteLibrary _library;

        /// <summary>Raised when the player asks to carry on.</summary>
        public event Action Resumed;

        /// <summary>Raised when the player asks to write a save.</summary>
        public event Action SaveRequested;

        /// <summary>Raised when the player asks to read one.</summary>
        public event Action LoadRequested;

        /// <summary>Raised when the player asks to leave for the title screen.</summary>
        public event Action QuitRequested;

        /// <summary>True while the menu is showing.</summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Builds the menu inside an existing layer.
        /// </summary>
        /// <param name="layer">
        /// The canvas the menu lives in. The language button and save panel are
        /// moved into it afterwards by the act, so they sit above the veil.
        /// </param>
        public void Initialize(RectTransform layer)
        {
            _library = UiSpriteLibrary.Load();

            transform.SetParent(layer, false);
            Stretch((RectTransform)transform);

            _group = UnityUtility.GetOrAdd<CanvasGroup>(layer.gameObject);

            BuildVeil();
            BuildColumn();

            ApplyClosedState();
        }

        /// <summary>Shows the menu.</summary>
        public void Open()
        {
            if (IsOpen)
            {
                return;
            }

            IsOpen = true;
            _fade.CancelIfRunning();

            AudioService.Play(SfxId.PageTurn, 0.8f);

            _group.blocksRaycasts = true;
            _group.interactable = true;

            _fade = _group.FadeTo(1f, FadeDuration, EaseType.OutCubic);
        }

        /// <summary>Hides the menu.</summary>
        public void Close(bool playSound = true)
        {
            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;
            _fade.CancelIfRunning();

            if (playSound)
            {
                AudioService.Play(SfxId.Cancel, 0.8f);
            }

            // Interaction is revoked immediately rather than at the end of the
            // fade: a click landing on a half-faded Load button would leave the
            // player somewhere they did not ask to be.
            _group.blocksRaycasts = false;
            _group.interactable = false;

            _fade = _group.FadeTo(0f, FadeDuration, EaseType.InCubic);
        }

        /// <summary>Opens the menu if it is closed, closes it otherwise.</summary>
        public void Toggle()
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        private void ApplyClosedState()
        {
            IsOpen = false;
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
        }

        private void BuildVeil()
        {
            GameObject host = new GameObject("PauseVeil", typeof(RectTransform));
            host.transform.SetParent(transform, false);
            host.transform.SetAsFirstSibling();

            Stretch((RectTransform)host.transform);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.Solid(Color.white);
            image.color = VeilColour;

            // A raycast target on purpose: while the menu is up, a click on the
            // background must not fall through to the story underneath and
            // advance the dialogue behind the player's back.
            image.raycastTarget = true;
        }

        private void BuildColumn()
        {
            GameObject host = new GameObject("PauseColumn", typeof(RectTransform));
            host.transform.SetParent(transform, false);

            _column = (RectTransform)host.transform;
            _column.anchorMin = new Vector2(0.5f, 0.5f);
            _column.anchorMax = new Vector2(0.5f, 0.5f);
            _column.pivot = new Vector2(0.5f, 0.5f);
            _column.sizeDelta = new Vector2(ButtonWidth, 0f);
            _column.anchoredPosition = Vector2.zero;

            VerticalLayoutGroup layout = host.AddComponent<VerticalLayoutGroup>();
            layout.spacing = ButtonGap;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            ContentSizeFitter fitter = host.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            BuildTitle();

            BuildButton("PauseResumeButton", LocKeys.PauseResume, ArtFor(LocKeys.PauseResume), () => Resumed?.Invoke());
            BuildButton("PauseSaveButton", LocKeys.PauseSave, ArtFor(LocKeys.PauseSave), () => SaveRequested?.Invoke());
            BuildButton("PauseLoadButton", LocKeys.PauseLoad, ArtFor(LocKeys.PauseLoad), () => LoadRequested?.Invoke());
            BuildButton("PauseQuitButton", LocKeys.PauseQuitToMenu, ArtFor(LocKeys.PauseQuitToMenu), () => QuitRequested?.Invoke());
        }

        private void BuildTitle()
        {
            GameObject host = new GameObject("PauseTitle", typeof(RectTransform));
            host.transform.SetParent(_column, false);

            LayoutElement element = host.AddComponent<LayoutElement>();
            element.preferredHeight = 96f;

            TMP_Text label = host.AddComponent<TextMeshProUGUI>();
            label.fontSize = GameConfig.ActTitleFontSize * 0.6f;
            label.color = TitleText;
            label.alignment = TextAlignmentOptions.Midline;
            label.textWrappingMode = TextWrappingModes.NoWrap;
            label.raycastTarget = false;

            StoryFonts.Apply(label);
            UnityUtility.GetOrAdd<LocalizedText>(host).Bind(LocKeys.PauseTitle);
        }

        private void BuildButton(string objectName, string labelKey, Sprite art, Action onPressed)
        {
            GameObject host = new GameObject(objectName, typeof(RectTransform));
            host.transform.SetParent(_column, false);

            LayoutElement element = host.AddComponent<LayoutElement>();
            element.preferredWidth = ButtonWidth;
            element.preferredHeight = ButtonHeight;

            Image image = host.AddComponent<Image>();

            if (art != null)
            {
                image.sprite = art;
                image.type = Image.Type.Simple;
                image.color = Color.white;
            }
            else
            {
                image.sprite = ProceduralUiSprites.RoundedRect(26, ButtonFill, ButtonBorder, 3f);
                image.type = Image.Type.Sliced;
            }

            Button button = host.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onPressed?.Invoke());

            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Button, UnityUtility.StableHash01(objectName));

            // Attached here rather than left to the scene sweep, which ran before
            // these buttons existed.
            UnityUtility.GetOrAdd<SelectableSfx>(host);

            GameObject labelHost = new GameObject("Label", typeof(RectTransform));
            labelHost.transform.SetParent(host.transform, false);

            RectTransform labelRect = (RectTransform)labelHost.transform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(24f, 10f);
            labelRect.offsetMax = new Vector2(-24f, -14f);

            TMP_Text label = labelHost.AddComponent<TextMeshProUGUI>();
            label.fontSize = GameConfig.ChoiceFontSize;
            label.color = art != null ? Color.white : ButtonText;
            label.alignment = TextAlignmentOptions.Midline;
            label.textWrappingMode = TextWrappingModes.NoWrap;
            label.raycastTarget = false;

            StoryFonts.Apply(label);
            UnityUtility.GetOrAdd<LocalizedText>(labelHost).Bind(labelKey);
        }

        private Sprite ArtFor(string labelKey)
        {
            if (!_useAuthoredArt || _library == null)
            {
                return null;
            }

            switch (labelKey)
            {
                case LocKeys.PauseResume: return _library.PauseResume;
                case LocKeys.PauseSave: return _library.PauseSave;
                case LocKeys.PauseLoad: return _library.PauseLoad;
                case LocKeys.PauseQuitToMenu: return _library.PauseMenu;
                default: return null;
            }
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
