// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryOverlayView.cs
// -----------------------------------------------------------------------------

using System.Collections;
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

        private CanvasGroup _captionGroup;
        private RectTransform _caption;
        private TMP_Text _captionLabel;

        private string _captionText;
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

            AudioService.Play(SfxId.GiftBox, 0.9f);

            _cardGroup.alpha = 0f;
            _cardGroup.FadeTo(1f, CardFadeIn, EaseType.OutCubic);

            yield return new WaitForSecondsRealtime(CardFadeIn + Mathf.Max(0f, hold));

            _cardGroup.FadeTo(0f, CardFadeOut, EaseType.InCubic);

            yield return new WaitForSecondsRealtime(CardFadeOut);
        }

        /// <summary>
        /// Shows the name of a place in the corner and takes it away again.
        /// </summary>
        /// <remarks>
        /// Fire and forget: the caption runs alongside the dialogue rather than
        /// holding it up, because a player who already knows where they are
        /// should not have to wait for the game to tell them.
        /// </remarks>
        public void ShowCaption(string placeName, float hold)
        {
            if (string.IsNullOrEmpty(placeName))
            {
                return;
            }

            _captionText = placeName;
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

        /// <summary>Moves the caption to the corner the new language reads from.</summary>
        public void RefreshLanguage()
        {
            ApplyCaptionDirection();
        }

        private IEnumerator CaptionRoutine(float hold)
        {
            _captionGroup.FadeTo(1f, CaptionFade, EaseType.OutCubic);

            yield return new WaitForSecondsRealtime(CaptionFade + Mathf.Max(0f, hold));

            _captionGroup.FadeTo(0f, CaptionFade, EaseType.InCubic);
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

            _seasonLabel = BuildCentredLabel(host.transform, "SeasonLabel", GameConfig.SeasonFontSize, SeasonColour, 62f);
            _titleLabel = BuildCentredLabel(host.transform, "ActTitleLabel", GameConfig.ActTitleFontSize, TitleColour, -32f);

            AmbientMotion motion = AmbientMotion.GetOrAdd(_titleLabel.gameObject);
            motion.Configure(AmbientMotionProfile.Title, 0.4f);
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

            ApplyCaptionDirection();
        }

        private void ApplyCardText(int seasonNumber, string actName)
        {
            StoryText.Set(_seasonLabel, LocalizationService.Format(LocKeys.LoadSlotSeason, seasonNumber));
            StoryText.Set(_titleLabel, actName);
        }

        private void ApplyCaptionText()
        {
            StoryText.Set(_captionLabel, _captionText);
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
