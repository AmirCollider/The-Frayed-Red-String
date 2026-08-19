// -----------------------------------------------------------------------------
//  The Frayed Red String
//  PersianEditorText.cs  (Editor only)
//
//  Persian that can be read inside an Editor window.
//
//  The game's Persian is drawn by UnityDirectTMP, which joins the letters and
//  puts the line in reading order using the font's own OpenType tables. None of
//  that reaches an EditorWindow: IMGUI is not TextMeshPro, it has no shaper, and
//  it draws a string exactly as the characters are stored — so a Persian line
//  typed into the Story Editor came out as separate letters running the wrong
//  way, in a font with no Arabic glyphs at all, which is to say as a row of
//  boxes.
//
//  This is the same job done twice over for IMGUI: find a font that can draw
//  Arabic letters, and hand it text that has already been shaped and reordered.
//  The package does the second half — DirectTMP.Prepare is documented as the
//  entry point "for text going somewhere that is not a TextMeshPro label", which
//  is precisely here.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityDirectTMP;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Draws Persian, in an Editor window, so that it reads.</summary>
    public static class PersianEditorText
    {
        /// <summary>
        /// One joined shape Unicode did assign a codepoint to: the initial lam.
        /// </summary>
        /// <remarks>
        /// Used to ask a font whether it carries the legacy presentation forms.
        /// A font that does can draw joined text; a font that does not can still
        /// draw the letters, just unjoined — a real difference in how readable
        /// the result is, and worth knowing before deciding what to hand it.
        /// </remarks>
        private const char JoinedProbe = 'ﻟ';

        /// <summary>A plain Persian letter, to ask whether a font has Arabic at all.</summary>
        private const char LetterProbe = 'م';

        /// <summary>
        /// Fonts installed on the machine that are known to carry Arabic, in the
        /// order they are worth trying.
        /// </summary>
        /// <remarks>
        /// Reached for only when the project itself has no font that can draw
        /// this text. All four ship with Windows and carry the presentation
        /// forms; Tahoma is first because it was drawn for exactly this job and
        /// is the most legible of them at Editor sizes.
        /// </remarks>
        private static readonly string[] SystemFontNames =
        {
            "Tahoma", "Segoe UI", "Arial", "Times New Roman"
        };

        private static Font _font;
        private static bool _searched;
        private static bool _joins;

        private static GUIStyle _label;
        private static GUIStyle _preview;
        private static GUIStyle _textArea;
        private static GUIStyle _miniLabel;

        /// <summary>
        /// A font that can draw Persian, or <c>null</c> if none was found.
        /// </summary>
        public static Font Font
        {
            get
            {
                Search();
                return _font;
            }
        }

        /// <summary>
        /// True when the chosen font carries joined shapes, so words can be
        /// drawn connected rather than letter by letter.
        /// </summary>
        public static bool Joins
        {
            get
            {
                Search();
                return _joins;
            }
        }

        /// <summary>True when this text has Arabic script in it.</summary>
        public static bool IsArabicScript(string text)
        {
            return !string.IsNullOrEmpty(text)
                   && (DirectJoining.NeedsShaping(text) || DirectBidi.ContainsRightToLeft(text));
        }

        /// <summary>
        /// The same text, ready to be drawn by IMGUI: joined where the font can
        /// join, and in reading order either way.
        /// </summary>
        /// <remarks>
        /// Reordering is done even when joining is not. Unjoined Persian is
        /// awkward to read; unjoined Persian running backwards is not Persian at
        /// all, and the difference between the two is the difference between
        /// checking a translation and guessing at it.
        /// </remarks>
        public static string Display(string text)
        {
            if (string.IsNullOrEmpty(text) || !IsArabicScript(text))
            {
                return text;
            }

            Search();

            // Every paragraph is reordered on its own. Bidi works a paragraph at
            // a time, and a line break carried through a reorder ends up at the
            // wrong end of the text — where IMGUI then breaks the line.
            string[] paragraphs = DirectTMP.Paragraphs(text);

            for (int i = 0; i < paragraphs.Length; i++)
            {
                paragraphs[i] = _joins
                    ? DirectTMP.Prepare(paragraphs[i])
                    : DirectBidi.Reorder(paragraphs[i]);
            }

            return string.Join("\n", paragraphs);
        }

        /// <summary>Right-aligned body text in the Persian font.</summary>
        public static GUIStyle PreviewStyle
        {
            get
            {
                Search();

                if (_preview == null)
                {
                    _preview = new GUIStyle(EditorStyles.textArea)
                    {
                        alignment = TextAnchor.UpperRight,
                        richText = false,
                        wordWrap = true,
                        fontSize = 14
                    };
                }

                _preview.font = _font;
                return _preview;
            }
        }

        /// <summary>A label in the Persian font, right aligned.</summary>
        public static GUIStyle LabelStyle
        {
            get
            {
                Search();

                if (_label == null)
                {
                    _label = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleRight,
                        richText = false,
                        fontSize = 13
                    };
                }

                _label.font = _font;
                return _label;
            }
        }

        /// <summary>A small label in the Persian font, right aligned.</summary>
        public static GUIStyle MiniLabelStyle
        {
            get
            {
                Search();

                if (_miniLabel == null)
                {
                    _miniLabel = new GUIStyle(EditorStyles.miniLabel)
                    {
                        alignment = TextAnchor.MiddleRight,
                        richText = false,
                        wordWrap = true
                    };
                }

                _miniLabel.font = _font;
                return _miniLabel;
            }
        }

        /// <summary>
        /// The editable box Persian is typed into.
        /// </summary>
        /// <remarks>
        /// It shows the letters unjoined and in the order they are stored, which
        /// is the only order a text field can edit correctly — a caret in
        /// reordered text moves the wrong way and deletes the wrong letter. The
        /// font is still the Persian one, so what is typed is at least legible
        /// letter by letter, and the preview underneath shows the line as the
        /// game will draw it.
        /// </remarks>
        public static GUIStyle TextAreaStyle
        {
            get
            {
                Search();

                if (_textArea == null)
                {
                    _textArea = new GUIStyle(EditorStyles.textArea)
                    {
                        alignment = TextAnchor.UpperRight,
                        wordWrap = true,
                        fontSize = 14
                    };
                }

                _textArea.font = _font;
                return _textArea;
            }
        }

        /// <summary>Forgets the chosen font, so the next draw looks again.</summary>
        public static void Forget()
        {
            _searched = false;
            _font = null;
            _joins = false;
        }

        /// <summary>
        /// One line saying what Persian is being drawn with, for the window's
        /// help panel.
        /// </summary>
        public static string Report()
        {
            Search();

            if (_font == null)
            {
                return "No font on this machine was found that can draw Persian, so Persian text in this " +
                       "window will show as boxes. Import a Persian .ttf into Assets/Fonts.";
            }

            return _joins
                ? $"Persian is drawn with '{_font.name}', joined and right to left, the same as the game."
                : $"Persian is drawn with '{_font.name}', in reading order but with the letters unjoined — " +
                  "that font has no joined shapes. It is readable; it is not what the game draws.";
        }

        /// <summary>
        /// Picks the font Persian is drawn with in this window.
        /// </summary>
        /// <remarks>
        /// The project's own fonts come first, so the window shows the text in
        /// the face the game ships with wherever that is possible. A font is
        /// only accepted if it can actually draw the letters — asking is cheap
        /// and the alternative is a window full of boxes with nothing to say
        /// why.
        /// </remarks>
        private static void Search()
        {
            if (_searched)
            {
                return;
            }

            _searched = true;

            List<Font> candidates = new List<Font>(8);
            CollectProjectFonts(candidates);

            // Two passes over the same list: a font that can join is worth more
            // than one that merely has the letters, whichever order they were
            // found in.
            for (int i = 0; i < candidates.Count; i++)
            {
                if (Has(candidates[i], JoinedProbe))
                {
                    _font = candidates[i];
                    _joins = true;
                    return;
                }
            }

            for (int i = 0; i < SystemFontNames.Length; i++)
            {
                Font system = CreateSystemFont(SystemFontNames[i]);

                if (Has(system, JoinedProbe))
                {
                    _font = system;
                    _joins = true;
                    return;
                }
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                if (Has(candidates[i], LetterProbe))
                {
                    _font = candidates[i];
                    _joins = false;
                    return;
                }
            }

            _font = null;
            _joins = false;
        }

        private static void CollectProjectFonts(List<Font> into)
        {
            string[] guids = AssetDatabase.FindAssets("t:Font");

            for (int i = 0; i < guids.Length; i++)
            {
                Font font = AssetDatabase.LoadAssetAtPath<Font>(AssetDatabase.GUIDToAssetPath(guids[i]));

                if (font != null)
                {
                    into.Add(font);
                }
            }
        }

        private static Font CreateSystemFont(string fontName)
        {
            try
            {
                return Font.CreateDynamicFontFromOSFont(fontName, 14);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Whether a font can draw one character.
        /// </summary>
        /// <remarks>
        /// Only dynamic fonts can answer, and a font imported as a bitmap throws
        /// rather than saying no — so the question is asked defensively and a
        /// refusal is read as "no".
        /// </remarks>
        private static bool Has(Font font, char character)
        {
            if (font == null)
            {
                return false;
            }

            try
            {
                return font.HasCharacter(character);
            }
            catch
            {
                return false;
            }
        }
    }
}
