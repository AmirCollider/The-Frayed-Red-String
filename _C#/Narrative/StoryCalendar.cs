// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryCalendar.cs
//
//  Dates, in a story that is built out of them.
//
//  The narrative document pins nearly every scene to a real day: the machine
//  room is the 26th of January 2017, the pencil case conversation is the 26th of
//  January 2025 — eight years later, to the day — and act four's 7/8/9 only
//  lands if the player has been shown enough of the calendar to do the
//  subtraction themselves. Act two alone runs from October to December, which on
//  screen reads as five consecutive days unless something says otherwise.
//
//  Two rules are worth writing down at the top, because both were learned the
//  expensive way:
//
//    * THE WEEKDAY IS NEVER TYPED. It is computed. The design document has the
//      game beginning on the 1st of September 2024, and that is a Sunday. A day
//      of the week written by hand is a day of the week that is wrong, and
//      nothing in a playthrough will ever catch it.
//
//    * NOTHING HERE GOES THROUGH System.Globalization. DateTime.ToString reads
//      the culture of the machine it is running on, so one build would print a
//      different date on a Japanese laptop and a Persian one, with the player's
//      chosen language having no say in it. The month and weekday names are
//      tables in this file, and DateTime is used for arithmetic and nothing
//      else.
// -----------------------------------------------------------------------------

