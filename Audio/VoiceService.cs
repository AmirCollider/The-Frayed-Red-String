// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VoiceService.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Core;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Plays the recorded reading of a line, when there is one.
    /// </summary>
    /// <remarks>
    /// <para>
    /// One source rather than a pool, on purpose. Two lines of dialogue playing
    /// over each other is never right, so a new line stops whatever was still
    /// speaking — which also means the player pressing on through a scene
    /// behaves the way it does in every voiced visual novel: the line stops when
    /// you stop reading it.
    /// </para>
    /// <para>
    /// Everything here is written so that a project with no recordings behaves
    /// exactly as it did before recordings existed. <see cref="Speak"/> returns
    /// false, the typewriter's own voice grain carries the line, and the story
    /// waits for the player rather than for a clip.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class VoiceService : MonoBehaviour
    {
        private static VoiceService _instance;

        /// <summary>
        /// Names already reported as missing, so each is said once.
        /// </summary>
        /// <remarks>
        /// A line is re-spoken every time the player changes language on it, and
        /// an act being played through in the editor will pass the same missing
        /// name several times. One warning per name is the useful amount.
        /// </remarks>
        private static readonly HashSet<string> Reported = new HashSet<string>();

        private AudioSource _source;
        private float _volume = GameConfig.DefaultVoiceVolume;

        /// <summary>The live service, or <c>null</c> before boot.</summary>
        public static VoiceService Instance => _instance;

        /// <summary>True while a recorded line is still playing.</summary>
        public static bool IsSpeaking =>
            _instance != null && _instance._source != null && _instance._source.isPlaying;

        /// <summary>Master level applied to recorded lines, 0..1.</summary>
        public static float Volume
        {
            get => _instance != null ? _instance._volume : GameConfig.DefaultVoiceVolume;
            set
            {
                if (_instance == null)
                {
                    return;
                }

                _instance._volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(GameConfig.PrefsVoiceVolume, _instance._volume);

                if (_instance._source != null)
                {
                    _instance._source.volume = _instance._volume;
                }
            }
        }

        /// <summary>Attaches the service to the persistent service host.</summary>
        public static VoiceService Install(GameObject host)
        {
            if (_instance != null || host == null)
            {
                return _instance;
            }

            _instance = host.AddComponent<VoiceService>();
            return _instance;
        }

        /// <summary>Clears the cached instance between play sessions.</summary>
        public static void ResetStatics()
        {
            _instance = null;
            Reported.Clear();
        }

        /// <summary>
        /// Plays the recording of a line.
        /// </summary>
        /// <param name="clipName">
        /// File name in <c>Assets/Audio/Voice</c>, without the extension.
        /// </param>
        /// <returns>
        /// True when a recording was found and started. False means the line has
        /// no voice — either because none was asked for, or because the file is
        /// not there yet — and the caller should carry on exactly as it would
        /// have without this service.
        /// </returns>
        public static bool Speak(string clipName)
        {
            if (string.IsNullOrWhiteSpace(clipName))
            {
                return false;
            }

            if (_instance == null)
            {
                return false;
            }

            VoiceLibrary library = VoiceLibrary.Load();
            AudioClip clip = library != null ? library.Find(clipName) : null;

            if (clip == null)
            {
                if (Reported.Add(clipName))
                {
                    Debug.Log(
                        $"[Voice] No recording named '{clipName}' in Assets/Audio/Voice, so the line is " +
                        "read by the typewriter as usual. Drop a file of that name in and it is picked up " +
                        "with no other change.");
                }

                return false;
            }

            _instance.PlayInternal(clip);
            return true;
        }

        /// <summary>Stops whatever is being said. Safe before boot.</summary>
        public static void Silence()
        {
            if (_instance != null && _instance._source != null)
            {
                _instance._source.Stop();
            }
        }

        /// <summary>How long a recording runs, or zero when there is none.</summary>
        public static float LengthOf(string clipName)
        {
            if (string.IsNullOrWhiteSpace(clipName))
            {
                return 0f;
            }

            VoiceLibrary library = VoiceLibrary.Load();
            AudioClip clip = library != null ? library.Find(clipName) : null;

            return clip != null ? clip.length : 0f;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            _volume = PlayerPrefs.GetFloat(GameConfig.PrefsVoiceVolume, GameConfig.DefaultVoiceVolume);

            GameObject host = new GameObject("VoiceSource");
            host.transform.SetParent(transform, false);

            _source = host.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.loop = false;
            _source.spatialBlend = 0f;
            _source.dopplerLevel = 0f;
            _source.bypassReverbZones = true;
            _source.volume = _volume;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void PlayInternal(AudioClip clip)
        {
            if (_source == null)
            {
                return;
            }

            _source.Stop();
            _source.clip = clip;
            _source.volume = _volume;
            _source.Play();
        }
    }
}
