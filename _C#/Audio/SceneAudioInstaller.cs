// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SceneAudioInstaller.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Gives every interactive control in a scene its click and hover sounds.
    /// </summary>
    /// <remarks>
    /// Doing this by hand would mean remembering to attach a component to every
    /// button ever added to the game, and the first one forgotten would be a
    /// silent button nobody notices until playtest. Sweeping the scene at load
    /// makes the sound a property of being a button, not of being wired up.
    /// </remarks>
    public static class SceneAudioInstaller
    {
        /// <summary>
        /// Attaches <see cref="SelectableSfx"/> to every <see cref="Selectable"/>
        /// in the scene, inactive ones included.
        /// </summary>
        /// <returns>How many controls were given sounds.</returns>
        public static int Install(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded)
            {
                return 0;
            }

            List<GameObject> roots = new List<GameObject>(scene.rootCount);
            scene.GetRootGameObjects(roots);

            int installed = 0;

            for (int i = 0; i < roots.Count; i++)
            {
                Selectable[] selectables = roots[i].GetComponentsInChildren<Selectable>(true);

                for (int s = 0; s < selectables.Length; s++)
                {
                    Selectable selectable = selectables[s];
                    if (selectable == null || selectable.GetComponent<SelectableSfx>() != null)
                    {
                        continue;
                    }

                    UnityUtility.GetOrAdd<SelectableSfx>(selectable.gameObject);
                    installed++;
                }
            }

            return installed;
        }
    }
}