using System;
using System.Text;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>One day in the story's calendar.</summary>
    /// <remarks>
    /// Three integers rather than a <see cref="DateTime"/>, because Unity
    /// serialises this into an act asset and a struct of ints is something a
    /// person can read in a diff. It also gives "nobody set this" a
    /// representation — an unset date is three zeroes — which a
    /// <see cref="DateTime"/> cannot do without smuggling in a sentinel year.
    /// </remarks>
    [Serializable]
    public struct StoryDate
    {
        [Tooltip("Four-digit year, e.g. 2024.")]
        public int Year;

        [Tooltip("1 to 12.")]
        public int Month;

        [Tooltip("1 to 31.")]
        public int Day;

        public StoryDate(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        /// <summary>True when all three fields hold a plausible value.</summary>
        public bool IsSet => Year > 0 && Month >= 1 && Month <= 12 && Day >= 1 && Day <= 31;

        /// <summary>
        /// The date as a <see cref="DateTime"/>, for arithmetic only.
        /// </summary>
        /// <returns>False when the fields do not describe a real day.</returns>
        public bool TryToDateTime(out DateTime value)
        {
            if (IsSet && Day <= DateTime.DaysInMonth(Year, Month))
            {
                value = new DateTime(Year, Month, Day);
                return true;
            }

            value = default;
            return false;
        }

        public override string ToString()
        {
            return IsSet ? $"{Year:0000}-{Month:00}-{Day:00}" : "(no date)";
        }
    }

    /// <summary>How much of a date to show.</summary>
    public enum DateStyle
    {
        /// <summary>With the day of the week. The act title card's.</summary>
        Full = 0,

        /// <summary>Without it. The corner stamp's.</summary>
        Short = 1,

        /// <summary>The year alone, for the jumps acts twelve and thirteen make.</summary>
        YearOnly = 2
    }

    /// <summary>
    /// Turns a <see cref="StoryDate"/> into text, natively in each language.
    /// </summary>
    public static class StoryCalendar
    {
        // ---------------------------------------------------------------------
        //  The locked calendar
        //
        //  Every date the narrative document pins, in one place, so a script can
        //  say MachineRoom rather than a triple of numbers somebody has to check
        //  against a Word file. AboutProject/Acts.md carries the same table in
        //  prose, with the weekday of each spelled out — and those weekdays were
        //  produced by this file rather than typed.
        // ---------------------------------------------------------------------

        /// <summary>Primary school, Sagamihara. Where K-SEI opens.</summary>
        public static readonly StoryDate FriendshipBegins = new StoryDate(2016, 1, 26);

        /// <summary>The Sagamihara attack reaches the news. The children do not understand it.</summary>
        public static readonly StoryDate SagamiharaNews = new StoryDate(2016, 7, 26);

        /// <summary>The machine room. Mizuki is ill and not at school.</summary>
        public static readonly StoryDate MachineRoom = new StoryDate(2017, 1, 26);

        /// <summary>The second term begins. The document's date for the start of the game.</summary>
        public static readonly StoryDate SecondTermBegins = new StoryDate(2024, 9, 1);

        /// <summary>
        /// Act one, day one.
        /// </summary>
        /// <remarks>
        /// The Monday after the term starts rather than the term's own first
        /// day, because that one is a Sunday and act one is six school days.
        /// See AboutProject/Acts.md, decision one.
        /// </remarks>
        public static readonly StoryDate ActOneDayOne = new StoryDate(2024, 9, 2);

        /// <summary>Act two opens.</summary>
        public static readonly StoryDate ActTwoBegins = new StoryDate(2024, 10, 3);

        /// <summary>Haru tells her about his friend. Act two's last day.</summary>
        public static readonly StoryDate TheFriendStory = new StoryDate(2024, 12, 21);

        /// <summary>Act three opens.</summary>
        public static readonly StoryDate ActThreeBegins = new StoryDate(2024, 12, 25);

        /// <summary>Act four opens.</summary>
        public static readonly StoryDate ActFourBegins = new StoryDate(2025, 1, 4);

        /// <summary>The pencil case. Eight years after the machine room, to the day.</summary>
        public static readonly StoryDate ThePencilCase = new StoryDate(2025, 1, 26);

        /// <summary>Act five opens.</summary>
        public static readonly StoryDate ActFiveBegins = new StoryDate(2025, 3, 1);

        /// <summary>Haru's house.</summary>
        public static readonly StoryDate TheLastDay = new StoryDate(2025, 3, 24);

        // ---------------------------------------------------------------------
        //  Name tables
        // ---------------------------------------------------------------------

        private static readonly string[] MonthsEnglish =
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        /// <summary>
        /// Gregorian months as Persian writes them.
        /// </summary>
        /// <remarks>
        /// Gregorian and not Jalali, deliberately. The story happens in Japan and
        /// half its dates are anniversaries of each other; converting them into
        /// another calendar for one of the three languages would break every one
        /// of those anniversaries for the players reading in it.
        /// </remarks>
        private static readonly string[] MonthsPersian =
        {
            "ژانویه", "فوریه", "مارس", "آوریل", "می", "ژوئن",
            "ژوئیه", "اوت", "سپتامبر", "اکتبر", "نوامبر", "دسامبر"
        };

        /// <summary>Sunday first, to match <see cref="DayOfWeek"/>.</summary>
        private static readonly string[] WeekdaysEnglish =
        {
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };

        private static readonly string[] WeekdaysPersian =
        {
            "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"
        };

        /// <summary>The single kanji Japanese puts in brackets after a date.</summary>
        private static readonly string[] WeekdaysJapanese =
        {
            "日", "月", "火", "水", "木", "金", "土"
        };

        private static readonly char[] PersianDigits =
        {
            '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'
        };

        // ---------------------------------------------------------------------
        //  Public
        // ---------------------------------------------------------------------

        /// <summary>
        /// The day of the week a date falls on.
        /// </summary>
        /// <remarks>
        /// The only correct source of this in the project. Nothing anywhere —
        /// not a script, not a document, not a comment — should state a weekday
        /// it did not get from here.
        /// </remarks>
        /// <returns>False when the date is not a real day.</returns>
        public static bool TryWeekdayOf(StoryDate date, out DayOfWeek weekday)
        {
            if (date.TryToDateTime(out DateTime value))
            {
                weekday = value.DayOfWeek;
                return true;
            }

            weekday = DayOfWeek.Sunday;
            return false;
        }

        /// <summary>
        /// A date written out in all three languages at once.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Returned as a <see cref="LocalizedLine"/> rather than as a string in
        /// the current language, and that is the point: the caption furniture
        /// keeps the line and re-renders it when the player switches language
        /// while it is on screen. A resolved string would be stuck in whichever
        /// language happened to be selected when the beat played.
        /// </para>
        /// <para>
        /// Three performances rather than three translations, like everything
        /// else in this game: Persian reads right to left with Persian digits,
        /// Japanese stacks the units largest first and puts the weekday in
        /// brackets, English leads with the weekday and a comma.
        /// </para>
        /// </remarks>
        public static LocalizedLine Line(StoryDate date, DateStyle style = DateStyle.Short)
        {
            if (!date.IsSet)
            {
                return new LocalizedLine(string.Empty, string.Empty, string.Empty);
            }

            if (style == DateStyle.YearOnly)
            {
                return new LocalizedLine(
                    date.Year.ToString(),
                    date.Year + "年",
                    ToPersianDigits(date.Year.ToString()));
            }

            // A date that is not a real day — the 31st of February, say — still
            // prints. It simply prints without a weekday, because there is not
            // one to compute. The Check tab is where a typo like that should be
            // caught; a caption is not the place to throw.
            bool haveWeekday = TryWeekdayOf(date, out DayOfWeek weekday);
            bool withWeekday = style == DateStyle.Full && haveWeekday;
            int dayIndex = haveWeekday ? (int)weekday : 0;

            string monthEnglish = MonthsEnglish[date.Month - 1];
            string monthPersian = MonthsPersian[date.Month - 1];

            string english = withWeekday
                ? $"{WeekdaysEnglish[dayIndex]}, {date.Day} {monthEnglish} {date.Year}"
                : $"{date.Day} {monthEnglish} {date.Year}";

            string japanese = withWeekday
                ? $"{date.Year}年{date.Month}月{date.Day}日（{WeekdaysJapanese[dayIndex]}）"
                : $"{date.Year}年{date.Month}月{date.Day}日";

            // Built from the parts rather than by converting a finished sentence,
            // so the month name is never walked by the digit rewriter.
            string persianDay = ToPersianDigits(date.Day.ToString());
            string persianYear = ToPersianDigits(date.Year.ToString());

            string persian = withWeekday
                ? $"{WeekdaysPersian[dayIndex]} {persianDay} {monthPersian} {persianYear}"
                : $"{persianDay} {monthPersian} {persianYear}";

            return new LocalizedLine(english, japanese, persian);
        }

        /// <summary>
        /// The same date in the language the player has selected.
        /// </summary>
        /// <remarks>
        /// For the act title card, which already resolves its own title this
        /// way. Anything that stays on screen long enough for a language switch
        /// to matter should take <see cref="Line"/> instead.
        /// </remarks>
        public static string Current(StoryDate date, DateStyle style = DateStyle.Full)
        {
            return Line(date, style).Current();
        }

        /// <summary>
        /// Rewrites the Western digits in a string as Persian ones.
        /// </summary>
        /// <remarks>
        /// Persian text with Latin numerals in it reads as a form field rather
        /// than as a sentence, and the rest of the Persian script in this game
        /// is written with Persian digits.
        /// </remarks>
        public static string ToPersianDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            StringBuilder builder = new StringBuilder(value.Length);

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                builder.Append(c >= '0' && c <= '9' ? PersianDigits[c - '0'] : c);
            }

            return builder.ToString();
        }
    }
}
