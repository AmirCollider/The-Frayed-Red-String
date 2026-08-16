// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ScreenFader.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// A full-screen black curtain used for every scene transition.
    /// </summary>
    /// <remarks>
    /// Building the curtain in code rather than authoring it per scene means a
    /// transition looks identical everywhere, and a scene added later inherits
    /// it for free. The canvas is created once and survives scene loads, so the
    /// screen stays covered across the load itself.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class ScreenFader : MonoBehaviour
    {
        /// <summary>
        /// Draw order for the curtain. Above every gameplay canvas, so nothing
        /// can ever render on top of a fade.
        /// </summary>
        private const int CurtainSortingOrder = 30000;

        private static ScreenFader _instance;

        private CanvasGroup _group;
        private Image _curtain;
        private TweenHandle _activeFade;

        /// <summary>The live fader, or <c>null</c> before boot.</summary>
        public static ScreenFader Instance => _instance;

        /// <summary>True while the screen is fully or partially covered.</summary>
        public static bool IsCovering => _instance != null && _instance._group.alpha > 0.001f;

        /// <summary>
        /// True once anything has asked for the curtain to be raised.
        /// </summary>
        /// <remarks>
        /// The boot safety net watches this rather than watching whether the
        /// screen is still dark. A slow first frame — and the first frame of
        /// this game builds a 7 MB font atlas — can leave a legitimate fade
        /// still running when the net checks, which would fire a warning about
        /// a problem that does not exist. Asking "did anyone take
        /// responsibility?" is the question that actually needs answering.
        /// </remarks>
        public static bool RevealRequested { get; private set; }

        /// <summary>Creates the curtain under the persistent service host.</summary>
        public static ScreenFader Install(GameObject host)
        {
            if (_instance != null || host == null)
            {
                return _instance;
            }

            _instance = host.AddComponent<ScreenFader>();
            return _instance;
        }

        /// <summary>Clears the cached instance between play sessions.</summary>
        public static void ResetStatics()
        {
            _instance = null;
            RevealRequested = false;
        }

        /// <summary>Fades from black to the scene.</summary>
        public static void FadeIn(float duration, Action onComplete = null)
        {
            RevealRequested = true;

            if (_instance == null)
            {
                onComplete?.Invoke();
                return;
            }

            _instance.FadeTo(0f, duration, EaseType.InOutSine, onComplete);
        }

        /// <summary>Fades from the scene to black.</summary>
        public static void FadeOut(float duration, Action onComplete = null)
        {
            if (_instance == null)
            {
                onComplete?.Invoke();
                return;
            }

            _instance.FadeTo(1f, duration, EaseType.InOutSine, onComplete);
        }

        /// <summary>Snaps the curtain to an alpha with no animation.</summary>
        public static void SetInstant(float alpha)
        {
            if (_instance == null)
            {
                return;
            }

            _instance._activeFade.CancelIfRunning();
            _instance.ApplyAlpha(Mathf.Clamp01(alpha));
        }

        /// <summary>Tint of the curtain. Black by default.</summary>
        public static void SetColor(Color color)
        {
            if (_instance == null || _instance._curtain == null)
            {
                return;
            }

            float alpha = _instance._curtain.color.a;
            color.a = alpha;
            _instance._curtain.color = color;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            BuildCurtain();

            // Boot behind a black screen: the first scene fades up rather than
            // popping in mid-layout.
            ApplyAlpha(1f);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void BuildCurtain()
        {
            GameObject canvasHost = new GameObject("ScreenFaderCanvas");
            canvasHost.transform.SetParent(transform, false);

            Canvas canvas = canvasHost.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = CurtainSortingOrder;

            // The curtain needs its own raycaster to swallow clicks. A
            // CanvasGroup alone would not do it: blocksRaycasts only governs
            // graphics inside this canvas, and cannot stop the menu's canvas
            // underneath from receiving the press. Being the top-most raycast
            // target on the top-most canvas is what actually blocks input.
            canvasHost.AddComponent<GraphicRaycaster>();

            _group = canvasHost.AddComponent<CanvasGroup>();
            _group.interactable = false;

            GameObject curtainHost = new GameObject("Curtain");
            curtainHost.transform.SetParent(canvasHost.transform, false);

            _curtain = curtainHost.AddComponent<Image>();
            _curtain.color = Color.black;

            RectTransform rect = _curtain.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void FadeTo(float target, float duration, EaseType ease, Action onComplete)
        {
            _activeFade.CancelIfRunning();

            // While a fade is running the player must not be able to click
            // through to a scene that is on its way out.
            _group.blocksRaycasts = true;

            float from = _group.alpha;

            _activeFade = TweenRunner.Play(
                duration,
                t => ApplyAlpha(Mathf.LerpUnclamped(from, target, t)),
                ease,
                0f,
                () =>
                {
                    ApplyAlpha(target);
                    onComplete?.Invoke();
                },
                this);
        }

        private void ApplyAlpha(float alpha)
        {
            _group.alpha = alpha;

            // A fully transparent curtain steps out of the way entirely; a
            // visible one keeps swallowing input.
            bool covering = alpha > 0.001f;
            _group.blocksRaycasts = covering;

            if (_curtain != null)
            {
                _curtain.raycastTarget = covering;
            }
        }
    }
}
