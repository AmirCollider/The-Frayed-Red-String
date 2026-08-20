// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryEditorWindow.cs  (Editor only)
//
//  The window the game is written in.
//
//  Everything an act is made of is reachable from here: the lines and their
//  three translations, who says them and with what face, where the scene is,
//  what it sounds like, how long it holds, and where the characters stand while
//  it happens. Nothing in it requires C#, and nothing in it requires the game to
//  be running.
//
//  It is built on SerializedObject rather than on direct field assignment, which
//  is what buys undo, dirty tracking and the reorderable list for free — and,
//  more importantly, means a value typed here is saved by the same machinery
//  that saves the Inspector, rather than by something hand-rolled that forgets
//  on the one path nobody tested.
//
//  Two things were wrong with it and both were about not being able to see what
//  you are doing. Persian was unreadable — IMGUI has no shaper, so every line
//  came out unjoined, backwards, and in a font with no Arabic in it; that is
//  PersianEditorText's job now. And the window told you what a field was called
//  but never what it was for, so the only way to find out what a beat did was to
//  set one and press Play. Every beat kind now says what it does, in the window,
//  next to the fields that change it, and the Guide tab has the whole workflow
//  in one page.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>The story editor: acts, beats, and the stage they play on.</summary>
    public sealed class StoryEditorWindow : EditorWindow
    {
        private enum Tab
        {
            Script,
            Stage,
            Check,
            Guide
        }

        /// <summary>One problem found in an act, and where it is.</summary>
        private readonly struct Finding
        {
            public readonly MessageType Severity;
            public readonly string Message;
            public readonly int BeatIndex;

            public Finding(MessageType severity, string message, int beatIndex = -1)
            {
                Severity = severity;
                Message = message;
                BeatIndex = beatIndex;
            }
        }

        private const float ListWidth = 380f;
        private const string NoneLabel = "— none —";

        // Colours for the beat list, so a script can be read at a glance by
        // shape rather than by reading every row.
        private static readonly Color LineTint = new Color(1f, 1f, 1f, 0f);
        private static readonly Color StageTint = new Color(0.45f, 0.70f, 1f, 0.16f);
        private static readonly Color SoundTint = new Color(1f, 0.80f, 0.35f, 0.16f);
        private static readonly Color FlowTint = new Color(0.75f, 0.45f, 1f, 0.16f);

        /// <summary>The beat kinds offered as one-press buttons, in writing order.</summary>
        /// <summary>
        /// The kinds worth a button, in the order a scene is usually written.
        /// </summary>
        /// <remarks>
        /// Every kind is reachable from the dropdown on the right; these are the
        /// ones frequent enough that reaching for the dropdown is friction. The
        /// second row is the presentation set — the beats acts five, six and
        /// seven are built out of, which were previously only findable by
        /// somebody who already knew they existed.
        /// </remarks>
        private static readonly StoryBeatKind[] QuickAdd =
        {
            StoryBeatKind.Line,
            StoryBeatKind.Background,
            StoryBeatKind.Enter,
            StoryBeatKind.Exit,
            StoryBeatKind.Beat,
            StoryBeatKind.Choice,
            StoryBeatKind.ClearStage,
            StoryBeatKind.Sound,
            StoryBeatKind.Music,
            StoryBeatKind.Interlude
        };

        /// <summary>The presentation beats, on a row of their own.</summary>
        private static readonly StoryBeatKind[] QuickAddPresentation =
        {
            StoryBeatKind.OpenFrame,
            StoryBeatKind.CloseFrame,
            StoryBeatKind.PullDownDialogue,
            StoryBeatKind.Grade,
            StoryBeatKind.Stain,
            StoryBeatKind.CutMusic,
            StoryBeatKind.EnterCinema,
            StoryBeatKind.ExitCinema,
            StoryBeatKind.Video,
            StoryBeatKind.EndGame
        };

        private ActAsset _act;
        private SerializedObject _serialized;
        private ReorderableList _list;

        private Tab _tab = Tab.Script;
        private GameLanguage _language = GameLanguage.English;
        private Vector2 _listScroll;
        private Vector2 _detailScroll;
        private Vector2 _stageScroll;
        private Vector2 _checkScroll;
        private Vector2 _guideScroll;

        private StageSpriteLibrary _sprites;
        private StageSettings _settings;
        private SerializedObject _settingsSerialized;

        private string[] _backgroundNames;
        private string[] _musicNames;

        [MenuItem("The Frayed Red String/Story Editor")]
        public static void Open()
        {
            StoryEditorWindow window = GetWindow<StoryEditorWindow>("Story Editor");
            window.minSize = new Vector2(980f, 560f);
            window.Show();
        }

        private void OnEnable()
        {
            _sprites = StageSpriteLibrary.Load();
            RefreshNameLists();

            if (_act == null)
            {
                List<ActAsset> acts = StoryAssetBuilder.FindActs();
                if (acts.Count > 0)
                {
                    SelectAct(acts[0]);
                }
            }
        }

        private void OnGUI()
        {
            // A script recompile wipes everything that is not a Unity object
            // reference, so the act survives the reload and its SerializedObject
            // and list do not. Rebuilding them here is what stops the window
            // throwing on the first repaint after every code change.
            if (_act != null && (_serialized == null || _list == null))
            {
                SelectAct(_act);
            }

            DrawToolbar();

            switch (_tab)
            {
                case Tab.Stage:
                    DrawStageTab();
                    return;

                case Tab.Guide:
                    DrawGuideTab();
                    return;
            }

            if (_act == null)
            {
                DrawEmptyState();
                return;
            }

            if (_tab == Tab.Check)
            {
                DrawCheckTab();
                return;
            }

            _serialized.Update();

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(ListWidth)))
                {
                    DrawActHeader();
                    DrawBeatList();
                    DrawQuickAdd();
                }

                DrawBeatDetail();
            }

            _serialized.ApplyModifiedProperties();
        }

        // ---------------------------------------------------------------------
        //  Chrome
        // ---------------------------------------------------------------------

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                _tab = (Tab)GUILayout.Toolbar(
                    (int)_tab,
                    new[] { "Script", "Stage", "Check", "Guide" },
                    EditorStyles.toolbarButton,
                    GUILayout.Width(280f));

                GUILayout.Space(10f);

                List<ActAsset> acts = StoryAssetBuilder.FindActs();
                string[] names = new string[acts.Count];
                int current = -1;

                for (int i = 0; i < acts.Count; i++)
                {
                    names[i] = acts[i].ActNumber > 0
                        ? $"Act {acts[i].ActNumber:00} — {acts[i].name}"
                        : $"Interlude — {acts[i].name}";

                    if (acts[i] == _act)
                    {
                        current = i;
                    }
                }

                int picked = EditorGUILayout.Popup(current, names, EditorStyles.toolbarPopup, GUILayout.Width(240f));
                if (picked >= 0 && picked < acts.Count && acts[picked] != _act)
                {
                    SelectAct(acts[picked]);
                }

                if (GUILayout.Button(
                        new GUIContent("New Act", "Start a new act, or a numberless interlude."),
                        EditorStyles.toolbarButton,
                        GUILayout.Width(70f)))
                {
                    CreateActMenu(acts);
                }

                if (_act != null && GUILayout.Button(
                        new GUIContent("Show in Project", "Select this act's asset in the Project window."),
                        EditorStyles.toolbarButton,
                        GUILayout.Width(100f)))
                {
                    EditorGUIUtility.PingObject(_act);
                    Selection.activeObject = _act;
                }

                GUILayout.FlexibleSpace();

                // The language selector changes which translation the text
                // fields show, so a translator can work down a whole act in one
                // language without three boxes fighting for the same space.
                GUILayout.Label("Language", EditorStyles.miniLabel);

                GameLanguage chosen = (GameLanguage)EditorGUILayout.EnumPopup(
                    _language, EditorStyles.toolbarPopup, GUILayout.Width(90f));

                if (chosen != _language)
                {
                    _language = chosen;
                    GUI.FocusControl(null);
                }

                if (GUILayout.Button(
                        new GUIContent("Rebuild", "Re-scan the project for acts, backgrounds and music."),
                        EditorStyles.toolbarButton,
                        GUILayout.Width(60f)))
                {
                    StoryAssetBuilder.Rebuild();
                    _sprites = StageSpriteLibrary.Load();
                    PersianEditorText.Forget();
                    RefreshNameLists();
                }
            }
        }

        private void DrawEmptyState()
        {
            EditorGUILayout.Space(40f);

            EditorGUILayout.HelpBox(
                "There are no acts yet.\n\n" +
                "An act is one asset holding the whole script of one chapter: every line, all three " +
                "translations, who says it and what the screen is doing while they do.\n\n" +
                "Start a new one below, then write it beat by beat. The Guide tab explains the rest.",
                MessageType.Info);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("New Act 01", GUILayout.Width(180f), GUILayout.Height(30f)))
                {
                    SelectAct(StoryAssetBuilder.CreateAct(1, "Act01"));
                }

                if (GUILayout.Button("Open the Guide", GUILayout.Width(180f), GUILayout.Height(30f)))
                {
                    _tab = Tab.Guide;
                }

                GUILayout.FlexibleSpace();
            }
        }

        private void CreateActMenu(List<ActAsset> existing)
        {
            GenericMenu menu = new GenericMenu();

            // The seven acts the design document names, offered by number so a
            // new one lands with the right number and title card without anybody
            // having to remember which is which.
            string[] titles =
            {
                "Cherry Blossom Mirage", "Maboroshi", "Serenity", "To Deepen",
                "Glass Shattering", "Mechanical Room", "Outro"
            };

            for (int i = 0; i < titles.Length; i++)
            {
                int number = i + 1;
                bool taken = existing.Exists(a => a.ActNumber == number);
                string label = $"Act {number:00} — {titles[i]}" + (taken ? "  (exists)" : string.Empty);

                if (taken)
                {
                    menu.AddDisabledItem(new GUIContent(label));
                    continue;
                }

                string title = titles[i];
                menu.AddItem(new GUIContent(label), false, () =>
                {
                    ActAsset act = StoryAssetBuilder.CreateAct(number, $"Act{number:00}");
                    act.Title = new LocalizedLine(title, string.Empty, string.Empty);
                    EditorUtility.SetDirty(act);
                    AssetDatabase.SaveAssets();
                    SelectAct(act);
                });
            }

            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Interlude (no number)"), false, () =>
                SelectAct(StoryAssetBuilder.CreateAct(0, "Interlude_New")));

            menu.ShowAsContext();
        }

        // ---------------------------------------------------------------------
        //  Act header and beat list
        // ---------------------------------------------------------------------

        private void DrawActHeader()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(
                    _serialized.FindProperty("ActNumber"),
                    new GUIContent("Act number", "Which act this is. The scene Act03.unity plays act 3. Interludes use 0."));

                EditorGUILayout.PropertyField(
                    _serialized.FindProperty("ShowTitleCard"),
                    new GUIContent("Name the act on screen", "Show the title card when the act begins."));

                DrawLocalized(_serialized.FindProperty("Title"), "Act title", 1);
                DrawTrackField(_serialized.FindProperty("MusicTrack"), "Act music");

                EditorGUILayout.LabelField(
                    $"{_act.Count} beats · {_act.SpokenCount()} spoken lines · plays in scene Act{Mathf.Max(1, _act.ActNumber):00}",
                    EditorStyles.miniLabel);
            }
        }

        /// <summary>
        /// The script, in a scroll view of its own.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A reorderable list draws its whole contents and its footer — the
        /// buttons that add and remove a beat — underneath them. Act one is a
        /// hundred and thirty-two beats, which is some three and a half thousand
        /// pixels of list in a window six hundred tall, so the footer was three
        /// screens below the bottom edge of a window that did not scroll. There
        /// was no way to add a beat, and no way to delete one.
        /// </para>
        /// <para>
        /// The list scrolls on its own now, and the buttons that act on it live
        /// outside the scroll view where they cannot be pushed off. The header
        /// above stays put too: an act's title and music are not things to go
        /// hunting for.
        /// </para>
        /// </remarks>
        private void DrawBeatList()
        {
            _listScroll = EditorGUILayout.BeginScrollView(
                _listScroll,
                GUILayout.ExpandHeight(true));

            _list.DoLayoutList();

            EditorGUILayout.EndScrollView();
        }

        /// <summary>Removes the selected beat, from outside the list's own footer.</summary>
        private void RemoveSelectedBeat()
        {
            SerializedProperty beats = _serialized.FindProperty("Beats");

            if (_list.index < 0 || _list.index >= beats.arraySize)
            {
                return;
            }

            int removed = _list.index;
            beats.DeleteArrayElementAtIndex(removed);

            _list.index = Mathf.Clamp(removed - 1, 0, Mathf.Max(0, beats.arraySize - 1));
            GUI.FocusControl(null);
        }

        /// <summary>Moves the selected beat one place up or down.</summary>
        private void MoveSelectedBeat(int direction)
        {
            SerializedProperty beats = _serialized.FindProperty("Beats");

            int from = _list.index;
            int to = from + direction;

            if (from < 0 || from >= beats.arraySize || to < 0 || to >= beats.arraySize)
            {
                return;
            }

            beats.MoveArrayElement(from, to);
            _list.index = to;
            GUI.FocusControl(null);
        }

        /// <summary>
        /// One button per beat kind that writing an act actually uses.
        /// </summary>
        /// <remarks>
        /// The reorderable list's own <c>+</c> adds a copy of the beat above,
        /// which is right when writing dialogue and useless when the next thing
        /// needed is a background change. Naming the six that get used covers
        /// most of an act without anyone having to learn the dropdown first —
        /// and the dropdown is still there for the other nine.
        /// </remarks>
        private void DrawQuickAdd()
        {
            int count = _serialized.FindProperty("Beats").arraySize;
            bool hasSelection = _list.index >= 0 && _list.index < count;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField(
                    hasSelection
                        ? $"Add a beat after {_list.index:000}"
                        : "Add a beat at the end",
                    EditorStyles.miniBoldLabel);

                using (new EditorGUILayout.HorizontalScope())
                {
                    for (int i = 0; i < QuickAdd.Length; i++)
                    {
                        StoryBeatKind kind = QuickAdd[i];

                        if (GUILayout.Button(new GUIContent(kind.ToString(), DescriptionOf(kind)), EditorStyles.miniButton))
                        {
                            AddBeat(kind);
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    for (int i = 0; i < QuickAddPresentation.Length; i++)
                    {
                        StoryBeatKind kind = QuickAddPresentation[i];

                        if (GUILayout.Button(new GUIContent(kind.ToString(), DescriptionOf(kind)), EditorStyles.miniButton))
                        {
                            AddBeat(kind);
                        }
                    }
                }

                EditorGUILayout.Space(2f);

                using (new EditorGUI.DisabledScope(!hasSelection))
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("▲", "Move this beat one place earlier."),
                            EditorStyles.miniButton, GUILayout.Width(34f)))
                    {
                        MoveSelectedBeat(-1);
                    }

                    if (GUILayout.Button(new GUIContent("▼", "Move this beat one place later."),
                            EditorStyles.miniButton, GUILayout.Width(34f)))
                    {
                        MoveSelectedBeat(1);
                    }

                    if (GUILayout.Button(new GUIContent("Duplicate", "Copy this beat, translations and all."),
                            EditorStyles.miniButton))
                    {
                        DuplicateSelectedBeat();
                    }

                    if (GUILayout.Button(new GUIContent("Delete beat", "Remove this beat from the act."),
                            EditorStyles.miniButton))
                    {
                        RemoveSelectedBeat();
                    }
                }
            }
        }

        /// <summary>Copies the selected beat, everything included.</summary>
        /// <remarks>
        /// The list's own add button does this and then blanks the text, which
        /// is right for writing the next line and wrong for making a variant of
        /// this one. Both are useful; they are separate buttons now.
        /// </remarks>
        private void DuplicateSelectedBeat()
        {
            SerializedProperty beats = _serialized.FindProperty("Beats");

            if (_list.index < 0 || _list.index >= beats.arraySize)
            {
                return;
            }

            int at = _list.index;
            beats.InsertArrayElementAtIndex(at);

            _list.index = at + 1;
            GUI.FocusControl(null);
        }

        private void AddBeat(StoryBeatKind kind)
        {
            SerializedProperty beats = _serialized.FindProperty("Beats");

            int at = _list.index >= 0 && _list.index < beats.arraySize ? _list.index + 1 : beats.arraySize;
            beats.InsertArrayElementAtIndex(at);

            SerializedProperty added = beats.GetArrayElementAtIndex(at);
            ResetBeat(added, kind);

            _list.index = at;
            GUI.FocusControl(null);
        }

        /// <summary>
        /// Empties a freshly inserted beat and points it at a kind.
        /// </summary>
        /// <remarks>
        /// A list insert copies its neighbour, which is useful for the text
        /// fields of a dialogue line and wrong for everything else — a new
        /// Choice inheriting the previous Choice's options is the kind of thing
        /// that gets shipped.
        /// </remarks>
        private static void ResetBeat(SerializedProperty beat, StoryBeatKind kind)
        {
            beat.FindPropertyRelative("Kind").enumValueIndex = (int)kind;

            ClearLocalized(beat.FindPropertyRelative("Text"));
            ClearLocalized(beat.FindPropertyRelative("Caption"));

            beat.FindPropertyRelative("Note").stringValue = string.Empty;
            beat.FindPropertyRelative("PlaySound").boolValue = kind == StoryBeatKind.Sound;

            RestoreDefaults(beat);

            if (kind != StoryBeatKind.Choice)
            {
                beat.FindPropertyRelative("Choices").arraySize = 0;
                return;
            }

            SerializedProperty choices = beat.FindPropertyRelative("Choices");
            choices.arraySize = 2;

            for (int i = 0; i < 2; i++)
            {
                SerializedProperty choice = choices.GetArrayElementAtIndex(i);
                ClearLocalized(choice.FindPropertyRelative("Text"));
                choice.FindPropertyRelative("Tone").enumValueIndex = i == 0 ? (int)ChoiceTone.Kind : (int)ChoiceTone.Cruel;
            }
        }

        /// <summary>
        /// Puts back the numbers a fresh insert leaves at zero.
        /// </summary>
        /// <remarks>
        /// Adding to an empty list does not run the class' own field
        /// initialisers — the serialized data is zeroed instead — so a brand new
        /// beat arrives holding for no time, playing at no volume, and with an
        /// interlude chance of never. All three are silent: the beat simply does
        /// nothing, and there is no obvious reason why.
        /// </remarks>
        private static void RestoreDefaults(SerializedProperty beat)
        {
            SerializedProperty seconds = beat.FindPropertyRelative("Seconds");
            if (seconds.floatValue <= 0f)
            {
                seconds.floatValue = 1.5f;
            }

            SerializedProperty chance = beat.FindPropertyRelative("Chance");
            if (chance.floatValue <= 0f)
            {
                chance.floatValue = 1f;
            }

            SerializedProperty volume = beat.FindPropertyRelative("SoundVolume");
            if (volume.floatValue <= 0f)
            {
                volume.floatValue = 1f;
            }
        }

        private void BuildList()
        {
            SerializedProperty beats = _serialized.FindProperty("Beats");

            _list = new ReorderableList(_serialized, beats, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Script  ·  drag to reorder"),
                elementHeight = EditorGUIUtility.singleLineHeight + 8f
            };

            _list.drawElementCallback = (rect, index, active, focused) =>
            {
                if (index < 0 || index >= beats.arraySize)
                {
                    return;
                }

                SerializedProperty beat = beats.GetArrayElementAtIndex(index);
                StoryBeatKind kind = (StoryBeatKind)beat.FindPropertyRelative("Kind").enumValueIndex;

                Color tint = TintFor(kind);
                if (tint.a > 0f)
                {
                    EditorGUI.DrawRect(rect, tint);
                }

                Rect number = new Rect(rect.x, rect.y + 3f, 34f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(number, index.ToString("000"), EditorStyles.miniLabel);

                Rect label = new Rect(rect.x + 34f, rect.y + 3f, rect.width - 36f, EditorGUIUtility.singleLineHeight);

                string summary = SummaryOf(beat, kind);

                // Persian in the list is drawn with the Persian font and in
                // reading order, the same as everywhere else in this window.
                if (PersianEditorText.IsArabicScript(summary))
                {
                    EditorGUI.LabelField(label, PersianEditorText.Display(summary), PersianEditorText.LabelStyle);
                }
                else
                {
                    EditorGUI.LabelField(label, summary);
                }
            };

            _list.onAddCallback = list =>
            {
                int at = list.index >= 0 ? list.index + 1 : beats.arraySize;
                beats.InsertArrayElementAtIndex(at);

                // A freshly inserted element is a copy of its neighbour, which is
                // usually what you want when writing dialogue — same speaker,
                // same expression — but the text has to go or the new line is a
                // duplicate of the old one.
                SerializedProperty added = beats.GetArrayElementAtIndex(at);
                ClearLocalized(added.FindPropertyRelative("Text"));
                added.FindPropertyRelative("Note").stringValue = string.Empty;
                RestoreDefaults(added);

                list.index = at;
            };
        }

        private static Color TintFor(StoryBeatKind kind)
        {
            switch (kind)
            {
                case StoryBeatKind.Line:
                    return LineTint;

                case StoryBeatKind.Background:
                case StoryBeatKind.Enter:
                case StoryBeatKind.Exit:
                case StoryBeatKind.ClearStage:
                case StoryBeatKind.Caption:
                    return StageTint;

                case StoryBeatKind.Sound:
                case StoryBeatKind.Music:
                case StoryBeatKind.CutMusic:
                    return SoundTint;

                case StoryBeatKind.Grade:
                case StoryBeatKind.Stain:
                    return StageTint;

                default:
                    return FlowTint;
            }
        }

        private string SummaryOf(SerializedProperty beat, StoryBeatKind kind)
        {
            switch (kind)
            {
                case StoryBeatKind.Line:
                {
                    Speaker speaker = (Speaker)beat.FindPropertyRelative("Speaker").enumValueIndex;
                    string text = LocalizedValue(beat.FindPropertyRelative("Text"));
                    string who = speaker == Speaker.Narrator ? string.Empty : speaker + ": ";
                    string patience = beat.FindPropertyRelative("MeasurePatience").boolValue ? "⏳ " : string.Empty;
                    return patience + who + Shorten(text, 42);
                }

                case StoryBeatKind.Background:
                    return "◈ " + Or(beat.FindPropertyRelative("Background").stringValue, "no background");

                case StoryBeatKind.Enter:
                    return $"→ {(Speaker)beat.FindPropertyRelative("Speaker").enumValueIndex} enters " +
                           $"({(Portrait)beat.FindPropertyRelative("Portrait").enumValueIndex})";

                case StoryBeatKind.Exit:
                    return $"← {(Speaker)beat.FindPropertyRelative("Speaker").enumValueIndex} leaves";

                case StoryBeatKind.ClearStage:
                    return "← clear the stage";

                case StoryBeatKind.Caption:
                    return "▭ " + Shorten(LocalizedValue(beat.FindPropertyRelative("Caption")), 42);

                case StoryBeatKind.TitleCard:
                    return "★ title card";

                case StoryBeatKind.Sound:
                    return "♪ " + (SfxId)beat.FindPropertyRelative("Sound").enumValueIndex;

                case StoryBeatKind.Music:
                    return "♫ " + Or(beat.FindPropertyRelative("MusicTrack").stringValue, "stop the music");

                case StoryBeatKind.Beat:
                    return $"⏸ hold {beat.FindPropertyRelative("Seconds").floatValue:0.0}s";

                case StoryBeatKind.Choice:
                {
                    string overridden = beat.FindPropertyRelative("YuaOverridesKindness").boolValue
                        ? ", she overrules blue"
                        : string.Empty;

                    return $"◆ choice ({beat.FindPropertyRelative("Choices").arraySize} options{overridden})";
                }

                case StoryBeatKind.Interlude:
                {
                    Object interlude = beat.FindPropertyRelative("Interlude").objectReferenceValue;
                    float chance = beat.FindPropertyRelative("Chance").floatValue;
                    return $"⟲ {(interlude != null ? interlude.name : "nothing")} ({chance:P0})";
                }

                case StoryBeatKind.OpenFrame:
                    return "⛶ open the frame";

                case StoryBeatKind.CloseFrame:
                    return "⛶ close the frame";

                case StoryBeatKind.PullDownDialogue:
                    return "⇩ he pulls the dialogue box down";

                case StoryBeatKind.Grade:
                {
                    float amount = beat.FindPropertyRelative("Amount").floatValue;
                    return amount <= 0.001f
                        ? "◐ take the veil off"
                        : $"◐ veil to {amount:P0}";
                }

                case StoryBeatKind.Stain:
                {
                    Color tint = beat.FindPropertyRelative("Tint").colorValue;
                    return tint.a <= 0.001f ? "◼ clear the screen" : "◼ stain the screen";
                }

                case StoryBeatKind.CutMusic:
                    return "♫ cut the music dead";

                case StoryBeatKind.EnterCinema:
                    return "▷ the story plays itself from here";

                case StoryBeatKind.ExitCinema:
                    return "▷ give the player the story back";

                case StoryBeatKind.Video:
                    return "▣ film: " + Or(beat.FindPropertyRelative("Film").stringValue, "nothing");

                case StoryBeatKind.EndGame:
                    return "■ the story ends, saves are erased";

                default:
                    return "■ end of act";
            }
        }

        /// <summary>
        /// What one beat kind does, in a sentence.
        /// </summary>
        /// <remarks>
        /// Shown next to the kind dropdown and used as the tooltip on the
        /// quick-add buttons. Fifteen kinds is more than anyone will remember
        /// from their names alone, and "what does ClearStage do, and does it
        /// wait for me?" is a question the window can simply answer.
        /// </remarks>
        private static string DescriptionOf(StoryBeatKind kind)
        {
            switch (kind)
            {
                case StoryBeatKind.Line:
                    return "One line of dialogue or narration. The game waits for the player to read it.";

                case StoryBeatKind.Background:
                    return "Move to a new place. Crossfades the picture and can name the place in the corner. "
                           + "The game waits for the transition, not for the player.";

                case StoryBeatKind.Enter:
                    return "Bring a character on stage, or change the face of one already there. Does not wait.";

                case StoryBeatKind.Exit:
                    return "Take one character off stage. Does not wait.";

                case StoryBeatKind.ClearStage:
                    return "Take everyone off stage. Does not wait.";

                case StoryBeatKind.Caption:
                    return "Name the place in the corner of the screen for a few seconds. Does not wait.";

                case StoryBeatKind.TitleCard:
                    return "Show the act's name over a darkened screen. Add one only if you want the card "
                           + "somewhere other than the start — the act shows it at the start by itself.";

                case StoryBeatKind.Sound:
                    return "Play one sound effect on its own. Does not wait.";

                case StoryBeatKind.Music:
                    return "Start a background track, or stop the current one by leaving the name empty.";

                case StoryBeatKind.Beat:
                    return "Silence. The dialogue box fades away and only the picture is left, for as long "
                           + "as you say. The most under-used tool here: two seconds of nothing says what a "
                           + "line of narration would ruin.";

                case StoryBeatKind.Choice:
                    return "Offer the player buttons and wait for one to be pressed. Blue counts as kind, "
                           + "green as controlling.";

                case StoryBeatKind.Interlude:
                    return "Play a short numberless act inline, either always or at random.";

                case StoryBeatKind.OpenFrame:
                    return "Widen the frame the game has been played inside until it is gone.";

                case StoryBeatKind.CloseFrame:
                    return "Put the frame back.";

                case StoryBeatKind.PullDownDialogue:
                    return "Haru takes hold of the dialogue box and drags it off the bottom of the screen. "
                           + "It travels rather than fading. The game waits for him to finish. Act five's.";

                case StoryBeatKind.Grade:
                    return "How much of the pastel veil the picture is seen through. 1 is how acts one to "
                           + "four look; 0 is the art with nothing in front of it, which is what act five "
                           + "means by the graphics becoming real. The game waits for it.";

                case StoryBeatKind.Stain:
                    return "Throw a colour across the screen and leave it there. An alpha of zero takes it "
                           + "off again. Zero seconds is a cut, which is what it is for.";

                case StoryBeatKind.CutMusic:
                    return "Stop the music in a single frame, with no fade at all. A Music beat with an "
                           + "empty track fades over about a second and reads as a scene ending; this one "
                           + "reads as something going wrong.";

                case StoryBeatKind.EnterCinema:
                    return "From here the story plays itself: every line holds for as long as it takes to "
                           + "read and then goes on its own, and clicking does nothing. Act six's, where "
                           + "the player is watching rather than reading. Pausing still works.";

                case StoryBeatKind.ExitCinema:
                    return "Give the player the story back. Not needed at the end of an act — a new act "
                           + "always starts with them holding it.";

                case StoryBeatKind.Video:
                    return "Play a film full screen, over everything. Looked for in Assets/Video and then "
                           + "in StreamingAssets/Video. A name with no film behind it carries straight on, "
                           + "so the act works before the video is cut. Needs Unity's Video module.";

                case StoryBeatKind.EndGame:
                    return "The story is over: every save is erased and the game returns to the title. "
                           + "Act seven and both endings finish with this. A replay has to start at the "
                           + "very beginning — the secret ending is a fact about a whole playthrough.";

                default:
                    return "The act ends here. Beats after this one are never played.";
            }
        }

        // ---------------------------------------------------------------------
        //  Beat detail
        // ---------------------------------------------------------------------

        private void DrawBeatDetail()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                SerializedProperty beats = _serialized.FindProperty("Beats");

                if (_list.index < 0 || _list.index >= beats.arraySize)
                {
                    EditorGUILayout.Space(20f);
                    EditorGUILayout.LabelField(
                        "Pick a beat on the left, or add one with the buttons underneath the list.",
                        EditorStyles.centeredGreyMiniLabel);
                    return;
                }

                _detailScroll = EditorGUILayout.BeginScrollView(_detailScroll);

                SerializedProperty beat = beats.GetArrayElementAtIndex(_list.index);
                SerializedProperty kindProp = beat.FindPropertyRelative("Kind");
                StoryBeatKind kind = (StoryBeatKind)kindProp.enumValueIndex;

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField($"Beat {_list.index:000}", EditorStyles.boldLabel);

                    GUILayout.FlexibleSpace();

                    using (new EditorGUI.DisabledScope(EditorApplication.isPlaying || _act.ActNumber <= 0))
                    {
                        if (GUILayout.Button(
                                new GUIContent(
                                    "▶ Play from here",
                                    "Open this act's scene and start the game at this beat. Everything " +
                                    "before it is applied silently first, exactly as loading a save does."),
                                GUILayout.Width(130f)))
                        {
                            StoryPlaytest.PlayFrom(_act, _list.index);
                        }
                    }
                }

                EditorGUILayout.PropertyField(kindProp, new GUIContent("This beat"));

                EditorGUILayout.LabelField(DescriptionOf(kind), EditorStyles.wordWrappedMiniLabel);
                EditorGUILayout.Space(8f);

                switch (kind)
                {
                    case StoryBeatKind.Line: DrawLineBeat(beat); break;
                    case StoryBeatKind.Background: DrawBackgroundBeat(beat); break;
                    case StoryBeatKind.Enter: DrawEnterBeat(beat); break;
                    case StoryBeatKind.Exit: DrawSpeakerOnly(beat); break;
                    case StoryBeatKind.Caption: DrawLocalized(beat.FindPropertyRelative("Caption"), "Place name", 2); break;
                    case StoryBeatKind.Music: DrawTrackField(beat.FindPropertyRelative("MusicTrack"), "Track"); break;
                    case StoryBeatKind.Beat: DrawSeconds(beat, "Hold for"); break;
                    case StoryBeatKind.Choice: DrawChoiceBeat(beat); break;
                    case StoryBeatKind.Interlude: DrawInterludeBeat(beat); break;
                    case StoryBeatKind.OpenFrame:
                    case StoryBeatKind.CloseFrame: DrawSeconds(beat, "Takes"); break;
                    case StoryBeatKind.PullDownDialogue: DrawSeconds(beat, "Takes"); break;
                    case StoryBeatKind.Grade: DrawGradeBeat(beat); break;
                    case StoryBeatKind.Stain: DrawStainBeat(beat); break;
                    case StoryBeatKind.Video: DrawFilmField(beat); break;
                }

                if (kind != StoryBeatKind.Music)
                {
                    EditorGUILayout.Space(8f);
                    DrawSoundBlock(beat, kind == StoryBeatKind.Sound);
                }

                EditorGUILayout.Space(8f);
                EditorGUILayout.PropertyField(
                    beat.FindPropertyRelative("Note"),
                    new GUIContent("Note to self", "Never shown to the player."));

                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawLineBeat(SerializedProperty beat)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(
                    beat.FindPropertyRelative("Speaker"),
                    new GUIContent("Who says it", "Narrator has no name plate."),
                    GUILayout.Width(260f));

                EditorGUILayout.PropertyField(
                    beat.FindPropertyRelative("Portrait"),
                    new GUIContent("Their face", "'Unchanged' leaves whatever is already on screen."),
                    GUILayout.Width(260f));

                GUILayout.FlexibleSpace();
            }

            DrawPortraitPreview(beat);
            EditorGUILayout.Space(6f);
            DrawLocalized(beat.FindPropertyRelative("Text"), "Line", 4);

            EditorGUILayout.Space(6f);

            SerializedProperty voice = beat.FindPropertyRelative("VoiceClip");

            EditorGUILayout.PropertyField(
                voice,
                new GUIContent(
                    "Recording",
                    "File name in Assets/Audio/Voice, without the extension. Leave it empty and the " +
                    "typewriter reads the line as it always has."),
                GUILayout.Width(420f));

            if (!string.IsNullOrWhiteSpace(voice.stringValue) && !IsKnownVoice(voice.stringValue))
            {
                EditorGUILayout.HelpBox(
                    $"There is no recording called '{voice.stringValue}' yet. That is not a problem — the " +
                    "line plays with the typewriter voice, exactly as an unvoiced one does. Drop a file of " +
                    "that name into Assets/Audio/Voice whenever it exists and it is picked up with no " +
                    "other change.",
                    MessageType.None);
            }

            EditorGUILayout.Space(6f);

            EditorGUILayout.PropertyField(
                beat.FindPropertyRelative("TypeSpeed"),
                new GUIContent(
                    "Typing speed",
                    "Characters per second. 0 is the normal reading speed; around 140 is the breathless "
                    + "rush the design document asks for in the two spoken endings."),
                GUILayout.Width(300f));

            EditorGUILayout.Space(6f);

            SerializedProperty patience = beat.FindPropertyRelative("MeasurePatience");

            EditorGUILayout.PropertyField(
                patience,
                new GUIContent(
                    "One of the silences",
                    "A player who sits here for five minutes without clicking is counted as having " +
                    "listened. The endings read that count."),
                GUILayout.Width(300f));

            if (patience.boolValue)
            {
                EditorGUILayout.HelpBox(
                    "Five minutes on this line counts once towards the endings. Nothing appears on screen " +
                    "and nothing tells the player — put it where the story has already stopped, on Haru's " +
                    "leg or on the machine room, and let the line say nothing that asks to be clicked past.",
                    MessageType.None);
            }
        }

        private void DrawEnterBeat(SerializedProperty beat)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(
                    beat.FindPropertyRelative("Speaker"), new GUIContent("Who"), GUILayout.Width(260f));

                EditorGUILayout.PropertyField(
                    beat.FindPropertyRelative("Portrait"), new GUIContent("Their face"), GUILayout.Width(260f));

                GUILayout.FlexibleSpace();
            }

            DrawPortraitPreview(beat);
        }

        private static void DrawSpeakerOnly(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(beat.FindPropertyRelative("Speaker"), new GUIContent("Who"), GUILayout.Width(260f));
        }

        private void DrawBackgroundBeat(SerializedProperty beat)
        {
            SerializedProperty background = beat.FindPropertyRelative("Background");
            DrawNamePopup(background, "Background", _backgroundNames);

            Sprite sprite = _sprites != null ? _sprites.FindBackground(background.stringValue) : null;
            if (sprite != null)
            {
                Rect rect = GUILayoutUtility.GetRect(320f, 180f, GUILayout.ExpandWidth(false));
                DrawSprite(rect, sprite);
            }

            EditorGUILayout.Space(6f);
            DrawLocalized(beat.FindPropertyRelative("Caption"), "Place name", 2);
        }

        private void DrawChoiceBeat(SerializedProperty beat)
        {
            SerializedProperty choices = beat.FindPropertyRelative("Choices");

            for (int i = 0; i < choices.arraySize; i++)
            {
                SerializedProperty choice = choices.GetArrayElementAtIndex(i);

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.PropertyField(
                            choice.FindPropertyRelative("Tone"),
                            new GUIContent("Colour", "Blue is kind, green is controlling, white counts as neither."),
                            GUILayout.Width(260f));

                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Remove", GUILayout.Width(70f)))
                        {
                            choices.DeleteArrayElementAtIndex(i);
                            return;
                        }
                    }

                    DrawLocalized(choice.FindPropertyRelative("Text"), $"Option {i + 1}", 2);
                }
            }

            if (GUILayout.Button("Add option", GUILayout.Width(120f)))
            {
                choices.InsertArrayElementAtIndex(choices.arraySize);
            }

            EditorGUILayout.HelpBox(
                "Blue counts as kind and green as controlling. One green choice anywhere in a " +
                "playthrough rules out the secret ending for good.",
                MessageType.None);

            EditorGUILayout.Space(8f);

            SerializedProperty overrides = beat.FindPropertyRelative("YuaOverridesKindness");

            EditorGUILayout.PropertyField(
                overrides,
                new GUIContent(
                    "Yua overrules blue",
                    "Taking the blue option makes her face go flat and the scene carry on the green " +
                    "way anyway. The press is still counted as kind."));

            if (!overrides.boolValue)
            {
                return;
            }

            EditorGUILayout.Space(4f);
            DrawLocalized(beat.FindPropertyRelative("OverrideLine"), "She takes it back with", 3);

            EditorGUILayout.HelpBox(
                "The act does not branch: whatever the player presses, the beats after this one play. " +
                "Write them as the green path and let her refusal carry the blue one.\n\n" +
                "Her face is left flat when the refusal ends, so give the next beat an expression — " +
                "the green path needs one too.\n\n" +
                "The press is still recorded as kind, so a player who is refused every single time " +
                "still reaches the secret ending. That is the point of it.",
                MessageType.None);
        }

        private static void DrawInterludeBeat(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(beat.FindPropertyRelative("Interlude"), new GUIContent("Plays"));
            EditorGUILayout.PropertyField(
                beat.FindPropertyRelative("Chance"),
                new GUIContent("Chance", "1 always plays it. 0.35 plays it about a third of the time."));

            EditorGUILayout.HelpBox(
                "An interlude is just another act with no number. Set the chance below 1 for the " +
                "recurring beats the design document wants to turn up at random.",
                MessageType.None);
        }

        private static void DrawFilmField(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(
                beat.FindPropertyRelative("Film"),
                new GUIContent("Film", "File name without the extension."),
                GUILayout.Width(420f));

            EditorGUILayout.HelpBox(
                "Assets/Video for a clip that gets packed into the build, or "
                + "Assets/StreamingAssets/Video for a file that stays a file — which is usually what a "
                + "several-minute credits sequence wants to be, because it can then be recut without "
                + "reimporting anything.\n\n"
                + "Needs Unity's Video module. The Frayed Red String ▸ Check The Video Module says "
                + "whether this project has it.",
                MessageType.None);
        }

        private static void DrawGradeBeat(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(
                beat.FindPropertyRelative("Amount"),
                new GUIContent("Veil", "1 is acts one to four. 0 is the art with nothing over it."),
                GUILayout.Width(300f));

            DrawSeconds(beat, "Over");

            EditorGUILayout.HelpBox(
                "The veil is the pane of frosted glass the design document has act five remove. Nobody "
                + "notices it while it is there, which is the point — so take it off slowly, and under a "
                + "picture the player is already looking at.",
                MessageType.None);
        }

        private static void DrawStainBeat(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(
                beat.FindPropertyRelative("Tint"),
                new GUIContent("Colour", "Alpha included. Zero alpha clears whatever is on screen."),
                GUILayout.Width(300f));

            DrawSeconds(beat, "Over");

            EditorGUILayout.HelpBox(
                "Zero seconds is a cut. One thing in this game needs this beat, and it needs it to happen "
                + "between two frames.",
                MessageType.None);
        }

        private static void DrawSeconds(SerializedProperty beat, string label)
        {
            EditorGUILayout.PropertyField(beat.FindPropertyRelative("Seconds"), new GUIContent(label), GUILayout.Width(300f));
        }

        private void DrawSoundBlock(SerializedProperty beat, bool required)
        {
            SerializedProperty play = beat.FindPropertyRelative("PlaySound");

            if (required)
            {
                play.boolValue = true;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(required))
                {
                    EditorGUILayout.PropertyField(play, new GUIContent("Play a sound"), GUILayout.Width(140f));
                }

                if (!play.boolValue)
                {
                    GUILayout.FlexibleSpace();
                    return;
                }

                SerializedProperty sound = beat.FindPropertyRelative("Sound");
                EditorGUILayout.PropertyField(sound, GUIContent.none, GUILayout.Width(180f));
                EditorGUILayout.PropertyField(beat.FindPropertyRelative("SoundVolume"), GUIContent.none, GUILayout.Width(160f));

                if (GUILayout.Button(new GUIContent("▶", "Hear it now, without pressing Play."), GUILayout.Width(28f)))
                {
                    PreviewSound((SfxId)sound.enumValueIndex);
                }

                // The machine-room drone is four seconds long and deliberately
                // unpleasant. Being able to stop it matters more here than it
                // would in a library of clicks.
                if (GUILayout.Button(new GUIContent("■", "Stop it."), GUILayout.Width(28f)))
                {
                    SoundPreview.Stop();
                }

                // The way out when ▶ does nothing. The Editor's own preview is
                // an internal API that moves between versions; a .wav on disk is
                // a .wav on disk, and the Inspector plays it with a button that
                // has been there for fifteen years.
                if (GUILayout.Button(
                        new GUIContent("To file", "Write it out as a .wav and select it, so the Inspector can play it."),
                        GUILayout.Width(56f)))
                {
                    SfxWavExporter.ExportAndReveal((SfxId)sound.enumValueIndex);
                }

                GUILayout.FlexibleSpace();
            }
        }

        // ---------------------------------------------------------------------
        //  Check tab
        // ---------------------------------------------------------------------

        /// <summary>
        /// Everything about this act that will misbehave at runtime, listed
        /// before it does.
        /// </summary>
        /// <remarks>
        /// Every one of these is something the game survives — a missing
        /// background keeps the previous one, an untranslated line falls back to
        /// English — which is exactly why they are worth listing. A failure that
        /// crashes gets fixed; a failure that quietly does something reasonable
        /// ships.
        /// </remarks>
        private void DrawCheckTab()
        {
            List<Finding> findings = Check();

            _checkScroll = EditorGUILayout.BeginScrollView(_checkScroll);

            EditorGUILayout.LabelField($"{_act.name}", EditorStyles.boldLabel);

            if (findings.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "Nothing to report. Every line has text, every background and track exists, and " +
                    "every beat has what it needs.",
                    MessageType.Info);

                EditorGUILayout.EndScrollView();
                return;
            }

            for (int i = 0; i < findings.Count; i++)
            {
                Finding finding = findings[i];

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.HelpBox(finding.Message, finding.Severity);

                    if (finding.BeatIndex >= 0 && GUILayout.Button("Show", GUILayout.Width(60f), GUILayout.Height(38f)))
                    {
                        _tab = Tab.Script;
                        _list.index = finding.BeatIndex;
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private List<Finding> Check()
        {
            List<Finding> findings = new List<Finding>();

            bool numberless = _act.ActNumber <= 0;
            bool interlude = _act.name.StartsWith("Interlude", System.StringComparison.OrdinalIgnoreCase);
            bool ending = _act.name.StartsWith("Ending", System.StringComparison.OrdinalIgnoreCase);

            if (numberless && !interlude && !ending)
            {
                findings.Add(new Finding(
                    MessageType.Warning,
                    "This act has no number, so no scene will ever play it. Numbered acts are played by the " +
                    "scene of the same number; only interludes and the two endings are meant to have none."));
            }

            if (ending && !numberless)
            {
                findings.Add(new Finding(
                    MessageType.Warning,
                    "An ending should have no act number. It is played in place of the rest of whatever " +
                    "scene the player was in when they earned it, not by a scene of its own."));
            }

            if (_act.ActNumber > 0)
            {
                List<ActAsset> all = StoryAssetBuilder.FindActs();

                for (int i = 0; i < all.Count; i++)
                {
                    if (all[i] != _act && all[i].ActNumber == _act.ActNumber)
                    {
                        findings.Add(new Finding(
                            MessageType.Error,
                            $"'{all[i].name}' also claims act {_act.ActNumber}. Only one of the two will ever be " +
                            "played, and which one is not defined."));
                    }
                }
            }

            if (_act.Count == 0)
            {
                findings.Add(new Finding(MessageType.Warning, "This act has no beats yet, so its scene shows a closing card and returns to the title."));
                return findings;
            }

            if (_act.Title.IsEmpty)
            {
                findings.Add(new Finding(MessageType.Warning, "This act has no title, so its card and its save slots have nothing to name it."));
            }

            int untranslatedPersian = 0;
            int untranslatedJapanese = 0;
            int measuredSilences = 0;
            int voicedLines = 0;
            int missingRecordings = 0;
            int cinemaDepth = 0;
            int cinemaChoices = 0;
            int cinemaSilences = 0;
            int missingFilms = 0;

            for (int i = 0; i < _act.Count; i++)
            {
                BeatData beat = _act.At(i);

                if (beat == null)
                {
                    findings.Add(new Finding(MessageType.Error, $"Beat {i:000} is empty.", i));
                    continue;
                }

                switch (beat.Kind)
                {
                    case StoryBeatKind.Line:
                        if (beat.MeasurePatience)
                        {
                            measuredSilences++;

                            if (cinemaDepth > 0)
                            {
                                cinemaSilences++;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(beat.Text.English))
                        {
                            findings.Add(new Finding(MessageType.Error, $"Beat {i:000} is a line with no English text. It will draw an empty box.", i));
                        }

                        if (string.IsNullOrWhiteSpace(beat.Text.Persian))
                        {
                            untranslatedPersian++;
                        }

                        if (string.IsNullOrWhiteSpace(beat.Text.Japanese))
                        {
                            untranslatedJapanese++;
                        }

                        if (!string.IsNullOrWhiteSpace(beat.VoiceClip))
                        {
                            voicedLines++;

                            if (!IsKnownVoice(beat.VoiceClip))
                            {
                                missingRecordings++;
                            }
                        }

                        break;

                    case StoryBeatKind.Background:
                        if (string.IsNullOrWhiteSpace(beat.Background))
                        {
                            findings.Add(new Finding(MessageType.Warning, $"Beat {i:000} changes the background to nothing, so nothing changes.", i));
                        }
                        else if (_sprites != null && _sprites.FindBackground(beat.Background) == null)
                        {
                            findings.Add(new Finding(
                                MessageType.Error,
                                $"Beat {i:000} asks for background '{beat.Background}', which is not in the library. " +
                                "The picture will not change. Check the spelling, or run Rebuild.",
                                i));
                        }

                        break;

                    case StoryBeatKind.Enter:
                        if (beat.Speaker == Speaker.Narrator || beat.Speaker == Speaker.Player)
                        {
                            findings.Add(new Finding(MessageType.Error, $"Beat {i:000} brings on {beat.Speaker}, who has no body on stage.", i));
                        }
                        else if (beat.Portrait == Portrait.Unchanged)
                        {
                            findings.Add(new Finding(
                                MessageType.Error,
                                $"Beat {i:000} brings on {beat.Speaker} with the face left unchanged, so nobody appears. " +
                                "Pick an expression.",
                                i));
                        }
                        else if (_sprites != null && _sprites.FindCharacter(beat.Speaker, beat.Portrait) == null)
                        {
                            findings.Add(new Finding(
                                MessageType.Error,
                                $"Beat {i:000} wants {beat.Speaker} {beat.Portrait}, and there is no sprite for it.",
                                i));
                        }

                        break;

                    case StoryBeatKind.Exit:
                        if (beat.Speaker == Speaker.Narrator || beat.Speaker == Speaker.Player)
                        {
                            findings.Add(new Finding(MessageType.Warning, $"Beat {i:000} takes {beat.Speaker} off stage, and they were never on it.", i));
                        }

                        break;

                    case StoryBeatKind.EnterCinema:
                        cinemaDepth++;
                        break;

                    case StoryBeatKind.ExitCinema:
                        cinemaDepth = 0;
                        break;

                    case StoryBeatKind.Video:
                        if (string.IsNullOrWhiteSpace(beat.Film))
                        {
                            findings.Add(new Finding(
                                MessageType.Warning,
                                $"Beat {i:000} plays a film with no name, so it does nothing.",
                                i));
                        }
                        else if (!FilmExists(beat.Film))
                        {
                            missingFilms++;
                        }

                        break;

                    case StoryBeatKind.Choice:
                        if (cinemaDepth > 0)
                        {
                            cinemaChoices++;
                        }

                        if (beat.Choices == null || beat.Choices.Length == 0)
                        {
                            findings.Add(new Finding(MessageType.Error, $"Beat {i:000} is a choice with no options. The game would wait forever.", i));
                        }
                        else if (beat.Choices.Length == 1)
                        {
                            findings.Add(new Finding(MessageType.Warning, $"Beat {i:000} is a choice with one option, which is not a choice.", i));
                        }

                        if (beat.YuaOverridesKindness && !HasTone(beat, ChoiceTone.Kind))
                        {
                            findings.Add(new Finding(
                                MessageType.Warning,
                                $"Beat {i:000} has Yua overruling the blue option, and there is no blue option " +
                                "to overrule. Nothing will happen.",
                                i));
                        }

                        break;

                    case StoryBeatKind.Interlude:
                        if (beat.Interlude == null)
                        {
                            findings.Add(new Finding(MessageType.Warning, $"Beat {i:000} plays an interlude but none is assigned, so it does nothing.", i));
                        }
                        else if (beat.Interlude == _act)
                        {
                            findings.Add(new Finding(MessageType.Error, $"Beat {i:000} plays this same act as an interlude, which never ends.", i));
                        }

                        break;

                    case StoryBeatKind.Music:
                        if (!string.IsNullOrWhiteSpace(beat.MusicTrack) && !IsKnownTrack(beat.MusicTrack))
                        {
                            findings.Add(new Finding(
                                MessageType.Warning,
                                $"Beat {i:000} plays music called '{beat.MusicTrack}', and no such file is in " +
                                "Assets/Audio. The act plays silently until one is added.",
                                i));
                        }

                        break;

                    case StoryBeatKind.Beat:
                        if (beat.Seconds <= 0f)
                        {
                            findings.Add(new Finding(MessageType.Warning, $"Beat {i:000} holds for zero seconds, so it is not a pause.", i));
                        }

                        break;
                }
            }

            if (measuredSilences == 0 && _act.ActNumber > 0 && _act.ActNumber < 5)
            {
                findings.Add(new Finding(
                    MessageType.Info,
                    "No line in this act is marked as one of the silences, so five minutes of patience " +
                    "cannot be earned anywhere in it. The design document puts one wherever Haru's leg " +
                    "stops him walking or the machine room reaches Yua."));
            }

            if (!string.IsNullOrWhiteSpace(_act.MusicTrack) && !IsKnownTrack(_act.MusicTrack))
            {
                findings.Add(new Finding(
                    MessageType.Warning,
                    $"The act's own music, '{_act.MusicTrack}', is not in Assets/Audio. The act plays silently."));
            }

            if (cinemaChoices > 0)
            {
                findings.Add(new Finding(
                    MessageType.Error,
                    $"{cinemaChoices} choice(s) are inside a stretch where the story plays itself. The " +
                    "player cannot press anything there, so the game would wait for a button that will " +
                    "never come. Move them, or add an ExitCinema first."));
            }

            if (cinemaSilences > 0)
            {
                findings.Add(new Finding(
                    MessageType.Warning,
                    $"{cinemaSilences} line(s) are marked as one of the silences inside a stretch where " +
                    "the story plays itself. Five minutes of patience means a player choosing not to " +
                    "press anything, and there is nothing to press — so it can never be earned there."));
            }

            if (cinemaDepth > 0)
            {
                findings.Add(new Finding(
                    MessageType.Info,
                    "This act ends while the story is still playing itself. That is fine — the mode " +
                    "belongs to the director and the next act starts with the player holding it again — " +
                    "and is worth knowing if it was not deliberate."));
            }

            if (missingFilms > 0)
            {
                findings.Add(new Finding(
                    MessageType.Info,
                    $"{missingFilms} Video beat(s) name a film that is not in Assets/Video or in " +
                    "StreamingAssets/Video yet. Those beats carry straight on, so the act plays " +
                    "correctly; drop the file in and it is picked up with no other change."));
            }

            if (missingRecordings > 0)
            {
                findings.Add(new Finding(
                    MessageType.Info,
                    $"{missingRecordings} of the {voicedLines} voiced line(s) have no recording in " +
                    "Assets/Audio/Voice yet. Those lines are read by the typewriter, so the act plays " +
                    "correctly; the names are reserved and the files can be added later without a rebuild."));
            }

            if (untranslatedPersian > 0)
            {
                findings.Add(new Finding(
                    MessageType.Info,
                    $"{untranslatedPersian} line(s) have no Persian yet. Those lines show the English instead."));
            }

            if (untranslatedJapanese > 0)
            {
                findings.Add(new Finding(
                    MessageType.Info,
                    $"{untranslatedJapanese} line(s) have no Japanese yet. Those lines show the English instead."));
            }

            return findings;
        }

        /// <summary>True when a choice offers at least one option of a tone.</summary>
        private static bool HasTone(BeatData beat, ChoiceTone tone)
        {
            if (beat.Choices == null)
            {
                return false;
            }

            for (int i = 0; i < beat.Choices.Length; i++)
            {
                if (beat.Choices[i].Tone == tone)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// True when a recording of that name exists, or when there is no voice
        /// library at all to check against.
        /// </summary>
        /// <remarks>
        /// The permissive answer for a project with no recordings is deliberate.
        /// A game whose script names two hundred clips and ships none of them is
        /// the normal state of this one for the foreseeable future, and a check
        /// tab that is two hundred lines of the same message is a check tab
        /// nobody reads.
        /// </remarks>
        /// <summary>
        /// True when a film of that name can be found, either way it can be
        /// supplied.
        /// </summary>
        private static bool FilmExists(string filmName)
        {
            if (string.IsNullOrWhiteSpace(filmName))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(
                    AssetPaths.FindPathByExactName("t:VideoClip " + filmName, filmName)))
            {
                return true;
            }

            string folder = System.IO.Path.Combine(
                Application.streamingAssetsPath, Presentation.VideoScreenView.StreamingFolder);

            string[] extensions = { ".mp4", ".webm", ".mov", ".m4v" };

            for (int i = 0; i < extensions.Length; i++)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(folder, filmName + extensions[i])))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsKnownVoice(string clipName)
        {
            if (string.IsNullOrWhiteSpace(clipName))
            {
                return true;
            }

            VoiceLibrary library = VoiceLibrary.Load();

            return library == null || library.Find(clipName) != null;
        }

        private bool IsKnownTrack(string trackName)
        {
            if (_musicNames == null)
            {
                return true;
            }

            for (int i = 0; i < _musicNames.Length; i++)
            {
                if (string.Equals(_musicNames[i], trackName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // ---------------------------------------------------------------------
        //  Guide tab
        // ---------------------------------------------------------------------

        /// <summary>
        /// How to write the game, in the window that writes it.
        /// </summary>
        /// <remarks>
        /// This existed as a markdown file next to the project, which is the
        /// wrong place: the question "what am I supposed to do with this window"
        /// is asked while looking at the window. It is written twice, in English
        /// and in Persian, because the person writing this game reads Persian —
        /// and the Persian half is drawn through the same shaper the game uses,
        /// which is the point being made.
        /// </remarks>
        private void DrawGuideTab()
        {
            _guideScroll = EditorGUILayout.BeginScrollView(_guideScroll);

            EditorGUILayout.Space(6f);
            EditorGUILayout.LabelField("Writing the game", EditorStyles.largeLabel);

            EditorGUILayout.HelpBox(PersianEditorText.Report(), MessageType.None);

            EditorGUILayout.LabelField(
                "An act is one asset. It holds the whole script of one chapter: every line, all three " +
                "translations of each, who says it, what face they wear, where the scene is, what it " +
                "sounds like and how long it holds. Nothing here needs C#.",
                EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space(10f);
            DrawGuideStep(
                "1 · Pick or make an act",
                "The dropdown at the top lists every act in the project. New Act starts one, already " +
                "numbered and titled. An act numbered 3 is played by the scene Act03.unity — that is the " +
                "whole of the connection between the two.");

            DrawGuideStep(
                "2 · Write it beat by beat",
                "The list on the left is the script, in order. The six buttons underneath it add the beats " +
                "an act is mostly made of; the dropdown on the right has all fifteen. Drag rows to reorder, " +
                "and read the sentence under the dropdown to see what the selected kind does.");

            DrawGuideStep(
                "3 · Three languages, one at a time",
                "The Language selector at the top right decides which translation the text boxes show. " +
                "Under every box are the other two languages as buttons, showing whether they are empty; " +
                "press one to switch to it. A line with no translation falls back to English at runtime " +
                "rather than showing a gap.");

            DrawGuideStep(
                "4 · Persian",
                "Type it normally, left to right, exactly as you would anywhere else. The box you type in " +
                "shows the letters unjoined — a text field can only edit text in the order it is stored — " +
                "and the grey line underneath shows the same text as the game will actually draw it: " +
                "joined, and right to left.");

            DrawGuideStep(
                "5 · The Stage tab",
                "Where the characters stand and how the picture is framed, with a real 16:9 preview that " +
                "uses the same arithmetic the game does. If a scene has character sprites placed in it by " +
                "hand — as Act01 does — the game takes their positions from the scene and these numbers " +
                "are only the fallback for scenes that do not.");

            DrawGuideStep(
                "6 · The Check tab",
                "Lists everything in this act that will quietly misbehave: a background whose name does not " +
                "match a file, a choice with one option, a line with no text. None of these crash, which is " +
                "why they are worth reading.");

            EditorGUILayout.Space(14f);
            EditorGUILayout.LabelField("راهنما", EditorStyles.boldLabel);

            DrawPersianGuideLine(
                "هر پرده یک فایل است. تمام دیالوگ‌ها، هر سه ترجمه، گوینده، حالت چهره، مکان و صدا داخل همان فایل است. هیچ کدی لازم نیست.");

            DrawPersianGuideLine(
                "۱ - از فهرست بالا یک پرده انتخاب کن یا با دکمه New Act یکی بساز. پرده شماره ۳ داخل صحنه Act03.unity اجرا میشود.");

            DrawPersianGuideLine(
                "۲ - ستون چپ خود متن نمایشنامه است. با شش دکمه زیر آن، بیت جدید اضافه کن و با کشیدن، ترتیب را عوض کن.");

            DrawPersianGuideLine(
                "۳ - زبان را از بالا سمت راست عوض کن. زیر هر کادر متن، دو زبان دیگر به شکل دکمه هستند.");

            DrawPersianGuideLine(
                "۴ - فارسی را عادی بنویس. کادر ویرایش حروف را جدا نشان میدهد چون فقط به همان ترتیب قابل ویرایش است، و خط خاکستری زیرش همان متن را همانطور که بازی نشان میدهد میچسباند و راست به چپ میکند.");

            DrawPersianGuideLine(
                "۵ - تب Stage جای کاراکترها و قاب بازی است، با یک پیش‌نمایش واقعی ۱۶:۹. اگر داخل خود صحنه کاراکتر گذاشته باشی، بازی جای همان‌ها را میخواند.");

            DrawPersianGuideLine(
                "۶ - تب Check هر چیزی را که بی‌سر و صدا خراب میشود فهرست میکند: اسم بک‌گراند اشتباه، انتخاب با یک گزینه، خط بدون متن.");

            EditorGUILayout.Space(16f);
            EditorGUILayout.EndScrollView();
        }

        private static void DrawGuideStep(string title, string body)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
                EditorGUILayout.LabelField(body, EditorStyles.wordWrappedLabel);
            }
        }

        private static void DrawPersianGuideLine(string persian)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField(
                    PersianEditorText.Display(persian),
                    PersianEditorText.MiniLabelStyle,
                    GUILayout.MinHeight(34f));
            }
        }

        // ---------------------------------------------------------------------
        //  Stage tab
        // ---------------------------------------------------------------------

        private void DrawStageTab()
        {
            if (_settings == null)
            {
                _settings = StoryAssetBuilder.EnsureStageSettings();
                _settingsSerialized = _settings != null ? new SerializedObject(_settings) : null;
            }

            if (_settingsSerialized == null)
            {
                EditorGUILayout.HelpBox("Stage settings could not be created.", MessageType.Error);
                return;
            }

            _settingsSerialized.Update();
            _stageScroll = EditorGUILayout.BeginScrollView(_stageScroll);

            EditorGUILayout.HelpBox(
                "Where the characters stand. Feet Y is how far their feet sit above the bottom of the " +
                "screen — negative pushes them down so a full-body sprite is cropped at the shin " +
                "instead of floating. The preview below is the real 16:9 frame.\n\n" +
                "A scene that has character sprites placed in it by hand overrides all of this: the game " +
                "reads their position and size straight out of the scene, so what you arrange in the " +
                "scene view is what it draws. These numbers are what an act with no such sprites uses.",
                MessageType.Info);

            EditorGUILayout.LabelField(AnchorReport(), EditorStyles.miniLabel);

            SerializedProperty placements = _settingsSerialized.FindProperty("_placements");
            EditorGUILayout.PropertyField(placements, new GUIContent("Characters"), true);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(
                        new GUIContent(
                            "Take the placement from this scene",
                            "Measure the Yua and Haru sprites in the open scene and record where they " +
                            "stand here, for every act."),
                        GUILayout.Height(24f)))
                {
                    int adopted = ActSceneSetup.AdoptSceneCharacters();

                    if (adopted == 0)
                    {
                        Debug.LogWarning(
                            "[Stage] No character sprites were found in the open scene. Drag a Yua… and a " +
                            "Haru… sprite into it, put them where they belong, and press this again.");
                    }

                    _settingsSerialized = new SerializedObject(_settings);
                }

                if (GUILayout.Button(
                        new GUIContent(
                            "Reset to the anchors",
                            "Put both characters back on the anchors the game is designed around, and move " +
                            "any marker sprites in the open scene onto them too."),
                        GUILayout.Width(150f),
                        GUILayout.Height(24f)))
                {
                    ActSceneSetup.ResetCharacterPlacement();

                    EditorUtility.SetDirty(_settings);
                    AssetDatabase.SaveAssets();

                    _settingsSerialized = new SerializedObject(_settings);
                }
            }

            EditorGUILayout.HelpBox(
                "These numbers are the game's, not this scene's. Every act uses them, and an act with no " +
                "character sprites in it — which is every act you make from now on — needs no setting up " +
                "at all.",
                MessageType.None);

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Veil", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(
                "The soft wash the picture is seen through. It is the design document's pane of frosted " +
                "glass: acts one to four wear it and act five takes it away, which is what that act means " +
                "by the graphics becoming real.\n\n" +
                "If the game looks slightly out of focus to you, this is what you are seeing. Turn Veil " +
                "Enabled off for a sharp game from the first frame — act five's Grade beats then simply " +
                "have nothing to remove, and nothing else changes. The preview below shows it.",
                MessageType.Info);

            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("VeilEnabled"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("VeilStrength"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("VeilColour"));

            EditorGUILayout.Space(6f);
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("InactiveTint"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("InactiveScale"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("EntranceSlide"));

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Frame", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(
                "The frame is a border on all four sides, and the two numbers below are how thick it is, " +
                "in the same canvas units as everything else on this tab — the screen is 1920 × 1080. " +
                "What aspect the picture ends up at is whatever falls out of them.",
                MessageType.None);

            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameEnabled"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameBorderX"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameBorderY"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameColour"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameOpenSeconds"));

            EditorGUILayout.LabelField(
                FrameReport(_settings),
                EditorStyles.miniLabel);

            EditorGUILayout.Space(12f);
            DrawStagePreview();

            EditorGUILayout.EndScrollView();
            _settingsSerialized.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws the stage exactly as the game lays it out.
        /// </summary>
        /// <remarks>
        /// The same arithmetic the runtime uses, at a smaller scale: anchored to
        /// the bottom edge, positioned by offset and feet height, sized by
        /// height and the sprite's own aspect. Getting the placement right by
        /// typing numbers and pressing play is miserable; getting it right by
        /// looking at it takes seconds.
        /// </remarks>
        private void DrawStagePreview()
        {
            const float previewWidth = 640f;
            float previewHeight = previewWidth * 9f / 16f;

            Rect frame = GUILayoutUtility.GetRect(previewWidth, previewHeight, GUILayout.ExpandWidth(false));
            EditorGUI.DrawRect(frame, new Color(0.12f, 0.12f, 0.14f, 1f));

            float scale = previewWidth / 1920f;

            Sprite background = _sprites != null ? _sprites.FindBackground(FirstBackgroundOfAct()) : null;
            if (background != null)
            {
                DrawSprite(frame, background);
            }

            DrawCharacterPreview(frame, scale, Speaker.Yua);
            DrawCharacterPreview(frame, scale, Speaker.Haru);

            // Over the scenery and the characters, under the frame and the box —
            // exactly where the game puts it, so what this preview shows is the
            // amount of softening the player actually gets.
            if (_settings.VeilEnabled && _settings.VeilStrength > 0f)
            {
                Color veil = _settings.VeilColour;

                EditorGUI.DrawRect(
                    frame,
                    new Color(veil.r, veil.g, veil.b, veil.a * Mathf.Clamp01(_settings.VeilStrength)));
            }

            // The same two numbers the runtime frame is built from, so what is
            // drawn here is what the game shows rather than an approximation of
            // it.
            float barX = _settings.FrameEnabled ? _settings.FrameBorderX * scale : 0f;
            float barY = _settings.FrameEnabled ? _settings.FrameBorderY * scale : 0f;

            if (_settings.FrameEnabled)
            {
                EditorGUI.DrawRect(new Rect(frame.x, frame.y, barX, frame.height), _settings.FrameColour);
                EditorGUI.DrawRect(new Rect(frame.xMax - barX, frame.y, barX, frame.height), _settings.FrameColour);
                EditorGUI.DrawRect(new Rect(frame.x + barX, frame.y, frame.width - barX * 2f, barY), _settings.FrameColour);
                EditorGUI.DrawRect(new Rect(frame.x + barX, frame.yMax - barY, frame.width - barX * 2f, barY), _settings.FrameColour);
            }

            // The dialogue box, inside the frame exactly as the game puts it, so
            // the crop can be judged against the thing that actually covers the
            // characters' legs in play.
            float boxHeight = 300f * scale;
            float boxMargin = 30f * scale;
            float boxSide = 40f * scale;

            Rect box = new Rect(
                frame.x + barX + boxSide,
                frame.yMax - barY - boxMargin - boxHeight,
                frame.width - (barX + boxSide) * 2f,
                boxHeight);

            EditorGUI.DrawRect(box, new Color(1f, 0.976f, 0.984f, 0.55f));

            EditorGUILayout.LabelField(
                "Preview shows the first background in this act, both characters, the frame and the dialogue box.",
                EditorStyles.miniLabel);
        }

        /// <summary>
        /// The three numbers a character is placed by, in the scene view's own
        /// terms.
        /// </summary>
        /// <remarks>
        /// The fields above are canvas units, which is what the stage draws in
        /// and nothing anybody arranges a scene in. Printing the world position
        /// and the scale beside them is what makes "put the marker back where it
        /// belongs" a thing you can do by typing three numbers into the
        /// Inspector rather than by working backwards through the conversion.
        /// </remarks>
        private static string AnchorReport()
        {
            return
                "The marker sprites in Act01 are the reference: " +
                $"Yua at X {StageSettings.Placement.YuaAnchorX:0.00}, " +
                $"Y {StageSettings.Placement.AnchorY:0.00}, scale {StageSettings.Placement.YuaScale:0.00} · " +
                $"Haru at X {StageSettings.Placement.HaruAnchorX:0.00}, " +
                $"Y {StageSettings.Placement.AnchorY:0.00}, scale {StageSettings.Placement.HaruScale:0.00}. " +
                $"That is {StageSettings.Placement.HeightFor(Speaker.Yua):0} and " +
                $"{StageSettings.Placement.HeightFor(Speaker.Haru):0} canvas units tall.";
        }

        /// <summary>
        /// What the border leaves behind, said in the terms the border is not
        /// written in.
        /// </summary>
        /// <remarks>
        /// The thicknesses are the handle, but the aspect is what somebody
        /// comparing this to a film or to another visual novel is thinking in.
        /// Printing it costs one line and saves the arithmetic.
        /// </remarks>
        private static string FrameReport(StageSettings settings)
        {
            if (settings == null || !settings.FrameEnabled)
            {
                return "No frame: the picture is the whole 1920 × 1080 window.";
            }

            float width = Mathf.Max(1f, 1920f - settings.FrameBorderX * 2f);
            float height = Mathf.Max(1f, 1080f - settings.FrameBorderY * 2f);

            return $"The picture inside the frame is {width:0} × {height:0} — an aspect of {width / height:0.00}.";
        }

        private void DrawCharacterPreview(Rect frame, float scale, Speaker speaker)
        {
            StageSettings.Placement placement = _settings.PlacementFor(speaker);
            Sprite sprite = _sprites != null ? _sprites.FindCharacter(speaker, Portrait.Neutral) : null;

            if (sprite == null)
            {
                return;
            }

            float height = placement.Height * scale;
            float width = height * (sprite.rect.width / sprite.rect.height);

            // y grows downward in GUI space and upward in canvas space, so the
            // feet offset is subtracted from the bottom edge rather than added.
            float bottom = frame.yMax - placement.FeetY * scale;
            float centreX = frame.x + frame.width * 0.5f + placement.OffsetX * scale;

            Rect rect = new Rect(centreX - width * 0.5f, bottom - height, width, height);

            GUI.BeginClip(frame);
            Rect local = new Rect(rect.x - frame.x, rect.y - frame.y, rect.width, rect.height);
            DrawSprite(local, sprite);
            GUI.EndClip();
        }

        private string FirstBackgroundOfAct()
        {
            if (_act == null)
            {
                return null;
            }

            for (int i = 0; i < _act.Count; i++)
            {
                BeatData beat = _act.At(i);

                if (beat != null && beat.Kind == StoryBeatKind.Background && !string.IsNullOrEmpty(beat.Background))
                {
                    return beat.Background;
                }
            }

            return null;
        }

        // ---------------------------------------------------------------------
        //  Shared drawing
        // ---------------------------------------------------------------------

        /// <summary>
        /// Draws one language of a <see cref="LocalizedLine"/>, with the other
        /// two a click away.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Showing all three at once was the first thing tried and it is worse:
        /// three tall boxes per beat pushes everything else off the screen, and
        /// nobody edits three languages in the same sitting anyway.
        /// </para>
        /// <para>
        /// Persian gets a second box under the first. The editable one has to
        /// hold the text in the order it is stored, or the caret moves the wrong
        /// way and backspace deletes the wrong letter; the read-only one shows
        /// the line joined and in reading order, which is the only way to check
        /// a Persian translation without launching the game.
        /// </para>
        /// </remarks>
        private void DrawLocalized(SerializedProperty line, string label, int rows)
        {
            SerializedProperty field = line.FindPropertyRelative(_language.ToString());
            string value = field.stringValue ?? string.Empty;

            EditorGUILayout.LabelField($"{label} — {_language}", EditorStyles.boldLabel);

            bool persian = _language == GameLanguage.Persian;

            field.stringValue = persian
                ? EditorGUILayout.TextArea(
                    value,
                    PersianEditorText.TextAreaStyle,
                    GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * rows))
                : EditorGUILayout.TextArea(
                    value,
                    GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * rows));

            if (persian && !string.IsNullOrEmpty(field.stringValue))
            {
                EditorGUILayout.LabelField("As the game draws it", EditorStyles.miniLabel);

                EditorGUILayout.SelectableLabel(
                    PersianEditorText.Display(field.stringValue),
                    PersianEditorText.PreviewStyle,
                    GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * Mathf.Max(2, rows - 1)));
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (GameLanguage other in new[] { GameLanguage.English, GameLanguage.Japanese, GameLanguage.Persian })
                {
                    if (other == _language)
                    {
                        continue;
                    }

                    string preview = line.FindPropertyRelative(other.ToString()).stringValue;
                    string state = string.IsNullOrEmpty(preview) ? "empty" : Shorten(preview, 40);

                    if (other == GameLanguage.Persian && !string.IsNullOrEmpty(preview))
                    {
                        state = PersianEditorText.Display(state);
                    }

                    if (GUILayout.Button($"{other}: {state}", EditorStyles.miniButton))
                    {
                        _language = other;
                        GUI.FocusControl(null);
                    }
                }
            }
        }

        private static void ClearLocalized(SerializedProperty line)
        {
            if (line == null)
            {
                return;
            }

            line.FindPropertyRelative("English").stringValue = string.Empty;
            line.FindPropertyRelative("Japanese").stringValue = string.Empty;
            line.FindPropertyRelative("Persian").stringValue = string.Empty;
        }

        private string LocalizedValue(SerializedProperty line)
        {
            SerializedProperty field = line?.FindPropertyRelative(_language.ToString());
            string value = field != null ? field.stringValue : null;

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            // Fall back to English in the list so an untranslated act still reads
            // as a script rather than as a column of blank rows.
            return line?.FindPropertyRelative("English")?.stringValue ?? string.Empty;
        }

        private void DrawTrackField(SerializedProperty track, string label)
        {
            DrawNamePopup(track, label, _musicNames);
        }

        /// <summary>
        /// A dropdown of known names that still accepts a name that is not in
        /// the list.
        /// </summary>
        /// <remarks>
        /// The free-text box matters: a track can be referenced before its file
        /// exists — which is exactly what act one's music is doing — and a
        /// dropdown alone would make that impossible to type.
        /// </remarks>
        private static void DrawNamePopup(SerializedProperty property, string label, string[] names)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                property.stringValue = EditorGUILayout.TextField(label, property.stringValue);

                if (names == null || names.Length == 0)
                {
                    return;
                }

                int index = System.Array.IndexOf(names, property.stringValue);
                int picked = EditorGUILayout.Popup(index, names, GUILayout.Width(30f));

                if (picked >= 0 && picked < names.Length)
                {
                    property.stringValue = picked == 0 ? string.Empty : names[picked];
                }
            }
        }

        private void DrawPortraitPreview(SerializedProperty beat)
        {
            Speaker speaker = (Speaker)beat.FindPropertyRelative("Speaker").enumValueIndex;
            Portrait portrait = (Portrait)beat.FindPropertyRelative("Portrait").enumValueIndex;

            if (_sprites == null || portrait == Portrait.Unchanged || speaker == Speaker.Narrator)
            {
                return;
            }

            Sprite sprite = _sprites.FindCharacter(speaker, portrait);
            if (sprite == null)
            {
                return;
            }

            Rect rect = GUILayoutUtility.GetRect(90f, 180f, GUILayout.ExpandWidth(false));
            DrawSprite(rect, sprite);
        }

        /// <summary>Draws a sprite into a rect, letterboxed to keep its aspect.</summary>
        private static void DrawSprite(Rect rect, Sprite sprite)
        {
            if (sprite == null || sprite.texture == null)
            {
                return;
            }

            Rect tex = sprite.textureRect;
            Rect coords = new Rect(
                tex.x / sprite.texture.width,
                tex.y / sprite.texture.height,
                tex.width / sprite.texture.width,
                tex.height / sprite.texture.height);

            float aspect = tex.width / tex.height;
            float drawWidth = rect.width;
            float drawHeight = drawWidth / aspect;

            if (drawHeight > rect.height)
            {
                drawHeight = rect.height;
                drawWidth = drawHeight * aspect;
            }

            Rect fitted = new Rect(
                rect.x + (rect.width - drawWidth) * 0.5f,
                rect.y + (rect.height - drawHeight) * 0.5f,
                drawWidth,
                drawHeight);

            GUI.DrawTextureWithTexCoords(fitted, sprite.texture, coords, true);
        }

        // ---------------------------------------------------------------------
        //  Plumbing
        // ---------------------------------------------------------------------

        private void SelectAct(ActAsset act)
        {
            _act = act;

            if (_act == null)
            {
                _serialized = null;
                _list = null;
                return;
            }

            _serialized = new SerializedObject(_act);
            BuildList();
            Repaint();
        }

        private void RefreshNameLists()
        {
            List<string> backgrounds = new List<string> { NoneLabel };
            if (_sprites != null)
            {
                foreach (StageSpriteLibrary.Entry entry in _sprites.BackgroundEntries)
                {
                    backgrounds.Add(entry.Name);
                }
            }

            _backgroundNames = backgrounds.ToArray();

            List<string> music = new List<string> { NoneLabel };
            foreach (string guid in AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Audio" }))
            {
                music.Add(System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)));
            }

            _musicNames = music.ToArray();
        }

        /// <summary>
        /// Plays one of the generated sounds without entering play mode.
        /// </summary>
        /// <remarks>
        /// The sound is synthesised on the spot if it has not been already —
        /// none of them are files, so there is nothing to import and nothing to
        /// wait for. Reaching the Editor's own preview is <see cref="SoundPreview"/>'s
        /// problem.
        /// </remarks>
        private static void PreviewSound(SfxId id)
        {
            AudioClip clip = ProceduralSfxLibrary.Get(id);

            if (clip == null)
            {
                Debug.LogWarning($"[Story] The sound '{id}' could not be synthesised.");
                return;
            }

            SoundPreview.Play(clip);
        }

        // Helper methods for UI text formatting
        private static string Shorten(string text, int maxLength = 25)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.Replace("\n", " ").Replace("\r", "");
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }

        private static string Or(string text, string fallback)
        {
            return string.IsNullOrWhiteSpace(text) ? fallback : text;
        }
    }
}
