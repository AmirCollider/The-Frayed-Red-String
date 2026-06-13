// ==========================================
// PhotorealisticSwitch - Reveal Photorealistic Break-Through Image via Dedicated Canvas
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.UI;

public class PhotorealisticSwitch : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (parent FrameSystem handles DontDestroyOnLoad)
    // ==========================================
    public static PhotorealisticSwitch Instance { get; private set; }

    // ==========================================
    // Inspector - Break Canvas (Sort Order must be > FrameCanvas Sort Order)
    // ==========================================
    [Header("Break Canvas (Screen Space Overlay, Sort Order > 5)")]
    [SerializeField] private Canvas breakCanvas;

    // ==========================================
    // Inspector - Photorealistic Image
    // ==========================================
    [Header("Photorealistic Image")]
    [SerializeField] private Image breakImage;
    [SerializeField] private Sprite photoRealisticSprite;

    // ==========================================
    // Inspector - Break Image Transform in 1920x1080 Canvas Space
    // ==========================================
    [Header("Break Image Transform (design at 1920x1080 — tune per art)")]
    [SerializeField] private Vector2 breakAnchoredPosition = new Vector2(-700f, 200f);
    [SerializeField] private Vector2 breakSizeDelta = new Vector2(600f, 900f);

    // ==========================================
    // Private State
    // ==========================================
    private bool _isActive;

    // ==========================================
    // Awake - Register Instance and Ensure Break Canvas Starts Deactivated
    // ==========================================
    private void Awake()
    {
        Instance = this;

        if (breakCanvas != null)
            breakCanvas.gameObject.SetActive(false);
    }

    // ==========================================
    // SwapToPhotorealistic - Enable Break Canvas, Apply Sprite, Position Image
    // ==========================================
    public void SwapToPhotorealistic()
    {
        if (_isActive) return;
        _isActive = true;

        if (breakCanvas != null)
            breakCanvas.gameObject.SetActive(true);

        if (breakImage != null)
        {
            if (photoRealisticSprite != null)
                breakImage.sprite = photoRealisticSprite;

            breakImage.rectTransform.anchoredPosition = breakAnchoredPosition;
            breakImage.rectTransform.sizeDelta = breakSizeDelta;
        }
    }

    // ==========================================
    // RestoreStandard - Deactivate Break Canvas and Reset State
    // ==========================================
    public void RestoreStandard()
    {
        if (!_isActive) return;
        _isActive = false;

        if (breakCanvas != null)
            breakCanvas.gameObject.SetActive(false);
    }
}