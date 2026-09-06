// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizationDatabase.cs
//
//  The single source of truth for on-screen text. Nothing in the game reads a
//  string typed into the Inspector any more; the scenes' authored text is
//  overwritten from here at load time.
//
//  A note on the Persian column: it is written without zero-width non-joiners.
//  The shaper handles them correctly, but they are invisible in a source file
//  and any tool that touches this text — a diff viewer, a copy-paste through a
//  chat window, an editor with aggressive whitespace trimming — can drop them
//  without leaving a trace. Every phrase here is spelled so that it reads
//  naturally with its parts joined, which is the ordinary informal spelling in
//  Persian anyway.
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace TheFrayedRedString.Localization
{
    /// <summary>One translation of a single localisation key.</summary>
    public readonly struct LocEntry
    {
        public readonly string English;
        public readonly string Japanese;
        public readonly string Persian;

        /// <summary>
        /// Creates an entry. <paramref name="persian"/> may be omitted while a
        /// line is still being translated; the entry then falls back to English
        /// rather than rendering as a gap.
        /// </summary>
        public LocEntry(string english, string japanese, string persian = null)
        {
            English = english;
            Japanese = japanese;
            Persian = persian;
        }

        /// <summary>Picks the variant for <paramref name="language"/>.</summary>
        public string For(GameLanguage language)
        {
            switch (language)
            {
                case GameLanguage.Japanese:
                    return string.IsNullOrEmpty(Japanese) ? English : Japanese;

                case GameLanguage.Persian:
                    return string.IsNullOrEmpty(Persian) ? English : Persian;

                default:
                    return English;
            }
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

        /// <summary>Returns the raw entry for a key, for callers that need every language.</summary>
        public static bool TryGetEntry(string key, out LocEntry entry)
        {
            return Entries.TryGetValue(key, out entry);
        }

        private static Dictionary<string, LocEntry> Build()
        {
            Dictionary<string, LocEntry> table = new Dictionary<string, LocEntry>(256);

            // -----------------------------------------------------------------
            //  Warning scene
            //
            //  WarningScene shows two languages at once, side by side, so these
            //  entries are never "switched": the EN label is bound to the
            //  English variant permanently, and the second label shows Japanese
            //  or Persian depending on what the player has selected.
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
                "（プレイを開始することで、上記の内容を理解・承諾したものとみなされます）",
                persian:
                "این بازی حاوی مضامین سنگین روانشناختی و موضوعات حساس است.\n" +
                "برای افراد زیر 18 سال یا کسانی که به چنین محتوایی حساسیت دارند توصیه نمیشود.\n" +
                "\n" +
                "هشدار محتوا:\n" +
                "• خودکشی و خودآزاری\n" +
                "• قتل، خونریزی و خشونت\n" +
                "• صداهای بلند ناگهانی، اختلال تصویری و جلوه های آزاردهنده\n" +
                "• تروماهای کودکی، شامل اشاره به آزار جسمی، عاطفی و جنسی\n" +
                "• دستکاری روانی، کنترلگری، وابستگی بیمارگونه و القای حس گناه\n" +
                "\n" +
                "(با شروع بازی، تایید میکنید که محتوای بالا را پذیرفته اید.)");

            table[LocKeys.WarningEnterPrompt] = new LocEntry(
                "Press Enter to continue",
                "Enter キーを押して続ける",
                "برای ادامه Enter را فشار دهید");

            // -----------------------------------------------------------------
            //  Main menu
            //
            //  The menu buttons are authored as sprites, so these strings are not
            //  drawn today. They exist so the buttons can carry accessible names
            //  and so a future text-based menu skin has nothing left to translate.
            // -----------------------------------------------------------------

            table[LocKeys.MenuStart] = new LocEntry("Start", "はじめる", "شروع");
            table[LocKeys.MenuLoad] = new LocEntry("Load", "つづきから", "ادامه");
            table[LocKeys.MenuGallery] = new LocEntry("Gallery", "ギャラリー", "گالری");
            table[LocKeys.MenuSettings] = new LocEntry("Settings", "設定", "تنظیمات");
            table[LocKeys.MenuExit] = new LocEntry("Exit", "おわる", "خروج");
            table[LocKeys.MenuBack] = new LocEntry("Back to Menu", "メニューへもどる", "بازگشت به منو");

            // -----------------------------------------------------------------
            //  Load panel
            // -----------------------------------------------------------------

            table[LocKeys.LoadPanelTitle] = new LocEntry(
                "Load Life Memory", "記憶を読み込む", "بارگذاری خاطرات");

            table[LocKeys.SavePanelTitle] = new LocEntry(
                "Keep This Memory", "この記憶を残す", "این خاطره را نگه دار");

            table[LocKeys.LoadSlotEmpty] = new LocEntry(
                "Unlived Season", "未踏の季節", "فصل نازیسته");

            table[LocKeys.LoadSlotSeason] = new LocEntry("Season {0:00}", "第{0}章", "فصل {0:00}");
            table[LocKeys.LoadSlotPlayTime] = new LocEntry("Play Time : {0}", "プレイ時間 : {0}", "زمان بازی : {0}");
            table[LocKeys.SaveWritten] = new LocEntry("Memory kept.", "記憶を残した。", "خاطره نگه داشته شد.");

            // -----------------------------------------------------------------
            //  Pause menu
            // -----------------------------------------------------------------

            table[LocKeys.PauseTitle] = new LocEntry("Paused", "一時停止", "توقف");
            table[LocKeys.PauseResume] = new LocEntry("Resume", "つづける", "ادامه");
            table[LocKeys.PauseSave] = new LocEntry("Save", "セーブ", "ذخیره");
            table[LocKeys.PauseLoad] = new LocEntry("Load", "ロード", "بارگذاری");
            table[LocKeys.PauseQuitToMenu] = new LocEntry("Main Menu", "メインメニュー", "منوی اصلی");

            // -----------------------------------------------------------------
            //  Story
            // -----------------------------------------------------------------

            table[LocKeys.StoryToBeContinued] = new LocEntry(
                "To be continued", "つづく", "ادامه دارد");

            // The two ending offers. Deliberately plain: after five minutes of
            // a scene asking for nothing, a sentence that sounds like a game
            // mechanic would undo the five minutes.
            table[LocKeys.EndingSpeakYua] = new LocEntry(
                "Let Yua speak.", "結愛に話させる。", "بگذار یوآ صحبت کند.");

            table[LocKeys.EndingSpeakHaru] = new LocEntry(
                "Let Haru speak.", "ハルに話させる。", "بگذار هارو صحبت کند.");

            table[LocKeys.EndingSayNothing] = new LocEntry(
                "Say nothing.", "何も言わない。", "چیزی نگو.");

            // -----------------------------------------------------------------
            //  Speaker names
            // -----------------------------------------------------------------

            table[LocKeys.SpeakerYua] = new LocEntry("Yua", "結愛", "یوآ");
            table[LocKeys.SpeakerHaru] = new LocEntry("Haru", "陽翔", "هارو");

            // Short, because a name plate is short, and because the weight is
            // in the number rather than in any wording around it.
            table[LocKeys.SpeakerYuaChild] = new LocEntry(
                "Yua-pi, nine", "結愛ぴ（九つ）", "یوآ‌پیِ کودک");

            table[LocKeys.SpeakerHaruChild] = new LocEntry(
                "Haru-pi, nine", "ハルぴ（九つ）", "هارو‌پیِ کودک");

            // Not a name. Three languages' worth of "some girl from your class",
            // which is exactly as much as either of them would give her.
            table[LocKeys.SpeakerClassmate] = new LocEntry(
                "A classmate", "クラスメイト", "یک همکلاسی");

            // -----------------------------------------------------------------
            //  Act titles, in narrative order
            // -----------------------------------------------------------------

            table[LocKeys.ActOne] = new LocEntry(
                "Cherry Blossom Mirage", "桜の蜃気楼", "سراب شکوفه های گیلاس");

            table[LocKeys.ActTwo] = new LocEntry("Maboroshi - 幻", "まぼろし - 幻", "مابوروشی - 幻");
            table[LocKeys.ActThree] = new LocEntry("Stillness", "静けさ", "آرامش");
            table[LocKeys.ActFour] = new LocEntry("Deepening", "深化", "عمیق شدن");
            table[LocKeys.ActFive] = new LocEntry("The Glass Breaks", "硝子が割れる", "شکستن شیشه");
            table[LocKeys.ActSix] = new LocEntry("Frayed Roots", "朽ちた根", "ریشه های پوسیده");
            table[LocKeys.ActSeven] = new LocEntry("Epilogue", "エピローグ", "اپیلوگ");

            // -----------------------------------------------------------------
            //  Act one: locations and dialogue
            //
            //  Kept in its own file. Two hundred lines of script inlined here
            //  would bury the interface text above it, and each act after this
            //  one would bury it further.
            // -----------------------------------------------------------------
            return table;
        }
    }
}
