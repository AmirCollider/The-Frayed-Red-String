// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ChoicePanelView.cs
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    /// The blue-and-green choice buttons.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Nothing in act one uses this. The mechanic belongs to act two, where Yua
    /// turns to the player, explains that the buttons exist and asks them to
    /// press the green ones — and the reveal only lands if the player has never
    /// seen a choice before that moment.
    /// </para>
    /// <para>
    /// It is built now because the director already has to know what to do when
    /// it meets a choice beat, and because writing it alongside the rest means
    /// act two is a script file rather than a script file plus a new screen.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class ChoicePanelView : MonoBehaviour
    {
        private const float ButtonWidth = 760f;
        private const float ButtonHeight = 168f;
        private const float ButtonGap = 26f;
        private const float RevealDuration = 0.45f;
        private const float StaggerPerButton = 0.11f;

        /// <summary>Scale each option grows from as it is dealt in.</summary>
        private const float RevealFromScale = 0.94f;

        private static readonly Color KindFallback = new Color(0.53f, 0.72f, 0.94f, 0.94f);
        private static readonly Color CruelFallback = new Color(0.55f, 0.83f, 0.62f, 0.94f);
        private static readonly Color NeutralFallback = new Color(0.96f, 0.95f, 0.97f, 0.94f);
        private static readonly Color LabelColour = new Color(0.16f, 0.13f, 0.17f, 1f);

        private RectTransform _root;
        private CanvasGroup _group;
        private UiSpriteLibrary _library;

        private readonly List<Button> _buttons = new List<Button>(4);
        private readonly List<TMP_Text> _labels = new List<TMP_Text>(4);
        private readonly List<ChoiceData> _shown = new List<ChoiceData>(4);
        private readonly List<CanvasGroup> _buttonGroups = new List<CanvasGroup>(4);
        private readonly List<AmbientMotion> _buttonMotions = new List<AmbientMotion>(4);

        private Action<int> _onChosen;

        /// <summary>True while the game is waiting on a choice.</summary>
        public bool IsOpen { get; private set; }

        /// <summary>Builds the panel as a full-screen layer.</summary>
        public void Initialize(RectTransform parent)
        {
            _library = UiSpriteLibrary.Load();

            _root = (RectTransform)transform;
            _root.SetParent(parent, false);

            _root.anchorMin = new Vector2(0.5f, 0.5f);
            _root.anchorMax = new Vector2(0.5f, 0.5f);
            _root.pivot = new Vector2(0.5f, 0.5f);
            _root.sizeDelta = new Vector2(ButtonWidth, 0f);
            _root.anchoredPosition = new Vector2(0f, 40f);

            VerticalLayoutGroup layout = gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = ButtonGap;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            ContentSizeFitter fitter = gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows a set of options and waits for one to be pressed.
        /// </summary>
        /// <param name="choices">Options, in the order they should appear.</param>
        /// <param name="onChosen">Receives the index of the option pressed.</param>
        public void Present(IReadOnlyList<ChoiceData> choices, Action<int> onChosen)
        {
            if (choices == null || choices.Count == 0)
            {
                onChosen?.Invoke(0);
                return;
            }

            _onChosen = onChosen;
            IsOpen = true;

            gameObject.SetActive(true);
            EnsureButtons(choices.Count);

            for (int i = 0; i < _buttons.Count; i++)
            {
                bool used = i < choices.Count;
                _buttons[i].gameObject.SetActive(used);

                if (!used)
                {
                    continue;
                }

                ApplyChoice(i, choices[i]);
            }

            AudioService.Play(SfxId.ChoiceAppear);

            _group.alpha = 1f;
            _group.blocksRaycasts = true;
            _group.interactable = true;

            RevealButtons(choices.Count);
        }

        /// <summary>
        /// Brings the options in one after another rather than all at once.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Fading the whole panel was what this did before, and it produced a
        /// block of buttons that simply became visible — the stagger constant
        /// was being added to one duration, which lengthens a fade rather than
        /// separating anything.
        /// </para>
        /// <para>
        /// Dealing them out matters here beyond decoration. A choice in this
        /// game is the moment the player is asked to take a side, and options
        /// that arrive in order are read in order; options that arrive together
        /// are scanned. The scale is pushed through the motion channel, which is
        /// also where the hover pulse lives, so the two compose instead of
        /// fighting.
        /// </para>
        /// </remarks>
        private void RevealButtons(int count)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                CanvasGroup group = i < _buttonGroups.Count ? _buttonGroups[i] : null;
                AmbientMotion motion = i < _buttonMotions.Count ? _buttonMotions[i] : null;

                if (group == null)
                {
                    continue;
                }

                if (i >= count)
                {
                    group.alpha = 0f;
                    group.blocksRaycasts = false;
                    continue;
                }

                group.alpha = 0f;

                // An option that cannot be seen must not be pressable. Half a
                // second is easily long enough for a player who was already
                // clicking to choose something they never read.
                group.blocksRaycasts = false;
                group.interactable = false;

                if (motion != null)
                {
                    motion.ExtraScale = new Vector3(RevealFromScale, RevealFromScale, 1f);
                }

                TweenRunner.Play(
                    RevealDuration,
                    t =>
                    {
                        if (group == null)
                        {
                            return;
                        }

                        group.alpha = t;

                        if (motion != null)
                        {
                            float scale = Mathf.LerpUnclamped(RevealFromScale, 1f, t);
                            motion.ExtraScale = new Vector3(scale, scale, 1f);
                        }
                    },
                    EaseType.OutCubic,
                    i * StaggerPerButton,
                    () =>
                    {
                        if (group != null)
                        {
                            group.alpha = 1f;
                            group.blocksRaycasts = true;
                            group.interactable = true;
                        }

                        if (motion != null)
                        {
                            motion.ExtraScale = Vector3.one;
                        }
                    },
                    this,
                    true,
                    true);
            }
        }

        /// <summary>Hides the panel without choosing anything.</summary>
        public void Dismiss()
        {
            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;
            _onChosen = null;

            _group.blocksRaycasts = false;
            _group.interactable = false;

            _group.FadeTo(0f, 0.22f, EaseType.InCubic, 0f, () =>
            {
                if (this != null && !IsOpen)
                {
                    gameObject.SetActive(false);
                }
            });
        }

        /// <summary>Re-renders the option labels after a language change.</summary>
        public void RefreshLanguage()
        {
            for (int i = 0; i < _labels.Count && i < _shown.Count; i++)
            {
                StoryText.Set(_labels[i], _shown[i].Text.Current());
            }
        }

        private void ApplyChoice(int index, ChoiceData choice)
        {
            while (_shown.Count <= index)
            {
                _shown.Add(default);
            }

            _shown[index] = choice;

            Image background = _buttons[index].image;
            Sprite art = ArtFor(choice.Tone);

            if (art != null)
            {
                background.sprite = art;
                background.type = Image.Type.Simple;
                background.color = Color.white;
            }
            else
            {
                // No authored art for this tone: fall back to a drawn pill in the
                // tone's own colour rather than to an untinted white box, so the
                // blue-versus-green read survives a missing asset.
                background.sprite = ProceduralUiSprites.RoundedRect(34, Color.white, LabelColour, 2f);
                background.type = Image.Type.Sliced;
                background.color = FallbackFor(choice.Tone);
            }

            StoryText.Set(_labels[index], choice.Text.Current());
        }

        private Sprite ArtFor(ChoiceTone tone)
        {
            if (_library == null)
            {
                return null;
            }

            switch (tone)
            {
                case ChoiceTone.Kind: return _library.ChoiceBlue;
                case ChoiceTone.Cruel: return _library.ChoiceGreen;
                default: return _library.ChoiceWhite;
            }
        }

        private static Color FallbackFor(ChoiceTone tone)
        {
            switch (tone)
            {
                case ChoiceTone.Kind: return KindFallback;
                case ChoiceTone.Cruel: return CruelFallback;
                default: return NeutralFallback;
            }
        }

        private void EnsureButtons(int wanted)
        {
            while (_buttons.Count < wanted)
            {
                CreateButton(_buttons.Count);
            }
        }

        private void CreateButton(int index)
        {
            GameObject host = new GameObject($"Choice{index:00}", typeof(RectTransform));
            host.transform.SetParent(_root, false);

            LayoutElement element = host.AddComponent<LayoutElement>();
            element.preferredWidth = ButtonWidth;
            element.preferredHeight = ButtonHeight;

            Image image = host.AddComponent<Image>();
            image.raycastTarget = true;

            Button button = host.AddComponent<Button>();
            button.targetGraphic = image;

            int captured = index;
            button.onClick.AddListener(() => OnPressed(captured));

            // The layout group above owns where this button sits, so idle motion
            // gets everything except the position channel.
            AmbientMotion motion = AmbientMotion.GetOrAdd(host);
            motion.Configure(AmbientMotionProfile.Button, UnityUtility.StableHash01("Choice" + index));
            motion.BorrowsPosition = true;

            CanvasGroup group = UnityUtility.GetOrAdd<CanvasGroup>(host);
            group.alpha = 0f;
            group.blocksRaycasts = false;
            group.interactable = false;

            GameObject labelHost = new GameObject("Label", typeof(RectTransform));
            labelHost.transform.SetParent(host.transform, false);

            RectTransform labelRect = (RectTransform)labelHost.transform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(46f, 18f);
            labelRect.offsetMax = new Vector2(-46f, -18f);

            TMP_Text label = labelHost.AddComponent<TextMeshProUGUI>();
            label.fontSize = GameConfig.ChoiceFontSize;
            label.color = LabelColour;
            label.alignment = TextAlignmentOptions.Midline;
            label.raycastTarget = false;

            StoryFonts.Apply(label);

            // Sound is attached here rather than left to the scene sweep: the
            // sweep runs when the scene loads, and these buttons do not exist
            // until a choice is reached.
            UnityUtility.GetOrAdd<SelectableSfx>(host);

            _buttons.Add(button);
            _labels.Add(label);
            _buttonGroups.Add(group);
            _buttonMotions.Add(motion);

            while (_shown.Count < _buttons.Count)
            {
                _shown.Add(default);
            }
        }

        private void OnPressed(int index)
        {
            if (!IsOpen)
            {
                return;
            }

            Action<int> callback = _onChosen;

            Dismiss();
            callback?.Invoke(index);
        }
    }
}
