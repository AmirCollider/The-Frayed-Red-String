// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryFrameView.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Narrative;
using TheFrayedRedString.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// The frame the story is played inside, and the moment it comes off.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The design document asks for a picture that is boxed in from the first
    /// act and stays that way until a character reaches out and widens it — the
    /// point where the game stops pretending the player is watching something
    /// safe. Two bars down the sides, holding the picture at a narrower aspect
    /// than the window, are enough to make that read: nobody consciously
    /// notices the frame while it is there, and everybody notices it leaving.
    /// </para>
    /// <para>
    /// The menus are deliberately never framed. The frame has to belong to the
    /// story for its removal to mean anything, and a title screen with black
    /// bars just looks like a broken resolution.
    /// </para>
    /// <para>
    /// Opening is driven by an <c>OpenFrame</c> beat rather than by an act
    /// number, so which act does it — and whether it ever closes again — is a
    /// decision made in the Story Editor and not in code.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class StoryFrameView : MonoBehaviour
    {
        private RectTransform _left;
        private RectTransform _right;
        private TweenHandle _animation;

        private float _closedAspect = 4f / 3f;
        private float _openness;

        /// <summary>0 while the frame is fully closed, 1 once it is fully open.</summary>
        public float Openness => _openness;

        /// <summary>Builds the bars as a full-screen layer.</summary>
        public void Initialize(RectTransform parent, StageSettings settings)
        {
            RectTransform root = (RectTransform)transform;
            root.SetParent(parent, false);
            Stretch(root);

            _closedAspect = Mathf.Max(0.5f, settings != null ? settings.FrameAspect : 4f / 3f);
            Color colour = settings != null ? settings.FrameColour : Color.black;

            _left = CreateBar("FrameBarLeft", root, colour, true);
            _right = CreateBar("FrameBarRight", root, colour, false);

            bool framed = settings == null || settings.FrameEnabled;
            SetOpenness(framed ? 0f : 1f);
        }

        /// <summary>Widens the frame until it is gone.</summary>
        public void Open(float duration)
        {
            AnimateTo(1f, duration);
        }

        /// <summary>Brings the frame back.</summary>
        public void Close(float duration)
        {
            AnimateTo(0f, duration);
        }

        /// <summary>Sets the frame without animating.</summary>
        public void SetOpenness(float openness)
        {
            _animation.CancelIfRunning();
            Apply(openness);
        }

        private void AnimateTo(float target, float duration)
        {
            _animation.CancelIfRunning();

            if (duration <= 0f)
            {
                Apply(target);
                return;
            }

            float from = _openness;

            _animation = TweenRunner.Play(
                duration,
                t => Apply(Mathf.LerpUnclamped(from, target, t)),

                // Slow to start and slow to finish. The frame coming off is a
                // moment the player is meant to watch happen, not an effect
                // that snaps.
                EaseType.InOutCubic,
                0f,
                () => Apply(target),
                this);
        }

        private void Apply(float openness)
        {
            _openness = Mathf.Clamp01(openness);

            if (_left == null || _right == null)
            {
                return;
            }

            // Width of one bar as a fraction of the screen, when the picture is
            // held at the closed aspect inside a 16:9 window. At openness 1 the
            // bars have no width left and the picture is the whole window.
            float screenAspect = 16f / 9f;
            float closedFraction = Mathf.Clamp01((1f - _closedAspect / screenAspect) * 0.5f);
            float fraction = Mathf.Lerp(closedFraction, 0f, _openness);

            _left.anchorMin = new Vector2(0f, 0f);
            _left.anchorMax = new Vector2(fraction, 1f);
            _left.offsetMin = Vector2.zero;
            _left.offsetMax = Vector2.zero;

            _right.anchorMin = new Vector2(1f - fraction, 0f);
            _right.anchorMax = new Vector2(1f, 1f);
            _right.offsetMin = Vector2.zero;
            _right.offsetMax = Vector2.zero;
        }

        private static RectTransform CreateBar(string barName, RectTransform parent, Color colour, bool left)
        {
            GameObject host = new GameObject(barName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.pivot = new Vector2(left ? 0f : 1f, 0.5f);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.Solid(Color.white);
            image.color = colour;

            // The bars are scenery, not a wall. A click that lands on one is
            // still a click on the story and should turn the page.
            image.raycastTarget = false;

            return rect;
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
