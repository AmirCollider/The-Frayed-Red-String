// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LanguageToggleButton.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// Turns the authored ChangeLanguageButton into a working language cycle —
    /// English, Japanese, Persian — complete with a card-flip animation on the
    /// flag.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The flag swap happens at the midpoint of the flip, while the image is
    /// edge-on and effectively invisible. Swapping at either end would show the
    /// sprite change as a pop.
    /// </para>
    /// <para>
    /// With three languages the button is a cycle rather than a toggle, and the
    /// flag it shows is the language currently active. Showing the *next*
    /// language instead is a defensible reading of a two-state toggle but an
    /// actively misleading one for a cycle, since the player can no longer infer
    /// the current state from the one alternative on offer.
    /// </para>
    /// </remarks>
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public sealed class LanguageToggleButton : MonoBehaviour
    {
        [Tooltip("Show the flag of the language currently active. Turn off to show the language the button switches to.")]
        [SerializeField] private bool _showActiveLanguage = true;

        private Button _button;
        private Image _flagImage;
        private AmbientMotion _motion;
        private UiSpriteLibrary _library;
        private TweenHandle _flip;
        private bool _isFlipping;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _flagImage = GetComponent<Image>();
            _motion = AmbientMotion.GetOrAdd(gameObject);
            _library = UiSpriteLibrary.Load();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
            LocalizationService.LanguageChanged += OnLanguageChanged;
            ApplyFlagSprite();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
            LocalizationService.LanguageChanged -= OnLanguageChanged;

            _flip.CancelIfRunning();
            _isFlipping = false;

            if (_motion != null)
            {
                _motion.ExtraScale = Vector3.one;
            }
        }

        private void OnClicked()
        {
            // Ignore repeat presses mid-flip: a second toggle would swap the
            // sprite while the first animation is still driving the scale.
            if (_isFlipping)
            {
                return;
            }

            AudioService.Play(SfxId.Toggle);
            PlayFlip();
        }

        /// <summary>
        /// Collapses the flag horizontally, switches language at the midpoint,
        /// then opens it back up.
        /// </summary>
        private void PlayFlip()
        {
            _isFlipping = true;
            _flip.CancelIfRunning();

            _flip = TweenRunner.Play(
                GameConfig.LanguageFlipHalfDuration,
                t => SetFlipAmount(1f - t),
                EaseType.InQuad,
                0f,
                () =>
                {
                    LocalizationService.ToggleLanguage();
                    ApplyFlagSprite();

                    _flip = TweenRunner.Play(
                        GameConfig.LanguageFlipHalfDuration,
                        t => SetFlipAmount(t),
                        EaseType.OutQuad,
                        0f,
                        () =>
                        {
                            SetFlipAmount(1f);
                            _isFlipping = false;
                        },
                        this);
                },
                this);
        }

        /// <summary>
        /// Drives the flip through the ambient-motion channel rather than the
        /// transform, so the button keeps breathing all the way through.
        /// </summary>
        private void SetFlipAmount(float openness)
        {
            if (_motion == null)
            {
                return;
            }

            // Never fully zero: a zero scale collapses the mesh and can make
            // uGUI drop the element for a frame.
            float x = Mathf.Max(0.02f, openness);
            _motion.ExtraScale = new Vector3(x, 1f, 1f);
        }

        private void OnLanguageChanged(GameLanguage language)
        {
            // Covers language changes that came from somewhere other than this
            // button, e.g. a settings screen added later.
            if (!_isFlipping)
            {
                ApplyFlagSprite();
            }
        }

        private void ApplyFlagSprite()
        {
            if (_flagImage == null || _library == null)
            {
                return;
            }

            GameLanguage shown = _showActiveLanguage
                ? LocalizationService.Current
                : LocalizationService.Current.Next();

            Sprite flag = _library.FlagFor(shown);

            // A language with no flag art keeps the previous image rather than
            // blanking the button, which would read as a broken control.
            if (flag != null)
            {
                _flagImage.sprite = flag;
            }
        }
    }
}
