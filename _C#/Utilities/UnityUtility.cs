// -----------------------------------------------------------------------------
//  The Frayed Red String
//  UnityUtility.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace TheFrayedRedString.Utilities
{
    /// <summary>
    /// Small hierarchy and component helpers used by the scene installers.
    /// </summary>
    public static class UnityUtility
    {
        /// <summary>Returns the component on <paramref name="target"/>, adding it if absent.</summary>
        public static T GetOrAdd<T>(GameObject target) where T : Component
        {
            if (target == null)
            {
                return null;
            }

            T existing = target.GetComponent<T>();
            return existing != null ? existing : target.AddComponent<T>();
        }

        /// <summary>Returns the component on <paramref name="target"/>, adding it if absent.</summary>
        public static T GetOrAdd<T>(Component target) where T : Component
        {
            return target == null ? null : GetOrAdd<T>(target.gameObject);
        }

        /// <summary>
        /// Depth-first search for a descendant with the given name. Inactive
        /// objects are included, which matters because the load panel ships
        /// disabled in MainMenu.unity.
        /// </summary>
        public static Transform FindDeep(Transform root, string name)
        {
            if (root == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (root.name == name)
            {
                return root;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform found = FindDeep(root.GetChild(i), name);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// Depth-first search across every root object of the active scene.
        /// </summary>
        public static Transform FindInScene(string name)
        {
            UnityEngine.SceneManagement.Scene scene =
                UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (!scene.IsValid())
            {
                return null;
            }

            List<GameObject> roots = new List<GameObject>(scene.rootCount);
            scene.GetRootGameObjects(roots);

            for (int i = 0; i < roots.Count; i++)
            {
                Transform found = FindDeep(roots[i].transform, name);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// Depth-first search for a descendant carrying <typeparamref name="T"/>,
        /// matching by GameObject name. Returns <c>null</c> when nothing matches.
        /// </summary>
        public static T FindDeep<T>(Transform root, string name) where T : Component
        {
            Transform found = FindDeep(root, name);
            return found == null ? null : found.GetComponent<T>();
        }

        /// <summary>
        /// Deterministic 0..1 hash of a string. Used to give every animated
        /// element its own phase offset without introducing per-run randomness,
        /// so the menu looks identical every time it is opened.
        /// </summary>
        public static float StableHash01(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0f;
            }

            unchecked
            {
                uint hash = 2166136261u;
                for (int i = 0; i < value.Length; i++)
                {
                    hash ^= value[i];
                    hash *= 16777619u;
                }

                return (hash % 100000u) / 100000f;
            }
        }
    }
}
