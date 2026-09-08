// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SafeFrameWatcher.cs
// -----------------------------------------------------------------------------

using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Watches for the screen changing shape and re-frames the game when it
    /// does.
    /// </summary>
    /// <remarks>
    /// <para>
    /// On a desktop that is somebody dragging the corner of the window. On a
    /// phone it is the player turning the handset over — which lands as a swap
    /// of width and height, in the middle of a frame, with no notification
    /// anybody can subscribe to. Polling two integers is the whole cost of
    /// handling it, and the comparison in
    /// <see cref="SafeFrame.TickResolution"/> means the work only happens on the
    /// frames where something actually moved.
    /// </para>
    /// <para>
    /// Lives on the letterbox object, which lives on the service host, so there
    /// is one of these for the whole game rather than one per scene.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class SafeFrameWatcher : MonoBehaviour
    {
        private void Update()
        {
            SafeFrame.TickResolution();
        }
    }
}
