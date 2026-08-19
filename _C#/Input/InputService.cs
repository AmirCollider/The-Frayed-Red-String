// -----------------------------------------------------------------------------
//  The Frayed Red String
//  InputService.cs
//
//  The project uses the new Input System package (the EventSystem in both
//  scenes runs InputSystemUIInputModule), so the legacy UnityEngine.Input API is
//  unavailable. This wrapper is the single place that talks to the package.
// -----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFrayedRedString.InputHandling
{
    /// <summary>
    /// Frame-accurate polling of the handful of inputs the menus care about.
    /// Every accessor tolerates a missing device, so the game still runs on a
    /// machine with no keyboard or no mouse attached.
    /// </summary>
    public static class InputService
    {
        /// <summary>
        /// True on the frame Enter (or numpad Enter) is pressed. The warning
        /// scene gates its transition on exactly this.
        /// </summary>
        public static bool ConfirmPressedThisFrame
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                if (keyboard == null)
                {
                    return false;
                }

                return keyboard.enterKey.wasPressedThisFrame
                       || keyboard.numpadEnterKey.wasPressedThisFrame;
            }
        }

        /// <summary>
        /// True on the frame the player asks the story to move on.
        /// </summary>
        /// <remarks>
        /// Space as well as Enter, because a visual novel is read with one hand
        /// resting on the keyboard and every other game in the genre accepts
        /// both. Mouse clicks are not folded in here: a click has to be tested
        /// against what it landed on, which is a question only the scene can
        /// answer.
        /// </remarks>
        public static bool AdvancePressedThisFrame
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                if (keyboard == null)
                {
                    return false;
                }

                return keyboard.enterKey.wasPressedThisFrame
                       || keyboard.numpadEnterKey.wasPressedThisFrame
                       || keyboard.spaceKey.wasPressedThisFrame;
            }
        }

        /// <summary>
        /// True while the player is holding the fast-forward key.
        /// </summary>
        /// <remarks>
        /// Control, following the convention every visual novel shares. Held
        /// rather than toggled, so it cannot be left on by accident and skip a
        /// scene the player wanted to read.
        /// </remarks>
        public static bool FastForwardHeld
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                if (keyboard == null)
                {
                    return false;
                }

                return keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed;
            }
        }

        /// <summary>True on the frame Escape is pressed.</summary>
        public static bool CancelPressedThisFrame
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                return keyboard != null && keyboard.escapeKey.wasPressedThisFrame;
            }
        }

        /// <summary>True on the frame the primary pointer button goes down.</summary>
        public static bool PointerPressedThisFrame
        {
            get
            {
                Mouse mouse = Mouse.current;
                if (mouse != null && mouse.leftButton.wasPressedThisFrame)
                {
                    return true;
                }

                Touchscreen touchscreen = Touchscreen.current;
                return touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame;
            }
        }

        /// <summary>Current pointer position in screen space.</summary>
        public static Vector2 PointerPosition
        {
            get
            {
                Mouse mouse = Mouse.current;
                if (mouse != null)
                {
                    return mouse.position.ReadValue();
                }

                Touchscreen touchscreen = Touchscreen.current;
                if (touchscreen != null)
                {
                    return touchscreen.primaryTouch.position.ReadValue();
                }

                return Vector2.zero;
            }
        }

        /// <summary>True while any key is held. Used only for "skip" affordances.</summary>
        public static bool AnyKeyHeld
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                return keyboard != null && keyboard.anyKey.isPressed;
            }
        }
    }
}
