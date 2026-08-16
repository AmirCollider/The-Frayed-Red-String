// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Light2DAmbientPulse.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheFrayedRedString.Motion
{
    /// <summary>
    /// Breathes the intensity of a URP 2D light so the whole frame gains and
    /// loses a little warmth over time, the way a room does.
    /// </summary>
    /// <remarks>
    /// The light is reached through reflection rather than a direct
    /// <c>UnityEngine.Rendering.Universal.Light2D</c> reference on purpose. The
    /// 2D lighting type has moved namespace across URP versions, and a broken
    /// type reference here would fail the whole assembly — taking the menu,
    /// audio and localisation down with it over one cosmetic effect. Reflection
    /// keeps the failure mode local: worst case this one component quietly does
    /// nothing.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class Light2DAmbientPulse : MonoBehaviour
    {
        private const string Light2DTypeName = "Light2D";
        private const string IntensityPropertyName = "intensity";

        [Tooltip("Peak intensity deviation as a fraction of the authored value.")]
        [SerializeField, Range(0f, 0.5f)] private float _amplitude = 0.06f;

        [Tooltip("Seconds for one full brightness cycle.")]
        [SerializeField] private float _period = 8.5f;

        private Component _light;
        private PropertyInfo _intensityProperty;
        private float _baseIntensity;
        private float _phase;

        /// <summary>
        /// Adds a pulse to every 2D light in the scene. Returns how many lights
        /// were found.
        /// </summary>
        public static int InstallAll(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded)
            {
                return 0;
            }

            List<GameObject> roots = new List<GameObject>(scene.rootCount);
            scene.GetRootGameObjects(roots);

            int installed = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                installed += InstallRecursive(roots[i].transform);
            }

            return installed;
        }

        private static int InstallRecursive(Transform current)
        {
            int installed = 0;

            if (FindLight2D(current.gameObject) != null
                && current.GetComponent<Light2DAmbientPulse>() == null)
            {
                current.gameObject.AddComponent<Light2DAmbientPulse>();
                installed++;
            }

            for (int i = 0; i < current.childCount; i++)
            {
                installed += InstallRecursive(current.GetChild(i));
            }

            return installed;
        }

        private static Component FindLight2D(GameObject target)
        {
            Component[] components = target.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                Component component = components[i];
                if (component != null && component.GetType().Name == Light2DTypeName)
                {
                    return component;
                }
            }

            return null;
        }

        private void Awake()
        {
            _light = FindLight2D(gameObject);

            if (_light == null)
            {
                enabled = false;
                return;
            }

            _intensityProperty = _light.GetType().GetProperty(
                IntensityPropertyName,
                BindingFlags.Public | BindingFlags.Instance);

            if (_intensityProperty == null
                || !_intensityProperty.CanRead
                || !_intensityProperty.CanWrite
                || _intensityProperty.PropertyType != typeof(float))
            {
                enabled = false;
                return;
            }

            _baseIntensity = (float)_intensityProperty.GetValue(_light);
            _phase = Random.value * Mathf.PI * 2f;
        }

        private void LateUpdate()
        {
            if (_light == null || _intensityProperty == null || _period <= 0.0001f)
            {
                return;
            }

            float wave = Mathf.Sin(Time.unscaledTime * (Mathf.PI * 2f / _period) + _phase);
            float intensity = _baseIntensity * (1f + wave * _amplitude);

            _intensityProperty.SetValue(_light, Mathf.Max(0f, intensity));
        }

        private void OnDisable()
        {
            // Leave the light exactly as the artist authored it.
            if (_light != null && _intensityProperty != null)
            {
                _intensityProperty.SetValue(_light, _baseIntensity);
            }
        }
    }
}
