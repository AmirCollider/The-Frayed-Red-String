// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryGrade.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// The pane of frosted glass the first four acts are seen through, and the
    /// blood act five throws at it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The design document asks for the picture to become photorealistic in act
    /// five and then says, in the same breath, that the game does not become
    /// three-dimensional — that perhaps the blurred glass which has been there
    /// from the start is simply taken away. That second reading is the one worth
    /// building. The art does not change; what changes is that there stops being
    /// something in front of it.
    /// </para>
    /// <para>
    /// So this is one wash of warm pink over the scenery, at an opacity nobody
    /// would name if they saw a single screenshot. Four acts of it become the
    /// way the game looks, and the moment it lifts, the same characters against
    /// the same backgrounds read as harder and colder without one asset having
    /// been replaced.
    /// </para>
    /// <para>
    /// It sits above the stage and below everything the player reads. Veiling
    /// the dialogue box would be a filter on the interface, which is a different
    /// and much worse idea: the box is not part of the fiction being softened.
    /// </para>
    /// <para>
    /// The stain shares this object because the two are the same mechanism
    /// pointed at different ends of the act — a colour over the picture, moved
    /// somewhere over some seconds. Giving blood its own component would mean a
    /// second full-screen graphic that exists to be transparent for six hours.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class StoryGrade : MonoBehaviour
    {
        /// <summary>The colour and strength this scene's veil was built with.</summary>
        /// <remarks>
        /// Read from <see cref="StageSettings"/> at build time rather than fixed
        /// here, so it can be judged against a real picture on the Story
        /// Editor's Stage tab instead of by editing a constant and pressing
        /// Play. Switching the veil off there sets this to a transparent colour,
        /// which makes every Grade beat in act five a no-op and costs nothing.
        /// </remarks>
        private Color _veilColour = new Color(1f, 0.87f, 0.91f, 0.17f);

        private Image _haze;
        private Image _stain;
        private Image _attention;

        private TweenHandle _hazeTween;
        private TweenHandle _stainTween;
        private TweenHandle _attentionTween;

        private float _amount = GameConfig.StoryGradeDefault;

        /// <summary>How veiled the picture is now. 1 is act one, 0 is act five.</summary>
        public float Amount => _amount;

        /// <summary>Builds both layers as a full-screen sheet.</summary>
        public void Initialize(RectTransform parent)
        {
            Initialize(parent, StageSettings.Load());
        }

        /// <summary>Builds both layers, taking the veil from the settings.</summary>
        public void Initialize(RectTransform parent, StageSettings settings)
        {
            if (settings != null)
            {
                Color colour = settings.VeilColour;
                float strength = settings.VeilEnabled ? Mathf.Clamp01(settings.VeilStrength) : 0f;

                _veilColour = new Color(colour.r, colour.g, colour.b, colour.a * strength);
            }

            RectTransform root = (RectTransform)transform;
            root.SetParent(parent, false);
            Stretch(root);

            _haze = CreateLayer("StoryHaze", root);

            // Above the veil and below the blood: the veil is how the world
            // looks, this is the world stepping back, and the blood is on the
            // lens in front of both.
            _attention = CreateLayer("StoryAttention", root);
            _stain = CreateLayer("StoryStain", root);

            _attention.color = new Color(0f, 0f, 0f, 0f);
            _stain.color = new Color(0f, 0f, 0f, 0f);

            SetAmount(GameConfig.StoryGradeDefault);
        }

        /// <summary>Moves the veil to a new strength over time.</summary>
        /// <param name="amount">1 leaves it as act one had it, 0 removes it.</param>
        /// <param name="duration">Seconds. Zero applies it at once.</param>
        public void FadeTo(float amount, float duration)
        {
            amount = Mathf.Clamp01(amount);

            _hazeTween.CancelIfRunning();

            if (duration <= 0f)
            {
                SetAmount(amount);
                return;
            }

            float from = _amount;

            _hazeTween = TweenRunner.Play(
                duration,
                t => SetAmount(Mathf.LerpUnclamped(from, amount, t)),

                // Slow at both ends. The veil lifting is something the player is
                // meant to half-notice while looking at something else, and a
                // linear ramp has a start and a stop you can feel.
                EaseType.InOutSine,
                0f,
                () => SetAmount(amount),
                this,
                true,
                true);
        }

        /// <summary>Sets the veil without animating.</summary>
        public void SetAmount(float amount)
        {
            _amount = Mathf.Clamp01(amount);

            if (_haze != null)
            {
                _haze.color = new Color(
                    _veilColour.r, _veilColour.g, _veilColour.b, _veilColour.a * _amount);
            }
        }

        /// <summary>
        /// Takes the room down behind somebody who has turned to the player.
        /// </summary>
        /// <param name="dim">0 gives the scene back, up to about 0.5 withdraws it.</param>
        /// <param name="duration">Seconds. Slow — the withdrawal is the effect.</param>
        /// <remarks>
        /// A flat black sheet rather than anything cleverer. The scene is not
        /// being blurred or desaturated, it is being turned down, and the
        /// simplest possible version of that is the one that reads as a
        /// deliberate act rather than as a filter.
        /// </remarks>
        public void SetAttention(float dim, float duration)
        {
            if (_attention == null)
            {
                return;
            }

            Color target = new Color(0f, 0f, 0f, Mathf.Clamp01(dim));

            _attentionTween.CancelIfRunning();

            if (duration <= 0f)
            {
                _attention.color = target;
                return;
            }

            Color from = _attention.color;

            _attentionTween = TweenRunner.Play(
                duration,
                t => _attention.color = Color.LerpUnclamped(from, target, t),
                EaseType.InOutSine,
                0f,
                () => _attention.color = target,
                this,
                true,
                true);
        }

        /// <summary>
        /// Throws a colour across the picture and leaves it there.
        /// </summary>
        /// <param name="colour">
        /// The colour, alpha included. An alpha of zero clears whatever is on
        /// screen, which is how the stain is taken off again.
        /// </param>
        /// <param name="duration">Seconds. Zero is a cut, which is the usual one.</param>
        public void Stain(Color colour, float duration)
        {
            if (_stain == null)
            {
                return;
            }

            _stainTween.CancelIfRunning();

            if (duration <= 0f)
            {
                _stain.color = colour;
                return;
            }

            Color from = _stain.color;

            _stainTween = TweenRunner.Play(
                duration,
                t => _stain.color = Color.LerpUnclamped(from, colour, t),
                EaseType.OutCubic,
                0f,
                () => _stain.color = colour,
                this,
                true,
                true);
        }

        private static Image CreateLayer(string layerName, RectTransform parent)
        {
            GameObject host = new GameObject(layerName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            Stretch((RectTransform)host.transform);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.Solid(Color.white);

            // Scenery, not a wall. A click that lands here is a click on the
            // story and still turns the page.
            image.raycastTarget = false;

            return image;
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
