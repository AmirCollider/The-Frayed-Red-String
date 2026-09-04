// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ProceduralSavePanel.cs
//
//  The save panel, for acts that were never given one.
//
//  MainMenu.unity and Act01.unity carry the panel as scene furniture, and act
//  two got it by having that furniture copied across by hand. Every act after
//  that did not, and the only sign of it was one warning in the console: the
//  pause menu still offered Save and Load, and both did nothing.
//
//  A panel that has to be copied between scenes is a panel that will be missing
//  from one of them. This builds the same hierarchy — the same object names, so
//  LoadPanelController and SaveSlotView find their pieces exactly as they do in
//  the authored version — out of shapes drawn at runtime, which is how every
//  other screen in this game is already made.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.SaveSystem;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>Builds a save panel matching the authored one, by name.</summary>
    public static class ProceduralSavePanel
    {
        private const float CardWidth = 1320f;
        private const float CardHeight = 800f;

        private const float SlotWidth = 1180f;
        private const float SlotHeight = 186f;
        private const float SlotGap = 22f;

        private const float ThumbnailWidth = 272f;
        private const float ThumbnailInset = 12f;

        /// <summary>How far past the screen edge the veil reaches.</summary>
        private const float Overscan = 64f;

        private static readonly Color Veil = new Color(0.04f, 0.02f, 0.04f, 0.72f);
        private static readonly Color CardFill = new Color(0.14f, 0.09f, 0.13f, 0.96f);
        private static readonly Color CardEdge = new Color(0.98f, 0.71f, 0.80f, 0.55f);
        private static readonly Color SlotFill = new Color(0.20f, 0.14f, 0.19f, 0.98f);
        private static readonly Color SlotEdge = new Color(0.98f, 0.78f, 0.85f, 0.42f);
        private static readonly Color Ink = new Color(0.98f, 0.94f, 0.96f, 1f);
        private static readonly Color FaintInk = new Color(0.86f, 0.78f, 0.83f, 1f);
        private static readonly Color EmptyThumbnail = new Color(0.30f, 0.30f, 0.30f, 1f);

        /// <summary>
        /// Builds the panel under <paramref name="layer"/> and returns it.
        /// </summary>
        /// <remarks>
        /// The caller adds <see cref="LoadPanelController"/> and initialises it,
        /// exactly as it would for the authored panel — this only produces the
        /// objects, so there is one code path that runs the panel and not two.
        /// </remarks>
        public static GameObject Build(RectTransform layer)
        {
            GameObject panel = new GameObject(ObjectNames.LoadPanel, typeof(RectTransform));
            panel.transform.SetParent(layer, false);

            Stretch((RectTransform)panel.transform);

            BuildVeil(panel.transform);
            RectTransform card = BuildCard(panel.transform);

            BuildTitle(card);

            for (int slot = 1; slot <= SaveService.SlotCount; slot++)
            {
                BuildSlot(card, slot);
            }

            return panel;
        }

        /// <summary>
        /// The full-screen dim behind the panel.
        /// </summary>
        /// <remarks>
        /// Deliberately larger than the screen. The panel above it breathes —
        /// the controller gives the whole panel a card's idle drift, sway and
        /// one per cent of scale — and this is a child of it, so a veil cut
        /// exactly to the screen would show a moving strip of bare game down one
        /// edge. Sixty-four units of overscan is more than the motion can ever
        /// use.
        /// </remarks>
        private static void BuildVeil(Transform parent)
        {
            GameObject host = new GameObject(ObjectNames.LoadPanelSheet, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = new Vector2(-Overscan, -Overscan);
            rect.offsetMax = new Vector2(Overscan, Overscan);

            Image sheet = host.AddComponent<Image>();
            sheet.color = Veil;

            // Clicks that land here belong to the panel's own backdrop, which
            // LoadPanelController puts underneath it to close on.
            sheet.raycastTarget = false;
        }

        private static RectTransform BuildCard(Transform parent)
        {
            GameObject host = new GameObject(ObjectNames.LoadPanelCard, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(CardWidth, CardHeight);
            rect.anchoredPosition = Vector2.zero;

            Image card = host.AddComponent<Image>();
            card.sprite = ProceduralUiSprites.RoundedRect(30, CardFill, CardEdge, 3f);
            card.type = Image.Type.Sliced;

            // The card blocks: a click meant for a slot that misses it should
            // not fall through to the backdrop and shut the panel.
            card.raycastTarget = true;

            return rect;
        }

        private static void BuildTitle(RectTransform card)
        {
            TMP_Text title = Label(card, ObjectNames.LoadPanelTitle, GameConfig.ActTitleFontSize * 0.62f, Ink);

            RectTransform rect = title.rectTransform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.offsetMin = new Vector2(40f, 0f);
            rect.offsetMax = new Vector2(-40f, -22f);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 74f);

            title.alignment = TextAlignmentOptions.Midline;

            // Bound by LoadPanelController, which re-titles it as the mode
            // changes. Adding the component here means the binding survives the
            // first frame, before the controller has run.
            UnityUtility.GetOrAdd<LocalizedText>(title.gameObject).Bind(LocKeys.LoadPanelTitle);
        }

        /// <summary>
        /// One slot card, with the five children <see cref="SaveSlotView"/>
        /// resolves by name.
        /// </summary>
        /// <remarks>
        /// The three detail labels are created active. The authored panel keeps
        /// them inactive and the view switches them on once; either way it is the
        /// <see cref="TMP_Text"/> component's own enabled flag that decides what
        /// is readable from then on.
        /// </remarks>
        private static void BuildSlot(RectTransform card, int slotNumber)
        {
            GameObject host = new GameObject(
                ObjectNames.SaveSlotPrefix + slotNumber.ToString("00"),
                typeof(RectTransform));

            host.transform.SetParent(card, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(SlotWidth, SlotHeight);

            // Below the title, then one row per slot.
            rect.anchoredPosition = new Vector2(
                0f,
                -(122f + (slotNumber - 1) * (SlotHeight + SlotGap)));

            Image background = host.AddComponent<Image>();
            background.sprite = ProceduralUiSprites.RoundedRect(22, SlotFill, SlotEdge, 2f);
            background.type = Image.Type.Sliced;
            background.raycastTarget = true;

            Button button = host.AddComponent<Button>();
            button.targetGraphic = background;

            // Built after the scene's audio sweep has already run, so the hover
            // and press sounds are attached here rather than left to it.
            UnityUtility.GetOrAdd<SelectableSfx>(host);

            BuildThumbnail(rect);
            BuildSlotLabels(rect);
        }

        private static void BuildThumbnail(RectTransform slot)
        {
            GameObject host = new GameObject(ObjectNames.SlotScreenshot, typeof(RectTransform));
            host.transform.SetParent(slot, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.offsetMin = new Vector2(ThumbnailInset, ThumbnailInset);
            rect.offsetMax = new Vector2(ThumbnailInset + ThumbnailWidth, -ThumbnailInset);

            Image thumbnail = host.AddComponent<Image>();

            // A drawn plate is what an empty slot shows, and what a slot whose
            // background could not be found falls back to. SaveSlotView captures
            // whatever is here as the authored thumbnail, so this is that.
            thumbnail.sprite = ProceduralUiSprites.RoundedRect(12, Color.white);
            thumbnail.type = Image.Type.Sliced;
            thumbnail.color = EmptyThumbnail;
            thumbnail.raycastTarget = false;
            thumbnail.preserveAspect = false;
        }

        private static void BuildSlotLabels(RectTransform slot)
        {
            float textLeft = ThumbnailInset * 2f + ThumbnailWidth;

            TMP_Text empty = Label(slot, ObjectNames.SlotEmptyLabel, GameConfig.SeasonFontSize, FaintInk);
            Fill(empty.rectTransform, textLeft, 0f, 0f, 0f);
            empty.alignment = TextAlignmentOptions.Midline;

            TMP_Text season = Label(slot, ObjectNames.SlotSeasonLabel, GameConfig.CaptionFontSize, FaintInk);
            Row(season.rectTransform, textLeft, 1f, 16f, 46f);

            TMP_Text seasonName = Label(slot, ObjectNames.SlotSeasonNameLabel, GameConfig.SeasonFontSize, Ink);
            Row(seasonName.rectTransform, textLeft, 0.5f, 0f, 56f);

            TMP_Text playTime = Label(slot, ObjectNames.SlotPlayTimeLabel, GameConfig.CaptionFontSize, FaintInk);
            Row(playTime.rectTransform, textLeft, 0f, -16f, 46f);
        }

        private static TMP_Text Label(Transform parent, string labelName, float size, Color colour)
        {
            GameObject host = new GameObject(labelName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            TMP_Text label = host.AddComponent<TextMeshProUGUI>();
            label.fontSize = size;
            label.color = colour;
            label.raycastTarget = false;
            label.alignment = TextAlignmentOptions.Left;
            label.textWrappingMode = TextWrappingModes.NoWrap;
            label.overflowMode = TextOverflowModes.Ellipsis;

            // Rule three: every label built in code takes its font here, before
            // anything writes to it.
            StoryFonts.Apply(label);

            return label;
        }

        /// <summary>Stretches a label across the slot, inset from the thumbnail.</summary>
        private static void Fill(RectTransform rect, float left, float right, float bottom, float top)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(-right - 18f, -top);
        }

        /// <summary>One line of a slot card, pinned at a height within it.</summary>
        private static void Row(RectTransform rect, float left, float anchorY, float offsetY, float height)
        {
            rect.anchorMin = new Vector2(0f, anchorY);
            rect.anchorMax = new Vector2(1f, anchorY);
            rect.pivot = new Vector2(0.5f, anchorY);
            rect.offsetMin = new Vector2(left, 0f);
            rect.offsetMax = new Vector2(-24f, 0f);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, offsetY);
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
