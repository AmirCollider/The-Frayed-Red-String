// -----------------------------------------------------------------------------
//  The Frayed Red String
//  AmbientMotion.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Motion
{
    /// <summary>
    /// Continuous idle motion for a single element: a slow scale pulse, a
    /// positional drift and a rotational sway, layered on top of the transform
    /// the scene was authored with.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This component is the <b>sole owner</b> of its transform. Nothing else in
    /// the codebase writes localPosition / localScale / localRotation on an
    /// object that has one. Other systems that want to move an element push
    /// their contribution through the <see cref="ExtraScale"/>,
    /// <see cref="ExtraOffset"/> and <see cref="ExtraRotation"/> channels
    /// instead, which are composed on top of the idle motion here.
    /// </para>
    /// <para>
    /// That single-writer rule is why entrance animations, the language flip and
    /// the breathing loop can all run at once without any of them stuttering.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(100)]
    public sealed class AmbientMotion : MonoBehaviour
    {
        /// <summary>
        /// Converts drift authored in UI units into world units for
        /// SpriteRenderer-based elements. Matches Unity's default 100 pixels
        /// per unit.
        /// </summary>
        private const float WorldDriftScale = 0.01f;

        [SerializeField] private AmbientMotionProfile _profile = AmbientMotionProfile.Card;

        [Tooltip("Per-object phase offset, 0..1. Keeps neighbouring elements from breathing in lockstep.")]
        [SerializeField, Range(0f, 1f)] private float _phase;

        private RectTransform _rectTransform;
        private Vector3 _baseScale = Vector3.one;
        private Vector3 _basePosition;
        private Quaternion _baseRotation = Quaternion.identity;
        private bool _captured;
        private bool _positionIsBorrowed;

        /// <summary>
        /// Overall strength of the idle motion, 0..1. Dropping this to zero
        /// freezes the element at its authored pose without disabling the
        /// component.
        /// </summary>
        public float Weight { get; set; } = 1f;

        /// <summary>
        /// External multiplicative scale, composed on top of the idle pulse.
        /// Used by reveal animations and by the language-flip effect.
        /// </summary>
        public Vector3 ExtraScale { get; set; } = Vector3.one;

        /// <summary>External positional offset, in the same units as the profile's drift.</summary>
        public Vector3 ExtraOffset { get; set; } = Vector3.zero;

        /// <summary>External rotation around Z, in degrees.</summary>
        public float ExtraRotation { get; set; }

        /// <summary>
        /// Leaves the element's position alone and animates only its scale and
        /// rotation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Set automatically for any element a uGUI layout group is arranging,
        /// and that is the case this exists for. A layout group writes
        /// <c>anchoredPosition</c> during the canvas' layout pass, which runs
        /// after every <c>LateUpdate</c> — including this component's. Both
        /// would then be writing the same field, and the last writer of the
        /// frame wins: the layout positions the children once, this component
        /// puts every one of them back at the rest pose it captured before the
        /// layout had ever run, and the layout is not marked dirty again.
        /// </para>
        /// <para>
        /// The visible result is a column of buttons stacked on top of one
        /// another in the middle of the screen — which is exactly what the pause
        /// menu did, and what the choice buttons would have done the first time
        /// an act offered a choice.
        /// </para>
        /// <para>
        /// So the position channel is simply handed back. The element still
        /// breathes and sways, and the layout group keeps sole ownership of
        /// where it sits.
        /// </para>
        /// </remarks>
        public bool BorrowsPosition
        {
            get => _positionIsBorrowed;
            set => _positionIsBorrowed = value;
        }

        /// <summary>The tuning currently driving this element.</summary>
        public AmbientMotionProfile Profile
        {
            get => _profile;
            set => _profile = value;
        }

        /// <summary>
        /// Returns the element's <see cref="AmbientMotion"/>, adding one with a
        /// silent profile if the element does not have it yet. Lets any system
        /// obtain a transform channel without caring how the element was set up.
        /// </summary>
        public static AmbientMotion GetOrAdd(GameObject target)
        {
            if (target == null)
            {
                return null;
            }

            AmbientMotion existing = target.GetComponent<AmbientMotion>();
            if (existing != null)
            {
                return existing;
            }

            AmbientMotion created = target.AddComponent<AmbientMotion>();
            created._profile = default;   // Silent: the caller only wants a channel.
            return created;
        }

        /// <summary>Configures the element with a profile and a stable phase offset.</summary>
        public void Configure(AmbientMotionProfile profile, float phase01)
        {
            _profile = profile;
            _phase = Mathf.Repeat(phase01, 1f);
        }

        /// <summary>
        /// Re-reads the current transform as the new rest pose. Call this after
        /// deliberately repositioning an element that is already animating.
        /// </summary>
        /// <remarks>
        /// Only safe before the first <c>LateUpdate</c> of an element's life, or
        /// immediately after every channel has been reset. Once the idle motion
        /// is running, the transform holds the rest pose *plus* the current
        /// frame's pulse and drift, and re-capturing folds those in permanently —
        /// so an element re-captured a few times slowly walks away from where it
        /// was authored and swells a few percent each time. Use
        /// <see cref="RecapturePosition"/> when only the position has moved,
        /// which is the usual case.
        /// </remarks>
        public void RecaptureBase()
        {
            _captured = false;
            CaptureBase();
        }

        /// <summary>
        /// Re-reads the current position as the new rest position, leaving the
        /// captured scale and rotation untouched.
        /// </summary>
        /// <remarks>
        /// The safe counterpart to <see cref="RecaptureBase"/> for an element
        /// that has been moved while already animating: the caller has just
        /// assigned the position, so what is read back is exactly what was
        /// written, with no drift folded in. Scale and rotation are the channels
        /// that would silently accumulate, and they are left alone.
        /// </remarks>
        public void RecapturePosition()
        {
            if (!_captured)
            {
                CaptureBase();
                return;
            }

            _basePosition = _rectTransform != null
                ? _rectTransform.anchoredPosition3D
                : transform.localPosition;
        }

        /// <summary>Returns the element to its authored pose and clears all channels.</summary>
        public void ResetToBase()
        {
            CaptureBase();

            ExtraScale = Vector3.one;
            ExtraOffset = Vector3.zero;
            ExtraRotation = 0f;

            ApplyPose(Vector3.one, Vector3.zero, 0f);
        }

        private void Awake()
        {
            CaptureBase();

            // A phase that was never assigned is derived from the object's path,
            // so the same element always breathes on the same beat between runs
            // while its neighbours stay out of sync with it.
            if (Mathf.Approximately(_phase, 0f))
            {
                _phase = UnityUtility.StableHash01(BuildPath());
            }
        }

        private void LateUpdate()
        {
            if (!_captured)
            {
                CaptureBase();
            }

            // Unscaled time: the menu keeps breathing even if gameplay elsewhere
            // has paused the simulation.
            float time = Time.unscaledTime;
            float phaseRadians = _phase * Mathf.PI * 2f;

            float scaleWave = Wave(time, _profile.ScalePeriod, phaseRadians);
            if (_profile.ScaleGrowsOnly)
            {
                scaleWave = (scaleWave + 1f) * 0.5f;
            }

            float driftWaveX = Wave(time, _profile.DriftPeriod, phaseRadians + 1.7f);
            float driftWaveY = Wave(time, _profile.DriftPeriod * 1.31f, phaseRadians + 0.4f);

            if (_profile.DriftOneDirection)
            {
                driftWaveX = (driftWaveX + 1f) * 0.5f;
                driftWaveY = (driftWaveY + 1f) * 0.5f;
            }

            float swayWave = Wave(time, _profile.SwayPeriod, phaseRadians + 2.9f);

            Vector3 pulse = Vector3.one + _profile.ScaleAxisWeights * (scaleWave * _profile.ScaleAmplitude * Weight);

            float unitScale = _rectTransform != null ? 1f : WorldDriftScale;
            Vector3 drift = new Vector3(
                driftWaveX * _profile.DriftAmplitude.x * unitScale,
                driftWaveY * _profile.DriftAmplitude.y * unitScale,
                0f) * Weight;

            float sway = swayWave * _profile.SwayDegrees * Weight;

            ApplyPose(pulse, drift, sway);
        }

        /// <summary>
        /// Two detuned sine waves summed together. A single sine reads as a
        /// machine; the second, slower wave at an irrational-ish ratio stops the
        /// loop from ever repeating visibly.
        /// </summary>
        private static float Wave(float time, float period, float phaseRadians)
        {
            if (period <= 0.0001f)
            {
                return 0f;
            }

            float primary = Mathf.Sin(time * (Mathf.PI * 2f / period) + phaseRadians);
            float secondary = Mathf.Sin(time * (Mathf.PI * 2f / (period * 1.618f)) + phaseRadians * 0.5f + 1.1f);

            return primary * 0.78f + secondary * 0.22f;
        }

        private void ApplyPose(Vector3 pulse, Vector3 drift, float swayDegrees)
        {
            float constant = _profile.ConstantScale <= 0f ? 1f : _profile.ConstantScale;

            Vector3 scale = Vector3.Scale(_baseScale, pulse) * constant;
            scale = Vector3.Scale(scale, ExtraScale);
            transform.localScale = scale;

            if (!_positionIsBorrowed)
            {
                Vector3 position = _basePosition + drift + ExtraOffset;

                if (_rectTransform != null)
                {
                    _rectTransform.anchoredPosition3D = position;
                }
                else
                {
                    transform.localPosition = position;
                }
            }

            transform.localRotation = _baseRotation * Quaternion.Euler(0f, 0f, swayDegrees + ExtraRotation);
        }

        private void CaptureBase()
        {
            if (_captured)
            {
                return;
            }

            _rectTransform = transform as RectTransform;
            _positionIsBorrowed = IsArrangedByALayoutGroup();

            _baseScale = transform.localScale;
            _baseRotation = transform.localRotation;
            _basePosition = _rectTransform != null
                ? _rectTransform.anchoredPosition3D
                : transform.localPosition;

            _captured = true;
        }

        /// <summary>
        /// True when a layout group owns this element's position.
        /// </summary>
        /// <remarks>
        /// Asked of the parent rather than of this object: a layout group
        /// arranges its children, so it is the parent that decides whether this
        /// element's position is its own. Re-asked on every capture, because an
        /// element can be reparented into a layout after it was built.
        /// </remarks>
        private bool IsArrangedByALayoutGroup()
        {
            if (_rectTransform == null)
            {
                return false;
            }

            Transform parent = _rectTransform.parent;
            return parent != null && parent.GetComponent<LayoutGroup>() != null;
        }

        private string BuildPath()
        {
            string path = name;
            Transform cursor = transform.parent;

            while (cursor != null)
            {
                path = cursor.name + "/" + path;
                cursor = cursor.parent;
            }

            return path + "#" + transform.GetSiblingIndex();
        }
    }
}
