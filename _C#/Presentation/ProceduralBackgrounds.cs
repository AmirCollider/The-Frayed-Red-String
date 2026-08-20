// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ProceduralBackgrounds.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Stand-in scenery for a place the art has not been drawn for yet.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The stage used to keep the previous background when it was asked for one
    /// it did not have, on the reasoning that a missing asset should read as
    /// "this scene did not change" rather than as a rendering failure. That is
    /// right for a typo and wrong for a script written ahead of the art: act
    /// five needs a bloodied corridor and act six needs a machine room, and
    /// neither exists, so both acts would play out entirely in the last room the
    /// player happened to be standing in — with no indication that anything was
    /// missing except a line in the console.
    /// </para>
    /// <para>
    /// So a name with no file behind it gets a picture generated from the name
    /// itself. Two bands of colour, a horizon, and a vignette, all derived from
    /// a stable hash — which means every location has its own recognisable
    /// stand-in, the same one every time, and a scene change reads as a scene
    /// change. It is obviously not finished art, and that is the intention: it
    /// should be possible to play an act through and judge its pacing without
    /// ever mistaking a placeholder for a decision somebody made.
    /// </para>
    /// <para>
    /// The moment a real file with that name lands in
    /// <c>Assets/Images/Backgrounds</c>, the library finds it and none of this
    /// runs. Nothing has to be removed later.
    /// </para>
    /// </remarks>
    public static class ProceduralBackgrounds
    {
        /// <summary>Size of the generated texture.</summary>
        /// <remarks>
        /// Small on purpose. It is stretched over a 1920-wide image and the
        /// content is nothing but gradients, so the only thing extra resolution
        /// would buy is a slower first frame in the one act that is not finished.
        /// </remarks>
        private const int Width = 320;

        private const int Height = 180;

        private static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        /// <summary>Drops every generated picture.</summary>
        public static void Clear()
        {
            foreach (KeyValuePair<string, Sprite> pair in Cache)
            {
                if (pair.Value == null)
                {
                    continue;
                }

                if (pair.Value.texture != null)
                {
                    Object.Destroy(pair.Value.texture);
                }

                Object.Destroy(pair.Value);
            }

            Cache.Clear();
        }

        /// <summary>
        /// A stand-in for one named place, generated once and kept.
        /// </summary>
        public static Sprite For(string backgroundName)
        {
            if (string.IsNullOrWhiteSpace(backgroundName))
            {
                return null;
            }

            if (Cache.TryGetValue(backgroundName, out Sprite cached) && cached != null)
            {
                return cached;
            }

            Sprite made = Build(backgroundName);
            Cache[backgroundName] = made;

            return made;
        }

        /// <summary>
        /// A stand-in body for a character pose that has not been drawn yet.
        /// </summary>
        /// <param name="poseName">
        /// The sprite name the library was asked for, used both as the cache key
        /// and as the seed, so one expression always looks the same.
        /// </param>
        /// <param name="warm">
        /// True for Yua's side of the palette, false for Haru's. The two have
        /// been pink and blue since the first name plate and the placeholders
        /// should not make the player relearn that.
        /// </param>
        /// <remarks>
        /// A flat silhouette on the character canvas' own 1200 × 2400, so it
        /// crops, scales and stands exactly where the real art will. It shows
        /// nothing about a face and is not supposed to: what it makes testable
        /// is who is on stage, where, at what size, and for how long — which is
        /// every question act six needs answered before its art exists.
        /// </remarks>
        public static Sprite SilhouetteFor(string poseName, bool warm)
        {
            if (string.IsNullOrWhiteSpace(poseName))
            {
                return null;
            }

            string key = "silhouette:" + poseName;

            if (Cache.TryGetValue(key, out Sprite cached) && cached != null)
            {
                return cached;
            }

            Sprite made = BuildSilhouette(poseName, warm);
            Cache[key] = made;

            return made;
        }

        /// <summary>
        /// Head, shoulders and a body, drawn as coverage on a 1200 × 2400
        /// canvas.
        /// </summary>
        /// <remarks>
        /// Built at a sixteenth of the real canvas and left to filter up. It is
        /// a blurred shape by intention — a crisp placeholder invites opinions
        /// about a silhouette nobody is going to ship.
        /// </remarks>
        private static Sprite BuildSilhouette(string poseName, bool warm)
        {
            const int w = 75;
            const int h = 150;

            uint hash = StableHash(poseName);

            // A little hue drift per expression, so two poses on stage at once
            // are visibly two different sprites rather than one repeated.
            float hue = (warm ? 0.94f : 0.58f) + ((hash % 40) - 20) / 1000f;
            Color body = Color.HSVToRGB(Mathf.Repeat(hue, 1f), 0.34f, 0.62f);

            Texture2D texture = new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                name = "PlaceholderPose_" + poseName,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color[] pixels = new Color[w * h];

            // Proportions in canvas fractions measured from the top, matching the
            // real art closely enough that the crop at the shin lands in the
            // same place.
            const float headTop = 0.10f;
            const float headBottom = 0.235f;
            const float headHalfWidth = 0.115f;
            const float shoulderY = 0.30f;
            const float bodyHalfWidth = 0.30f;

            for (int y = 0; y < h; y++)
            {
                float fromTop = 1f - (y + 0.5f) / h;

                for (int x = 0; x < w; x++)
                {
                    float u = (x + 0.5f) / w - 0.5f;
                    float alpha = 0f;

                    if (fromTop >= headTop && fromTop <= headBottom)
                    {
                        float t = Mathf.InverseLerp(headTop, headBottom, fromTop);
                        float radius = headHalfWidth * Mathf.Sin(Mathf.PI * Mathf.Clamp01(t) * 0.92f + 0.28f);
                        alpha = Mathf.Abs(u) <= radius ? 1f : 0f;
                    }
                    else if (fromTop > headBottom)
                    {
                        float t = Mathf.InverseLerp(headBottom, 1f, fromTop);
                        float shoulder = Mathf.InverseLerp(headBottom, shoulderY, fromTop);
                        float half = Mathf.Lerp(headHalfWidth * 1.15f, bodyHalfWidth, Mathf.Clamp01(shoulder));
                        half *= Mathf.Lerp(1f, 0.86f, t);
                        alpha = Mathf.Abs(u) <= half ? 1f : 0f;
                    }

                    // Darker towards the outside, so the shape reads as a volume
                    // rather than as a cut-out rectangle.
                    float shade = 1f - 0.30f * Mathf.Clamp01(Mathf.Abs(u) / bodyHalfWidth);

                    pixels[y * w + x] = new Color(
                        body.r * shade, body.g * shade, body.b * shade, alpha * 0.92f);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);

            Sprite sprite = Sprite.Create(
                texture, new Rect(0f, 0f, w, h), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);

            sprite.name = texture.name;
            sprite.hideFlags = HideFlags.HideAndDontSave;

            return sprite;
        }

        /// <summary>
        /// Two colours and a horizon, chosen by the name.
        /// </summary>
        /// <remarks>
        /// Hue comes from the hash and the rest is fixed, so the whole set comes
        /// out muted and slightly desaturated rather than as a row of primary
        /// colours. A placeholder that is louder than the finished art distorts
        /// every judgement made while looking at it.
        /// </remarks>
        private static Sprite Build(string backgroundName)
        {
            uint hash = StableHash(backgroundName);

            float skyHue = (hash % 1000) / 1000f;
            float groundHue = Mathf.Repeat(skyHue + 0.08f + ((hash >> 10) % 120) / 1000f, 1f);

            Color sky = Color.HSVToRGB(skyHue, 0.22f, 0.72f);
            Color skyLow = Color.HSVToRGB(skyHue, 0.30f, 0.55f);
            Color ground = Color.HSVToRGB(groundHue, 0.26f, 0.34f);

            // Between a third and two thirds of the way down, so the same two
            // colours still produce a different picture.
            float horizon = 0.35f + ((hash >> 20) % 300) / 1000f;

            Texture2D texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false)
            {
                name = $"PlaceholderBackground_{backgroundName}",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color[] pixels = new Color[Width * Height];

            for (int y = 0; y < Height; y++)
            {
                // Texture y grows upward and the horizon is measured from the
                // top, which is the way anybody describes a horizon.
                float fromTop = 1f - (y + 0.5f) / Height;

                for (int x = 0; x < Width; x++)
                {
                    Color colour;

                    if (fromTop < horizon)
                    {
                        colour = Color.Lerp(sky, skyLow, fromTop / Mathf.Max(0.001f, horizon));
                    }
                    else
                    {
                        float depth = (fromTop - horizon) / Mathf.Max(0.001f, 1f - horizon);
                        colour = Color.Lerp(skyLow * 0.9f, ground, depth);
                    }

                    // A soft darkening towards the corners, so the placeholder
                    // sits under the characters the way a photograph does rather
                    // than competing with them.
                    float u = (x + 0.5f) / Width * 2f - 1f;
                    float v = (y + 0.5f) / Height * 2f - 1f;
                    float vignette = 1f - 0.35f * Mathf.Clamp01(u * u * 0.6f + v * v * 0.6f);

                    pixels[y * Width + x] = new Color(
                        colour.r * vignette,
                        colour.g * vignette,
                        colour.b * vignette,
                        1f);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, Width, Height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);

            sprite.name = texture.name;
            sprite.hideFlags = HideFlags.HideAndDontSave;

            return sprite;
        }

        /// <summary>
        /// A hash that is the same on every machine and every run.
        /// </summary>
        /// <remarks>
        /// <see cref="string.GetHashCode"/> is explicitly not guaranteed to be
        /// stable between processes, and a placeholder whose colour changes
        /// between two runs of the same scene is worse than no placeholder — it
        /// makes the picture unreliable evidence about the thing being judged.
        /// </remarks>
        private static uint StableHash(string text)
        {
            unchecked
            {
                uint hash = 2166136261u;

                for (int i = 0; i < text.Length; i++)
                {
                    hash ^= text[i];
                    hash *= 16777619u;
                }

                return hash;
            }
        }
    }
}
