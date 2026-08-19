// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ProceduralUiSprites.cs
//
//  The story interface draws its own furniture: the dialogue panel, the name
//  plate, the caption pill, the scrim behind the characters, the little marker
//  that says a line has finished.
//
//  Doing that in code rather than importing PNGs is the same decision the audio
//  layer made, for the same reasons. These shapes are rounded rectangles with a
//  border — describing one takes a line, and a description can be recoloured,
//  resized and reused, where a bitmap has to be redrawn and reimported. It also
//  means the whole story UI works in a fresh clone with no art beyond the
//  backgrounds and characters that were already there.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Generates and caches the flat shapes the story interface is built from.
    /// </summary>
    public static class ProceduralUiSprites
    {
        private const float PixelsPerUnit = 100f;

        private static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        /// <summary>Drops cached sprites; used when statics are reset between play sessions.</summary>
        public static void Clear()
        {
            Cache.Clear();
        }

        /// <summary>
        /// A rounded rectangle set up for nine-slicing, so one small texture can
        /// fill a panel of any size without the corners stretching.
        /// </summary>
        /// <param name="radius">Corner radius in pixels.</param>
        /// <param name="fill">Interior colour, alpha included.</param>
        /// <param name="border">Outline colour. Pass a transparent colour for no outline.</param>
        /// <param name="borderWidth">Outline thickness in pixels.</param>
        public static Sprite RoundedRect(int radius, Color fill, Color border = default, float borderWidth = 0f)
        {
            radius = Mathf.Clamp(radius, 1, 128);
            borderWidth = Mathf.Max(0f, borderWidth);

            string key = $"rr:{radius}:{Key(fill)}:{Key(border)}:{borderWidth:0.##}";
            if (Cache.TryGetValue(key, out Sprite cached) && cached != null)
            {
                return cached;
            }

            // Two radii of corner plus a single-pixel middle row and column. The
            // middle pixel is what the nine-slice stretches, so one pixel is
            // exactly enough.
            int size = radius * 2 + 1;
            Texture2D texture = NewTexture(size, size, $"UI_RoundedRect_{radius}");

            Color[] pixels = new Color[size * size];
            Vector2 half = new Vector2(size * 0.5f, size * 0.5f);
            float cornerRadius = radius;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Sample at pixel centres, or the shape sits half a pixel off
                    // and the two sides of the sprite come out different widths.
                    Vector2 point = new Vector2(x + 0.5f, y + 0.5f) - half;
                    float distance = RoundedBoxDistance(point, half, cornerRadius);

                    pixels[y * size + x] = Blend(distance, fill, border, borderWidth);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);

            float slice = radius;
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                PixelsPerUnit,
                0,
                SpriteMeshType.FullRect,
                new Vector4(slice, slice, slice, slice));

            sprite.name = key;
            Cache[key] = sprite;
            return sprite;
        }

        /// <summary>
        /// A vertical gradient, one pixel wide, for scrims behind text.
        /// </summary>
        /// <remarks>
        /// Stretched horizontally by the Image component. A one-pixel column is
        /// all the information a vertical gradient contains, and keeping it that
        /// narrow means the texture costs nothing.
        /// </remarks>
        public static Sprite VerticalGradient(Color top, Color bottom, int height = 64)
        {
            height = Mathf.Clamp(height, 2, 512);

            string key = $"vg:{Key(top)}:{Key(bottom)}:{height}";
            if (Cache.TryGetValue(key, out Sprite cached) && cached != null)
            {
                return cached;
            }

            Texture2D texture = NewTexture(1, height, "UI_VerticalGradient");
            Color[] pixels = new Color[height];

            for (int y = 0; y < height; y++)
            {
                // Smoothstep rather than a straight lerp: a linear alpha ramp has
                // a visible hard edge where it meets full transparency, because
                // the eye picks up the discontinuity in the slope.
                float t = y / (float)(height - 1);
                pixels[y] = Color.Lerp(bottom, top, t * t * (3f - 2f * t));
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, 1f, height),
                new Vector2(0.5f, 0.5f),
                PixelsPerUnit,
                0,
                SpriteMeshType.FullRect,
                new Vector4(0f, 1f, 0f, 1f));

            sprite.name = key;
            Cache[key] = sprite;
            return sprite;
        }

        /// <summary>
        /// A small downward triangle with softened corners: the marker that a
        /// line has finished typing and the game is waiting.
        /// </summary>
        public static Sprite DownArrow(int size, Color color)
        {
            size = Mathf.Clamp(size, 4, 128);

            string key = $"da:{size}:{Key(color)}";
            if (Cache.TryGetValue(key, out Sprite cached) && cached != null)
            {
                return cached;
            }

            Texture2D texture = NewTexture(size, size, "UI_DownArrow");
            Color[] pixels = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (x + 0.5f) / size;
                    float v = (y + 0.5f) / size;

                    // Inside the triangle when the horizontal distance from the
                    // centre line is under half the remaining height.
                    float halfWidth = v * 0.5f;
                    float distance = Mathf.Abs(u - 0.5f) - halfWidth;

                    float alpha = Mathf.Clamp01(0.5f - distance * size);
                    Color pixel = color;
                    pixel.a *= alpha;

                    pixels[y * size + x] = pixel;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                PixelsPerUnit);

            sprite.name = key;
            Cache[key] = sprite;
            return sprite;
        }

        /// <summary>A flat, fully opaque single-pixel sprite.</summary>
        public static Sprite Solid(Color color)
        {
            string key = "sl:" + Key(color);
            if (Cache.TryGetValue(key, out Sprite cached) && cached != null)
            {
                return cached;
            }

            Texture2D texture = NewTexture(1, 1, "UI_Solid");
            texture.SetPixel(0, 0, color);
            texture.Apply(false, false);

            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), PixelsPerUnit);
            sprite.name = key;
            Cache[key] = sprite;
            return sprite;
        }

        /// <summary>
        /// Signed distance from a point to a rounded box: negative inside,
        /// positive outside, and equal to the true distance in pixels either
        /// way. Having a real distance rather than a boolean is what lets the
        /// edge be antialiased and the border be drawn as a band.
        /// </summary>
        private static float RoundedBoxDistance(Vector2 point, Vector2 half, float radius)
        {
            Vector2 q = new Vector2(
                Mathf.Abs(point.x) - (half.x - radius),
                Mathf.Abs(point.y) - (half.y - radius));

            float outside = new Vector2(Mathf.Max(q.x, 0f), Mathf.Max(q.y, 0f)).magnitude;
            float inside = Mathf.Min(Mathf.Max(q.x, q.y), 0f);

            return outside + inside - radius;
        }

        private static Color Blend(float distance, Color fill, Color border, float borderWidth)
        {
            // Coverage across one pixel of the outer edge.
            float coverage = Mathf.Clamp01(0.5f - distance);

            Color colour = fill;

            if (borderWidth > 0f && border.a > 0f)
            {
                // How far inside the band this pixel sits, again with a pixel of
                // softness so the inner edge of the outline is not stair-stepped.
                float bandDepth = Mathf.Clamp01((distance + borderWidth) + 0.5f);
                colour = Color.Lerp(fill, border, bandDepth);
            }

            colour.a *= coverage;
            return colour;
        }

        private static Texture2D NewTexture(int width, int height, string name)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                name = name,
                filterMode = FilterMode.Bilinear,

                // Clamp matters for the nine-slice: with Repeat, bilinear
                // sampling at the sprite's edge pulls in the opposite side and
                // leaves a faint seam down the border of every panel.
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            return texture;
        }

        private static string Key(Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}
