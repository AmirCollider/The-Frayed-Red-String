// -----------------------------------------------------------------------------
//  The Frayed Red String
//  PointerClickRelay.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// Reports pointer clicks on a plain graphic without making it a
    /// <see cref="UnityEngine.UI.Selectable"/>.
    /// </summary>
    /// <remarks>
    /// Used for the invisible sheet behind the load panel. A Button there would
    /// be wrong twice over: it would fire a hover sound every time the cursor
    /// crossed the screen, and it would register as an interactive control, so
    /// the ambient click sound would suppress itself over the entire backdrop.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class PointerClickRelay : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>Raised when the graphic is clicked.</summary>
        public event Action Clicked;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }

        private void OnDestroy()
        {
            Clicked = null;
        }
    }
}
