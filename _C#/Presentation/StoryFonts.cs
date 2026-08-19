// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryFonts.cs
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TMPro;
using UnityDirectTMP;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Gives a label built at runtime the same font setup the scene's authored
    /// labels already have.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The project draws its text with the UnityDirectTMP package: a
    /// <see cref="DirectFont"/> next to a label points at a .ttf and the label
    /// is rendered from it, with Persian joined and put in reading order by the
    /// font's own OpenType tables. Nothing else in this codebase has to know
    /// anything about scripts, shaping or direction.
    /// </para>
    /// <para>
    /// The catch is that the per-language font slots on that component are
    /// serialized private fields with no public setters — by design, since they
    /// are meant to be filled in the Inspector. So rather than guess at fonts
    /// from code, this copies a label that has already been set up by hand.
    /// Whatever fonts are assigned to the scene's own labels are the fonts the
    /// dialogue box gets, and changing them in the Inspector changes both.
    /// </para>
    /// <para>
    /// Two things then have to be true for that to keep working, and neither was.
    /// The template has to be found again in every scene — a cached component
    /// from the previous scene has been destroyed, and a destroyed component
    /// copies nothing, which is why Persian came back as empty boxes the second
    /// time an act was entered. And a scene with no hand-made label in it at all
    /// — which is every act after the first one today — has to get its fonts
    /// from somewhere, or the same boxes appear the first time instead.
    /// </para>
    /// </remarks>
    public static class StoryFonts
    {
        /// <summary>Names that mark a font as the one to use for Persian.</summary>
        private static readonly string[] PersianFontNames =
        {
            "iran", "vazir", "sahel", "shabnam", "estedad", "yekan", "dana", "arabic", "naskh"
        };

        /// <summary>Names that mark a font as the one to use for Japanese.</summary>
        private static readonly string[] JapaneseFontNames =
        {
            "klee", "noto sans jp", "notosansjp", "sawarabi", "kosugi", "mplus", "m plus", "cjk", "jp"
        };

        /// <summary>Names that mark a font as the one to use for Latin text.</summary>
        private static readonly string[] LatinFontNames =
        {
            "liberation", "inter", "roboto", "poppins", "lato", "arial", "noto sans"
        };

        /// <summary>Labels already reported as having no font at all.</summary>
        private static readonly HashSet<string> _warnedBaseFont = new HashSet<string>();

        private static DirectFont _template;
        private static GameObject _fallbackHost;
        private static bool _searched;
        private static bool _warnedMissing;
        private static bool _warnedUnbuilt;

        /// <summary>Clears the cached template between play sessions.</summary>
        public static void ResetStatics()
        {
            _template = null;
            _searched = false;
            _warnedMissing = false;
            _warnedUnbuilt = false;
            _warnedBaseFont.Clear();

            if (_fallbackHost != null)
            {
                Object.Destroy(_fallbackHost);
            }

            _fallbackHost = null;
        }

        /// <summary>
        /// Forgets which label the fonts were copied from, so the next label
        /// built looks for one again.
        /// </summary>
        /// <remarks>
        /// Called as each scene loads. The template is a component in a scene,
        /// and a scene load destroys it — after which the cached reference is a
        /// destroyed object that compares equal to null, copies nothing, and
        /// leaves every label built afterwards on TextMeshPro's default font.
        /// In Persian that is not a subtle degradation: the default font has no
        /// Arabic glyphs at all, so the whole script comes out as boxes.
        /// </remarks>
        public static void ForgetTemplate()
        {
            // Always, even when the current template is the carrier this class
            // built. A scene that has no hand-made label falls back to the baked
            // fonts, and the next scene may well have one — whoever set fonts in
            // the Inspector should get them back. The carrier itself is kept and
            // reused rather than rebuilt, so the cost of asking again is one
            // search.
            _template = null;
            _searched = false;
        }

        /// <summary>
        /// Uses an explicit label as the source of font settings, instead of
        /// whichever one happens to be found first.
        /// </summary>
        public static void SetTemplate(DirectFont template)
        {
            if (template == null)
            {
                return;
            }

            _template = template;
            _searched = true;
        }

        /// <summary>
        /// Attaches a <see cref="DirectFont"/> to a label and copies the
        /// template's settings onto it.
        /// </summary>
        /// <returns>The component, or <c>null</c> when there was nothing to copy.</returns>
        public static DirectFont Apply(TMP_Text label)
        {
            if (label == null)
            {
                return null;
            }

            DirectFont template = FindTemplate();

            DirectFont direct = label.GetComponent<DirectFont>();
            if (direct == null)
            {
                direct = label.gameObject.AddComponent<DirectFont>();
            }

            if (template == null || template == direct)
            {
                return direct;
            }

            // JsonUtility carries serialized private fields, which is the whole
            // point here — the per-language font slots cannot be reached any
            // other way from code. Object references travel as instance IDs,
            // which are only meaningful inside one session; that is fine,
            // because both components exist in the session doing the copying.
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(template), direct);

            // A template whose own Font field is empty copies that emptiness
            // onto every label in the game, and an empty Font field is a Direct
            // Font that does nothing whatsoever.
            EnsureBaseFont(direct);

            direct.Rebuild();

            if (direct.FontAsset == null && !_warnedUnbuilt)
            {
                _warnedUnbuilt = true;
                Debug.LogWarning(
                    "[Fonts] A Direct Font was copied onto a story label but no font asset came out of it, " +
                    "so that label falls back to TextMeshPro's default and Persian will not join. " +
                    "Check that the template label's Font field points at a .ttf.");
            }

            return direct;
        }

        /// <summary>
        /// Makes sure a label has a base font, because without one the package
        /// does nothing at all.
        /// </summary>
        /// <returns>True when the label ends up with a font to draw from.</returns>
        /// <remarks>
        /// <para>
        /// <c>DirectFont.Apply</c> opens with <c>if (_label == null || font ==
        /// null) return;</c> — so a component whose main Font field is empty is
        /// inert. It never scans the text, never picks a per-language font, and
        /// never installs the shaper. The three language slots underneath it are
        /// not consulted, because execution never reaches them.
        /// </para>
        /// <para>
        /// That is the whole of why filling in those slots last time changed
        /// nothing on the content-warning screen. The label was not choosing the
        /// wrong font; it was not choosing at all, and TextMeshPro was drawing it
        /// with whatever asset it had — which for a Latin default means English
        /// reads perfectly and every other script is a row of boxes.
        /// </para>
        /// </remarks>
        private static bool EnsureBaseFont(DirectFont direct)
        {
            if (direct == null)
            {
                return false;
            }

            if (direct.Font != null)
            {
                return true;
            }

            DirectFontBytesTable table = DirectFontBytes.Table;

            if (table == null)
            {
                return false;
            }

            // The Latin face is the base by preference: the per-language slots
            // take Persian and CJK off it, so what is left for the base to draw
            // is exactly the script a Latin font is for.
            // Explicit tests rather than ??. Unity overloads == on its objects
            // and ?? does not go through the overload, so a destroyed font would
            // slip past a null-coalescing chain and be assigned.
            Font baseFont = PickFont(table, LatinFontNames);

            if (baseFont == null)
            {
                baseFont = PickFont(table, PersianFontNames);
            }

            if (baseFont == null)
            {
                baseFont = PickFont(table, JapaneseFontNames);
            }

            if (baseFont == null)
            {
                baseFont = FirstFont(table);
            }

            if (baseFont == null)
            {
                // Named, and said once. Three rounds of this went by with the
                // symptom being a screenshot of boxes and no way to tell which
                // label was inert — so when it cannot be fixed, at least say
                // which object to open.
                if (_warnedBaseFont.Add(direct.name))
                {
                    Debug.LogWarning(
                        $"[Fonts] '{direct.name}' has no font on its Direct Font component and none could " +
                        "be found to give it, so that label draws nothing but boxes in any script but " +
                        "Latin. Set its Font field, or run Unity DirectTMP > Bake Fonts For Build.");
                }

                return false;
            }

            direct.Font = baseFont;
            return true;
        }

        /// <summary>
        /// Builds the font for whatever script this label now holds, and makes
        /// the label lay itself out again with it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The package decides which of its fonts a label needs by scanning the
        /// text, and it does that from <c>LateUpdate</c>. Assign Persian to a
        /// label that was holding English and the canvas draws it that same
        /// frame with the English font still on it — after which nothing is
        /// dirty, so nothing draws it again. The line stays wrong.
        /// </para>
        /// <para>
        /// <c>Rebuild</c> is the package's own way of saying "do it now instead
        /// of next frame"; the forced mesh update is what makes TextMeshPro run
        /// the shaper over the text a second time with the font that just
        /// arrived. Called once per line and once per language change, not per
        /// character.
        /// </para>
        /// </remarks>
        public static void Refresh(TMP_Text label)
        {
            if (label == null)
            {
                return;
            }

            DirectFont direct = label.GetComponent<DirectFont>();

            if (direct == null || !EnsureBaseFont(direct))
            {
                return;
            }

            direct.Rebuild();

            // Only a label that is awake. Forcing a mesh update on one whose
            // Awake has not run yet reaches into TextMeshPro state that does not
            // exist — and a label that is off will lay itself out the moment it
            // is switched on, which is soon enough.
            if (label.isActiveAndEnabled)
            {
                label.ForceMeshUpdate(false, true);
            }
        }

        /// <summary>
        /// Re-lays every label in the game that is drawn from a font file.
        /// </summary>
        /// <returns>How many labels were redrawn.</returns>
        /// <remarks>
        /// The blunt version of <see cref="Refresh"/>, for the two moments when
        /// every label on screen may have changed script at once: a language
        /// change, and the first frames of a scene, where a label can be asked
        /// to draw a script before the font for it has finished being built.
        /// </remarks>
        public static int RefreshAll()
        {
            DirectFont[] labels = Object.FindObjectsByType<DirectFont>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            int redrawn = 0;

            for (int i = 0; i < labels.Length; i++)
            {
                DirectFont direct = labels[i];

                if (direct == null || (_fallbackHost != null && direct.gameObject == _fallbackHost))
                {
                    continue;
                }

                TMP_Text label = direct.GetComponent<TMP_Text>();

                // An empty label has no script to pick a font for, and most of
                // the labels in a scene are empty most of the time — the three
                // save cards alone are a dozen of them.
                if (label == null || string.IsNullOrEmpty(label.text) || !EnsureBaseFont(direct))
                {
                    continue;
                }

                direct.Rebuild();

                if (label.isActiveAndEnabled)
                {
                    label.ForceMeshUpdate(false, true);
                }

                redrawn++;
            }

            return redrawn;
        }

        /// <summary>
        /// Finds a label in the loaded scene that already has its fonts set up,
        /// or builds one from the fonts baked for the build.
        /// </summary>
        /// <remarks>
        /// Inactive objects are included on purpose: in an act scene the only
        /// hand-authored labels are the save-slot cards inside a menu panel that
        /// starts hidden, and they are exactly the ones worth copying.
        /// </remarks>
        private static DirectFont FindTemplate()
        {
            // The null test is not redundant with the flag. A template found in
            // a previous scene is a destroyed object by now, and a destroyed
            // Unity object is equal to null while the reference itself is not —
            // so the search has to run again even though it has already run once.
            if (_searched && _template != null)
            {
                return _template;
            }

            _searched = true;
            _template = FindAuthoredTemplate();

            if (_template != null)
            {
                return _template;
            }

            if (_fallbackHost != null)
            {
                _template = _fallbackHost.GetComponent<DirectFont>();
            }

            if (_template == null)
            {
                _template = BuildFallbackTemplate();
            }

            if (_template == null && !_warnedMissing)
            {
                _warnedMissing = true;
                Debug.LogWarning(
                    "[Fonts] No label with a Direct Font component was found in this scene and no fonts have " +
                    "been baked, so the story interface has no font to use and will fall back to " +
                    "TextMeshPro's default — Persian will render as empty boxes. " +
                    "Add a Direct Font to any label in the scene, or run " +
                    "Unity DirectTMP > Bake Fonts For Build once.");
            }

            return _template;
        }

        /// <summary>
        /// The first Direct Font in the loaded scenes that somebody set up by
        /// hand, ignoring the carrier this class may have built earlier.
        /// </summary>
        /// <remarks>
        /// The carrier is a Direct Font too, and finding it instead of a real
        /// label would quietly replace the fonts chosen in the Inspector with
        /// the ones guessed from the baked table. Whoever set fonts by hand
        /// should win.
        /// </remarks>
        private static DirectFont FindAuthoredTemplate()
        {
            DirectFont[] found = Object.FindObjectsByType<DirectFont>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            for (int i = 0; i < found.Length; i++)
            {
                if (found[i] != null && (_fallbackHost == null || found[i].gameObject != _fallbackHost))
                {
                    return found[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Builds a template out of the fonts UnityDirectTMP baked into
        /// Resources, for a scene that has no hand-made label to copy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The baked table exists so a player build can read a font's own
        /// joining rules, and it lists every <see cref="Font"/> a Direct Font
        /// anywhere in the project points at. That makes it the one place at
        /// runtime where the project's fonts can be reached by name — ordinary
        /// assets outside a Resources folder cannot be loaded at all.
        /// </para>
        /// <para>
        /// The carrier is an inactive GameObject that survives scene loads.
        /// Inactive matters: a Direct Font with no label next to it would
        /// otherwise run its own OnEnable and find nothing to draw. Nothing ever
        /// renders it; it exists to be copied from.
        /// </para>
        /// </remarks>
        private static DirectFont BuildFallbackTemplate()
        {
            DirectFontBytesTable table = DirectFontBytes.Table;

            if (table == null || table.Rows == null || table.Rows.Length == 0)
            {
                return null;
            }

            Font persian = PickFont(table, PersianFontNames);
            Font japanese = PickFont(table, JapaneseFontNames);
            Font latin = PickFont(table, LatinFontNames);
            Font any = FirstFont(table);

            Font baseFont = latin != null ? latin : persian != null ? persian : japanese != null ? japanese : any;

            if (baseFont == null)
            {
                return null;
            }

            // Ordinary object, kept across scene loads and switched off. Not
            // hidden: a hidden, un-saved object is the kind of thing that
            // outlives play mode and gets reported as a leak, and there is
            // nothing here worth hiding.
            _fallbackHost = new GameObject("[TFRS] Story Font Template");
            _fallbackHost.SetActive(false);
            Object.DontDestroyOnLoad(_fallbackHost);

            DirectFont carrier = _fallbackHost.AddComponent<DirectFont>();

            // Public property first, so the component is usable even if the
            // per-language write below turns out to be a no-op.
            carrier.Font = baseFont;

            if (!AssignLanguageSlots(carrier, persian, japanese, latin) && persian != null)
            {
                // The slots could not be written — the package renamed a field,
                // most likely. One font has to serve every script, so it is the
                // one whose absence is unreadable rather than merely wrong.
                carrier.Font = persian;

                Debug.LogWarning(
                    "[Fonts] The per-language font slots could not be filled in from code, so every language " +
                    $"is drawn with '{persian.name}'. Add a Direct Font to a label in the scene and set its " +
                    "three language fonts by hand to fix this properly.");
            }

            Debug.Log(
                $"[Fonts] No hand-made label to copy, so the story interface uses the baked fonts: " +
                $"Persian '{Name(persian)}', Japanese '{Name(japanese)}', Latin '{Name(latin)}'.");

            return carrier;
        }

        /// <summary>
        /// Writes the per-language font slots, which have no setters.
        /// </summary>
        /// <returns>True when the write actually landed.</returns>
        /// <remarks>
        /// <para>
        /// Serialized object references travel through JsonUtility as an
        /// instance id, so a slot can be filled by handing the component the
        /// same shape it would have serialized itself. Unknown keys are ignored
        /// rather than throwing, which is why the result is read back and
        /// checked instead of assumed.
        /// </para>
        /// <para>
        /// A null font writes nothing at all rather than writing an empty
        /// reference. That distinction is what lets this fill the gaps in a
        /// component somebody has already set up by hand without touching the
        /// slot they filled in themselves.
        /// </para>
        /// </remarks>
        private static bool AssignLanguageSlots(DirectFont target, Font persian, Font japanese, Font latin)
        {
            if (target == null)
            {
                return false;
            }

            List<string> slots = new List<string>(3);

            AddSlot(slots, "persianArabic", persian);
            AddSlot(slots, "japaneseChineseKorean", japanese);
            AddSlot(slots, "latin", latin);

            if (slots.Count == 0)
            {
                return true;
            }

            try
            {
                JsonUtility.FromJsonOverwrite("{" + string.Join(",", slots) + "}", target);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[Fonts] Could not fill in the per-language fonts: {exception.Message}");
                return false;
            }

            string written = JsonUtility.ToJson(target);

            return (persian == null || Mentions(written, persian))
                   && (japanese == null || Mentions(written, japanese))
                   && (latin == null || Mentions(written, latin));
        }

        private static void AddSlot(List<string> into, string field, Font font)
        {
            if (font != null)
            {
                into.Add($"\"{field}\":{{\"instanceID\":{font.GetInstanceID()}}}");
            }
        }

        /// <summary>
        /// Fills in the language fonts the scene's own labels were never given.
        /// </summary>
        /// <returns>How many labels were completed.</returns>
        /// <remarks>
        /// <para>
        /// A Direct Font can hold four fonts: one for everything, and one each
        /// for Persian, for CJK and for Latin. Filling in only the first is the
        /// natural thing to do and it works — right up until a label is asked to
        /// draw a script that font has no glyphs for. The content warning is
        /// exactly that case: one label, set up once, showing Japanese to a
        /// Japanese reader and Persian to a Persian one, from a single font that
        /// cannot possibly have both.
        /// </para>
        /// <para>
        /// So every label in the scene gets its empty slots filled from the
        /// baked table, once, as the scene loads. A slot somebody chose is never
        /// touched — this only ever adds a font where there was none, which
        /// cannot make a label worse than the boxes it was drawing.
        /// </para>
        /// </remarks>
        public static int CompleteAuthoredFonts()
        {
            DirectFontBytesTable table = DirectFontBytes.Table;

            if (table == null || table.Rows == null || table.Rows.Length == 0)
            {
                return 0;
            }

            Font persian = PickFont(table, PersianFontNames);
            Font japanese = PickFont(table, JapaneseFontNames);

            if (persian == null && japanese == null)
            {
                return 0;
            }

            DirectFont[] labels = Object.FindObjectsByType<DirectFont>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            int completed = 0;

            for (int i = 0; i < labels.Length; i++)
            {
                DirectFont label = labels[i];

                if (label == null || (_fallbackHost != null && label.gameObject == _fallbackHost))
                {
                    continue;
                }

                // The base font first. It is the gate: with it empty the
                // package returns before it has looked at anything else, so a
                // label missing this one field is missing all four.
                bool gained = label.Font == null && EnsureBaseFont(label);

                string json = JsonUtility.ToJson(label);

                // Only the two scripts a Latin font cannot draw at all. Filling
                // the Latin slot as well was the previous version of this and it
                // buys nothing: the base font already draws Latin, and every
                // font named here is also added to the label's fallback chain,
                // so a third one is a third atlas for glyphs already covered.
                Font addPersian = IsSlotEmpty(json, "persianArabic") ? persian : null;
                Font addJapanese = IsSlotEmpty(json, "japaneseChineseKorean") ? japanese : null;

                if (addPersian == null && addJapanese == null)
                {
                    if (gained)
                    {
                        label.Rebuild();
                        completed++;
                    }

                    continue;
                }

                if (AssignLanguageSlots(label, addPersian, addJapanese, null) || gained)
                {
                    label.Rebuild();
                    completed++;
                }
            }

            return completed;
        }

        /// <summary>
        /// True when a serialized object slot holds nothing.
        /// </summary>
        /// <remarks>
        /// An unset reference serializes as instance id zero. Reading the JSON
        /// is the only way to ask: the fields are private and have no getters,
        /// which is the same reason writing them goes the same way round.
        /// </remarks>
        private static bool IsSlotEmpty(string json, string field)
        {
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            int at = json.IndexOf($"\"{field}\"", System.StringComparison.Ordinal);

            if (at < 0)
            {
                // The package renamed the field. Leaving it alone is the safe
                // reading: this only ever adds fonts, and it cannot add one to
                // a slot it cannot find.
                return false;
            }

            int idAt = json.IndexOf("\"instanceID\"", at, System.StringComparison.Ordinal);

            if (idAt < 0)
            {
                return false;
            }

            int colon = json.IndexOf(':', idAt + 12);
            int close = json.IndexOf('}', colon + 1);

            if (colon < 0 || close < 0)
            {
                return false;
            }

            string value = json.Substring(colon + 1, close - colon - 1).Trim();

            return value == "0";
        }

        private static bool Mentions(string json, Font font)
        {
            return font != null && json != null && json.Contains(font.GetInstanceID().ToString());
        }

        /// <summary>The first baked font whose file name contains one of these words.</summary>
        private static Font PickFont(DirectFontBytesTable table, string[] wanted)
        {
            DirectFontBytesTable.Row[] rows = table.Rows;

            for (int i = 0; i < wanted.Length; i++)
            {
                for (int j = 0; j < rows.Length; j++)
                {
                    Font font = rows[j].font;

                    if (font == null)
                    {
                        continue;
                    }

                    if (font.name.ToLowerInvariant().Contains(wanted[i]))
                    {
                        return font;
                    }
                }
            }

            return null;
        }

        private static Font FirstFont(DirectFontBytesTable table)
        {
            DirectFontBytesTable.Row[] rows = table.Rows;

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].font != null)
                {
                    return rows[i].font;
                }
            }

            return null;
        }

        private static string Name(Font font)
        {
            return font != null ? font.name : "—";
        }
    }
}
