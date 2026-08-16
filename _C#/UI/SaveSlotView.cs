// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SaveSlotView.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Flow;
using TheFrayedRedString.Localization;
using TheFrayedRedString.SaveSystem;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// Presents one save slot: either an "Unlived Season" placeholder or the
    /// act, title and play time of an existing file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Slot labels are written directly rather than through
    /// <see cref="LocalizedText"/> because two of them are format strings fed by
    /// live save data; a single owner for all four keeps the refresh atomic.
    /// </para>
    /// <para>
    /// Which labels are visible is controlled by enabling and disabling the
    /// <see cref="TMP_Text"/> components, not their GameObjects. The scene
    /// authors the detail labels inactive, so they are activated once during
    /// <see cref="Initialize"/> and then never toggled again — repeatedly
    /// disabling them would re-run the DirectTMP font binder on every refresh.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class SaveSlotView : MonoBehaviour, ILocalizedView
    {
        [SerializeField] private int _slotNumber;

        private Button _button;
        private TMP_Text _emptyLabel;
        private TMP_Text _seasonLabel;
        private TMP_Text _seasonNameLabel;
        private TMP_Text _playTimeLabel;
        private Image _screenshot;

        private bool _resolved;
        private SaveSlotData _data;

        /// <summary>Which slot this card shows, 1-based.</summary>
        public int SlotNumber => _slotNumber;

        /// <summary>Binds the view to a slot number and resolves its child labels.</summary>
        public void Initialize(int slotNumber)
        {
            _slotNumber = slotNumber;
            _resolved = false;

            ResolveLabels();
            Refresh();
        }

        /// <summary>Re-reads the slot from storage and updates every label.</summary>
        public void Refresh()
        {
            // Resolve lazily so a refresh triggered from outside — a language
            // change sweeping the whole scene, say — works even if it happens to
            // land before the load panel has initialised this card.
            ResolveLabels();

            _data = SaveService.Read(_slotNumber);

            SetVisible(_emptyLabel, !_data.Exists);
            SetVisible(_seasonLabel, _data.Exists);
            SetVisible(_seasonNameLabel, _data.Exists);
            SetVisible(_playTimeLabel, _data.Exists);

            if (_screenshot != null)
            {
                // Dim the thumbnail frame on an empty slot so the card reads as
                // unwritten rather than broken.
                Color tint = _screenshot.color;
                tint.a = _data.Exists ? 1f : 0.35f;
                _screenshot.color = tint;
            }

            if (!_data.Exists)
            {
                if (_emptyLabel != null)
                {
                    _emptyLabel.text = LocalizationService.Get(LocKeys.LoadSlotEmpty);
                }

                return;
            }

            if (_seasonLabel != null)
            {
                _seasonLabel.text = LocalizationService.Format(LocKeys.LoadSlotSeason, _data.ActNumber);
            }

            if (_seasonNameLabel != null)
            {
                _seasonNameLabel.text = _data.GetActTitle();
            }

            if (_playTimeLabel != null)
            {
                _playTimeLabel.text = LocalizationService.Format(
                    LocKeys.LoadSlotPlayTime,
                    _data.GetFormattedPlayTime());
            }
        }

        private void Awake()
        {
            ResolveLabels();
        }

        private void OnEnable()
        {
            LocalizationService.LanguageChanged += OnLanguageChanged;
            SaveService.SlotsChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            SaveService.SlotsChanged -= Refresh;
        }

        /// <summary>
        /// Finds the child labels, and activates the ones the scene authored
        /// inactive so their visibility can be driven by component state from
        /// here on.
        /// </summary>
        private void ResolveLabels()
        {
            if (_resolved)
            {
                return;
            }

            _resolved = true;

            if (_slotNumber <= 0)
            {
                _slotNumber = ParseSlotNumberFromName(name);
            }

            _button = GetComponent<Button>();
            _emptyLabel = UnityUtility.FindDeep<TMP_Text>(transform, ObjectNames.SlotEmptyLabel);
            _seasonLabel = UnityUtility.FindDeep<TMP_Text>(transform, ObjectNames.SlotSeasonLabel);
            _seasonNameLabel = UnityUtility.FindDeep<TMP_Text>(transform, ObjectNames.SlotSeasonNameLabel);
            _playTimeLabel = UnityUtility.FindDeep<TMP_Text>(transform, ObjectNames.SlotPlayTimeLabel);
            _screenshot = UnityUtility.FindDeep<Image>(transform, ObjectNames.SlotScreenshot);

            ActivateOnce(_emptyLabel);
            ActivateOnce(_seasonLabel);
            ActivateOnce(_seasonNameLabel);
            ActivateOnce(_playTimeLabel);

            if (_button != null)
            {
                _button.onClick.RemoveListener(OnClicked);
                _button.onClick.AddListener(OnClicked);
            }
        }

        /// <summary>
        /// Reads the trailing digits of a name like "SaveSlot02".
        /// </summary>
        /// <remarks>
        /// A fallback for the case where the view was added without anyone
        /// calling <see cref="Initialize"/>. Defaults to slot 1 rather than 0,
        /// because slot 0 does not exist and would make every card read empty.
        /// </remarks>
        private static int ParseSlotNumberFromName(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
            {
                return 1;
            }

            int end = objectName.Length;
            int start = end;

            while (start > 0 && char.IsDigit(objectName[start - 1]))
            {
                start--;
            }

            if (start == end)
            {
                return 1;
            }

            return int.TryParse(objectName.Substring(start, end - start), out int parsed) && parsed > 0
                ? parsed
                : 1;
        }

        private void OnClicked()
        {
            if (!_data.Exists)
            {
                // SelectableSfx already played the press sound; this is the
                // second, unmistakable "there is nothing here" beat.
                AudioService.Play(SfxId.Denied, 0.8f);
                return;
            }

            GameFlowService.LoadSlot(_slotNumber);
        }

        private void OnLanguageChanged(GameLanguage language)
        {
            Refresh();
        }

        private static void ActivateOnce(Component target)
        {
            if (target != null && !target.gameObject.activeSelf)
            {
                target.gameObject.SetActive(true);
            }
        }

        private static void SetVisible(Behaviour target, bool visible)
        {
            if (target != null && target.enabled != visible)
            {
                target.enabled = visible;
            }
        }
    }
}
