// -----------------------------------------------------------------------------
//  The Frayed Red String
//  MusicService.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using TheFrayedRedString.Tweening;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Plays looping background music, cross-fading between tracks.
    /// </summary>
    /// <remarks>
    /// Two sources rather than one: changing track on a single source means a
    /// gap, and a gap in a menu loop is more noticeable than the change itself.
    /// The service lives on the persistent host so music survives scene loads
    /// and can carry across a transition uninterrupted.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class MusicService : MonoBehaviour
    {
        private static MusicService _instance;

        private AudioSource _sourceA;
        private AudioSource _sourceB;
        private AudioSource _active;
        private MusicLibrary _library;
        private TweenHandle _fade;
        private string _currentTrack;
        private float _volume = GameConfig.DefaultMusicVolume;

        /// <summary>Name of the track currently playing, or <c>null</c>.</summary>
        public static string CurrentTrack => _instance != null ? _instance._currentTrack : null;

        /// <summary>Master music level, 0..1.</summary>
        public static float Volume
        {
            get => _instance != null ? _instance._volume : GameConfig.DefaultMusicVolume;
            set
            {
                if (_instance == null)
                {
                    return;
                }

                _instance._volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(GameConfig.PrefsMusicVolume, _instance._volume);

                if (_instance._active != null && _instance._active.isPlaying)
                {
                    _instance._active.volume = _instance._volume;
                }
            }
        }

        /// <summary>Attaches the service to the persistent service host.</summary>
        public static MusicService Install(GameObject host)
        {
            if (_instance != null || host == null)
            {
                return _instance;
            }

            _instance = host.AddComponent<MusicService>();
            return _instance;
        }

        /// <summary>Clears the cached instance between play sessions.</summary>
        public static void ResetStatics()
        {
            _instance = null;
        }

        /// <summary>
        /// Starts a looping track, fading it in. Asking for the track that is
        /// already playing does nothing, so a scene reload does not restart the
        /// music from the top.
        /// </summary>
        /// <param name="trackName">Asset file name, from <see cref="MusicTracks"/>.</param>
        /// <param name="fadeInSeconds">Length of the fade up from silence.</param>
        public static void PlayLoop(string trackName, float fadeInSeconds = GameConfig.MusicFadeInDuration)
        {
            if (_instance == null)
            {
                return;
            }

            _instance.PlayLoopInternal(trackName, fadeInSeconds);
        }

        /// <summary>Fades the current track out and stops it.</summary>
        public static void Stop(float fadeOutSeconds = GameConfig.MusicFadeOutDuration)
        {
            if (_instance == null)
            {
                return;
            }

            _instance.StopInternal(fadeOutSeconds);
        }

        /// <summary>
        /// Cuts all music dead in a single frame, with no fade.
        /// </summary>
        /// <remarks>
        /// The story calls for exactly this at two moments: the half-second
        /// menu, and the shot at the end of act five. A fade would soften what
        /// is meant to be a hole punched in the soundtrack.
        /// </remarks>
        public static void CutOff()
        {
            if (_instance == null)
            {
                return;
            }

            _instance._fade.CancelIfRunning();

            if (_instance._sourceA != null)
            {
                _instance._sourceA.Stop();
            }

            if (_instance._sourceB != null)
            {
                _instance._sourceB.Stop();
            }

            _instance._currentTrack = null;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            _volume = PlayerPrefs.GetFloat(GameConfig.PrefsMusicVolume, GameConfig.DefaultMusicVolume);

            _sourceA = CreateSource("MusicSourceA");
            _sourceB = CreateSource("MusicSourceB");
            _active = _sourceA;

            _library = MusicLibrary.Load();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private AudioSource CreateSource(string sourceName)
        {
            GameObject host = new GameObject(sourceName);
            host.transform.SetParent(transform, false);

            AudioSource source = host.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.spatialBlend = 0f;
            source.dopplerLevel = 0f;
            source.bypassReverbZones = true;
            source.volume = 0f;

            return source;
        }

        /// <summary>Track names already reported as missing.</summary>
        private readonly System.Collections.Generic.HashSet<string> _missingTracks =
            new System.Collections.Generic.HashSet<string>();

        private void PlayLoopInternal(string trackName, float fadeInSeconds)
        {
            if (string.Equals(_currentTrack, trackName) && _active != null && _active.isPlaying)
            {
                return;
            }

            if (_library == null)
            {
                _library = MusicLibrary.Load();
            }

            AudioClip clip = _library != null ? _library.Find(trackName) : null;

            if (clip == null)
            {
                // Once per name. An act asks for its track when it starts, again
                // whenever a Music beat comes round, and again every time a save
                // is loaded into it — so one missing file writes the same three
                // lines to the console over and over until the real warnings are
                // buried under them.
                if (_missingTracks.Add(trackName))
                {
                    Debug.LogWarning(
                        $"[Music] No track named '{trackName}' is in the music library. " +
                        "Check that the file exists under Assets/Audio/BackgroundMusics and re-run " +
                        "The Frayed Red String > Rebuild Music Library.");
                }

                return;
            }

            _fade.CancelIfRunning();

            AudioSource outgoing = _active;
            AudioSource incoming = _active == _sourceA ? _sourceB : _sourceA;

            incoming.clip = clip;
            incoming.volume = 0f;
            incoming.Play();

            _active = incoming;
            _currentTrack = trackName;

            float outgoingFrom = outgoing != null ? outgoing.volume : 0f;
            float target = _volume;

            _fade = TweenRunner.Play(
                fadeInSeconds,
                t =>
                {
                    if (incoming != null)
                    {
                        incoming.volume = target * t;
                    }

                    if (outgoing != null)
                    {
                        outgoing.volume = outgoingFrom * (1f - t);
                    }
                },
                EaseType.InOutSine,
                0f,
                () =>
                {
                    if (incoming != null)
                    {
                        incoming.volume = target;
                    }

                    if (outgoing != null)
                    {
                        outgoing.volume = 0f;
                        outgoing.Stop();
                    }
                },
                this);
        }

        private void StopInternal(float fadeOutSeconds)
        {
            if (_active == null || !_active.isPlaying)
            {
                _currentTrack = null;
                return;
            }

            _fade.CancelIfRunning();
            _currentTrack = null;

            AudioSource outgoing = _active;
            float from = outgoing.volume;

            _fade = TweenRunner.Play(
                fadeOutSeconds,
                t =>
                {
                    if (outgoing != null)
                    {
                        outgoing.volume = from * (1f - t);
                    }
                },
                EaseType.InOutSine,
                0f,
                () =>
                {
                    if (outgoing != null)
                    {
                        outgoing.volume = 0f;
                        outgoing.Stop();
                    }
                },
                this);
        }
    }
}
