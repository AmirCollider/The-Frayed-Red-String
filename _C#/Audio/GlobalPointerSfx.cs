// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GlobalPointerSfx.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.InputHandling;
using TheFrayedRedString.Presentation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Gives a click anywhere on screen an audible response, not just clicks
    /// that land on a button.
    /// </summary>
    /// <remarks>
    /// This is what makes WarningScene audible at all: that scene contains no
    /// <see cref="Selectable"/> whatsoever, so without a global listener the
    /// player's clicks would be silent.
    ///
    /// To avoid doubling up, the component first raycasts through the
    /// EventSystem. If the press landed on a <see cref="Selectable"/>, that
    /// control's own <see cref="SelectableSfx"/> is about to speak and this
    /// component stays quiet.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class GlobalPointerSfx : MonoBehaviour
    {
        private static readonly List<RaycastResult> RaycastBuffer = new List<RaycastResult>(16);

        [SerializeField] private SfxId _sound = SfxId.Tap;
        [SerializeField, Range(0f, 2f)] private float _volume = 0.55f;

        /// <summary>Suppresses the ambient tap, e.g. while a transition is running.</summary>
        public bool Muted { get; set; }

        /// <summary>Attaches the listener to the persistent service host.</summary>
        public static GlobalPointerSfx Install(GameObject host)
        {
            if (host == null)
            {
                return null;
            }

            GlobalPointerSfx existing = host.GetComponent<GlobalPointerSfx>();
            return existing != null ? existing : host.AddComponent<GlobalPointerSfx>();
        }

        private void Update()
        {
            if (Muted || !InputService.PointerPressedThisFrame)
            {
                return;
            }

            // Behind the fade curtain there is nothing to click, so a click
            // there should not sound like there was.
            if (ScreenFader.IsCovering)
            {
                return;
            }

            if (PointerIsOverSelectable(InputService.PointerPosition))
            {
                return;
            }

            AudioService.Play(_sound, _volume);
        }

        /// <summary>
        /// True when the pointer is over a UI element that is (or is parented
        /// under) an interactive control.
        /// </summary>
        /// <remarks>
        /// <see cref="EventSystem.IsPointerOverGameObject"/> is not usable here:
        /// the menu's full-screen background Image is a raycast target, so that
        /// call would report "over UI" for literally every pixel of MainMenu.
        /// An explicit raycast and a Selectable check is the precise question.
        /// </remarks>
        private static bool PointerIsOverSelectable(Vector2 screenPosition)
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                return false;
            }

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = screenPosition
            };

            RaycastBuffer.Clear();
            eventSystem.RaycastAll(pointerData, RaycastBuffer);

            for (int i = 0; i < RaycastBuffer.Count; i++)
            {
                GameObject hit = RaycastBuffer[i].gameObject;
                if (hit == null)
                {
                    continue;
                }

                Selectable selectable = hit.GetComponentInParent<Selectable>();
                if (selectable != null && selectable.IsInteractable())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
