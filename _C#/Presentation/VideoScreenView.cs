// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VideoScreenView.cs
// -----------------------------------------------------------------------------

using System;
using System.IO;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using UnityEngine;
using UnityEngine.UI;

#if TFRS_VIDEO
using UnityEngine.Video;
#endif

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// A full-screen film, played over everything.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Act seven's ending credits, and the only place in the game where the
    /// screen stops being a stage with sprites on it. The picture is rendered
    /// into a texture and drawn to a canvas above the story, the frame and the
    /// dialogue, so nothing the game normally puts on screen can appear over it.
    /// </para>
    /// <para>
    /// The whole class survives the Video module being absent. This project's
    /// manifest does not include it, and code that names <c>VideoPlayer</c> in a
    /// project without it does not degrade — it stops the entire assembly from
    /// compiling. So every reference is behind <c>TFRS_VIDEO</c>, which
    /// <c>VideoModuleGuard</c> sets by looking for the type. Without it,
    /// <see cref="Play"/> reports that it could not and returns immediately, and
    /// act seven plays its closing cards over black exactly as it would after a
    /// video that had finished.
    /// </para>
    /// <para>
    /// Two places a clip can come from, and both are supported because they suit
    /// different films. A <c>VideoClip</c> in the library is convenient and gets
    /// packed into the build; a file in <c>StreamingAssets</c> is a file on disk
    /// that can be replaced without reimporting anything, which is what a
    /// several-minute 1080p credits sequence usually wants to be.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class VideoScreenView : MonoBehaviour
    {
        /// <summary>Folder inside StreamingAssets searched for a named film.</summary>
        public const string StreamingFolder = "Video";

        /// <summary>Extensions tried, in order, when looking for a file.</summary>
        private static readonly string[] Extensions = { ".mp4", ".webm", ".mov", ".m4v" };

        private RectTransform _root;
        private CanvasGroup _group;
        private RawImage _surface;
        private Image _backdrop;

#if TFRS_VIDEO
        private VideoPlayer _player;
        private RenderTexture _target;
        private AudioSource _audio;
        private bool _finished;
#endif

        /// <summary>True while a film is on screen.</summary>
        public bool IsPlaying { get; private set; }

        /// <summary>Builds the screen on a canvas of its own, above everything.</summary>
        public void Initialize(RectTransform parent)
        {
            _root = (RectTransform)transform;
            _root.SetParent(parent, false);
            Stretch(_root);

            _group = gameObject.AddComponent<CanvasGroup>();
            _group.alpha = 0f;

            // Nothing behind a film should be visible through it, including
            // between frames and around the edges of a clip whose aspect does
            // not match the window.
            GameObject backdropHost = new GameObject("VideoBackdrop", typeof(RectTransform));
            backdropHost.transform.SetParent(_root, false);
            Stretch((RectTransform)backdropHost.transform);

            _backdrop = backdropHost.AddComponent<Image>();
            _backdrop.sprite = ProceduralUiSprites.Solid(Color.white);
            _backdrop.color = Color.black;

            // A film is not a button, and a click during the credits belongs to
            // whatever is listening for a skip rather than to this.
            _backdrop.raycastTarget = false;

            GameObject surfaceHost = new GameObject("VideoSurface", typeof(RectTransform));
            surfaceHost.transform.SetParent(_root, false);
            Stretch((RectTransform)surfaceHost.transform);

            _surface = surfaceHost.AddComponent<RawImage>();
            _surface.color = Color.white;
            _surface.raycastTarget = false;

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Starts a film.
        /// </summary>
        /// <param name="filmName">
        /// Name of a clip in the video library, or of a file in
        /// <c>StreamingAssets/Video</c>, without the extension.
        /// </param>
        /// <param name="onFinished">
        /// Called when the film ends, when it is skipped, or straight away when
        /// there is nothing to play. Always called exactly once.
        /// </param>
        /// <returns>False when nothing was found or the module is missing.</returns>
        public bool Play(string filmName, Action onFinished)
        {
#if TFRS_VIDEO
            if (string.IsNullOrWhiteSpace(filmName))
            {
                onFinished?.Invoke();
                return false;
            }

            VideoClip clip = VideoLibrary.Find(filmName);
            string path = clip == null ? FindStreamingFile(filmName) : null;

            if (clip == null && path == null)
            {
                Debug.Log(
                    $"[Video] No film called '{filmName}'. Put an .mp4 of that name in Assets/Video, or in " +
                    $"Assets/StreamingAssets/{StreamingFolder}, and it plays with no other change. The act " +
                    "carries on without it in the meantime.");

                onFinished?.Invoke();
                return false;
            }

            EnsurePlayer();

            _finished = false;
            IsPlaying = true;

            gameObject.SetActive(true);
            _group.alpha = 1f;
            transform.SetAsLastSibling();

            if (clip != null)
            {
                _player.source = VideoSource.VideoClip;
                _player.clip = clip;
            }
            else
            {
                _player.source = VideoSource.Url;
                _player.url = path;
            }

            void Ended(VideoPlayer _)
            {
                Finish(onFinished);
            }

            void Failed(VideoPlayer _, string message)
            {
                Debug.LogWarning($"[Video] '{filmName}' could not be played: {message}");
                Finish(onFinished);
            }

            _player.loopPointReached += Ended;
            _player.errorReceived += Failed;

            _player.Play();

            return true;
#else
            Debug.Log(
                "[Video] Unity's Video module is not enabled in this project, so there is no film. " +
                "The Frayed Red String ▸ Check The Video Module says how to turn it on. Act seven plays " +
                "its closing cards either way.");

            onFinished?.Invoke();
            return false;
#endif
        }

        /// <summary>Ends the film early, as a skip does.</summary>
        public void Stop()
        {
#if TFRS_VIDEO
            if (!IsPlaying)
            {
                return;
            }

            Finish(null);
#endif
        }

#if TFRS_VIDEO
        private void Finish(Action onFinished)
        {
            if (_finished)
            {
                return;
            }

            _finished = true;
            IsPlaying = false;

            if (_player != null)
            {
                _player.Stop();
            }

            _group.alpha = 0f;
            gameObject.SetActive(false);

            onFinished?.Invoke();
        }

        /// <summary>
        /// Builds the player, its render target and its audio output once.
        /// </summary>
        /// <remarks>
        /// The target is made at the window's size at the moment the first film
        /// starts, and a RawImage stretched over the screen scales it the rest of
        /// the way. Rebuilding it on every resize would be correct and is not
        /// worth it for one sequence at the end of the game.
        /// </remarks>
        private void EnsurePlayer()
        {
            if (_player != null)
            {
                return;
            }

            _target = new RenderTexture(
                Mathf.Max(640, Screen.width),
                Mathf.Max(360, Screen.height),
                0,
                RenderTextureFormat.ARGB32)
            {
                name = "VideoTarget"
            };

            _target.Create();
            _surface.texture = _target;

            _audio = gameObject.AddComponent<AudioSource>();
            _audio.playOnAwake = false;
            _audio.spatialBlend = 0f;

            _player = gameObject.AddComponent<VideoPlayer>();
            _player.playOnAwake = false;
            _player.isLooping = false;
            _player.renderMode = VideoRenderMode.RenderTexture;
            _player.targetTexture = _target;
            _player.aspectRatio = VideoAspectRatio.FitInside;

            // Through an AudioSource rather than direct, so the film obeys the
            // same mixer and the same volume the rest of the game does.
            _player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _player.SetTargetAudioSource(0, _audio);

            // The game's own music has no business running underneath a scored
            // credits sequence.
            MusicService.CutOff();
        }

        private void OnDestroy()
        {
            if (_target != null)
            {
                _target.Release();
                Destroy(_target);
            }
        }

        /// <summary>The first matching file in StreamingAssets, or null.</summary>
        private static string FindStreamingFile(string filmName)
        {
            string folder = Path.Combine(Application.streamingAssetsPath, StreamingFolder);

            for (int i = 0; i < Extensions.Length; i++)
            {
                string candidate = Path.Combine(folder, filmName + Extensions[i]);

                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }
#endif

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
