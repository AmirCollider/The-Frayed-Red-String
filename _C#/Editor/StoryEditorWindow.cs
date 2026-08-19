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
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
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
            Stage
        }

        private const float ListWidth = 340f;
        private const string NoneLabel = "— none —";

        // Colours for the beat list, so a script can be read at a glance by
        // shape rather than by reading every row.
        private static readonly Color LineTint = new Color(1f, 1f, 1f, 0f);
        private static readonly Color StageTint = new Color(0.45f, 0.70f, 1f, 0.16f);
        private static readonly Color SoundTint = new Color(1f, 0.80f, 0.35f, 0.16f);
        private static readonly Color FlowTint = new Color(0.75f, 0.45f, 1f, 0.16f);

        private ActAsset _act;
        private SerializedObject _serialized;
        private ReorderableList _list;

        private Tab _tab = Tab.Script;
        private GameLanguage _language = GameLanguage.English;
        private Vector2 _detailScroll;
        private Vector2 _stageScroll;

        private StageSpriteLibrary _sprites;
        private StageSettings _settings;
        private SerializedObject _settingsSerialized;

        private string[] _backgroundNames;
        private string[] _musicNames;

        [MenuItem("The Frayed Red String/Story Editor")]
        public static void Open()
        {
            StoryEditorWindow window = GetWindow<StoryEditorWindow>("Story Editor");
            window.minSize = new Vector2(900f, 520f);
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

            if (_tab == Tab.Stage)
            {
                DrawStageTab();
                return;
            }

            if (_act == null)
            {
                DrawEmptyState();
                return;
            }

            _serialized.Update();

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(ListWidth)))
                {
                    DrawActHeader();
                    DrawBeatList();
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
                _tab = (Tab)GUILayout.Toolbar((int)_tab, new[] { "Script", "Stage" },
                    EditorStyles.toolbarButton, GUILayout.Width(140f));

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

                if (GUILayout.Button("New Act", EditorStyles.toolbarButton, GUILayout.Width(70f)))
                {
                    CreateActMenu(acts);
                }

                GUILayout.FlexibleSpace();

                // The language selector changes which translation the text
                // fields show, so a translator can work down a whole act in one
                // language without three boxes fighting for the same space.
                GUILayout.Label("Language", EditorStyles.miniLabel);
                _language = (GameLanguage)EditorGUILayout.EnumPopup(
                    _language, EditorStyles.toolbarPopup, GUILayout.Width(90f));

                if (GUILayout.Button("Rebuild", EditorStyles.toolbarButton, GUILayout.Width(60f)))
                {
                    StoryAssetBuilder.Rebuild();
                    _sprites = StageSpriteLibrary.Load();
                    RefreshNameLists();
                }
            }
        }

        private void DrawEmptyState()
        {
            EditorGUILayout.Space(40f);

            EditorGUILayout.HelpBox(
                "There are no acts yet.\n\n" +
                "If act one is still written in C#, import it — every line and all three " +
                "translations come across exactly as they are. Otherwise start a new act.",
                MessageType.Info);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Import Act 01 From Code", GUILayout.Width(220f), GUILayout.Height(30f)))
                {
                    SelectAct(ActLibrary.Find(1));
                }

                if (GUILayout.Button("New Empty Act", GUILayout.Width(160f), GUILayout.Height(30f)))
                {
                    SelectAct(StoryAssetBuilder.CreateAct(1, "Act01"));
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
                EditorGUILayout.PropertyField(_serialized.FindProperty("ActNumber"));
                EditorGUILayout.PropertyField(_serialized.FindProperty("ShowTitleCard"));

                DrawLocalized(_serialized.FindProperty("Title"), "Act title", 1);
                DrawTrackField(_serialized.FindProperty("MusicTrack"), "Act music");

                EditorGUILayout.LabelField(
                    $"{_act.Count} beats · {_act.SpokenCount()} spoken lines",
                    EditorStyles.miniLabel);
            }
        }

        private void DrawBeatList()
        {
            _list.DoLayoutList();
        }

        private void BuildList()
        {
            SerializedProperty beats = _serialized.FindProperty("Beats");

            _list = new ReorderableList(_serialized, beats, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Script"),
                elementHeight = EditorGUIUtility.singleLineHeight + 6f
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
                EditorGUI.LabelField(label, SummaryOf(beat, kind));
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
                    return SoundTint;

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
                    return who + Shorten(text);
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
                    return "▭ " + Shorten(LocalizedValue(beat.FindPropertyRelative("Caption")));

                case StoryBeatKind.TitleCard:
                    return "★ title card";

                case StoryBeatKind.Sound:
                    return "♪ " + (SfxId)beat.FindPropertyRelative("Sound").enumValueIndex;

                case StoryBeatKind.Music:
                    return "♫ " + Or(beat.FindPropertyRelative("MusicTrack").stringValue, "stop the music");

                case StoryBeatKind.Beat:
                    return $"⏸ hold {beat.FindPropertyRelative("Seconds").floatValue:0.0}s";

                case StoryBeatKind.Choice:
                    return $"◆ choice ({beat.FindPropertyRelative("Choices").arraySize} options)";

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

                default:
                    return "■ end of act";
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
                    EditorGUILayout.LabelField("Select a beat on the left.", EditorStyles.centeredGreyMiniLabel);
                    return;
                }

                _detailScroll = EditorGUILayout.BeginScrollView(_detailScroll);

                SerializedProperty beat = beats.GetArrayElementAtIndex(_list.index);
                SerializedProperty kindProp = beat.FindPropertyRelative("Kind");
                StoryBeatKind kind = (StoryBeatKind)kindProp.enumValueIndex;

                EditorGUILayout.PropertyField(kindProp, new GUIContent("This beat"));
                EditorGUILayout.Space(6f);

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
                }

                if (kind != StoryBeatKind.Music)
                {
                    EditorGUILayout.Space(8f);
                    DrawSoundBlock(beat, kind == StoryBeatKind.Sound);
                }

                EditorGUILayout.Space(8f);
                EditorGUILayout.PropertyField(beat.FindPropertyRelative("Note"), new GUIContent("Note to self"));

                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawLineBeat(SerializedProperty beat)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(beat.FindPropertyRelative("Speaker"), GUILayout.Width(240f));
                EditorGUILayout.PropertyField(beat.FindPropertyRelative("Portrait"), GUILayout.Width(240f));
                GUILayout.FlexibleSpace();
            }

            DrawPortraitPreview(beat);
            EditorGUILayout.Space(6f);
            DrawLocalized(beat.FindPropertyRelative("Text"), "Line", 4);
        }

        private void DrawEnterBeat(SerializedProperty beat)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(beat.FindPropertyRelative("Speaker"), GUILayout.Width(240f));
                EditorGUILayout.PropertyField(beat.FindPropertyRelative("Portrait"), GUILayout.Width(240f));
                GUILayout.FlexibleSpace();
            }

            DrawPortraitPreview(beat);
        }

        private static void DrawSpeakerOnly(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(beat.FindPropertyRelative("Speaker"), GUILayout.Width(240f));
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
                        EditorGUILayout.PropertyField(choice.FindPropertyRelative("Tone"), GUILayout.Width(240f));
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
        }

        private static void DrawInterludeBeat(SerializedProperty beat)
        {
            EditorGUILayout.PropertyField(beat.FindPropertyRelative("Interlude"), new GUIContent("Plays"));
            EditorGUILayout.PropertyField(beat.FindPropertyRelative("Chance"), new GUIContent("Chance"));

            EditorGUILayout.HelpBox(
                "An interlude is just another act with no number. Set the chance below 1 for the " +
                "recurring beats the design document wants to turn up at random.",
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

                if (GUILayout.Button("▶", GUILayout.Width(28f)))
                {
                    PreviewSound((SfxId)sound.enumValueIndex);
                }

                GUILayout.FlexibleSpace();
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
                "instead of floating. The preview below is the real 16:9 frame.",
                MessageType.Info);

            SerializedProperty placements = _settingsSerialized.FindProperty("_placements");
            EditorGUILayout.PropertyField(placements, new GUIContent("Characters"), true);

            EditorGUILayout.Space(6f);
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("InactiveTint"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("InactiveScale"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("EntranceSlide"));

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Frame", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameEnabled"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameAspect"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameColour"));
            EditorGUILayout.PropertyField(_settingsSerialized.FindProperty("FrameOpenSeconds"));

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

            if (_settings.FrameEnabled)
            {
                float closed = Mathf.Clamp01((1f - _settings.FrameAspect / (16f / 9f)) * 0.5f);
                float bar = frame.width * closed;

                EditorGUI.DrawRect(new Rect(frame.x, frame.y, bar, frame.height), _settings.FrameColour);
                EditorGUI.DrawRect(new Rect(frame.xMax - bar, frame.y, bar, frame.height), _settings.FrameColour);
            }

            // The dialogue box, so the crop can be judged against the thing that
            // actually covers the characters' legs in play.
            float boxHeight = 300f * scale;
            float boxMargin = 46f * scale;
            Rect box = new Rect(
                frame.x + (frame.width - 1720f * scale) * 0.5f,
                frame.yMax - boxMargin - boxHeight,
                1720f * scale,
                boxHeight);

            EditorGUI.DrawRect(box, new Color(1f, 0.976f, 0.984f, 0.55f));

            EditorGUILayout.LabelField(
                "Preview shows the first background in this act, both characters, the frame and the dialogue box.",
                EditorStyles.miniLabel);
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
        /// Showing all three at once was the first thing tried and it is worse:
        /// three tall boxes per beat pushes everything else off the screen, and
        /// nobody edits three languages in the same sitting anyway.
        /// </remarks>
        private void DrawLocalized(SerializedProperty line, string label, int rows)
        {
            SerializedProperty field = line.FindPropertyRelative(_language.ToString());

            EditorGUILayout.LabelField($"{label} — {_language}", EditorStyles.boldLabel);
            field.stringValue = EditorGUILayout.TextArea(
                field.stringValue ?? string.Empty,
                GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * rows));

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

                    if (GUILayout.Button($"{other}: {state}", EditorStyles.miniButton))
                    {
                        _language = other;
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
        /// The editor's clip preview is internal, so it is reached by reflection
        /// — and wrapped, because an internal method is exactly the kind of thing
        /// that gets renamed between Unity versions. If it ever disappears the
        /// button quietly does nothing instead of throwing on every repaint.
        /// </remarks>
        private static void PreviewSound(SfxId id)
        {
            AudioClip clip = ProceduralSfxLibrary.Get(id);
            if (clip == null)
            {
                return;
            }

            System.Type util = typeof(EditorApplication).Assembly.GetType("UnityEditor.AudioUtil");

            MethodInfo play = util?.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null);

            if (play == null)
            {
                Debug.Log($"[Story] Preview of {id} is not available in this Unity version.");
                return;
            }

            play.Invoke(null, new object[] { clip, 0, false });
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
