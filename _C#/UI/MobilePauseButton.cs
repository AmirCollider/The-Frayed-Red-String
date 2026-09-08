// -----------------------------------------------------------------------------
//  The Frayed Red String
//  MobilePauseButton.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// The way into the pause menu on a phone: a small white disc in the top
    /// left, with a pink halo behind it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Pausing was Escape and nothing else, which on a handset with no keyboard
    /// means the menu — Save, Load, language, the way back to the title — is
    /// unreachable for the whole game. So the phone build gets a button, and
    /// only the phone build: a desktop player has the key, and a control
    /// floating over every line of a story that is trying to look like an
    /// innocent dating sim costs something to put there.
    /// </para>
    /// <para>
    /// Top left, mirroring the language flag's top right inside the pause menu.
    /// That is deliberate and it is the whole layout: the two meta controls this
    /// game has sit at the two top corners, one in the menu and one in the
    /// story, and a player who has found either knows where to look for the
    /// other.
    /// </para>
    /// <para>
    /// Built the way the menu buttons are — in code, out of
    /// <see cref="ProceduralUiSprites"/>, with nothing in any scene file. The
    /// pause menu, the choice buttons and the save cards are all already drawn
    /// this way, so a control that needed a PNG imported and placed in seven act
    /// scenes would be the odd one out and the one that goes missing from act
    /// five.
    /// </para>
    /// <para>
    /// White disc, pink halo, and the halo breathes: it is the same soft pastel
    /// the pause menu's own card is edged in, and the same idle motion every
    /// other element in this game carries. It is meant to read as belonging to
    /// the sweet version of the game — which, for the acts where the player can
    /// still believe that, it does.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class MobilePauseButton : MonoBehaviour
    {
        /// <summary>Diameter of the white disc, in canvas units.</summary>
        private const float Diameter = 84f;

        /// <summary>How far past the disc the halo reaches.</summary>
        private const float HaloSpread = 34f;

        /// <summary>
        /// Inset from the top and left edges.
        /// </summary>
        /// <remarks>
        /// The same 56 the language flag is inset from the top and right, so the
        /// two really are at mirrored positions rather than at two positions
        /// that look similar.
        /// </remarks>
        private const float Margin = 56f;

        private const float BarWidth = 11f;
        private const float BarHeight = 34f;
        private const float BarGap = 11f;

        private static readonly Color Disc = new Color(1f, 0.988f, 0.992f, 0.96f);
        private static readonly Color DiscEdge = new Color(0.98f, 0.71f, 0.80f, 1f);
        private static readonly Color Halo = new Color(1f, 0.62f, 0.76f, 0.55f);
        private static readonly Color Bars = new Color(0.36f, 0.22f, 0.30f, 1f);

        private Button _button;
        private CanvasGroup _group;

        /// <summary>Raised when the player asks for the menu.</summary>
        public event Action Pressed;

        /// <summary>
        /// Builds the button into a layer.
        /// </summary>
        /// <param name="layer">
        /// The pause canvas. It draws above the story and above the advance
        /// catcher, which is what stops a press on the button from also turning
        /// the page underneath it.
        /// </param>
        public void Initialize(RectTransform layer)
        {
            RectTransform rect = (RectTransform)transform;
            rect.SetParent(layer, false);

            // Top left, pivoted top left, so the inset means the same thing on
            // every screen: this is the corner the language flag is measured
            // from, reflected.
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.sizeDelta = new Vector2(Diameter, Diameter);
            rect.anchoredPosition = new Vector2(Margin, -Margin);

            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);

            BuildHalo(rect);
            BuildDisc(rect);

            // On the root, so the disc, the halo and the two bars pulse as one
            // control. Given to the halo alone it would swell and shrink behind
            // a disc that stayed put, which reads as two objects rather than as
            // one glowing one.
            //
            // Added after the rect is placed: AmbientMotion takes the transform
            // as its rest pose the moment it is enabled, and a component added
            // before the corner offset was written would treat the middle of the
            // screen as home and slide the button there.
            AmbientMotion motion = AmbientMotion.GetOrAdd(gameObject);
            motion.Configure(AmbientMotionProfile.Button, UnityUtility.StableHash01("MobilePauseButton"));

            // Under the pause menu in the hierarchy order of this canvas, so the
            // menu's veil covers the button rather than the button floating over
            // its own menu.
            rect.SetAsFirstSibling();
        }

        /// <summary>
        /// Shows or hides the button without destroying it.
        /// </summary>
        /// <remarks>
        /// Hidden while the menu is open. Resume is a button inside the menu,
        /// and a second control in the corner that also closes it is two answers
        /// to one question. Interactable is cleared with the alpha so a press on
        /// the hidden button cannot land through the veil.
        /// </remarks>
        public void SetVisible(bool visible)
        {
            if (_group == null)
            {
                return;
            }

            _group.alpha = visible ? 1f : 0f;
            _group.interactable = visible;
            _group.blocksRaycasts = visible;
        }

        private void BuildHalo(RectTransform parent)
        {
            GameObject host = new GameObject("PauseButtonHalo", typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            // Negative offsets grow the halo past the disc on all four sides.
            rect.offsetMin = new Vector2(-HaloSpread, -HaloSpread);
            rect.offsetMax = new Vector2(HaloSpread, HaloSpread);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.RadialGlow(96, Halo);
            image.color = Color.white;

            // The halo has no hard edge, so a press near its outside would be a
            // press on nothing visible. The disc takes the input.
            image.raycastTarget = false;
        }

        private void BuildDisc(RectTransform parent)
        {
            GameObject host = new GameObject("PauseButtonDisc", typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.Circle(64, Disc, DiscEdge, 3f);
            image.color = Color.white;
            image.raycastTarget = true;

            _button = host.AddComponent<Button>();
            _button.targetGraphic = image;
            _button.onClick.AddListener(() => Pressed?.Invoke());

            UnityUtility.GetOrAdd<SelectableSfx>(host);

            BuildBar(rect, "PauseBarLeft", -(BarGap + BarWidth) * 0.5f);
            BuildBar(rect, "PauseBarRight", (BarGap + BarWidth) * 0.5f);
        }

        private static void BuildBar(RectTransform parent, string barName, float offsetX)
        {
            GameObject host = new GameObject(barName, typeof(RectTransform));
            host.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(BarWidth, BarHeight);
            rect.anchoredPosition = new Vector2(offsetX, 0f);

            Image image = host.AddComponent<Image>();
            image.sprite = ProceduralUiSprites.RoundedRect(5, Bars);
            image.type = Image.Type.Sliced;
            image.color = Color.white;

            // The disc is the button. A bar that took raycasts would leave two
            // dead stripes across the middle of it.
            image.raycastTarget = false;
        }
    }
}
