// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SelectableSfx.cs
// -----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Gives one <see cref="Selectable"/> its hover and press sounds.
    /// </summary>
    /// <remarks>
    /// The press sound fires on pointer *down* rather than on click: waiting for
    /// the release makes a button feel a frame behind the player's finger. The
    /// button's actual action still runs on click, as uGUI intends.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class SelectableSfx : MonoBehaviour,
        IPointerEnterHandler,
        IPointerDownHandler,
        ISubmitHandler,
        ISelectHandler
    {
        [Header("Sounds")]
        [SerializeField] private SfxId _hoverSound = SfxId.Hover;
        [SerializeField] private SfxId _pressSound = SfxId.Click;
        [SerializeField] private SfxId _blockedSound = SfxId.Denied;

        [Header("Levels")]
        [SerializeField, Range(0f, 2f)] private float _hoverVolume = 0.45f;
        [SerializeField, Range(0f, 2f)] private float _pressVolume = 1f;

        private Selectable _selectable;

        /// <summary>Overrides the sound played when this control is pressed.</summary>
        public SfxId PressSound
        {
            get => _pressSound;
            set => _pressSound = value;
        }

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (IsUsable)
            {
                AudioService.Play(_hoverSound, _hoverVolume);
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            PlayPress();
        }

        void ISubmitHandler.OnSubmit(BaseEventData eventData)
        {
            PlayPress();
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            // Keyboard / gamepad navigation should sound the same as hovering
            // with the mouse.
            if (IsUsable)
            {
                AudioService.Play(_hoverSound, _hoverVolume);
            }
        }

        private void PlayPress()
        {
            AudioService.Play(IsUsable ? _pressSound : _blockedSound, _pressVolume);
        }

        /// <summary>
        /// Whether pressing this control should sound like it did something.
        /// A missing Selectable counts as usable: the component is only ever
        /// attached to controls, so its absence means something else is odd —
        /// not that the button is disabled.
        /// </summary>
        private bool IsUsable => _selectable == null || _selectable.IsInteractable();
    }
}
