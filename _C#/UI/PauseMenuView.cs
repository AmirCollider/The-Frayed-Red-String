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
    /// The fade lives on this object's own <see cref="CanvasGroup"/> rather than
    /// on the canvas it sits in. That canvas is shared: the act moves the
    /// language button and the save panel into it so both draw above the veil,
    /// and a group on the canvas took those two down with the menu — a language
    /// button the player could not see for the whole act, and a save panel that
    /// only existed while the game was paused.
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
        private const float ButtonGap = 18f;
        private const float ColumnPadding = 34f;
        private const float TitleHeight = 86f;
        private const float FadeDuration = 0.28f;

        /// <summary>How far the column rises as it appears, in canvas units.</summary>
        private const float RiseDistance = 34f;

        private static readonly Color VeilColour = new Color(0.06f, 0.03f, 0.06f, 0.62f);
        private static readonly Color CardFill = new Color(0.14f, 0.09f, 0.13f, 0.90f);
        private static readonly Color CardBorder = new Color(0.98f, 0.71f, 0.80f, 0.75f);
        private static readonly Color ButtonFill = new Color(1f, 0.972f, 0.982f, 0.95f);
        private static readonly Color ButtonBorder = new Color(0.98f, 0.71f, 0.80f, 1f);
        private static readonly Color ButtonText = new Color(0.20f, 0.16f, 0.19f, 1f);
        private static readonly Color TitleText = new Color(1f, 0.94f, 0.96f, 1f);

        [Tooltip("Use the PauseMenuButton* artwork instead of drawn buttons. The art has English words baked in.")]
        [SerializeField] private bool _useAuthoredArt;

        private CanvasGroup _group;
        private RectTransform _card;
        private RectTransform _column;
        private AmbientMotion _cardMotion;
        private TweenHandle _fade;
        private UiSpriteLibrary _library;

        /// <summary>
        /// True once a language flag has been taken in from the scene.
        /// </summary>
        /// <remarks>
        /// Read by the act, which builds one itself when the scene had none —
        /// only the first two acts were ever given the authored button.
        /// </remarks>
        public bool HasLanguageButton { get; private set; }

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
        /// moved into it afterwards by the act, so they sit above the veil —
        /// which is why the menu's own fade must not be attached to it.
        /// </param>
        public void Initialize(RectTransform layer)
        {
            _library = UiSpriteLibrary.Load();

            transform.SetParent(layer, false);
            Stretch((RectTransform)transform);

            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);

            BuildVeil();
            BuildCard();

            ApplyClosedState();
        }

        /// <summary>
        /// Moves an element into the menu, so it comes and goes with it.
        /// </summary>
        /// <remarks>
        /// For the language button, which the act inherits from the scene's old
        /// menu canvas. Left on the pause layer it sat in the corner of the
        /// screen for the whole act — a control with nothing to do with the
        /// story, visible over every line of it. Inside the menu it is where a
        /// player goes looking for it: with Save, Load and the way back to the
        /// title.
        /// </remarks>
        public void Adopt(Transform element)
        {
            if (element == null)
            {
                return;
            }

            if (element.name == ObjectNames.LanguageButtonMask)
            {
                HasLanguageButton = true;
            }

            element.SetParent(transform, false);

            // Above the veil, which is this object's first child.
            element.SetAsLastSibling();
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

            Animate(1f, EaseType.OutCubic);
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

            Animate(0f, EaseType.InCubic);
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

        /// <summary>
        /// Fades the menu and lifts the card at the same time.
        /// </summary>
        /// <remarks>
        /// The rise is pushed through the card's motion channel rather than
        /// written to its transform, so it composes with the idle sway instead
        /// of fighting it.
        /// </remarks>
        private void Animate(float target, EaseType ease)
        {
            float fromAlpha = _group.alpha;

            _fade = TweenRunner.Play(
                FadeDuration,
                t =>
                {
                    if (_group == null)
                    {
                        return;
                    }

                    float alpha = Mathf.LerpUnclamped(fromAlpha, target, t);
                    _group.alpha = alpha;

                    if (_cardMotion != null)
                    {
                        _cardMotion.ExtraOffset = new Vector3(0f, (1f - alpha) * -RiseDistance, 0f);
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

                    if (_cardMotion != null)
                    {
                        _cardMotion.ExtraOffset = new Vector3(0f, (1f - target) * -RiseDistance, 0f);
                    }
                },
                this);
        }

        private void ApplyClosedState()
        {
            IsOpen = false;
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;

            if (_cardMotion != null)
            {
                _cardMotion.ExtraOffset = new Vector3(0f, -RiseDistance, 0f);
            }
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

        /// <summary>
        /// Builds the card the menu sits on, and the column inside it.
        /// </summary>
        /// <remarks>
        /// Two objects rather than one because a <see cref="ContentSizeFitter"/>
        /// has to measure the column, and the card wants padding around whatever
        /// that measurement comes out as. Nesting the fitter inside a card that
        /// fits itself to the fitter is the standard way to get both.
        /// </remarks>
        private void BuildCard()
        {
            GameObject cardHost = new GameObject("PauseCard", typeof(RectTransform));
            cardHost.transform.SetParent(transform, false);

            _card = (RectTransform)cardHost.transform;
            _card.anchorMin = new Vector2(0.5f, 0.5f);
            _card.anchorMax = new Vector2(0.5f, 0.5f);
            _card.pivot = new Vector2(0.5f, 0.5f);
            _card.sizeDelta = new Vector2(ButtonWidth + ColumnPadding * 2f, 0f);
            _card.anchoredPosition = Vector2.zero;

            Image card = cardHost.AddComponent<Image>();
            card.sprite = ProceduralUiSprites.RoundedRect(30, CardFill, CardBorder, 3f);
            card.type = Image.Type.Sliced;
            card.raycastTarget = true;

            VerticalLayoutGroup cardLayout = cardHost.AddComponent<VerticalLayoutGroup>();
            cardLayout.padding = new RectOffset(
                (int)ColumnPadding, (int)ColumnPadding, (int)ColumnPadding, (int)ColumnPadding);
            cardLayout.childAlignment = TextAnchor.MiddleCenter;
            cardLayout.childForceExpandWidth = true;
            cardLayout.childForceExpandHeight = false;
            cardLayout.childControlWidth = true;
            cardLayout.childControlHeight = true;

            ContentSizeFitter cardFitter = cardHost.AddComponent<ContentSizeFitter>();
            cardFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            cardFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _cardMotion = AmbientMotion.GetOrAdd(cardHost);
            _cardMotion.Configure(AmbientMotionProfile.Panel, UnityUtility.StableHash01("PauseCard"));

            GameObject columnHost = new GameObject("PauseColumn", typeof(RectTransform));
            columnHost.transform.SetParent(_card, false);

            _column = (RectTransform)columnHost.transform;

            VerticalLayoutGroup layout = columnHost.AddComponent<VerticalLayoutGroup>();
            layout.spacing = ButtonGap;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            // No ContentSizeFitter here on purpose. The card's layout group is
            // already sizing this object, and a fitter fighting a parent layout
            // for the same rect is the standard way to get a column that
            // resizes itself once per frame forever.
            //
            // It does not need one: a VerticalLayoutGroup reports the preferred
            // height of its own children, which is exactly the number the card
            // above asks for.
            LayoutElement columnElement = columnHost.AddComponent<LayoutElement>();
            columnElement.preferredWidth = ButtonWidth;

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
            element.preferredHeight = TitleHeight;
            element.preferredWidth = ButtonWidth;

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

            // Idle motion, but not idle drift: the layout group owns where this
            // button sits, and two writers on one anchoredPosition is how the
            // whole column ended up stacked in the middle of the screen.
            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Button, UnityUtility.StableHash01(objectName));
            motion.BorrowsPosition = true;

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
