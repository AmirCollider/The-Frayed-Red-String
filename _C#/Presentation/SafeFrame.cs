// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SafeFrame.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Holds the whole game inside a centred 16:9 area, whatever shape the
    /// screen it is running on turns out to be.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every piece of art in this project is 16:9 — the fourteen backgrounds are
    /// 1920×1080 exactly — and every position in it is authored in 1920×1080
    /// canvas units: both character slots, the dialogue box, the choice buttons,
    /// the four bars of <see cref="StoryFrameView"/>. A desktop window is 16:9
    /// too, so nothing ever had to say what happens when it is not.
    /// </para>
    /// <para>
    /// A phone is not. A common handset is 19.5:9 or 20:9 in landscape, which is
    /// about a fifth wider than the game is drawn. Left alone the background
    /// image stretches to fill it — <c>preserveAspect</c> is off, because
    /// stretching a 16:9 picture across a 16:9 screen does nothing — so every
    /// background and every face on a phone comes out a fifth too wide, and the
    /// two character slots drift apart because the canvas they are positioned in
    /// got wider while their offsets did not.
    /// </para>
    /// <para>
    /// The fix is the one this particular game has no choice about: letterbox.
    /// Not because it is the tidiest of the options — cropping the picture to
    /// fill the screen would be — but because the shape of the frame is a story
    /// beat here. The design document asks for a picture that is boxed in from
    /// act one and for a character to widen it to a full 16:9 in act five. A
    /// port that quietly gives the player a wider picture the whole way through
    /// has spent the ending before the story reaches it. So the picture stays
    /// 16:9 on every device, and the screen the device has left over is painted
    /// out.
    /// </para>
    /// <para>
    /// Two things are needed for that, and this class is both. Each canvas gets
    /// a <see cref="ContentName"/> child holding everything that was in it,
    /// anchored to the 16:9 slice of the screen — so laying out at 1920×1080
    /// stays correct by construction rather than by every view being taught
    /// about margins. And the leftover screen gets black bars, on a canvas above
    /// every other, so nothing world-space can spill into it.
    /// </para>
    /// <para>
    /// On a 16:9 screen the whole thing is arithmetically a no-op: the slice is
    /// the screen, the bars are zero pixels wide. Which is the point — the
    /// desktop build this was added to keeps rendering exactly what it rendered
    /// before.
    /// </para>
    /// </remarks>
    public static class SafeFrame
    {
        /// <summary>The shape everything in the game is drawn at.</summary>
        public const float Aspect = 16f / 9f;

        /// <summary>Name of the child every canvas's contents are moved into.</summary>
        public const string ContentName = "[TFRS] Safe Frame";

        /// <summary>
        /// Turn off to let the game fill the screen the old way. Leaves the
        /// stretch in, and is here so a build can be compared against one.
        /// </summary>
        public static bool Enabled = true;

        private static readonly List<Canvas> Framed = new List<Canvas>();

        private static RectTransform _barLeft;
        private static RectTransform _barRight;
        private static RectTransform _barTop;
        private static RectTransform _barBottom;

        private static int _lastWidth;
        private static int _lastHeight;

        /// <summary>Clears static state between play sessions.</summary>
        public static void ResetStatics()
        {
            Framed.Clear();

            _barLeft = null;
            _barRight = null;
            _barTop = null;
            _barBottom = null;

            _lastWidth = 0;
            _lastHeight = 0;
        }

        /// <summary>
        /// The slice of the screen the game is drawn in, as fractions from 0 to
        /// 1. The whole screen when it is already 16:9.
        /// </summary>
        public static Rect Viewport
        {
            get
            {
                if (!Enabled)
                {
                    return new Rect(0f, 0f, 1f, 1f);
                }

                float height = Mathf.Max(1, Screen.height);
                float screenAspect = Mathf.Max(1, Screen.width) / height;

                if (Mathf.Approximately(screenAspect, Aspect))
                {
                    return new Rect(0f, 0f, 1f, 1f);
                }

                if (screenAspect > Aspect)
                {
                    // Wider than the game: bars down the left and right.
                    float width = Aspect / screenAspect;
                    return new Rect((1f - width) * 0.5f, 0f, width, 1f);
                }

                // Taller than the game — a portrait phone, or a window dragged
                // into a square: bars along the top and bottom.
                float tall = screenAspect / Aspect;
                return new Rect(0f, (1f - tall) * 0.5f, 1f, tall);
            }
        }

        /// <summary>
        /// Returns the rect inside <paramref name="canvas"/> that content
        /// belongs in, creating it and moving the canvas's existing children
        /// into it the first time.
        /// </summary>
        /// <remarks>
        /// Safe to call twice on the same canvas: the second call finds the
        /// child it made the first time and re-applies the anchors, which is
        /// exactly what a rotated phone needs.
        /// </remarks>
        public static RectTransform ContentOf(Canvas canvas)
        {
            if (canvas == null)
            {
                return null;
            }

            RectTransform canvasRect = (RectTransform)canvas.transform;

            if (!Enabled)
            {
                return canvasRect;
            }

            ApplyScaler(canvas);

            Transform existing = canvasRect.Find(ContentName);
            RectTransform content;

            if (existing != null)
            {
                content = (RectTransform)existing;
            }
            else
            {
                GameObject host = new GameObject(ContentName, typeof(RectTransform));
                content = (RectTransform)host.transform;
                content.SetParent(canvasRect, false);

                // Backwards, and the direction is the whole of it: reparenting
                // shifts every later sibling down one index, so walking forwards
                // skips every other child. Backwards, the indices below the one
                // being moved do not change.
                for (int i = canvasRect.childCount - 1; i >= 0; i--)
                {
                    Transform child = canvasRect.GetChild(i);
                    if (child == content)
                    {
                        continue;
                    }

                    child.SetParent(content, false);
                    child.SetAsFirstSibling();
                }
            }

            content.pivot = new Vector2(0.5f, 0.5f);
            content.localScale = Vector3.one;
            content.localRotation = Quaternion.identity;

            Rect viewport = Viewport;
            content.anchorMin = new Vector2(viewport.xMin, viewport.yMin);
            content.anchorMax = new Vector2(viewport.xMax, viewport.yMax);
            content.offsetMin = Vector2.zero;
            content.offsetMax = Vector2.zero;

            if (!Framed.Contains(canvas))
            {
                Framed.Add(canvas);
            }

            return content;
        }

        /// <summary>
        /// Frames every canvas a scene was authored with.
        /// </summary>
        /// <remarks>
        /// For the canvases that come out of the .unity files — MainMenu's
        /// BackgroundCanvas, the acts' BackgroundCanvas and leftover MenuCanvas.
        /// Canvases the scene controllers build for themselves are framed as
        /// they are built instead.
        /// </remarks>
        public static void InstallInto(Scene scene)
        {
            if (!Enabled || !scene.IsValid() || !scene.isLoaded)
            {
                return;
            }

            Prune();

            List<GameObject> roots = new List<GameObject>(scene.rootCount);
            scene.GetRootGameObjects(roots);

            for (int i = 0; i < roots.Count; i++)
            {
                Canvas[] canvases = roots[i].GetComponentsInChildren<Canvas>(true);

                for (int j = 0; j < canvases.Length; j++)
                {
                    // Nested canvases inherit their parent's rect, so framing
                    // the root is enough and framing the child would apply the
                    // inset twice.
                    if (canvases[j].isRootCanvas)
                    {
                        ContentOf(canvases[j]);
                    }
                }
            }

            EnsureBars();
            ApplyBars();
        }

        /// <summary>
        /// Re-applies the frame to every canvas it is holding. Cheap enough to
        /// call on a resize and nothing else.
        /// </summary>
        public static void Refresh()
        {
            if (!Enabled)
            {
                return;
            }

            Prune();

            for (int i = 0; i < Framed.Count; i++)
            {
                ContentOf(Framed[i]);
            }

            ApplyBars();
        }

        /// <summary>
        /// Notices a screen that has changed shape and re-frames the game.
        /// Called every frame from the bars' own component.
        /// </summary>
        public static void TickResolution()
        {
            if (Screen.width == _lastWidth && Screen.height == _lastHeight)
            {
                return;
            }

            _lastWidth = Screen.width;
            _lastHeight = Screen.height;

            Refresh();
        }

        /// <summary>
        /// Builds the black bars, once, on the object that survives scene loads.
        /// </summary>
        public static void EnsureBars()
        {
            if (!Enabled || _barLeft != null)
            {
                return;
            }

            GameObject host = new GameObject("[TFRS] Letterbox", typeof(RectTransform));

            GameObject owner = GameBootstrap.ServiceHost;
            if (owner != null)
            {
                host.transform.SetParent(owner.transform, false);
            }
            else
            {
                Object.DontDestroyOnLoad(host);
            }

            Canvas canvas = host.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = GameConfig.LetterboxCanvasOrder;

            // No CanvasScaler and no GraphicRaycaster, both deliberately. The
            // bars are placed by anchors alone so they need no reference
            // resolution, and a raycaster here would let the bars swallow
            // presses that land near the edge of the picture.

            host.AddComponent<SafeFrameWatcher>();

            RectTransform root = (RectTransform)host.transform;

            _barLeft = CreateBar("LetterboxLeft", root);
            _barRight = CreateBar("LetterboxRight", root);
            _barTop = CreateBar("LetterboxTop", root);
            _barBottom = CreateBar("LetterboxBottom", root);

            ApplyBars();
        }

        /// <summary>
        /// Sets the canvas scaler so the framed rect is exactly 1920×1080 canvas
        /// units.
        /// </summary>
        /// <remarks>
        /// The match has to follow the screen. Matching height keeps 1080 units
        /// tall, which is right whenever the bars are down the sides; on a
        /// screen taller than 16:9 the picture is limited by width instead and
        /// matching height would scale the interface off the top and bottom of
        /// it. The old fixed match of 1 was correct for every case that existed
        /// before this — a 16:9 window, and anything wider.
        /// </remarks>
        private static void ApplyScaler(Canvas canvas)
        {
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                return;
            }

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            float screenAspect = Mathf.Max(1, Screen.width) / (float)Mathf.Max(1, Screen.height);
            scaler.matchWidthOrHeight = screenAspect >= Aspect ? 1f : 0f;
        }

        private static void ApplyBars()
        {
            if (_barLeft == null)
            {
                return;
            }

            Rect viewport = Viewport;

            SetBar(_barLeft, Vector2.zero, new Vector2(viewport.xMin, 1f));
            SetBar(_barRight, new Vector2(viewport.xMax, 0f), Vector2.one);
            SetBar(_barTop, new Vector2(viewport.xMin, viewport.yMax), new Vector2(viewport.xMax, 1f));
            SetBar(_barBottom, new Vector2(viewport.xMin, 0f), new Vector2(viewport.xMax, viewport.yMin));
        }

        private static void SetBar(RectTransform bar, Vector2 min, Vector2 max)
        {
            if (bar == null)
            {
                return;
            }

            bar.anchorMin = min;
            bar.anchorMax = max;
            bar.offsetMin = Vector2.zero;
            bar.offsetMax = Vector2.zero;

            // A bar of no width still draws a hairline on some scale factors,
            // which on a 16:9 screen is a black line down the edge of a game
            // that is supposed to be filling it.
            bar.gameObject.SetActive(max.x - min.x > 0.0005f && max.y - min.y > 0.0005f);
        }

        private static RectTransform CreateBar(string barName, RectTransform parent)
        {
            GameObject host = new GameObject(barName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.Solid(Color.white);
            image.color = Color.black;
            image.raycastTarget = false;

            return (RectTransform)host.transform;
        }

        private static void Prune()
        {
            for (int i = Framed.Count - 1; i >= 0; i--)
            {
                if (Framed[i] == null)
                {
                    Framed.RemoveAt(i);
                }
            }
        }
    }
}
