// -----------------------------------------------------------------------------
//  The Frayed Red String
//  AudioService.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Plays the procedurally generated UI sounds through a small pool of
    /// voices, so overlapping clicks never cut each other off.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AudioService : MonoBehaviour
    {
        private static AudioService _instance;

        private AudioSource[] _voices;
        private int _nextVoice;
        private float _volume = GameConfig.DefaultSfxVolume;

        /// <summary>The live service, or <c>null</c> before boot.</summary>
        public static AudioService Instance => _instance;

        /// <summary>Master level applied to every UI sound, 0..1.</summary>
        public static float Volume
        {
            get => _instance != null ? _instance._volume : GameConfig.DefaultSfxVolume;
            set
            {
                if (_instance == null)
                {
                    return;
                }

                _instance._volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(GameConfig.PrefsSfxVolume, _instance._volume);
            }
        }

        /// <summary>Attaches the service to the persistent service host.</summary>
        public static AudioService Install(GameObject host)
        {
            if (_instance != null || host == null)
            {
                return _instance;
            }

            _instance = host.AddComponent<AudioService>();
            return _instance;
        }

        /// <summary>Clears the cached instance between play sessions.</summary>
        public static void ResetStatics()
        {
            _instance = null;
        }

        /// <summary>
        /// Plays a one-shot UI sound with a little random pitch spread.
        /// Safe to call before boot: it simply does nothing.
        /// </summary>
        /// <param name="id">Which sound to play.</param>
        /// <param name="volumeScale">Extra multiplier on top of the master volume.</param>
        /// <param name="pitch">Base pitch before jitter is applied.</param>
        public static void Play(SfxId id, float volumeScale = 1f, float pitch = 1f)
        {
            if (_instance == null)
            {
                return;
            }

            _instance.PlayInternal(id, volumeScale, pitch);
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            _volume = PlayerPrefs.GetFloat(GameConfig.PrefsSfxVolume, GameConfig.DefaultSfxVolume);

            CreateVoices();

            // Synthesising the interface clips takes a couple of milliseconds;
            // doing it during boot keeps the first click free of a frame hitch.
            // The story palette is longer and is built when an act loads, behind
            // the fade curtain.
            ProceduralSfxLibrary.WarmUpCore();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void CreateVoices()
        {
            _voices = new AudioSource[GameConfig.SfxVoiceCount];

            for (int i = 0; i < _voices.Length; i++)
            {
                GameObject voiceHost = new GameObject($"SfxVoice_{i:00}");
                voiceHost.transform.SetParent(transform, false);

                AudioSource source = voiceHost.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                source.spatialBlend = 0f;      // Pure 2D: UI sound must not pan.
                source.dopplerLevel = 0f;
                source.bypassReverbZones = true;
                source.ignoreListenerPause = true;
                source.ignoreListenerVolume = false;

                _voices[i] = source;
            }
        }

        /// <summary>
        /// Makes sure exactly one AudioListener is active.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Both authored scenes carry a listener on their Main Camera, but a
        /// scene added later might not, and a scene with no listener is silent
        /// with no error to explain why. So the service keeps a spare.
        /// </para>
        /// <para>
        /// It has to be checked per scene rather than once at boot. The service
        /// is built before the first scene exists, so at that moment there is
        /// never a camera listener to find — adding one unconditionally would
        /// guarantee the "2 audio listeners in the scene" warning and leave the
        /// wrong one active.
        /// </para>
        /// </remarks>
        public static void EnsureListener()
        {
            if (_instance == null)
            {
                return;
            }

            AudioListener spare = _instance.GetComponent<AudioListener>();

            AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude);

            bool sceneHasListener = false;
            for (int i = 0; i < listeners.Length; i++)
            {
                if (listeners[i] != null && listeners[i] != spare)
                {
                    sceneHasListener = true;
                    break;
                }
            }

            if (sceneHasListener)
            {
                if (spare != null)
                {
                    spare.enabled = false;
                }

                return;
            }

            if (spare == null)
            {
                spare = _instance.gameObject.AddComponent<AudioListener>();
            }

            spare.enabled = true;
        }

        private void PlayInternal(SfxId id, float volumeScale, float pitch)
        {
            AudioClip clip = ProceduralSfxLibrary.Get(id);
            if (clip == null || _voices == null || _voices.Length == 0)
            {
                return;
            }

            AudioSource voice = _voices[_nextVoice];
            _nextVoice = (_nextVoice + 1) % _voices.Length;

            if (voice == null)
            {
                return;
            }

            float jitter = Random.Range(-GameConfig.SfxPitchJitter, GameConfig.SfxPitchJitter);
            voice.pitch = Mathf.Clamp(pitch + jitter, 0.25f, 3f);
            voice.PlayOneShot(clip, Mathf.Clamp01(_volume * volumeScale));
        }
    }
}
