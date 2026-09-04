// -----------------------------------------------------------------------------
//  The Frayed Red String
//  AssetPaths.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Shared asset-database helpers for the library generators.
    /// </summary>
    public static class AssetPaths
    {
        /// <summary>Creates a folder and any missing parents.</summary>
        public static void EnsureFolder(string path)
        {
            if (string.IsNullOrEmpty(path) || AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string leaf = Path.GetFileName(path);

            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(leaf))
            {
                return;
            }

            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }

        /// <summary>
        /// Finds a sprite by asset file name, importing the texture as a sprite
        /// first if it is not one yet.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The conversion step is what makes the art pipeline hands-off. A PNG
        /// dropped into the project imports with whatever the project's default
        /// texture type is, and if that is not "Sprite (2D and UI)" then
        /// <c>LoadAssetAtPath&lt;Sprite&gt;</c> returns null — the file is
        /// clearly there, the name is clearly right, and the library comes out
        /// empty with nothing to explain why. Rather than log that puzzle and
        /// ask for a manual fix on every new asset, this changes the import
        /// setting and reimports.
        /// </para>
        /// <para>
        /// Only textures matched by name are touched, and only when they are not
        /// already sprites, so nothing outside the art folders can be
        /// reinterpreted by accident.
        /// </para>
        /// </remarks>
        public static Sprite FindSprite(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return null;
            }

            Sprite existing = LoadByExactName<Sprite>("t:Sprite " + assetName, assetName);
            if (existing != null)
            {
                return existing;
            }

            string texturePath = FindPathByExactName("t:Texture2D " + assetName, assetName);
            if (string.IsNullOrEmpty(texturePath))
            {
                return null;
            }

            if (!ConvertToSprite(texturePath))
            {
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        }

        /// <summary>
        /// Finds every sprite in a folder, keyed by asset file name with
        /// surrounding whitespace removed.
        /// </summary>
        /// <remarks>
        /// The trim is load-bearing: one of the character sprites ships as
        /// <c>"HaruSeriousAngryFrown .png"</c>, with a space before the
        /// extension. Looking it up by the name anyone would actually type has
        /// to succeed.
        /// </remarks>
        public static Dictionary<string, Sprite> CollectSprites(string folder)
        {
            Dictionary<string, Sprite> found = new Dictionary<string, Sprite>(32);

            if (!AssetDatabase.IsValidFolder(folder))
            {
                return found;
            }

            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                string key = Path.GetFileNameWithoutExtension(path)?.Trim();

                if (string.IsNullOrEmpty(key) || found.ContainsKey(key))
                {
                    continue;
                }

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (sprite == null && ConvertToSprite(path))
                {
                    sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }

                if (sprite != null)
                {
                    found[key] = sprite;
                }
            }

            return found;
        }

        /// <summary>Loads a typed asset whose file name matches exactly.</summary>
        public static T LoadByExactName<T>(string filter, string assetName) where T : Object
        {
            string path = FindPathByExactName(filter, assetName);
            return string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<T>(path);
        }

        /// <summary>
        /// Resolves a search filter to the single asset whose file name matches
        /// <paramref name="assetName"/> exactly.
        /// </summary>
        /// <remarks>
        /// <see cref="AssetDatabase.FindAssets(string)"/> matches loosely, so
        /// searching for "MainMenu" also returns "MainMenuTest". Confirming the
        /// file name is what makes the result trustworthy.
        /// </remarks>
        public static string FindPathByExactName(string filter, string assetName)
        {
            string[] guids = AssetDatabase.FindAssets(filter);

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                if (Path.GetFileNameWithoutExtension(path)?.Trim() == assetName)
                {
                    return path;
                }
            }

            return null;
        }

        private static bool ConvertToSprite(string texturePath)
        {
            if (AssetImporter.GetAtPath(texturePath) is not TextureImporter importer)
            {
                return false;
            }

            if (importer.textureType == TextureImporterType.Sprite)
            {
                return true;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.SaveAndReimport();

            Debug.Log($"[Assets] Reimported '{texturePath}' as a Sprite so the runtime can load it.");
            return true;
        }
    }
}
