// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LoadPanelController.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.InputHandling;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.SaveSystem;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>What pressing a slot in the panel does.</summary>
    public enum LoadPanelMode
    {
        /// <summary>Pressing a slot resumes the playthrough stored in it.</summary>
        Load,

        /// <summary>Pressing a slot writes the current playthrough into it.</summary>
        Save
    }

    /// <summary>
    /// Opens and closes the load panel, and keeps its three save slots in sync.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The panel is shown and hidden through its <see cref="CanvasGroup"/>, and
    /// its GameObject is left active for the whole scene. Toggling the
    /// GameObject would be the obvious approach and is the wrong one here: it
    /// re-runs OnEnable on every label underneath, which re-triggers the
    /// DirectTMP font binder and its fallback chain each time the panel opens,
    /// and it silently unsubscribes the save slots from language changes
    /// whenever the panel is shut. Alpha costs nothing and has neither problem.
    /// </para>
    /// <para>
    /// The panel ships with no close affordance of its own, so this controller
    /// supplies three ways out — Escape, a click on the backdrop, and the
    /// optional MenuButtonBackToMenu if one is ever added. A modal the player
    /// cannot dismiss is a dead end.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class LoadPanelController : MonoBehaviour
    {
        private CanvasGroup _group;
        private AmbientMotion _motion;
        private GameObject _backdrop;
        private SaveSlotView[] _slots;
        private TweenHandle _animation;

        /// <summary>
        /// The game-title art out in the main menu. It shares its screen
        /// position with the panel's own copy, so it is faded out while the
        /// panel is open and restored when the panel closes.
        /// </summary>
        private Graphic _menuTitleArt;

        /// <summary>
        /// The menu title's authored opacity, captured so restoring it returns
        /// the artist's value rather than assuming fully opaque.
        /// </summary>
        private float _menuTitleBaseAlpha = 1f;

        /// <summary>True while the panel is open or opening.</summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Whether slots are being read or written.
        /// </summary>
        /// <remarks>
        /// The same panel serves both. Building a second one would mean two
        /// copies of the slot cards to keep in step, and the two jobs differ
        /// only in what a press does and what the heading says.
        /// </remarks>
        public LoadPanelMode Mode { get; private set; } = LoadPanelMode.Load;

        /// <summary>Wires the panel up. Called once by the menu controller.</summary>
        /// <param name="menuTitleArt">
        /// The main menu's title image, hidden while the panel is open. May be
        /// null; the panel works without it.
        /// </param>
        public void Initialize(Graphic menuTitleArt = null)
        {
            _menuTitleArt = menuTitleArt;

            if (_menuTitleArt != null)
            {
                _menuTitleBaseAlpha = _menuTitleArt.color.a;
            }

            // The panel may be authored either active or inactive depending on
            // how the scene was last saved. Force it active once, then never
            // touch activation again.
            gameObject.SetActive(true);

            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);

            // A real profile, not the silent channel GetOrAdd hands out. The
            // panel is a bare transform with no graphic on it, so the sweep that
            // animates the scene skips it — and then only the elements inside it
            // moved, each on its own phase, which makes a panel look like it is
            // coming apart rather than breathing.
            //
            // Moving the panel moves everything in it together. Its backdrops
            // are held still to match; see AmbientMotionInstaller.
            _motion = AmbientMotion.GetOrAdd(gameObject);
            _motion.Configure(AmbientMotionProfile.Card, UnityUtility.StableHash01(name));

            BindTitle();
            BindSlots();
            BindCloseButton();
            CreateBackdrop();

            IsOpen = false;
            ApplyClosedState();
        }

        /// <summary>Opens the panel in load mode.</summary>
        public void Open()
        {
            Open(LoadPanelMode.Load);
        }

        /// <summary>Opens the panel with a fade and a small spring.</summary>
        /// <remarks>
        /// A panel that is already open still takes the new mode. It used to
        /// return before reading it, which meant a Load press landing on a panel
        /// left open in save mode showed the player a load screen that wrote to
        /// whatever they touched.
        /// </remarks>
        public void Open(LoadPanelMode mode)
        {
            if (IsOpen)
            {
                if (Mode != mode)
                {
                    SetMode(mode);
                    RefreshSlots();
                }

                return;
            }

            SetMode(mode);
            IsOpen = true;
            _animation.CancelIfRunning();

            SetBackdropActive(true);
            RefreshSlots();

            _group.blocksRaycasts = true;
            _group.interactable = true;

            float fromAlpha = _group.alpha;
            float fromScale = GameConfig.PanelOpenFromScale;

            FadeMenuTitle(0f, GameConfig.PanelOpenDuration);

            _animation = TweenRunner.Play(
                GameConfig.PanelOpenDuration,
                t =>
                {
                    _group.alpha = Mathf.Clamp01(Mathf.LerpUnclamped(fromAlpha, 1f, t));

                    float scale = Mathf.LerpUnclamped(fromScale, 1f, t);
                    _motion.ExtraScale = new Vector3(scale, scale, 1f);
                },

                // Not OutBack. The overshoot that gives a small element its
                // snap scales the whole panel past 1, and everything in it moves
                // outward from the centre in proportion to how far out it
                // already is — so the piece furthest from the middle is the one
                // that leaves the screen.
                EaseType.OutCubic,
                0f,
                () =>
                {
                    _group.alpha = 1f;
                    _motion.ExtraScale = Vector3.one;
                },
                this);
        }

        /// <summary>Closes the panel and fades it out.</summary>
        public void Close(bool playSound = true)
        {
            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;
            _animation.CancelIfRunning();

            if (playSound)
            {
                AudioService.Play(SfxId.Cancel);
            }

            // Stop accepting clicks immediately: the panel is on its way out and
            // must not register a slot press during the fade.
            _group.blocksRaycasts = false;
            _group.interactable = false;

            float fromAlpha = _group.alpha;
            float toScale = GameConfig.PanelOpenFromScale;

            FadeMenuTitle(_menuTitleBaseAlpha, GameConfig.PanelCloseDuration);

            _animation = TweenRunner.Play(
                GameConfig.PanelCloseDuration,
                t =>
                {
                    _group.alpha = Mathf.LerpUnclamped(fromAlpha, 0f, t);

                    float scale = Mathf.LerpUnclamped(1f, toScale, t);
                    _motion.ExtraScale = new Vector3(scale, scale, 1f);
                },
                EaseType.InQuad,
                0f,
                () =>
                {
                    _group.alpha = 0f;
                    _motion.ExtraScale = Vector3.one;
                    SetBackdropActive(false);
                },
                this);
        }

        /// <summary>Opens the panel if it is closed, closes it if it is open.</summary>
        public void Toggle()
        {
            Toggle(LoadPanelMode.Load);
        }

        /// <summary>Opens the panel in a mode, or closes it if it is already showing that mode.</summary>
        /// <remarks>
        /// Pressing Save while the panel is already open on Load switches it
        /// rather than shutting it, which is what the player means by the press.
        /// </remarks>
        public void Toggle(LoadPanelMode mode)
        {
            if (IsOpen && Mode == mode)
            {
                Close();
                return;
            }

            if (IsOpen)
            {
                SetMode(mode);

                // Re-read them. Going from Load to Save without this leaves the
                // cards showing what was on disk when the panel opened, which is
                // exactly one save out of date the moment anybody uses it.
                RefreshSlots();
                return;
            }

            Open(mode);
        }

        /// <summary>Re-titles the panel and re-points the slots.</summary>
        private void SetMode(LoadPanelMode mode)
        {
            Mode = mode;

            BindTitle();

            if (_slots == null)
            {
                return;
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null)
                {
                    _slots[i].Mode = mode;
                }
            }
        }

        /// <summary>Re-reads every slot from storage.</summary>
        public void RefreshSlots()
        {
            if (_slots == null)
            {
                return;
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null)
                {
                    _slots[i].Refresh();
                }
            }
        }

        private void Update()
        {
            if (IsOpen && InputService.CancelPressedThisFrame)
            {
                Close();
            }
        }

        private void ApplyClosedState()
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
            _motion.ExtraScale = Vector3.one;

            SetBackdropActive(false);
            FadeMenuTitle(_menuTitleBaseAlpha, 0f);
        }

        /// <summary>
        /// Cross-fades the outer menu title against the panel. Both images sit
        /// in the same place, so showing them together would read as a smear.
        /// </summary>
        private void FadeMenuTitle(float targetAlpha, float duration)
        {
            if (_menuTitleArt == null)
            {
                return;
            }

            if (duration <= 0f)
            {
                _menuTitleArt.SetAlpha(targetAlpha);
                return;
            }

            _menuTitleArt.FadeTo(targetAlpha, duration, EaseType.InOutSine);
        }

        private void BindTitle()
        {
            TMP_Text title = UnityUtility.FindDeep<TMP_Text>(transform, ObjectNames.LoadPanelTitle);
            if (title == null)
            {
                return;
            }

            string key = Mode == LoadPanelMode.Save ? LocKeys.SavePanelTitle : LocKeys.LoadPanelTitle;
            UnityUtility.GetOrAdd<LocalizedText>(title.gameObject).Bind(key);
        }

        private void BindSlots()
        {
            _slots = new SaveSlotView[SaveService.SlotCount];

            for (int slot = 1; slot <= SaveService.SlotCount; slot++)
            {
                string slotName = ObjectNames.SaveSlotPrefix + slot.ToString("00");
                Transform slotTransform = UnityUtility.FindDeep(transform, slotName);

                if (slotTransform == null)
                {
                    Debug.LogWarning($"[LoadPanel] Expected a child named '{slotName}' but found none.");
                    continue;
                }

                SaveSlotView view = UnityUtility.GetOrAdd<SaveSlotView>(slotTransform.gameObject);
                view.Initialize(slot);
                view.Mode = Mode;

                view.Written -= OnSlotWritten;
                view.Written += OnSlotWritten;

                _slots[slot - 1] = view;
            }
        }

        /// <summary>
        /// Shuts the panel the moment a slot has been written.
        /// </summary>
        /// <remarks>
        /// A save is a completed decision, and the panel has nothing left to ask
        /// once it is made. Left open, it goes on accepting presses in save mode
        /// over three cards the player is now reading rather than choosing
        /// between — which is how one intended save became a second, unintended
        /// one on top of a file they wanted to keep.
        /// </remarks>
        private void OnSlotWritten(int slotNumber)
        {
            // Without the sound: the slot has just played its own confirmation,
            // and a cancel chime immediately after it says the opposite of what
            // happened.
            Close(false);
        }

        private void BindCloseButton()
        {
            Button backButton = UnityUtility.FindDeep<Button>(transform, ObjectNames.BackToMenuButton);
            if (backButton != null)
            {
                backButton.onClick.AddListener(() => Close());
            }
        }

        /// <summary>
        /// Creates the invisible full-screen sheet that closes the panel when
        /// clicked. It is a sibling placed immediately below the panel, so it
        /// covers the menu without covering the panel itself.
        /// </summary>
        private void CreateBackdrop()
        {
            Transform parent = transform.parent;
            if (parent == null)
            {
                return;
            }

            GameObject backdrop = new GameObject("LoadPanelBackdrop", typeof(RectTransform));
            backdrop.transform.SetParent(parent, false);
            backdrop.transform.SetSiblingIndex(transform.GetSiblingIndex());

            RectTransform rect = (RectTransform)backdrop.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image sheet = backdrop.AddComponent<Image>();

            // Fully transparent but still a raycast target: uGUI hit-tests the
            // rect, not the pixels, so a zero-alpha Image blocks and reports
            // clicks exactly like a visible one.
            sheet.color = new Color(0f, 0f, 0f, 0f);
            sheet.raycastTarget = true;

            PointerClickRelay relay = backdrop.AddComponent<PointerClickRelay>();
            relay.Clicked += () => Close();

            _backdrop = backdrop;
            _backdrop.SetActive(false);
        }

        private void SetBackdropActive(bool active)
        {
            if (_backdrop != null)
            {
                _backdrop.SetActive(active);
            }
        }
    }
}
