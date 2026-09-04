// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryOverlayView.cs
// -----------------------------------------------------------------------------

using System.Collections;
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
    /// The two pieces of text the story puts over the picture rather than into
    /// the dialogue box: the card that names an act, and the caption that names
    /// a place.
    /// </summary>
    /// <remarks>
    /// Both exist to answer a question the player has not asked out loud yet —
    /// "where am I?" and "what is this chapter called?" — and both have to get
    /// out of the way before the answer becomes an interruption. Neither waits
    /// for input.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class StoryOverlayView : MonoBehaviour
    {
        private const float CardFadeIn = 1.1f;
        private const float CardFadeOut = 0.9f;
        private const float CaptionFade = 0.55f;

        /// <summary>Width the divider under the act title grows to.</summary>
        private const float DividerWidth = 520f;

        private const float DividerHeight = 2f;

        /// <summary>Scale the act title springs up from.</summary>
        private const float TitleFromScale = 0.90f;

        /// <summary>How far the season line drifts down as the card settles.</summary>
        private const float SeasonRise = 18f;

        /// <summary>How far the place name slides in from, in canvas units.</summary>
        private const float CaptionSlide = 40f;

        private static readonly Color VeilColour = new Color(0.06f, 0.03f, 0.06f, 0.62f);
        private static readonly Color SeasonColour = new Color(0.98f, 0.78f, 0.86f, 1f);
        private static readonly Color TitleColour = new Color(1f, 0.98f, 0.99f, 1f);

        private static readonly Color CaptionFill = new Color(0.12f, 0.08f, 0.12f, 0.62f);
        private static readonly Color CaptionBorder = new Color(0.98f, 0.71f, 0.80f, 0.55f);
        private static readonly Color CaptionText = new Color(1f, 0.97f, 0.98f, 1f);

        private RectTransform _root;

        private CanvasGroup _cardGroup;
        private TMP_Text _seasonLabel;
        private TMP_Text _titleLabel;
        private AmbientMotion _titleMotion;
        private AmbientMotion _seasonMotion;
        private RectTransform _divider;

        private CanvasGroup _captionGroup;
        private RectTransform _caption;
        private TMP_Text _captionLabel;
        private AmbientMotion _captionMotion;

        private LocalizedLine _captionLine;
        private Coroutine _captionRoutine;

        /// <summary>Builds both overlays as a full-screen layer.</summary>
        public void Initialize(RectTransform parent)
        {
            _root = (RectTransform)transform;
            _root.SetParent(parent, false);
            Stretch(_root);

            BuildTitleCard();
            BuildCaption();
        }

        /// <summary>
        /// Fades an act's name up over a darkened screen, holds it, and clears.
        /// </summary>
        /// <param name="seasonNumber">1-based act number, shown as "Season 01".</param>
        /// <param name="actName">The act's title, already in the player's language.</param>
        /// <param name="hold">Seconds the card stays at full opacity.</param>
        public IEnumerator PlayTitleCard(int seasonNumber, string actName, float hold)
        {
            ApplyCardText(seasonNumber, actName);
            SetDividerVisible(true);

            AudioService.Play(SfxId.GiftBox, 0.9f);

            _cardGroup.alpha = 0f;
            _cardGroup.FadeTo(1f, CardFadeIn, EaseType.OutCubic, 0f, null, true);

            PlayCardEntrance();

            yield return StoryClock.Wait(CardFadeIn + hold);

            _cardGroup.FadeTo(0f, CardFadeOut, EaseType.InCubic, 0f, null, true);

            yield return StoryClock.Wait(CardFadeOut);
        }

        /// <summary>
        /// The closing card an act with no script yet shows before the game
        /// walks back to the title.
        /// </summary>
        /// <remarks>
        /// Same furniture as the title card, with the season line and the rule
        /// left out — there is no season to name, and a divider under a single
        /// sentence reads as a heading that lost its body.
        /// </remarks>
        public IEnumerator PlayClosingCard(string message, float hold)
        {
            StoryText.Set(_seasonLabel, string.Empty);
            StoryText.Set(_titleLabel, message);
            SetDividerVisible(false);

            AudioService.Play(SfxId.WishStar, 0.8f);

            _cardGroup.alpha = 0f;
            _cardGroup.FadeTo(1f, CardFadeIn, EaseType.OutCubic, 0f, null, true);

            PlayCardEntrance();

            yield return StoryClock.Wait(CardFadeIn + hold);

            _cardGroup.FadeTo(0f, CardFadeOut, EaseType.InCubic, 0f, null, true);

            yield return StoryClock.Wait(CardFadeOut);
        }

        /// <summary>
        /// Settles the three pieces of the card into place as it fades up.
        /// </summary>
        /// <remarks>
        /// A card that only fades reads as a screenshot appearing. Three small
        /// movements — the title growing the last tenth of its size, the season
        /// line drifting down onto it, and the rule drawing itself outward from
        /// the centre — read as something being presented. All three are pushed
        /// through the motion channels, so the idle sway underneath keeps
        /// running and nothing has to be reset afterwards.
        /// </remarks>
        private void PlayCardEntrance()
        {
            if (_titleMotion != null)
            {
                TweenRunner.Play(
                    CardFadeIn,
                    t =>
                    {
                        if (_titleMotion == null)
                        {
                            return;
                        }

                        float scale = Mathf.LerpUnclamped(TitleFromScale, 1f, t);
                        _titleMotion.ExtraScale = new Vector3(scale, scale, 1f);
                    },
                    EaseType.OutCubic,
                    0f,
                    () =>
                    {
                        if (_titleMotion != null)
                        {
                            _titleMotion.ExtraScale = Vector3.one;
                        }
                    },
                    this,
                    true,
                    true);
            }

            if (_seasonMotion != null)
            {
                TweenRunner.Play(
                    CardFadeIn,
                    t =>
                    {
                        if (_seasonMotion != null)
                        {
                            _seasonMotion.ExtraOffset = new Vector3(0f, Mathf.LerpUnclamped(SeasonRise, 0f, t), 0f);
                        }
                    },
                    EaseType.OutCubic,
                    0f,
                    () =>
                    {
                        if (_seasonMotion != null)
                        {
                            _seasonMotion.ExtraOffset = Vector3.zero;
                        }
                    },
                    this,
                    true,
                    true);
            }

            if (_divider != null && _divider.gameObject.activeSelf)
            {
                TweenRunner.Play(
                    CardFadeIn * 1.25f,
                    t =>
                    {
                        if (_divider != null)
                        {
                            _divider.sizeDelta = new Vector2(Mathf.LerpUnclamped(0f, DividerWidth, t), DividerHeight);
                        }
                    },
                    EaseType.OutCubic,
                    0f,
                    () =>
                    {
                        if (_divider != null)
                        {
                            _divider.sizeDelta = new Vector2(DividerWidth, DividerHeight);
                        }
                    },
                    this,
                    true,
                    true);
            }
        }

        private void SetDividerVisible(bool visible)
        {
            if (_divider == null)
            {
                return;
            }

            _divider.gameObject.SetActive(visible);
            _divider.sizeDelta = new Vector2(0f, DividerHeight);
        }

        /// <summary>
        /// Shows the name of a place in the corner and takes it away again.
        /// </summary>
        /// <remarks>
        /// Fire and forget: the caption runs alongside the dialogue rather than
        /// holding it up, because a player who already knows where they are
        /// should not have to wait for the game to tell them.
        /// </remarks>
        public void ShowCaption(LocalizedLine placeName, float hold)
        {
            if (placeName.IsEmpty)
            {
                return;
            }

            // The line is kept, not the string it resolved to. A caption sits on
            // screen for a couple of seconds and the player can change language
            // inside that window — and the name of a place is exactly the kind
            // of thing they change language to read.
            _captionLine = placeName;
            ApplyCaptionText();

            // Stopped by handle rather than by name: the string overload only
            // cancels coroutines that were also started by name, so mixing the
            // two silently leaves the previous caption running and the two fades
            // fight over the same CanvasGroup.
            if (_captionRoutine != null)
            {
                StopCoroutine(_captionRoutine);
            }

            _captionRoutine = StartCoroutine(CaptionRoutine(hold));
        }

        /// <summary>
        /// Re-reads the caption and moves it to the corner the new language
        /// reads from.
        /// </summary>
        /// <remarks>
        /// Both halves matter, and only the second one was here. A caption that
        /// moves to the other corner while still spelling the place name in the
        /// language the player just switched away from is worse than one that
        /// did not move at all.
        /// </remarks>
        public void RefreshLanguage()
        {
            ApplyCaptionText();
            ApplyCaptionDirection();
        }

        private IEnumerator CaptionRoutine(float hold)
        {
            _captionGroup.FadeTo(1f, CaptionFade, EaseType.OutCubic, 0f, null, true);
            SlideCaption(CaptionSlide, 0f, CaptionFade, EaseType.OutCubic);

            yield return StoryClock.Wait(CaptionFade + hold);

            _captionGroup.FadeTo(0f, CaptionFade, EaseType.InCubic, 0f, null, true);
            SlideCaption(0f, CaptionSlide, CaptionFade, EaseType.InCubic);
        }

        /// <summary>
        /// Slides the place name in from, and back out towards, the edge it is
        /// parked against.
        /// </summary>
        /// <remarks>
        /// It comes from the outside in both readings — the left in English and
        /// Japanese, the right in Persian — because the caption is arriving from
        /// off-screen, and which side that is depends on which corner it lives
        /// in.
        /// </remarks>
        private void SlideCaption(float from, float to, float duration, EaseType ease)
        {
            if (_captionMotion == null)
            {
                return;
            }

            float direction = LocalizationService.Current.IsRightToLeft() ? 1f : -1f;

            TweenRunner.Play(
                duration,
                t =>
                {
                    if (_captionMotion != null)
                    {
                        _captionMotion.ExtraOffset =
                            new Vector3(Mathf.LerpUnclamped(from, to, t) * direction, 0f, 0f);
                    }
                },
                ease,
                0f,
                () =>
                {
                    if (_captionMotion != null)
                    {
                        _captionMotion.ExtraOffset = new Vector3(to * direction, 0f, 0f);
                    }
                },
                this,
                true,
                true);
        }

        // ---------------------------------------------------------------------
        //  Construction
        // ---------------------------------------------------------------------

        private void BuildTitleCard()
        {
            GameObject host = new GameObject("ActTitleCard", typeof(RectTransform));
            host.transform.SetParent(_root, false);
            Stretch((RectTransform)host.transform);

            _cardGroup = host.AddComponent<CanvasGroup>();
            _cardGroup.alpha = 0f;
            _cardGroup.blocksRaycasts = false;
            _cardGroup.interactable = false;

            GameObject veil = new GameObject("Veil", typeof(RectTransform));
            veil.transform.SetParent(host.transform, false);
            Stretch((RectTransform)veil.transform);

            Image veilImage = veil.AddComponent<Image>();
            veilImage.sprite = ProceduralUiSprites.Solid(Color.white);
            veilImage.color = VeilColour;
            veilImage.raycastTarget = false;

            _seasonLabel = BuildCentredLabel(host.transform, "SeasonLabel", GameConfig.SeasonFontSize, SeasonColour, 96f);
            _divider = BuildDivider(host.transform);
            _titleLabel = BuildCentredLabel(host.transform, "ActTitleLabel", GameConfig.ActTitleFontSize, TitleColour, -20f);

            _titleMotion = AmbientMotion.GetOrAdd(_titleLabel.gameObject);
            _titleMotion.Configure(AmbientMotionProfile.Title, 0.4f);

            _seasonMotion = AmbientMotion.GetOrAdd(_seasonLabel.gameObject);
            _seasonMotion.Configure(AmbientMotionProfile.Text, 0.72f);
        }

        /// <summary>The hairline rule between the season number and the title.</summary>
        private static RectTransform BuildDivider(Transform parent)
        {
            GameObject host = new GameObject("TitleDivider", typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(0f, DividerHeight);
            rect.anchoredPosition = new Vector2(0f, 52f);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.Solid(Color.white);
            image.color = SeasonColour;
            image.raycastTarget = false;

            return rect;
        }

        private static TMP_Text BuildCentredLabel(
            Transform parent,
            string objectName,
            float fontSize,
            Color colour,
            float offsetY)
        {
            GameObject host = new GameObject(objectName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(1500f, 140f);
            rect.anchoredPosition = new Vector2(0f, offsetY);

            TMP_Text label = host.AddComponent<TextMeshProUGUI>();
            label.fontSize = fontSize;
            label.color = colour;
            label.alignment = TextAlignmentOptions.Center;
            label.textWrappingMode = TextWrappingModes.NoWrap;
            label.raycastTarget = false;

            StoryFonts.Apply(label);
            return label;
        }

        private void BuildCaption()
        {
            GameObject host = new GameObject("PlaceCaption", typeof(RectTransform));
            host.transform.SetParent(_root, false);

            _caption = (RectTransform)host.transform;
            _caption.sizeDelta = new Vector2(0f, 58f);

            _captionGroup = host.AddComponent<CanvasGroup>();
            _captionGroup.alpha = 0f;
            _captionGroup.blocksRaycasts = false;
            _captionGroup.interactable = false;

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.RoundedRect(20, CaptionFill, CaptionBorder, 2f);
            image.type = Image.Type.Sliced;
            image.raycastTarget = false;

            HorizontalLayoutGroup layout = host.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(30, 30, 2, 8);
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            ContentSizeFitter fitter = host.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            GameObject labelHost = new GameObject("CaptionLabel", typeof(RectTransform));
            labelHost.transform.SetParent(_caption, false);

            _captionLabel = labelHost.AddComponent<TextMeshProUGUI>();
            _captionLabel.fontSize = GameConfig.CaptionFontSize;
            _captionLabel.color = CaptionText;
            _captionLabel.alignment = TextAlignmentOptions.Midline;
            _captionLabel.textWrappingMode = TextWrappingModes.NoWrap;
            _captionLabel.raycastTarget = false;

            StoryFonts.Apply(_captionLabel);

            _captionMotion = AmbientMotion.GetOrAdd(host);
            _captionMotion.Configure(AmbientMotionProfile.Card, 0.58f);

            ApplyCaptionDirection();
        }

        private void ApplyCardText(int seasonNumber, string actName)
        {
            StoryText.Set(_seasonLabel, LocalizationService.Format(LocKeys.LoadSlotSeason, seasonNumber));
            StoryText.Set(_titleLabel, actName);
        }

        private void ApplyCaptionText()
        {
            if (_captionLine.IsEmpty)
            {
                return;
            }

            StoryText.Set(_captionLabel, _captionLine.Current());
        }

        /// <summary>
        /// Parks the caption in the upper corner the language reads from, so it
        /// sits where the eye already starts a line.
        /// </summary>
        private void ApplyCaptionDirection()
        {
            if (_caption == null)
            {
                return;
            }

            bool rightToLeft = LocalizationService.Current.IsRightToLeft();
            Vector2 anchor = new Vector2(rightToLeft ? 1f : 0f, 1f);

            _caption.anchorMin = anchor;
            _caption.anchorMax = anchor;
            _caption.pivot = anchor;
            _caption.anchoredPosition = new Vector2(rightToLeft ? -64f : 64f, -56f);

            // Position only. The caption drifts, so re-capturing its whole pose
            // here would fold the current beat of that drift into its rest pose
            // and walk it a little further from the corner on every language
            // change.
            _captionMotion?.RecapturePosition();
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
