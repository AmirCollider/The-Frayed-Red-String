// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SoundPreview.cs  (Editor only)
//
//  Playing a sound in the Editor without entering play mode.
//
//  There is no public API for this. The Project window's own audio preview runs
//  through UnityEditor.AudioUtil, which is internal, so the only way in is
//  reflection — and the Story Editor's ▶ button was doing exactly that, against
//  one hard-coded assembly and one hard-coded method signature. Unity moved the
//  type into a different assembly, the lookup returned null, and the button did
//  nothing at all: no sound, and a Debug.Log nobody was looking for.
//
//  So this looks for the type wherever it actually is, tries every name and
//  signature the method has shipped under, remembers what it found, and says so
//  plainly and once when there is nothing to find.
// -----------------------------------------------------------------------------

using System;
using System.Reflection;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Plays an AudioClip in the Editor, outside play mode.</summary>
    public static class SoundPreview
    {
        /// <summary>The names the preview method has gone by.</summary>
        private static readonly string[] PlayNames =
        {
            "PlayPreviewClip", "PlayClip", "PlayPreviewAudioClip", "PlayAudioClip"
        };

        /// <summary>And the names for stopping it again.</summary>
        private static readonly string[] StopNames =
        {
            "StopAllPreviewClips", "StopAllClips", "StopAllPreviewAudioClips"
        };

        private const BindingFlags Search =
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        private static MethodInfo _play;
        private static MethodInfo _stop;
        private static bool _resolved;
        private static bool _warned;

        /// <summary>True when this Unity version will play a preview at all.</summary>
        public static bool IsAvailable
        {
            get
            {
                Resolve();
                return _play != null;
            }
        }

        /// <summary>Plays a clip once, from the start.</summary>
        /// <returns>False when the Editor offers no way to do it.</returns>
        public static bool Play(AudioClip clip)
        {
            if (clip == null)
            {
                return false;
            }

            Resolve();

            if (_play == null)
            {
                Warn();
                return false;
            }

            if (IsEditorMuted())
            {
                Debug.LogWarning(
                    "[Story] The sound played, but the Editor is muted — that is the speaker button in the " +
                    "Game view's toolbar. Turn it off and press ▶ again.");
            }

            try
            {
                Stop();
                _play.Invoke(null, ArgumentsFor(_play, clip));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[Story] The Editor refused to play that sound: {exception.Message}");
                return false;
            }
        }

        /// <summary>Silences whatever the Editor is previewing.</summary>
        public static void Stop()
        {
            Resolve();

            if (_stop == null)
            {
                return;
            }

            try
            {
                _stop.Invoke(null, null);
            }
            catch (Exception)
            {
                // Nothing was playing, or this version disagrees about how to
                // stop it. Either way there is no sound to chase.
            }
        }

        /// <summary>
        /// Fills in whatever arguments the resolved overload happens to want.
        /// </summary>
        /// <remarks>
        /// The three-argument form is clip, start sample and loop; the
        /// one-argument form is just the clip. Reading the parameters rather
        /// than assuming one shape is what lets both work without a version
        /// check.
        /// </remarks>
        private static object[] ArgumentsFor(MethodInfo method, AudioClip clip)
        {
            ParameterInfo[] parameters = method.GetParameters();
            object[] arguments = new object[parameters.Length];

            arguments[0] = clip;

            for (int i = 1; i < parameters.Length; i++)
            {
                Type type = parameters[i].ParameterType;

                arguments[i] = type == typeof(bool)
                    ? false                       // loop
                    : type == typeof(int) ? 0     // start sample
                    : type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            return arguments;
        }

        /// <summary>
        /// Finds AudioUtil wherever this Unity version keeps it.
        /// </summary>
        /// <remarks>
        /// Every loaded assembly is asked rather than one being named. The type
        /// has lived in UnityEditor.dll and in UnityEditor.CoreModule.dll at
        /// different times, and a lookup that names the wrong one comes back
        /// null — which is indistinguishable, from the caller, from a Unity that
        /// cannot preview audio at all.
        /// </remarks>
        private static void Resolve()
        {
            if (_resolved)
            {
                return;
            }

            _resolved = true;

            Type audioUtil = FindAudioUtil();

            if (audioUtil == null)
            {
                return;
            }

            _play = FindMethod(audioUtil, PlayNames, typeof(AudioClip));
            _stop = FindMethod(audioUtil, StopNames, null);
        }

        /// <summary>
        /// Finds the Editor's audio preview helper, wherever it now lives.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The first version of this named the type — <c>UnityEditor.AudioUtil</c>
        /// — and searched the assemblies whose name begins with UnityEditor.
        /// That is two assumptions, and one of them is enough to make the
        /// button silent: the namespace has moved as well as the assembly, and
        /// asking for a full type name that no longer exists returns null with
        /// nothing to say about it.
        /// </para>
        /// <para>
        /// So nothing is assumed. Every loaded assembly is walked for a type
        /// simply <em>called</em> AudioUtil that has a static method taking an
        /// AudioClip. Slow, once, on the first press.
        /// </para>
        /// </remarks>
        private static Type FindAudioUtil()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // The known place first, so the common case costs one lookup.
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type direct = assemblies[i].GetType("UnityEditor.AudioUtil", false);

                if (direct != null && HasPlayMethod(direct))
                {
                    return direct;
                }
            }

            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types;

                try
                {
                    types = assemblies[i].GetTypes();
                }
                catch (ReflectionTypeLoadException loaded)
                {
                    // An assembly with a broken reference still hands back the
                    // types it did manage to load.
                    types = loaded.Types;
                }
                catch (Exception)
                {
                    continue;
                }

                for (int j = 0; j < types.Length; j++)
                {
                    Type candidate = types[j];

                    if (candidate != null && candidate.Name == "AudioUtil" && HasPlayMethod(candidate))
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }

        private static bool HasPlayMethod(Type candidate)
        {
            return FindMethod(candidate, PlayNames, typeof(AudioClip)) != null;
        }

        /// <summary>
        /// Everything this class managed to work out, written to the Console.
        /// </summary>
        /// <remarks>
        /// There is no way to see any of this from the outside: the button
        /// either makes a sound or it does not. When it does not, this says
        /// which of the four things went wrong — no type, no method, a muted
        /// Editor, or a clip that was never synthesised.
        /// </remarks>
        public static string Describe()
        {
            Resolve();

            Type audioUtil = FindAudioUtil();

            string type = audioUtil == null
                ? "not found in any loaded assembly"
                : $"{audioUtil.FullName}, in {audioUtil.Assembly.GetName().Name}";

            string play = _play == null
                ? "not found"
                : $"{_play.Name}({string.Join(", ", Array.ConvertAll(_play.GetParameters(), p => p.ParameterType.Name))})";

            string stop = _stop == null ? "not found" : _stop.Name;

            return $"Editor audio preview\n"
                   + $"  type   : {type}\n"
                   + $"  play   : {play}\n"
                   + $"  stop   : {stop}\n"
                   + $"  muted  : {IsEditorMuted()}";
        }

        /// <summary>
        /// True when the Editor itself is silenced.
        /// </summary>
        /// <remarks>
        /// The Game view's speaker toggle. It silences previews as well as play
        /// mode, so with it on the ▶ button works perfectly and produces no
        /// sound — which is indistinguishable from the button being broken.
        /// </remarks>
        public static bool IsEditorMuted()
        {
            Type utility = FindType("UnityEditor.EditorUtility");
            PropertyInfo muted = utility?.GetProperty("audioMasterMute", Search);

            try
            {
                return muted != null && (bool)muted.GetValue(null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Type FindType(string fullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
                Type found = assemblies[i].GetType(fullName, false);

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// The first method with one of these names whose first parameter is
        /// <paramref name="firstParameter"/>, or which takes none at all when
        /// that is null.
        /// </summary>
        private static MethodInfo FindMethod(Type owner, string[] names, Type firstParameter)
        {
            for (int i = 0; i < names.Length; i++)
            {
                MethodInfo[] candidates = owner.GetMethods(Search);

                for (int j = 0; j < candidates.Length; j++)
                {
                    if (candidates[j].Name != names[i])
                    {
                        continue;
                    }

                    ParameterInfo[] parameters = candidates[j].GetParameters();

                    if (firstParameter == null)
                    {
                        if (parameters.Length == 0)
                        {
                            return candidates[j];
                        }

                        continue;
                    }

                    if (parameters.Length > 0 && parameters[0].ParameterType == firstParameter)
                    {
                        return candidates[j];
                    }
                }
            }

            return null;
        }

        private static void Warn()
        {
            if (_warned)
            {
                return;
            }

            _warned = true;

            Debug.LogWarning(
                "[Story] This Unity version does not expose the Editor's audio preview, so the ▶ buttons " +
                "in the Story Editor cannot play anything. The sounds themselves are fine — press Play " +
                "and they will be there.");
        }
    }
}
