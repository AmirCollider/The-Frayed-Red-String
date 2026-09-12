// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SeasonalFall.cs
//
//  Things falling through the air: cherry petals, maple and ginkgo leaves, snow.
//
//  Why this exists at all, and why it is not decoration:
//
//      The game runs from the 2nd of September 2024 to the 24th of March 2025.
//      It is autumn, then winter, then the very start of spring. IT IS NEVER
//      CHERRY BLOSSOM SEASON — and act one is called Cherry Blossom Mirage. The
//      blossom is a lie the player brings with them from the genre, and the
//      alley they walk down every morning is full of dead leaves.
//
//      So petals are not a weather effect here. They are a tell. Anywhere they
//      fall, somebody is looking at something that is not there: a memory, a
//      dream, or the first act.
//
//  Driven by beats rather than by the background, because a scene can change
//  weight without changing place — the wind picks up as a conversation turns,
//  and it drops to nothing when somebody starts talking to the player. The
//  director owns it; StoryBeatKind.Fall is how a script asks.
//
//  Everything is drawn in code, like the rest of this project's art that is not
//  a character or a room. A petal is four numbers and a colour; a PNG of one is
//  a file somebody has to redraw to recolour.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Core;
using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Presentation
{
    /// <summary>What is falling.</summary>
    /// <remarks>
    /// Pinned to numbers for the same reason <see cref="Narrative.StoryBeatKind"/>
    /// is: an act asset stores this as its integer, so a value inserted anywhere
    /// but the end would quietly re-point every beat already written.
    /// </remarks>
    public enum FallKind
    {
        /// <summary>Nothing. The default, and most of the game.</summary>
        None = 0,

        /// <summary>
        /// Cherry petals.
        /// </summary>
        /// <remarks>
        /// Only ever in a flashback, a dream, or the ending that is named for
        /// one. There is no September in which these are real.
        /// </remarks>
        Sakura = 1,

        /// <summary>Maple. Act one and act two's weather.</summary>
        MapleLeaf = 2,

        /// <summary>Ginkgo. The yellow half of the same alley.</summary>
        GinkgoLeaf = 3,

        /// <summary>Snow, for the deep of act three and four.</summary>
        Snow = 4
    }

    /// <summary>
    /// A drifting layer of petals or leaves over the background.
    /// </summary>
    /// <remarks>
    /// Lives on the background canvas, above the picture and behind the
    /// characters. In front of them would need a second layer and a reason, and
    /// no scene in the document has asked for one.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class SeasonalFall : MonoBehaviour
    {
        /// <summary>
        /// The most particles that will ever exist.
        /// </summary>
        /// <remarks>
        /// Density one is a windy day, not a blizzard. A layer dense enough to
        /// read as weather takes the scene away from the two people in it, which
        /// is the same mistake a busy narrator makes.
        /// </remarks>
        private const int MaxParticles = 40;

        /// <summary>
        /// The layer's size, when it has not been laid out yet.
        /// </summary>
        /// <remarks>
        /// The real size is read from the rect every frame instead. The frame
        /// insets the background layer, and it opens during act five — a layer
        /// that assumed 1920 by 1080 would spawn leaves outside the picture
        /// before that and inside it afterwards.
        /// </remarks>
        private const float FallbackWidth = 1920f;
        private const float FallbackHeight = 1080f;

        /// <summary>How far above the top edge a particle is respawned.</summary>
        private const float SpawnMargin = 120f;

        private const float MinFallSpeed = 26f;
        private const float MaxFallSpeed = 62f;

        private const float MinSwayAmplitude = 14f;
        private const float MaxSwayAmplitude = 52f;

        private const float MinSwayFrequency = 0.18f;
        private const float MaxSwayFrequency = 0.55f;

        /// <summary>Seconds a density or kind change takes, when none is given.</summary>
        private const float DefaultChangeSeconds = 2.5f;

        private RectTransform _root;
        private CanvasGroup _group;

        private readonly List<Particle> _particles = new List<Particle>(MaxParticles);

        /// <summary>
        /// What the particles are currently drawn as.
        /// </summary>
        /// <remarks>
        /// Not the same thing as what the script last asked for, and the
        /// difference matters while a layer is going away. Clearing takes a
        /// couple of seconds; if the kind were dropped to None the moment the
        /// beat played, every leaf still in the air would lose its sprite on
        /// that frame and the fade would read as a glitch. So the drawn kind
        /// only ever changes to a real one, and the fade is done with density.
        /// </remarks>
        private FallKind _drawn = FallKind.None;

        /// <summary>What the script last asked for. Reported by <see cref="Kind"/>.</summary>
        private FallKind _requested = FallKind.None;

        private float _density;

        /// <summary>Where the density is heading, and how fast.</summary>
        private float _targetDensity;
        private float _densityRate;

        /// <summary>Set while somebody is talking to the player, or a film is running.</summary>
        private bool _suspended;

        /// <summary>Half the layer's current size, refreshed each frame.</summary>
        private float _halfWidth = FallbackWidth * 0.5f;
        private float _halfHeight = FallbackHeight * 0.5f;

        /// <summary>What the script last asked to fall.</summary>
        public FallKind Kind => _requested;

        /// <summary>How much of it, from nothing to a windy day.</summary>
        public float Density => _density;

        /// <summary>Builds the layer. Nothing falls until a beat asks.</summary>
        public void Initialize(RectTransform parent)
        {
            _root = (RectTransform)transform;
            _root.SetParent(parent, false);

            _root.anchorMin = Vector2.zero;
            _root.anchorMax = Vector2.one;
            _root.pivot = new Vector2(0.5f, 0.5f);
            _root.offsetMin = Vector2.zero;
            _root.offsetMax = Vector2.zero;

            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);
            _group.alpha = 1f;
            _group.blocksRaycasts = false;
            _group.interactable = false;

            // Behind the characters, in front of the picture. The stage puts the
            // background image in first, so being last in the layer is enough.
            _root.SetAsLastSibling();
        }

        /// <summary>
        /// Changes what is falling and how much of it.
        /// </summary>
        /// <param name="kind">What falls. <see cref="FallKind.None"/> clears it.</param>
        /// <param name="density">
        /// 0 is nothing and 1 is a windy day. Act one and two sit around 0.25.
        /// </param>
        /// <param name="seconds">
        /// How long the change takes. Zero is a cut, and a cut is almost always
        /// wrong — weather that appears between two frames reads as a bug. It is
        /// allowed because the act six montage cuts everything.
        /// </param>
        public void Set(FallKind kind, float density, float seconds = DefaultChangeSeconds)
        {
            density = Mathf.Clamp01(density);

            _requested = kind;

            // Only ever restyled towards something real. See _drawn.
            if (kind != FallKind.None && kind != _drawn)
            {
                _drawn = kind;
                Recolour();
            }

            _targetDensity = kind == FallKind.None ? 0f : density;

            if (seconds <= 0.001f)
            {
                _density = _targetDensity;
                _densityRate = 0f;
                ApplyDensity();
                return;
            }

            _densityRate = Mathf.Abs(_targetDensity - _density) / seconds;
        }

        /// <summary>Takes it down to nothing without forgetting what it was.</summary>
        public void Clear(float seconds = DefaultChangeSeconds)
        {
            Set(FallKind.None, 0f, seconds);
        }

        /// <summary>
        /// Holds the layer out of the way, and gives it back.
        /// </summary>
        /// <remarks>
        /// For the two places the picture has to stop moving: somebody speaking
        /// to the player, and a film. Separate from density so that whatever the
        /// scene had asked for is still there when the aside ends.
        /// </remarks>
        public void Suspend(bool suspended)
        {
            _suspended = suspended;

            if (_group != null)
            {
                _group.alpha = suspended ? 0f : 1f;
            }
        }

        /// <summary>
        /// Drops every particle and forgets the weather. For a scene change.
        /// </summary>
        /// <remarks>
        /// Not called Reset: that is a MonoBehaviour message Unity invokes in the
        /// editor whenever the component is added or reset from the Inspector,
        /// and a public method with that name would quietly become an editor
        /// callback as well as an API.
        /// </remarks>
        public void ResetLayer()
        {
            _drawn = FallKind.None;
            _requested = FallKind.None;
            _density = 0f;
            _targetDensity = 0f;
            _densityRate = 0f;
            _suspended = false;

            if (_group != null)
            {
                _group.alpha = 1f;
            }

            ApplyDensity();
        }

        private void Update()
        {
            float delta = StoryClock.DeltaTime;

            if (delta <= 0f)
            {
                return;
            }

            if (!Mathf.Approximately(_density, _targetDensity))
            {
                _density = Mathf.MoveTowards(_density, _targetDensity, _densityRate * delta);
                ApplyDensity();
            }

            // Deliberately not gated on the requested kind: particles still on
            // screen during a clear have to keep drifting, or they hang in the
            // air and dim.
            if (_suspended)
            {
                return;
            }

            MeasureLayer();

            for (int i = 0; i < _particles.Count; i++)
            {
                Particle particle = _particles[i];

                if (!particle.Active)
                {
                    continue;
                }

                particle.Advance(delta, _halfWidth, _halfHeight);
                _particles[i] = particle;
            }
        }

        /// <summary>
        /// Brings particles into existence or retires them to match the density.
        /// </summary>
        /// <remarks>
        /// Retired rather than destroyed. A scene that breathes in and out over
        /// an act would otherwise allocate a hundred GameObjects and drop them
        /// on the collector one at a time, which is the sort of thing that shows
        /// up as a stutter in the middle of a quiet line.
        /// </remarks>
        private void ApplyDensity()
        {
            MeasureLayer();

            int wanted = Mathf.RoundToInt(Mathf.Clamp01(_density) * MaxParticles);

            while (_particles.Count < wanted)
            {
                _particles.Add(CreateParticle(_particles.Count));
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                Particle particle = _particles[i];
                bool active = i < wanted;

                if (particle.Active != active)
                {
                    particle.SetActive(active);

                    if (active)
                    {
                        particle.Respawn(true, _halfWidth, _halfHeight);
                    }

                    _particles[i] = particle;
                }
            }
        }

        /// <summary>Reads the layer's current size, so the frame opening is followed.</summary>
        private void MeasureLayer()
        {
            if (_root == null)
            {
                return;
            }

            Rect rect = _root.rect;

            if (rect.width > 1f && rect.height > 1f)
            {
                _halfWidth = rect.width * 0.5f;
                _halfHeight = rect.height * 0.5f;
            }
        }

        private void Recolour()
        {
            Sprite sprite = FallSprites.For(_drawn);

            for (int i = 0; i < _particles.Count; i++)
            {
                Particle particle = _particles[i];
                particle.Restyle(_drawn, sprite);
                _particles[i] = particle;
            }
        }

        private Particle CreateParticle(int index)
        {
            GameObject host = new GameObject("Fall" + index.ToString("00"), typeof(RectTransform));
            host.transform.SetParent(_root, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            Image image = host.AddComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = true;

            Particle particle = new Particle(rect, image);
            particle.Restyle(_drawn, FallSprites.For(_drawn));
            particle.SetActive(false);
            return particle;
        }

        // ---------------------------------------------------------------------
        //  One falling thing
        // ---------------------------------------------------------------------

        /// <summary>
        /// A single petal or leaf, and everything about how it moves.
        /// </summary>
        /// <remarks>
        /// A struct held in a list and written back, rather than a component per
        /// particle. Forty MonoBehaviours each with their own Update is forty
        /// managed-to-native calls a frame for something that is arithmetic.
        /// </remarks>
        private struct Particle
        {
            private readonly RectTransform _rect;
            private readonly Image _image;

            private float _x;
            private float _y;
            private float _fallSpeed;
            private float _swayAmplitude;
            private float _swayFrequency;
            private float _swayPhase;
            private float _spin;
            private float _angle;
            private float _age;

            /// <summary>Whether this one is in the air. A plain field: see the constructor.</summary>
            public bool Active;

            public Particle(RectTransform rect, Image image)
            {
                _rect = rect;
                _image = image;

                _x = 0f;
                _y = 0f;
                _fallSpeed = 0f;
                _swayAmplitude = 0f;
                _swayFrequency = 0f;
                _swayPhase = 0f;
                _spin = 0f;
                _angle = 0f;
                _age = 0f;

                Active = false;
            }

            public void SetActive(bool active)
            {
                Active = active;

                if (_rect != null)
                {
                    _rect.gameObject.SetActive(active);
                }
            }

            /// <summary>Gives it the look of a kind, and a fresh size.</summary>
            public void Restyle(FallKind kind, Sprite sprite)
            {
                if (_image == null || _rect == null)
                {
                    return;
                }

                _image.sprite = sprite;
                _image.color = FallSprites.TintFor(kind);

                float size = FallSprites.SizeFor(kind) * Random.Range(0.72f, 1.28f);
                _rect.sizeDelta = new Vector2(size, size);
            }

            /// <summary>
            /// Puts it back at the top with a new set of numbers.
            /// </summary>
            /// <param name="anywhere">
            /// True on the first spawn, so a scene does not open with an empty
            /// sky and one neat wave arriving together a few seconds later.
            /// </param>
            public void Respawn(bool anywhere, float halfWidth, float halfHeight)
            {
                // A little wider than the layer: a leaf that sways in from just
                // outside the edge looks like wind, one that appears exactly on
                // the edge looks like a spawner.
                _x = Random.Range(-halfWidth * 1.1f, halfWidth * 1.1f);

                _y = anywhere
                    ? Random.Range(-halfHeight, halfHeight + SpawnMargin)
                    : halfHeight + Random.Range(0f, SpawnMargin);

                _fallSpeed = Random.Range(MinFallSpeed, MaxFallSpeed);
                _swayAmplitude = Random.Range(MinSwayAmplitude, MaxSwayAmplitude);
                _swayFrequency = Random.Range(MinSwayFrequency, MaxSwayFrequency);
                _swayPhase = Random.Range(0f, Mathf.PI * 2f);
                _spin = Random.Range(-42f, 42f);
                _angle = Random.Range(0f, 360f);
                _age = Random.Range(0f, 6f);

                Apply();
            }

            public void Advance(float delta, float halfWidth, float halfHeight)
            {
                if (_rect == null)
                {
                    return;
                }

                // A first Advance before any Respawn would drop a particle from
                // the origin in a straight line. Nothing calls it that way today,
                // but the cost of the guard is one comparison.
                if (_fallSpeed <= 0f)
                {
                    Respawn(true, halfWidth, halfHeight);
                    return;
                }

                _age += delta;
                _y -= _fallSpeed * delta;
                _angle += _spin * delta;

                if (_y < -(halfHeight + SpawnMargin))
                {
                    Respawn(false, halfWidth, halfHeight);
                    return;
                }

                Apply();
            }

            private void Apply()
            {
                float sway = Mathf.Sin(_age * _swayFrequency * Mathf.PI * 2f + _swayPhase) * _swayAmplitude;

                _rect.anchoredPosition = new Vector2(_x + sway, _y);
                _rect.localRotation = Quaternion.Euler(0f, 0f, _angle);
            }
        }
    }

    /// <summary>
    /// The shapes and colours the fall layer is drawn from.
    /// </summary>
    /// <remarks>
    /// Kept beside <see cref="SeasonalFall"/> rather than in
    /// <see cref="ProceduralUiSprites"/>, which is furniture for the interface.
    /// These are weather.
    /// </remarks>
    internal static class FallSprites
    {
        private const int Resolution = 64;
        private const float PixelsPerUnit = 1f;

        private static readonly Dictionary<FallKind, Sprite> Cache = new Dictionary<FallKind, Sprite>();

        /// <summary>Drops cached sprites; used when statics are reset between play sessions.</summary>
        public static void Clear()
        {
            Cache.Clear();
        }

        /// <summary>Roughly how wide one of these is, in canvas units.</summary>
        public static float SizeFor(FallKind kind)
        {
            switch (kind)
            {
                case FallKind.Sakura: return 22f;
                case FallKind.MapleLeaf: return 34f;
                case FallKind.GinkgoLeaf: return 30f;
                case FallKind.Snow: return 12f;
                default: return 20f;
            }
        }

        /// <summary>
        /// The colour one is tinted, with a little variation per particle.
        /// </summary>
        /// <remarks>
        /// Varied here rather than baked into the sprite, so one texture serves
        /// a whole tree's worth of slightly different leaves.
        /// </remarks>
        public static Color TintFor(FallKind kind)
        {
            switch (kind)
            {
                case FallKind.Sakura:
                    return new Color(
                        1f,
                        Random.Range(0.78f, 0.90f),
                        Random.Range(0.86f, 0.94f),
                        Random.Range(0.55f, 0.85f));

                case FallKind.MapleLeaf:
                    return new Color(
                        Random.Range(0.72f, 0.92f),
                        Random.Range(0.22f, 0.42f),
                        Random.Range(0.10f, 0.20f),
                        Random.Range(0.70f, 0.95f));

                case FallKind.GinkgoLeaf:
                    return new Color(
                        Random.Range(0.92f, 0.99f),
                        Random.Range(0.74f, 0.87f),
                        Random.Range(0.20f, 0.34f),
                        Random.Range(0.70f, 0.95f));

                case FallKind.Snow:
                    return new Color(1f, 1f, 1f, Random.Range(0.45f, 0.80f));

                default:
                    return Color.clear;
            }
        }

        /// <summary>The silhouette for a kind, generated once and kept.</summary>
        public static Sprite For(FallKind kind)
        {
            if (kind == FallKind.None)
            {
                return null;
            }

            if (Cache.TryGetValue(kind, out Sprite cached) && cached != null)
            {
                return cached;
            }

            Texture2D texture = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false)
            {
                name = "Fall_" + kind,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color[] pixels = new Color[Resolution * Resolution];

            for (int y = 0; y < Resolution; y++)
            {
                for (int x = 0; x < Resolution; x++)
                {
                    // Centred on the texture, in the range -1 to 1.
                    float u = (x + 0.5f) / Resolution * 2f - 1f;
                    float v = (y + 0.5f) / Resolution * 2f - 1f;

                    pixels[y * Resolution + x] = new Color(1f, 1f, 1f, Coverage(kind, u, v));
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, Resolution, Resolution),
                new Vector2(0.5f, 0.5f),
                PixelsPerUnit);

            sprite.name = texture.name;
            sprite.hideFlags = HideFlags.HideAndDontSave;

            Cache[kind] = sprite;
            return sprite;
        }

        /// <summary>
        /// How much of the pixel at (u, v) the shape covers, from 0 to 1.
        /// </summary>
        /// <remarks>
        /// Every shape is described as a distance from the centre that varies
        /// with angle, which is the shortest way to write a petal, a fan and a
        /// five-lobed leaf without three different algorithms. The soft edge is
        /// a smoothstep across the boundary rather than a hard test, so a thirty
        /// pixel leaf does not look like it was cut out with scissors.
        /// </remarks>
        private static float Coverage(FallKind kind, float u, float v)
        {
            float radius = Mathf.Sqrt(u * u + v * v);

            if (radius > 1f)
            {
                return 0f;
            }

            float angle = Mathf.Atan2(v, u);
            float edge;

            switch (kind)
            {
                case FallKind.Sakura:
                    // Five rounded lobes with a notch in each, which is what
                    // makes a cherry petal read as a cherry petal and not a
                    // flower. Squashed slightly along one axis.
                    edge = 0.62f + 0.24f * Mathf.Cos(5f * angle);
                    edge -= 0.08f * Mathf.Abs(Mathf.Sin(2.5f * angle));
                    edge *= 1f - 0.18f * Mathf.Abs(Mathf.Sin(angle));
                    break;

                case FallKind.MapleLeaf:
                    // Five points, deep gaps between them, and a stem at the
                    // bottom that the cosine term leaves room for.
                    edge = 0.52f + 0.40f * Mathf.Pow(Mathf.Abs(Mathf.Cos(2.5f * (angle - Mathf.PI * 0.5f))), 0.65f);
                    edge *= 0.92f;
                    break;

                case FallKind.GinkgoLeaf:
                {
                    // A fan: wide at the top, gathered to a point at the bottom.
                    float fan = Mathf.Clamp01((v + 0.9f) / 1.6f);
                    edge = 0.28f + 0.62f * fan;
                    edge *= 1f - 0.22f * Mathf.Abs(Mathf.Sin(angle * 0.5f));
                    break;
                }

                default:
                    edge = 0.78f;
                    break;
            }

            edge = Mathf.Max(0.05f, edge);

            // Roughly one texel of softness at this resolution.
            const float Softness = 0.06f;
            return Mathf.SmoothStep(1f, 0f, (radius - (edge - Softness)) / Softness);
        }
    }
}
