// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VideoModuleGuard.cs  (Editor only)
// -----------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Turns the video code on when Unity's Video module is present, and off
    /// when it is not.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <c>UnityEngine.Video</c> is a built-in module that a project can be
    /// created without, and this one was: it is not in the manifest. Code that
    /// references <c>VideoPlayer</c> in a project without it does not fail
    /// gracefully — it fails to compile, which takes every other script in the
    /// game down with it, including the ones that would have told you why.
    /// </para>
    /// <para>
    /// So the video path is behind a define, and this sets the define by looking
    /// for the type rather than by trusting anybody to remember. Enable the
    /// module and the credits play; leave it off and act seven says so in the
    /// console and carries on to the closing cards. Neither state is broken.
    /// </para>
    /// </remarks>
    [InitializeOnLoad]
    public static class VideoModuleGuard
    {
        /// <summary>Defined while <c>UnityEngine.Video</c> is available.</summary>
        public const string Define = "TFRS_VIDEO";

        private const string ManifestHint =
            "Window ▸ Package Manager ▸ Built-in ▸ Video ▸ Enable, or add " +
            "\"com.unity.modules.video\": \"1.0.0\" to Packages/manifest.json.";

        static VideoModuleGuard()
        {
            EditorApplication.delayCall += Sync;
        }

        /// <summary>Adds or removes the define to match the project.</summary>
        [MenuItem("The Frayed Red String/Check The Video Module")]
        public static void Sync()
        {
            bool present = FindVideoPlayerType() != null;

            NamedBuildTarget target = NamedBuildTarget.FromBuildTargetGroup(
                BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget));

            string symbols = PlayerSettings.GetScriptingDefineSymbols(target) ?? string.Empty;
            bool defined = Array.IndexOf(symbols.Split(';'), Define) >= 0;

            if (present == defined)
            {
                return;
            }

            string updated = present
                ? (string.IsNullOrEmpty(symbols) ? Define : symbols + ";" + Define)
                : string.Join(";", Array.FindAll(symbols.Split(';'), s => s != Define && s.Length > 0));

            PlayerSettings.SetScriptingDefineSymbols(target, updated);

            Debug.Log(
                present
                    ? "[Video] Unity's Video module is enabled, so the credits video is switched on. " +
                      "Unity will recompile."
                    : "[Video] Unity's Video module is not in this project, so the credits video is " +
                      $"switched off and act seven plays without it. To turn it on: {ManifestHint}");
        }

        /// <summary>
        /// The <c>VideoPlayer</c> type, or <c>null</c> when the module is absent.
        /// </summary>
        /// <remarks>
        /// Searched across loaded assemblies by name rather than referenced,
        /// because referencing it is the thing that cannot compile when it is
        /// missing.
        /// </remarks>
        private static Type FindVideoPlayerType()
        {
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type found = assembly.GetType("UnityEngine.Video.VideoPlayer", false);

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
    }
}
