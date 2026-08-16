// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizationDatabase.cs
//
//  The single source of truth for on-screen text. Nothing in the game reads a
//  string typed into the Inspector any more; the scenes' authored text is
//  overwritten from here at load time.
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace TheFrayedRedString.Localization
{
    /// <summary>An English / Japanese pair for one localisation key.</summary>
    public readonly struct LocEntry
    {
        public readonly string English;
        public readonly string Japanese;

        public LocEntry(string english, string japanese)
        {
            English = english;
            Japanese = japanese;
        }

        /// <summary>Picks the variant for <paramref name="language"/>.</summary>
        public string For(GameLanguage language)
        {
            return language == GameLanguage.Japanese ? Japanese : English;
        }
    }

    /// <summary>In-memory string table.</summary>
    public static class LocalizationDatabase
    {
        private static readonly Dictionary<string, LocEntry> Entries = Build();

        /// <summary>Number of keys in the table. Handy for a smoke test.</summary>
        public static int Count => Entries.Count;

        /// <summary>True when the key exists.</summary>
        public static bool Has(string key)
        {
            return !string.IsNullOrEmpty(key) && Entries.ContainsKey(key);
        }

        /// <summary>
        /// Looks up a key. Missing keys return a visibly wrong
        /// <c>#key#</c> marker rather than an empty string, so a gap in the table
        /// is obvious on screen instead of silently blanking a label.
        /// </summary>
        public static string Get(string key, GameLanguage language)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            return Entries.TryGetValue(key, out LocEntry entry)
                ? entry.For(language)
                : $"#{key}#";
        }

        /// <summary>Returns the raw pair for a key, for callers that need both languages.</summary>
        public static bool TryGetEntry(string key, out LocEntry entry)
        {
            return Entries.TryGetValue(key, out entry);
        }

        private static Dictionary<string, LocEntry> Build()
        {
            Dictionary<string, LocEntry> table = new Dictionary<string, LocEntry>(48);

            // -----------------------------------------------------------------
            //  Warning scene
            //
            //  WarningScene shows both languages at once, side by side, so this
            //  entry is never "switched" — the EN label is bound to the English
            //  variant and the JP label to the Japanese one, permanently.
            // -----------------------------------------------------------------

            table[LocKeys.WarningBody] = new LocEntry(
                english:
                "This game contains heavy psychological themes and sensitive topics.\n" +
                "It is not recommended for players under 18 or those sensitive to such material.\n" +
                "\n" +
                "Content Warning:\n" +
                "• Suicide and self-harm\n" +
                "• Murder, bloodshed, and violence\n" +
                "• Sudden loud noises, visual glitches, and disturbing effects\n" +
                "• Childhood trauma, including references to physical, emotional, and sexual abuse\n" +
                "• Psychological manipulation, coercive control, codependency, and induced guilt\n" +
                "\n" +
                "(By starting the game, you acknowledge and accept the content above.)",
                japanese:
                "本作には重度の心理的描写や繊細なテーマが含まれており、18歳未満の方や刺激に敏感な方のプレイは推奨されません。\n" +
                "\n" +
                "警告・含まれる表現：\n" +
                "・自殺および自傷行為\n" +
                "・殺人、流血、および暴力描写\n" +
                "・突発的な音、画面のグリッチ、精神的不安を煽る演出\n" +
                "・幼少期のトラウマ、ならびに身体的・感情的・性的虐待の示唆\n" +
                "・心理的操作（ガスライティング）、過度な支配、共依存、および罪悪感の植え付け\n" +
                "\n" +
                "（プレイを開始することで、上記の内容を理解・承諾したものとみなされます）");

            table[LocKeys.WarningEnterPrompt] = new LocEntry(
                "Press Enter to continue",
                "Enter キーを押して続ける");

            // -----------------------------------------------------------------
            //  Main menu
            //
            //  The menu buttons are authored as sprites, so these strings are not
            //  drawn today. They exist so the buttons can carry accessible names
            //  and so a future text-based menu skin has nothing left to translate.
            // -----------------------------------------------------------------

            table[LocKeys.MenuStart] = new LocEntry("Start", "はじめる");
            table[LocKeys.MenuLoad] = new LocEntry("Load", "つづきから");
            table[LocKeys.MenuGallery] = new LocEntry("Gallery", "ギャラリー");
            table[LocKeys.MenuSettings] = new LocEntry("Settings", "設定");
            table[LocKeys.MenuExit] = new LocEntry("Exit", "おわる");
            table[LocKeys.MenuBack] = new LocEntry("Back to Menu", "メニューへもどる");

            // -----------------------------------------------------------------
            //  Load panel
            // -----------------------------------------------------------------

            table[LocKeys.LoadPanelTitle] = new LocEntry("Load Life Memory", "記憶を読み込む");
            table[LocKeys.LoadSlotEmpty] = new LocEntry("Unlived Season", "未踏の季節");
            table[LocKeys.LoadSlotSeason] = new LocEntry("Season {0:00}", "第{0}章");
            table[LocKeys.LoadSlotPlayTime] = new LocEntry("Play Time : {0}", "プレイ時間 : {0}");

            // -----------------------------------------------------------------
            //  Act titles, in narrative order
            // -----------------------------------------------------------------

            table[LocKeys.ActOne] = new LocEntry("Cherry Blossom Mirage", "桜の蜃気楼");
            table[LocKeys.ActTwo] = new LocEntry("Maboroshi - 幻", "まぼろし - 幻");
            table[LocKeys.ActThree] = new LocEntry("Stillness", "静けさ");
            table[LocKeys.ActFour] = new LocEntry("Deepening", "深化");
            table[LocKeys.ActFive] = new LocEntry("The Glass Breaks", "硝子が割れる");
            table[LocKeys.ActSix] = new LocEntry("Frayed Roots", "朽ちた根");
            table[LocKeys.ActSeven] = new LocEntry("Epilogue", "エピローグ");

            return table;
        }
    }
}
