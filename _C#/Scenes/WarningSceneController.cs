// -----------------------------------------------------------------------------
//  The Frayed Red String
//  WarningSceneController.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Flow;
using TheFrayedRedString.InputHandling;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;

namespace TheFrayedRedString.Scenes
{
    /// <summary>
    /// Drives WarningScene: shows the content warning in both languages, holds
    /// the player there for a fixed beat, then reveals the Enter prompt and
    /// waits for confirmation.
    /// </summary>
    /// <remarks>
    /// The delay before the prompt appears is the point of the scene. Making the
    /// player sit with the warning for five seconds before there is any way
    /// forward is deliberate: it is the same five minutes of patience the
    /// game's endings are built around, in miniature.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class WarningSceneController : MonoBehaviour
    {
        [Header("Timing")]
        [Tooltip("Seconds the warning is shown before the Enter prompt can appear.")]
        [SerializeField] private float _promptDelay = GameConfig.WarningPromptDelay;

        [Tooltip("Length of the prompt's reveal animation.")]
        [SerializeField] private float _revealDuration = GameConfig.WarningPromptRevealDuration;

        [Header("Text")]
        [Tooltip("Overwrite the authored warning text with the string table's version.")]
        [SerializeField] private bool _applyLocalizedText = true;

        private SpriteRenderer _promptRenderer;
        private AmbientMotion _promptMotion;
        private Camera _camera;

        private float _elapsed;
        private bool _promptRevealed;
        private bool _confirmed;

        /// <summary>True once the Enter prompt is visible and will accept input.</summary>
        public bool IsPromptReady => _promptRevealed && !_confirmed;

        private void Start()
        {
            _camera = Camera.main;

            BindWarningText();
            PreparePrompt();

            ScreenFader.FadeIn(GameConfig.SceneFadeInDuration);
        }

        private void Update()
        {
            if (_confirmed)
            {
                return;
            }

            if (!_promptRevealed)
            {
                _elapsed += Time.unscaledDeltaTime;

                if (_elapsed >= _promptDelay)
                {
                    RevealPrompt();
                }

                return;
            }

            if (InputService.ConfirmPressedThisFrame || PromptWasClicked())
            {
                Confirm();
            }
        }

        /// <summary>
        /// Pins each warning label to a fixed language.
        /// </summary>
        /// <remarks>
        /// This screen shows English and Japanese side by side rather than
        /// switching between them — a content warning has to be readable to
        /// everyone regardless of which language the menu happens to be set to.
        /// So the first label is pinned to English with
        /// <see cref="LocalizedText.BindFixed"/>, and the second follows the
        /// player's choice through <see cref="LocalizedText.BindSecondary"/> —
        /// showing Japanese by default and Persian once Persian is selected, so
        /// the pair is never English twice.
        /// </remarks>
        private void BindWarningText()
        {
            if (!_applyLocalizedText)
            {
                return;
            }

            BindLabel(ObjectNames.WarningTextEnglish, GameLanguage.English);
            BindLabel(ObjectNames.WarningTextJapanese, null);
        }

        /// <summary>
        /// Binds one warning label, either to a fixed language or — when
        /// <paramref name="language"/> is null — to the second slot of the
        /// bilingual pair.
        /// </summary>
        private static void BindLabel(string objectName, GameLanguage? language)
        {
            Transform target = UnityUtility.FindInScene(objectName);

            if (target == null)
            {
                Debug.LogWarning($"[WarningScene] Could not find a label named '{objectName}'.");
                return;
            }

            if (target.GetComponent<TMP_Text>() == null)
            {
                Debug.LogWarning($"[WarningScene] '{objectName}' has no TMP_Text component.");
                return;
            }

            LocalizedText label = UnityUtility.GetOrAdd<LocalizedText>(target.gameObject);

            if (language.HasValue)
            {
                label.BindFixed(LocKeys.WarningBody, language.Value);
            }
            else
            {
                label.BindSecondary(LocKeys.WarningBody);
            }
        }

        /// <summary>Hides the Enter prompt and freezes its idle motion until the reveal.</summary>
        private void PreparePrompt()
        {
            Transform prompt = UnityUtility.FindInScene(ObjectNames.EnterPrompt);

            if (prompt == null)
            {
                Debug.LogWarning(
                    $"[WarningScene] No '{ObjectNames.EnterPrompt}' found. " +
                    "Enter will still advance the scene, but there will be no visible prompt.");
                return;
            }

            _promptRenderer = prompt.GetComponent<SpriteRenderer>();

            if (_promptRenderer == null)
            {
                Debug.LogWarning($"[WarningScene] '{ObjectNames.EnterPrompt}' has no SpriteRenderer to fade.");
                return;
            }

            _promptMotion = AmbientMotion.GetOrAdd(prompt.gameObject);
            _promptMotion.Configure(AmbientMotionProfile.Prompt, 0f);

            // Held at zero so the prompt sits perfectly still behind its own
            // invisibility; the heartbeat starts as it appears.
            _promptMotion.Weight = 0f;
            _promptMotion.ExtraScale = Vector3.one * GameConfig.WarningPromptRevealFromScale;

            _promptRenderer.SetAlpha(0f);
        }

        /// <summary>Springs the prompt into view and starts its pulse.</summary>
        private void RevealPrompt()
        {
            _promptRevealed = true;

            if (_promptRenderer == null || _promptMotion == null)
            {
                return;
            }

            AudioService.Play(SfxId.Hover, 0.7f, 0.9f);

            _promptRenderer.FadeTo(1f, _revealDuration, EaseType.OutCubic);

            float fromScale = GameConfig.WarningPromptRevealFromScale;

            TweenRunner.Play(
                _revealDuration,
                t =>
                {
                    if (_promptMotion == null)
                    {
                        return;
                    }

                    float scale = Mathf.LerpUnclamped(fromScale, 1f, t);
                    _promptMotion.ExtraScale = new Vector3(scale, scale, 1f);

                    // Ease the heartbeat in alongside the pop, so the pulse does
                    // not snap on at full strength the instant the tween ends.
                    // Clamped because OutBack deliberately overshoots past 1.
                    _promptMotion.Weight = Mathf.Clamp01(t);
                },
                EaseType.OutBack,
                0f,
                () =>
                {
                    if (_promptMotion == null)
                    {
                        return;
                    }

                    _promptMotion.ExtraScale = Vector3.one;
                    _promptMotion.Weight = 1f;
                },
                this);
        }

        /// <summary>
        /// True when the player clicked the prompt sprite itself. The sprite is
        /// a world-space SpriteRenderer with no collider, so this is a plain
        /// bounds test rather than a physics query.
        /// </summary>
        private bool PromptWasClicked()
        {
            if (_promptRenderer == null || !InputService.PointerPressedThisFrame)
            {
                return false;
            }

            Camera activeCamera = _camera != null ? _camera : Camera.main;
            if (activeCamera == null)
            {
                return false;
            }

            Vector3 world = activeCamera.ScreenToWorldPoint(InputService.PointerPosition);
            Bounds bounds = _promptRenderer.bounds;

            // Compare in 2D only: a sprite's bounds have zero depth, so a
            // three-axis Bounds.Contains would reject every point whose z does
            // not match the sprite's plane exactly.
            return world.x >= bounds.min.x && world.x <= bounds.max.x
                   && world.y >= bounds.min.y && world.y <= bounds.max.y;
        }

        /// <summary>Accepts the warning and moves on to the main menu.</summary>
        private void Confirm()
        {
            if (_confirmed)
            {
                return;
            }

            _confirmed = true;

            AudioService.Play(SfxId.Confirm);

            // Let the confirmation chime land before the curtain starts moving.
            TweenRunner.After(
                GameConfig.WarningConfirmSettleDuration,
                GameFlowService.EnterMainMenu,
                this);
        }
    }
}
