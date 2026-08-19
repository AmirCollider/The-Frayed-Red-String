// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SfxWavExporter.cs  (Editor only)
//
//  The sounds, as files you can actually play.
//
//  Every sound in this game is synthesised at runtime — there is no folder of
//  .wav files to click on, which is why hearing one before pressing Play needs
//  the Editor's internal audio preview, and why it stops working the moment
//  Unity moves that API.
//
//  This route does not depend on any of it. The clip is written to disk as an
//  ordinary .wav, and from that point on it is a normal asset: Unity's own
//  Inspector plays it, the Project window plays it, and so does anything else
//  on the machine.
// -----------------------------------------------------------------------------

using System;
using System.IO;
using TheFrayedRedString.Audio;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Writes the generated sound effects out as .wav files.</summary>
    public static class SfxWavExporter
    {
        /// <summary>Where the exported files land.</summary>
        public const string Folder = "Assets/Audio/GeneratedSfx";

        /// <summary>Writes every sound the game can make, and shows the folder.</summary>
        [MenuItem("The Frayed Red String/Export Sound Effects To WAV")]
        public static void ExportAll()
        {
            AssetPaths.EnsureFolder("Assets/Audio");
            AssetPaths.EnsureFolder(Folder);

            int written = 0;
            string last = null;

            foreach (SfxId id in Enum.GetValues(typeof(SfxId)))
            {
                string path = Write(id);

                if (path != null)
                {
                    written++;
                    last = path;
                }
            }

            AssetDatabase.Refresh();

            if (last != null)
            {
                Object folder = AssetDatabase.LoadAssetAtPath<Object>(Folder);
                EditorGUIUtility.PingObject(folder);
                Selection.activeObject = folder;
            }

            Debug.Log(
                $"[Story] Wrote {written} sound(s) to {Folder}. " +
                "Click one in the Project window and the Inspector will play it.");
        }

        /// <summary>
        /// Writes one sound and selects it, so the Inspector's own player comes
        /// up with it loaded.
        /// </summary>
        /// <returns>The asset path, or null if it could not be written.</returns>
        public static string ExportAndReveal(SfxId id)
        {
            AssetPaths.EnsureFolder("Assets/Audio");
            AssetPaths.EnsureFolder(Folder);

            string path = Write(id);

            if (path == null)
            {
                Debug.LogWarning($"[Story] '{id}' could not be synthesised, so there was nothing to write.");
                return null;
            }

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            AudioClip asset = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

            if (asset != null)
            {
                Selection.activeObject = asset;
                EditorGUIUtility.PingObject(asset);
            }

            return path;
        }

        /// <summary>Writes one sound to disk. Returns its path, or null.</summary>
        private static string Write(SfxId id)
        {
            AudioClip clip = ProceduralSfxLibrary.Get(id);

            if (clip == null || clip.samples <= 0)
            {
                return null;
            }

            float[] samples = new float[clip.samples * clip.channels];

            if (!clip.GetData(samples, 0))
            {
                return null;
            }

            string path = $"{Folder}/{id}.wav";
            File.WriteAllBytes(path, Encode(samples, clip.channels, clip.frequency));

            return path;
        }

        /// <summary>
        /// Encodes samples as a 16-bit PCM RIFF/WAVE file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Written out by hand because the format is forty-four bytes of header
        /// and then the samples, and pulling in a library to do that would be a
        /// dependency for something shorter than the sentence describing it.
        /// </para>
        /// <para>
        /// 16-bit rather than the 32-bit float the clip holds: every player on
        /// earth reads 16-bit PCM, the files are half the size, and these are
        /// short blips whose top four bits of precision nobody is going to hear.
        /// </para>
        /// </remarks>
        private static byte[] Encode(float[] samples, int channels, int frequency)
        {
            const int headerBytes = 44;
            const short bitsPerSample = 16;

            int dataBytes = samples.Length * sizeof(short);
            byte[] file = new byte[headerBytes + dataBytes];

            using (MemoryStream stream = new MemoryStream(file))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                short blockAlign = (short)(channels * (bitsPerSample / 8));

                writer.Write(new[] { 'R', 'I', 'F', 'F' });
                writer.Write(headerBytes - 8 + dataBytes);
                writer.Write(new[] { 'W', 'A', 'V', 'E' });

                writer.Write(new[] { 'f', 'm', 't', ' ' });
                writer.Write(16);                                   // chunk size
                writer.Write((short)1);                             // PCM
                writer.Write((short)channels);
                writer.Write(frequency);
                writer.Write(frequency * blockAlign);               // bytes per second
                writer.Write(blockAlign);
                writer.Write(bitsPerSample);

                writer.Write(new[] { 'd', 'a', 't', 'a' });
                writer.Write(dataBytes);

                for (int i = 0; i < samples.Length; i++)
                {
                    // Clamped before scaling. A synthesised sum can overshoot 1,
                    // and letting it wrap turns a loud note into a burst of
                    // noise at the opposite polarity.
                    float sample = Mathf.Clamp(samples[i], -1f, 1f);
                    writer.Write((short)Mathf.RoundToInt(sample * short.MaxValue));
                }
            }

            return file;
        }
    }
}
