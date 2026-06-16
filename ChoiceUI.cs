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
    [SerializeField] private float buttonSlideOffset = 18f;

    // ==========================================
    // Inspector — Choice SFX (UIClick AudioEvent)
    // ==========================================
    [Header("Choice SFX")]
    [SerializeField] private AudioEvent clickSfx;

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

        _onChosen = onChosen;
        ClearButtons();
        gameObject.SetActive(true);

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

            TextMeshProUGUI lbl = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (lbl != null) lbl.text = choices[i].GetLabel();

            Button b = btn.GetComponent<Button>();
            if (b != null)
                b.onClick.AddListener(() => OnChoiceSelected(branchId, capturedIdx));

            _activeButtons.Add(btn);

            if (i > 0)
                StartCoroutine(StaggerButton(btn, i * staggerDelay, buttonSlideOffset));
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
    // StaggerButton - Delay Visibility and Slide Each Button Up
    // ==========================================
    private IEnumerator StaggerButton(GameObject btn, float delay, float slideOffset)
    {
        CanvasGroup cg = btn.GetComponent<CanvasGroup>();
        if (cg == null) cg = btn.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        yield return new WaitForSeconds(delay);

        RectTransform rt = btn.GetComponent<RectTransform>();
        Vector2 origin = rt != null ? rt.anchoredPosition : Vector2.zero;
        if (rt != null) rt.anchoredPosition = origin - new Vector2(0f, slideOffset);

        float dur = 0.16f;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / dur);
            cg.alpha = p;
            if (rt != null) rt.anchoredPosition = Vector2.Lerp(origin - new Vector2(0f, slideOffset), origin, p);
            yield return null;
        }

        cg.alpha = 1f;
        if (rt != null) rt.anchoredPosition = origin;
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