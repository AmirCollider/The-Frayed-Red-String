// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ActSceneController.cs
// -----------------------------------------------------------------------------

using System.Collections;
using System.Text.RegularExpressions;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Flow;
using TheFrayedRedString.InputHandling;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.UI;
using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Scenes
{
    /// <summary>
    /// Turns any act scene into a playable act.
    /// </summary>
    /// <remarks>
    /// <para>
    /// One controller for every act, not one per act. Which act it is comes from
    /// the scene's own name — Act01 is act one, Act05 is act five — and the
    /// script comes from the matching <see cref="ActAsset"/>. So a new act needs
    /// a scene with a camera in it and an act written in the Story Editor, and
    /// no code at all.
    /// </para>
    /// <para>
    /// The scene is otherwise built from nothing: stage, frame, dialogue box,
    /// overlays, choices and pause menu are all created at load. What the scene
    /// does provide is adopted — its background image becomes the stage's first
    /// layer, and a leftover MenuCanvas donates its language button and save
    /// panel to the pause layer.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class ActSceneController : MonoBehaviour
    {
        private VisualNovelStage _stage;
        private StoryFrameView _frame;
        private DialogueBoxView _dialogue;
        private StoryOverlayView _overlay;
        private ChoicePanelView _choices;
        private PauseMenuView _pause;
        private LoadPanelController _loadPanel;
        private StoryDirector _director;

        private RectTransform _storyLayer;
        private RectTransform _safeArea;

        private ActAsset _act;
        private bool _finished;

        /// <summary>Which act this scene is, taken from its name.</summary>
        public int ActNumber { get; private set; } = 1;

        private void Start()
        {
            ActNumber = ParseActNumber(gameObject.scene.name);
            _act = ActLibrary.Find(ActNumber);

            // Behind the fade curtain, so the couple of milliseconds spent
            // synthesising the story sounds cost nothing visible.
            ProceduralSfxLibrary.WarmUpStory();

            RectTransform backgroundLayer = PrepareBackgroundCanvas(out Image authoredBackground);
            _storyLayer = CreateCanvas("StoryCanvas", GameConfig.StoryCanvasOrder);

            // Order of construction is order of drawing. The stage goes in
            // first so the characters are behind everything; the catcher next,
            // so a click that misses a control still turns the page; then the
            // framed interface; then the frame itself, over all of it.
            BuildStage(authoredBackground, backgroundLayer, _storyLayer);
            BuildAdvanceCatcher(_storyLayer);

            _safeArea = CreateSafeArea(_storyLayer);

            BuildDialogue(_safeArea);
            BuildFrame(_storyLayer, _safeArea);
            BuildPauseLayer();
            BuildDirector();

            LocalizationService.LanguageChanged += OnLanguageChanged;

            ScreenFader.FadeIn(GameConfig.SceneFadeInDuration);

            StorySession.EnterActScene(ActNumber);

            if (HasScript())
            {
                _director.Play(_act, StorySession.ConsumeResumeLine());
                return;
            }

            StartCoroutine(PlayUnwrittenAct());
        }

        private void OnDestroy()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;

            if (_director != null)
            {
                _director.Finished -= OnActFinished;
            }
        }

        private void Update()
        {
            // Time spent in the pause menu is not time spent playing. The
            // counter it feeds is printed on the save card, and a file that
            // gains an hour because somebody walked away with the menu open
            // says something untrue about the playthrough.
            if (_pause == null || !_pause.IsOpen)
            {
                StorySession.Tick(Time.unscaledDeltaTime);
            }

            if (!InputService.CancelPressedThisFrame)
            {
                return;
            }

            // Escape backs out one layer at a time: the save panel first, then
            // the pause menu. Collapsing both at once would make one keypress
            // undo two decisions.
            if (_loadPanel != null && _loadPanel.IsOpen)
            {
                _loadPanel.Close();
                return;
            }

            SetPaused(_pause == null || !_pause.IsOpen);
        }

        /// <summary>
        /// Reads the act number out of a scene name like <c>Act03</c>.
        /// </summary>
        /// <remarks>
        /// Falling back to act one rather than to zero: a scene that has been
        /// renamed still has a story in it, and playing the wrong act is a
        /// visible mistake somebody will notice, where playing nothing at all
        /// looks like the game is broken.
        /// </remarks>
        private static int ParseActNumber(string sceneName)
        {
            Match match = Regex.Match(sceneName ?? string.Empty, @"(\d+)");

            return match.Success && int.TryParse(match.Groups[1].Value, out int number) && number > 0
                ? number
                : 1;
        }

        // ---------------------------------------------------------------------
        //  Construction
        // ---------------------------------------------------------------------

        private RectTransform PrepareBackgroundCanvas(out Image authoredBackground)
        {
            Transform found = UnityUtility.FindInScene(ObjectNames.BackgroundCanvas);
            Canvas canvas = found != null ? found.GetComponent<Canvas>() : null;

            RectTransform layer = canvas != null
                ? (RectTransform)canvas.transform
                : CreateCanvas("StoryBackgroundCanvas", GameConfig.BackgroundCanvasOrder);

            if (canvas != null)
            {
                canvas.sortingOrder = GameConfig.BackgroundCanvasOrder;
            }

            authoredBackground = UnityUtility.FindDeep<Image>(layer, ObjectNames.StoryBackground);

            if (authoredBackground != null)
            {
                authoredBackground.raycastTarget = false;
            }

            return layer;
        }

        private void BuildStage(Image authoredBackground, RectTransform backgroundLayer, RectTransform storyLayer)
        {
            GameObject host = new GameObject("[TFRS] Stage");
            host.transform.SetParent(transform, false);

            _stage = host.AddComponent<VisualNovelStage>();
            _stage.Initialize(authoredBackground, backgroundLayer, storyLayer);
        }

        /// <summary>
        /// The rect everything the player reads is laid out in.
        /// </summary>
        /// <remarks>
        /// Its insets are owned by <see cref="StoryFrameView"/>, so the dialogue
        /// box, the captions and the choice buttons are all inside the frame by
        /// construction rather than by three separate sets of margins that have
        /// to be kept in step with it. Widening the frame widens this with it.
        /// </remarks>
        private static RectTransform CreateSafeArea(RectTransform storyLayer)
        {
            GameObject host = new GameObject("StorySafeArea", typeof(RectTransform));
            host.transform.SetParent(storyLayer, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            return rect;
        }

        private void BuildDialogue(RectTransform safeArea)
        {
            GameObject dialogueHost = new GameObject("[TFRS] Dialogue Box", typeof(RectTransform));
            _dialogue = dialogueHost.AddComponent<DialogueBoxView>();
            _dialogue.Initialize(safeArea);

            GameObject overlayHost = new GameObject("[TFRS] Story Overlay", typeof(RectTransform));
            _overlay = overlayHost.AddComponent<StoryOverlayView>();
            _overlay.Initialize(safeArea);

            GameObject choiceHost = new GameObject("[TFRS] Choices", typeof(RectTransform));
            _choices = choiceHost.AddComponent<ChoicePanelView>();
            _choices.Initialize(safeArea);
        }

        /// <summary>
        /// Builds the frame last, so its bars sit over the dialogue box as well
        /// as over the picture.
        /// </summary>
        /// <remarks>
        /// That is the point of a frame: everything the game shows is inside it,
        /// interface included. A frame the dialogue box escaped from would look
        /// like a rendering mistake rather than like a boundary.
        /// </remarks>
        private void BuildFrame(RectTransform storyLayer, RectTransform safeArea)
        {
            GameObject host = new GameObject("[TFRS] Frame", typeof(RectTransform));

            _frame = host.AddComponent<StoryFrameView>();
            _frame.Initialize(storyLayer, StageSettings.Load());
            _frame.BindContent(safeArea);
        }

        /// <summary>
        /// A transparent full-screen sheet that turns a click anywhere into
        /// "next line".
        /// </summary>
        /// <remarks>
        /// Preferred over asking the EventSystem whether the pointer is over UI.
        /// That question cannot be answered usefully here: the background is a
        /// full-screen graphic, so "over UI" would be true for every pixel. A
        /// dedicated catcher below the buttons and above everything else gets
        /// the right answer by construction — a press that reaches it is, by
        /// definition, a press that missed every control.
        /// </remarks>
        private void BuildAdvanceCatcher(RectTransform storyLayer)
        {
            GameObject host = new GameObject("AdvanceCatcher", typeof(RectTransform));
            host.transform.SetParent(storyLayer, false);

            RectTransform rect = (RectTransform)host.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image sheet = host.AddComponent<Image>();
            sheet.color = new Color(0f, 0f, 0f, 0f);
            sheet.raycastTarget = true;

            PointerClickRelay relay = host.AddComponent<PointerClickRelay>();
            relay.Clicked += () => _director?.NotifyAdvanceClicked();
        }

        /// <summary>
        /// Builds the pause layer on a canvas of its own.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An earlier version adopted the scene's leftover MenuCanvas as the
        /// pause layer, and the menu came out underneath the characters with its
        /// buttons stacked on top of each other. Reusing a canvas means
        /// inheriting its sorting order, its scaler, its render mode and its
        /// layout — four things that have to be right, are invisible when they
        /// are wrong, and are set in a scene file nobody is looking at.
        /// </para>
        /// <para>
        /// So the pause layer gets its own canvas, built here with known values,
        /// and the two pieces of the old menu that are worth keeping — the
        /// language button and the save panel — are moved into it. The empty
        /// shell is then switched off.
        /// </para>
        /// </remarks>
        private void BuildPauseLayer()
        {
            RectTransform layer = CreateCanvas("PauseCanvas", GameConfig.PauseCanvasOrder);

            GameObject pauseHost = new GameObject("[TFRS] Pause Menu", typeof(RectTransform));
            _pause = pauseHost.AddComponent<PauseMenuView>();
            _pause.Initialize(layer);

            _pause.Resumed += () => SetPaused(false);
            _pause.SaveRequested += () => _loadPanel?.Toggle(LoadPanelMode.Save);
            _pause.LoadRequested += () => _loadPanel?.Toggle(LoadPanelMode.Load);
            _pause.QuitRequested += GameFlowService.EnterMainMenu;

            AdoptOldMenuCanvas(layer);
        }

        private void AdoptOldMenuCanvas(RectTransform layer)
        {
            Transform menuCanvas = UnityUtility.FindInScene(ObjectNames.ActMenuCanvas);
            if (menuCanvas == null)
            {
                return;
            }

            // Moved rather than copied, and moved after the pause menu was built
            // so both end up drawn on top of its veil.
            Transform languageMask = UnityUtility.FindDeep(menuCanvas, ObjectNames.LanguageButtonMask);
            Transform panel = UnityUtility.FindDeep(menuCanvas, ObjectNames.LoadPanel);

            if (languageMask != null)
            {
                // Into the menu rather than onto the layer, so it is only there
                // while the game is paused.
                _pause.Adopt(languageMask);

                Button languageButton = UnityUtility.FindDeep<Button>(languageMask, ObjectNames.LanguageButton);
                if (languageButton != null)
                {
                    UnityUtility.GetOrAdd<LanguageToggleButton>(languageButton.gameObject);
                }
            }

            if (panel != null)
            {
                panel.SetParent(layer, false);
                panel.SetAsLastSibling();

                _loadPanel = UnityUtility.GetOrAdd<LoadPanelController>(panel.gameObject);
                _loadPanel.Initialize();
            }
            else
            {
                Debug.LogWarning(
                    $"[Act] No '{ObjectNames.LoadPanel}' was found, so this act cannot save or load. " +
                    "Copy the panel across from MainMenu.unity into the scene to enable it.");
            }

            menuCanvas.gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------------
        //  Acts that have not been written yet
        // ---------------------------------------------------------------------

        /// <summary>True when there is an act asset with beats in it to play.</summary>
        private bool HasScript()
        {
            return _act != null && _act.Count > 0;
        }

        /// <summary>
        /// What an act scene does when nobody has written its act yet.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Acts two to seven exist as scenes with a camera in them and nothing
        /// else, on purpose — they are where the story goes next. Reaching one
        /// used to warn to the console, report itself finished, and immediately
        /// advance to the act after it, so finishing act one flicked the player
        /// through six empty scenes in about a second and dropped them on the
        /// title with no idea what had happened.
        /// </para>
        /// <para>
        /// A closing card and a walk back to the title says the true thing
        /// instead: this is as far as the story goes today.
        /// </para>
        /// </remarks>
        private IEnumerator PlayUnwrittenAct()
        {
            Debug.Log(
                $"[Act] Act {ActNumber:00} has no script yet. Showing the closing card and returning to the title. " +
                "Write it in The Frayed Red String > Story Editor and this scene plays it with no further changes.");

            // Taken and thrown away. It is a one-shot instruction to a director
            // that is never going to run, and leaving it set would hand it to
            // whichever act the player opens next — dropping them into the
            // middle of a scene they asked to start.
            StorySession.ConsumeResumeLine();

            // Let the scene's own fade-in finish first, so the card arrives on a
            // visible screen rather than through the curtain.
            yield return StoryClock.Wait(GameConfig.SceneFadeInDuration);

            yield return _overlay.PlayClosingCard(
                LocalizationService.Get(LocKeys.StoryToBeContinued),
                GameConfig.TitleCardHoldDuration);

            GameFlowService.EnterMainMenu();
        }

        private void BuildDirector()
        {
            _director = UnityUtility.GetOrAdd<StoryDirector>(gameObject);
            _director.Initialize(_stage, _dialogue, _overlay, _choices, _frame);
            _director.Finished += OnActFinished;
        }

        private static RectTransform CreateCanvas(string canvasName, int sortingOrder)
        {
            GameObject host = new GameObject(canvasName, typeof(RectTransform));

            Canvas canvas = host.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = host.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            // Matched on height. The game is authored at 16:9 and every
            // background is exactly that; matching width instead would grow the
            // interface on an ultrawide monitor while the art stayed put.
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            host.AddComponent<GraphicRaycaster>();

            return (RectTransform)host.transform;
        }

        // ---------------------------------------------------------------------
        //  Flow
        // ---------------------------------------------------------------------

        private void SetPaused(bool paused)
        {
            if (_pause == null)
            {
                return;
            }

            if (paused)
            {
                _pause.Open();
            }
            else
            {
                _loadPanel?.Close(false);
                _pause.Close();
            }

            if (_director != null)
            {
                _director.IsPaused = paused;
            }
        }

        /// <summary>
        /// Moves on when the act runs out of script.
        /// </summary>
        /// <remarks>
        /// An act with no scene after it returns to the title rather than
        /// sitting on a dead screen. Progress is recorded first, so continuing
        /// from the menu picks up where the player stopped once the next act
        /// exists.
        /// </remarks>
        private void OnActFinished()
        {
            if (_finished)
            {
                return;
            }

            _finished = true;

            StorySession.EnterAct(ActNumber + 1);

            if (StorySession.ActiveSlot > 0)
            {
                StorySession.WriteTo(StorySession.ActiveSlot);
            }

            GameFlowService.AdvanceToAct(ActNumber + 1);
        }

        private void OnLanguageChanged(GameLanguage language)
        {
            _dialogue?.RefreshLanguage();
            _overlay?.RefreshLanguage();
            _choices?.RefreshLanguage();
            _director?.RefreshLanguage();
        }
    }
}
