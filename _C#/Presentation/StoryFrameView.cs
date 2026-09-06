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
    /// safe. Nobody consciously notices the frame while it is there, and
    /// everybody notices it leaving.
    /// </para>
    /// <para>
    /// It is a border on all four sides, not two bars down the edges. Two bars
    /// read as a letterbox — as something that happened to the picture — where
    /// a border reads as something somebody put around it. The bars are derived
    /// from one aspect and one margin so the four sides can never disagree.
    /// </para>
    /// <para>
    /// The frame also owns <see cref="Content"/>: the rect everything the player
    /// reads is laid out inside. That is the other half of the same idea. A
    /// dialogue box that runs on underneath the bars is not framed, it is
    /// clipped, and the missing half-word at each end reads as a bug rather than
    /// as a boundary. Because the content rect follows the animation, opening
    /// the frame widens the dialogue box along with the picture.
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
        /// <summary>The canvas the game is laid out in.</summary>
        private const float ReferenceWidth = 1920f;

        private const float ReferenceHeight = 1080f;

        private RectTransform _left;
        private RectTransform _right;
        private RectTransform _top;
        private RectTransform _bottom;
        private RectTransform _content;

        private TweenHandle _animation;

        private float _closedBorderX = 149f;
        private float _closedBorderY = 69f;
        private float _openness;

        /// <summary>0 while the frame is fully closed, 1 once it is fully open.</summary>
        public float Openness => _openness;

        /// <summary>
        /// The rect inside the frame. Anything the player reads belongs in here.
        /// </summary>
        /// <remarks>
        /// Null until <see cref="BindContent"/> is called. The frame works
        /// without one — it simply draws its border and leaves the interface
        /// where it is — so a scene that has not been told about the content
        /// rect degrades to the old behaviour rather than throwing.
        /// </remarks>
        public RectTransform Content => _content;

        /// <summary>Builds the bars as a full-screen layer.</summary>
        public void Initialize(RectTransform parent, StageSettings settings)
        {
            RectTransform root = (RectTransform)transform;
            root.SetParent(parent, false);
            Stretch(root);

            _closedBorderX = Mathf.Clamp(settings != null ? settings.FrameBorderX : 149f, 0f, ReferenceWidth * 0.45f);
            _closedBorderY = Mathf.Clamp(settings != null ? settings.FrameBorderY : 69f, 0f, ReferenceHeight * 0.45f);

            Color colour = settings != null ? settings.FrameColour : Color.black;

            _left = CreateBar("FrameBarLeft", root, colour);
            _right = CreateBar("FrameBarRight", root, colour);
            _top = CreateBar("FrameBarTop", root, colour);
            _bottom = CreateBar("FrameBarBottom", root, colour);

            bool framed = settings == null || settings.FrameEnabled;
            SetOpenness(framed ? 0f : 1f);
        }

        /// <summary>
        /// Hands the frame the rect the interface is laid out in, so the two
        /// move together.
        /// </summary>
        public void BindContent(RectTransform content)
        {
            _content = content;
            Apply(_openness);
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
                this,
                true,
                true);
        }

        private void Apply(float openness)
        {
            _openness = Mathf.Clamp01(openness);

            // Inset of the picture from each edge, as a fraction of the screen.
            // The two thicknesses are authored in canvas units and converted
            // here, so the border keeps the same apparent weight on every
            // monitor rather than the same number of real pixels. At openness 1
            // both are zero and the picture is the whole window.
            float insetX = Mathf.Lerp(_closedBorderX / ReferenceWidth, 0f, _openness);
            float insetY = Mathf.Lerp(_closedBorderY / ReferenceHeight, 0f, _openness);

            SetBar(_left, Vector2.zero, new Vector2(insetX, 1f));
            SetBar(_right, new Vector2(1f - insetX, 0f), Vector2.one);
            SetBar(_top, new Vector2(insetX, 1f - insetY), new Vector2(1f - insetX, 1f));
            SetBar(_bottom, new Vector2(insetX, 0f), new Vector2(1f - insetX, insetY));

            if (_content != null)
            {
                _content.anchorMin = new Vector2(insetX, insetY);
                _content.anchorMax = new Vector2(1f - insetX, 1f - insetY);
                _content.offsetMin = Vector2.zero;
                _content.offsetMax = Vector2.zero;
            }
        }

        /// <summary>
        /// Places one bar by anchors alone, so it stays correct at every window
        /// size without anything having to recompute it on a resize.
        /// </summary>
        private static void SetBar(RectTransform bar, Vector2 min, Vector2 max)
        {
            if (bar == null)
            {
                return;
            }

            bar.anchorMin = min;
            bar.anchorMax = max;
            bar.offsetMin = Vector2.zero;
            bar.offsetMax = Vector2.zero;
        }

        private static RectTransform CreateBar(string barName, RectTransform parent, Color colour)
        {
            GameObject host = new GameObject(barName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.pivot = new Vector2(0.5f, 0.5f);

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
