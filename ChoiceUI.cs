// ==========================================
// ChoiceUI - Branching Choice Button Panel
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUI : MonoBehaviour
{
    // ==========================================
    // Inspector — Panel Root
    // ==========================================
    [Header("Panel")]
    [SerializeField] private CanvasGroup panelGroup;

    // ==========================================
    // Inspector — Button Instantiation
    // ==========================================
    [Header("Button Template")]
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Transform buttonContainer;

    // ==========================================
    // Inspector — Animation
    // ==========================================
    [Header("Animation")]
    [SerializeField] private float fadeInDuration = 0.20f;
    [SerializeField] private float fadeOutDuration = 0.15f;
    [SerializeField] private float staggerDelay = 0.06f;

    // ==========================================
    // Inspector — Choice SFX (UIClick AudioEvent)
    // ==========================================
    [Header("Choice SFX")]
    [SerializeField] private AudioEvent clickSfx;

    // ==========================================
    // Inspector — Color Profiles (GDD §1.3 — Blue/Green/White)
    // ==========================================
    [Header("Color Profiles")]
    [SerializeField] private Color greenButtonColor = new Color(0.18f, 0.55f, 0.32f, 0.96f);
    [SerializeField] private Color blueButtonColor = new Color(0.20f, 0.42f, 0.70f, 0.96f);
    [SerializeField] private Color whiteButtonColor = new Color(0.86f, 0.86f, 0.90f, 0.96f);
    [SerializeField] private Color darkLabelColor = new Color(0.10f, 0.10f, 0.14f, 1f);
    [SerializeField] private Color lightLabelColor = new Color(0.97f, 0.97f, 1f, 1f);

    // ==========================================
    // Inspector — Button Background Art (GDD §1.3 — Green/Blue sprite skins)
    //   Assign BackGrundGreenButton.png and BackGrundBloueButton.png. When a
    //   sprite is set it replaces the flat color; flat color is only a fallback.
    // ==========================================
    [Header("Button Background Sprites")]
    [SerializeField] private Sprite greenButtonSprite;
    [SerializeField] private Sprite blueButtonSprite;
    [SerializeField] private Sprite whiteButtonSprite;

    // ==========================================
    // Inspector — Design Rule Guard (every branch should be exactly 2 options)
    // ==========================================
    [Header("Validation")]
    [SerializeField] private bool warnIfNotTwoOptions = true;

    // ==========================================
    // Private State
    // ==========================================
    private readonly List<GameObject> _activeButtons = new List<GameObject>();
    private Action<int> _onChosen;
    private Coroutine _fadeCo;

    // ==========================================
    // Awake - Force Hidden on Scene Load
    // ==========================================
    private void Awake()
    {
        ForceHidden();
    }

    // ==========================================
    // Present - Instantiate Choice Buttons and Register Callback
    // ==========================================
    public void Present(List<DialogueChoice> choices, string branchId, Action<int> onChosen)
    {
        if (choices == null || choices.Count == 0) { onChosen?.Invoke(-1); return; }

        // ==========================================
        // Design Rule — Branches must present exactly two options (Green + Blue)
        // ==========================================
        if (warnIfNotTwoOptions && choices.Count != 2)
            Debug.LogWarning($"[ChoiceUI] Branch '{branchId}' has {choices.Count} options. Design rule: exactly 2 (one Green, one Blue).");
        _onChosen = onChosen;
        ClearButtons();
        gameObject.SetActive(true);
        EnsureContainerLayout();

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.interactable = true;
            panelGroup.blocksRaycasts = true;
        }

        for (int i = 0; i < choices.Count; i++)
        {
            int capturedIdx = i;
            GameObject btn = Instantiate(choiceButtonPrefab, buttonContainer);

            // ==========================================
            // Label — Language-Appropriate Text + Color-Matched Foreground
            // ==========================================
            TextMeshProUGUI lbl = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (lbl != null)
            {
                lbl.text = choices[i].GetLabel();
                lbl.color = choices[i].color == ChoiceColor.White ? darkLabelColor : lightLabelColor;

                // ==========================================
                // Phase Font — Pull the language-correct TMP font so choice
                // labels match the active act (handled here because buttons
                // are instantiated at runtime, after LocalizedFontController)
                // ==========================================
                if (LocalizedFontController.Instance != null && LocalizedFontController.Instance.CurrentFont != null)
                    lbl.font = LocalizedFontController.Instance.CurrentFont;
            }

            // ==========================================
            // Background — Guarantee an Image, then apply the color-coded button
            // art sprite (BackGrundGreenButton / BackGrundBloueButton). Flat color
            // is only a fallback when no sprite is assigned (GDD §1.3)
            // ==========================================
            Image bg = btn.GetComponent<Image>();
            if (bg == null) bg = btn.AddComponent<Image>();
            Sprite bgSprite = SpriteForChoice(choices[i].color);
            if (bgSprite != null)
            {
                bg.sprite = bgSprite;
                bg.type = Image.Type.Sliced;
                bg.color = Color.white;
            }
            else
            {
                bg.sprite = null;
                bg.color = ColorForChoice(choices[i].color);
            }
            bg.raycastTarget = true;
            // ==========================================
            // Per-Button Box — force a real row so the Vertical Layout Group
            // places distinct colored buttons (never a centered dry-text stack)
            // ==========================================
            EnsureButtonLayout(btn, lbl);

            Button b = btn.GetComponent<Button>();
            if (b != null)
                b.onClick.AddListener(() => OnChoiceSelected(branchId, capturedIdx));

            _activeButtons.Add(btn);

            // ==========================================
            // Layout-Safe Reveal — Scale + Alpha only (never anchoredPosition),
            // so the Vertical Layout Group keeps full control of placement (no overlap)
            // ==========================================
            StartCoroutine(RevealButton(btn, i * staggerDelay));
        }

        FadeTo(1f, fadeInDuration);
    }

    // ==========================================
    // OnChoiceSelected - Record to BranchRecord, Fade Out, Fire Callback
    // ==========================================
    private void OnChoiceSelected(string branchId, int index)
    {
        clickSfx?.Play();

        if (BranchRecord.Instance != null)
            BranchRecord.Instance.Record(branchId, index);

        Action<int> cb = _onChosen;
        _onChosen = null;

        if (_fadeCo != null) StopCoroutine(_fadeCo);
        _fadeCo = StartCoroutine(HideAndCallback(cb, index));
    }

    // ==========================================
    // HideAndCallback - Fade Out Then Invoke Choice Callback
    // ==========================================
    private IEnumerator HideAndCallback(Action<int> cb, int index)
    {
        float start = panelGroup != null ? panelGroup.alpha : 1f;
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            if (panelGroup != null) panelGroup.alpha = Mathf.Lerp(start, 0f, t / fadeOutDuration);
            yield return null;
        }
        ForceHidden();
        cb?.Invoke(index);
    }

    // ==========================================
    // RevealButton - Layout-Safe Stagger: CanvasGroup Alpha + Local Scale Pop
    // ==========================================
    private IEnumerator RevealButton(GameObject btn, float delay)
    {
        CanvasGroup cg = btn.GetComponent<CanvasGroup>();
        if (cg == null) cg = btn.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        RectTransform rt = btn.GetComponent<RectTransform>();
        Vector3 fullScale = rt != null ? rt.localScale : Vector3.one;
        if (rt != null) rt.localScale = fullScale * 0.92f;

        if (delay > 0f) yield return new WaitForSeconds(delay);

        float dur = 0.16f;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / dur);
            cg.alpha = p;
            if (rt != null) rt.localScale = Vector3.Lerp(fullScale * 0.92f, fullScale, p);
            yield return null;
        }

        cg.alpha = 1f;
        if (rt != null) rt.localScale = fullScale;
    }

    // ==========================================

    // ColorForChoice - Map ChoiceColor Enum to Configured Button Background Color

    // ==========================================

    private Color ColorForChoice(ChoiceColor color)

    {

        switch (color)

        {

            case ChoiceColor.Green: return greenButtonColor;

            case ChoiceColor.Blue: return blueButtonColor;

            default: return whiteButtonColor;

        }

    }
    // ==========================================
    // SpriteForChoice - Map ChoiceColor Enum to the Configured Button Art Sprite
    // ==========================================
    private Sprite SpriteForChoice(ChoiceColor color)
    {
        switch (color)
        {
            case ChoiceColor.Green: return greenButtonSprite;
            case ChoiceColor.Blue: return blueButtonSprite;
            default: return whiteButtonSprite;
        }
    }

    // ==========================================
    // EnsureContainerLayout - Force a Vertical Layout Group + Content Size Fitter
    // on the button container so options stack centered with no overlap (Batch 2)
    // ==========================================
    private void EnsureContainerLayout()
    {
        if (buttonContainer == null) return;

        VerticalLayoutGroup vlg = buttonContainer.GetComponent<VerticalLayoutGroup>();
        if (vlg == null) vlg = buttonContainer.gameObject.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 12f;
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = false;
        vlg.childForceExpandHeight = false;
        vlg.padding = new RectOffset(8, 8, 8, 8);

        ContentSizeFitter csf = buttonContainer.GetComponent<ContentSizeFitter>();
        if (csf == null) csf = buttonContainer.gameObject.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }

    // ==========================================
    // EnsureButtonLayout - Size the Button to the 700x140 Art + Auto-Fit Label
    //   Button matches the BackGrund*Button.png art (700x140) so it is never
    //   over-wide; the label is stretched to fill the button, inset on all four
    //   sides for the frame, and auto-sized so text always fits inside.
    // ==========================================
    private void EnsureButtonLayout(GameObject btn, TextMeshProUGUI lbl)
    {
        LayoutElement le = btn.GetComponent<LayoutElement>();
        if (le == null) le = btn.AddComponent<LayoutElement>();
        le.minWidth = 700f;
        le.preferredWidth = 700f;
        le.minHeight = 140f;
        le.preferredHeight = 140f;
        le.flexibleWidth = 0f;
        le.flexibleHeight = 0f;
        if (lbl != null)
        {
            // ==========================================
            // Stretch the label to fill the button so auto-sizing has the full
            // box to work with, then inset it for the button frame border
            // ==========================================
            RectTransform lblRt = lbl.rectTransform;
            lblRt.anchorMin = Vector2.zero;
            lblRt.anchorMax = Vector2.one;
            lblRt.offsetMin = Vector2.zero;
            lblRt.offsetMax = Vector2.zero;

            lbl.enableAutoSizing = true;
            lbl.fontSizeMin = 16f;
            lbl.fontSizeMax = 40f;
            lbl.alignment = TextAlignmentOptions.Center;
            lbl.textWrappingMode = TMPro.TextWrappingModes.Normal;
            lbl.overflowMode = TextOverflowModes.Ellipsis;
            lbl.margin = new Vector4(40f, 22f, 40f, 22f);
            lbl.raycastTarget = false;
        }
    }

    // ==========================================
    // ClearButtons - Destroy All Instantiated Choice Buttons
    // ==========================================
    private void ClearButtons()
    {
        foreach (GameObject btn in _activeButtons)
            if (btn != null) Destroy(btn);
        _activeButtons.Clear();
    }

    // ==========================================
    // FadeTo - Interrupt Active Fade and Start New One
    // ==========================================
    private void FadeTo(float target, float dur)
    {
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        float from = panelGroup != null ? panelGroup.alpha : 0f;
        _fadeCo = StartCoroutine(FadeCoroutine(from, target, dur));
    }

    // ==========================================
    // FadeCoroutine - Lerp CanvasGroup Alpha
    // ==========================================
    private IEnumerator FadeCoroutine(float from, float to, float dur)
    {
        if (panelGroup == null) yield break;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            panelGroup.alpha = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        panelGroup.alpha = to;
        _fadeCo = null;
    }

    // ==========================================
    // ForceHidden - Instant Zero-Alpha, No Interaction, Deactivate
    // ==========================================
    private void ForceHidden()
    {
        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
        }
        ClearButtons();
        gameObject.SetActive(false);
    }
}