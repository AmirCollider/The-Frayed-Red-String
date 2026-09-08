// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GamePlatform.cs
// -----------------------------------------------------------------------------

using UnityEngine;

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// What kind of machine the game is running on, and the handful of device
    /// settings that follow from the answer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// One place answers "is this a phone", because two different files asking
    /// the question two different ways is how a build ends up with the touch
    /// controls of a phone and the screen handling of a desktop.
    /// </para>
    /// <para>
    /// <see cref="ForceMobileLayout"/> exists so the phone layout can be looked
    /// at in the editor without making a build. A change nobody can see before
    /// exporting is a change that gets tested once, on a device, at the point
    /// where fixing it is most expensive.
    /// </para>
    /// </remarks>
    public static class GamePlatform
    {
        /// <summary>
        /// Treat this session as a phone even when it is not one. Editor and
        /// development builds only; a release build ignores it.
        /// </summary>
        public static bool ForceMobileLayout;

        /// <summary>True on a phone or tablet.</summary>
        public static bool IsMobile
        {
            get
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (ForceMobileLayout)
                {
                    return true;
                }
#endif

#if UNITY_ANDROID || UNITY_IOS
                // Compile-time on the platforms that are always touch devices,
                // so the answer does not depend on a runtime query that the
                // editor answers differently from the device.
                return true;
#else
                return Application.isMobilePlatform;
#endif
            }
        }

        /// <summary>
        /// Applies the device settings the game needs to be playable on a
        /// phone. Does nothing anywhere else.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Landscape, locked. Everything in this game — every background, the
        /// frame, both character positions, the dialogue box — is laid out at
        /// 1920×1080, and a portrait phone has nowhere to put it. Allowing both
        /// landscape orientations rather than one means the player can hold the
        /// phone whichever way round they like and the game does not care.
        /// </para>
        /// <para>
        /// And the screen must not sleep. The good and normal endings are only
        /// reachable by waiting five real minutes without touching anything —
        /// which is, to the letter, the behaviour a phone reads as "the player
        /// has walked away" before it dims and locks. The one mechanic the
        /// story turns on is the one the default sleep timer would make
        /// impossible to reach.
        /// </para>
        /// </remarks>
        public static void ConfigureDevice()
        {
            if (!IsMobile)
            {
                return;
            }

            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.AutoRotation;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Pinned rather than left to the platform default, which on
            // Android is 30. The idle motion every character and background
            // carries is a slow sine, and a slow sine at 30fps is visibly
            // stepped where the same motion at 60 reads as breathing.
            Application.targetFrameRate = 60;
        }
    }
}
