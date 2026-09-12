# کاراکترها و آرتشان

**نخِ قرمزِ پوسیده** · یونیتی (2D / ویژوال ناول)

فایل‌های واقعی در `AboutProject/Characters/<Name>/` هستند. در پروژه‌ی یونیتی جایشان `Assets/Images/Characters/<Name>/` است.

---

## ۱. مشخصاتِ فنی

* **اندازه‌ی بوم:** `1200 × 2400 px` — همه‌ی اسپرایت‌ها، بدونِ استثنا. فرمت `PNG` با آلفا.
* **قد:** هارو ۱۷۳ سانتی‌متر · یوآ ۱۶۵ سانتی‌متر.
* **اختلافِ ۸ سانتی داخلِ خودِ تصویر پخته شده** — یوآ حاشیه‌ی بالای بوم دارد و کلِ ۲۴۰۰ پیکسل را پر نمی‌کند.
* **در موتور هیچ مقیاسِ دستی‌ای اعمال نمی‌شود.** `Transform.localScale = (1, 1, 1)` برای هر دو.

### جای ایستادن

عددها در `_C#/Narrative/StageSettings.cs` (`Placement`) و در `StageSettings.asset` هستند. مختصاتِ **world**، نه canvas.

| | X | Y | Scale |
|---|---|---|---|
| یوآ | `-4.55` | `-1.50` | `0.48` |
| هارو | `4.70` | `-1.50` | `0.50` |
| یوآ (۹ ساله) | `-4.55` | `-1.50` | `0.335` |
| هارو (۹ ساله) | `4.70` | `-1.50` | `0.350` |

> بازی **فقط** `StageSettings.asset` را می‌خواند. Marker های داخلِ Scene خوانده نمی‌شوند.
> جزئیات و تاریخچه‌ی خرابی‌هایش: `Guide-Engine.md` بخش «جای کاراکترها».

---

## ۲. گوینده‌ها — `Speaker`

`_C#/Narrative/CharacterArt.cs`. عددها ثابت‌اند و داخلِ asset ها ذخیره می‌شوند — **هیچ‌وقت وسطِ enum اضافه نکن.**

| # | `Speaker` | پلاکِ اسم | اسپرایت | توضیح |
|---|---|---|---|---|
| ۰ | `Narrator` | ندارد | ندارد | توصیف، فضاسازی، کنشِ فیزیکی |
| ۱ | `Yua` | یوآ / ユア / Yua | دارد | شخصیتِ بازیکن، و کسی که بازی را می‌گرداند |
| ۲ | `Haru` | هارو / ハル / Haru | دارد | |
| ۳ | `Player` | دارد | ندارد | خطاب به کسی که کنترلر دستش است. از پرده‌ی دوم |
| ۴ | `YuaChild` | دارد | **هنوز کشیده نشده** | یوآی نُه‌ساله، پرده‌ی ششم |
| ۵ | `HaruChild` | دارد | **هنوز کشیده نشده** | هاروی نُه‌ساله، پرده‌ی ششم |
| ۶ | `Classmate` | دارد | **عمداً ندارد** | دختری از کلاسشان. صدایی از کنارِ صحنه |

### چرا `Classmate` بدن ندارد

او فقط برای **یک کار** وجود دارد: در ژاپنی `ぴ` اسلنگِ واقعیِ صمیمیت است و بازیکنِ ژاپنی بارش را بارِ اول می‌گیرد؛ بازیکنِ فارسی و انگلیسی فقط یک لقب می‌شنود. پس کسی از بیرونِ آن دو نفر باید **یک بار** بلند به آن اشاره کند و جوابِ «به تو ربطی ندارد» بگیرد — و پسوند همان بارِ زبانی را بگیرد که در یکی از سه زبان از اول داشت.

بدون اسپرایت، `VisualNovelStage` خودش کارِ درست را می‌کند: هیچ‌کدام از دو جایگاه او را ندارند، پس یوآ و هارو هر دو یک قدم عقب می‌روند — دقیقاً همان چیزی که در یک راهرو وقتی نفرِ سوم حرف می‌زند اتفاق می‌افتد.

> ⚠️ **نقصِ فعلی:** صحنه‌ی این گفت‌وگو در پرده‌ی اول نوشته شده ولی پسوند در آن شنیده نمی‌شود. نگاه کن به `Roadmap.md` نقصِ ۱.

---

## ۳. حالت‌های چهره — `Portrait`

enum ها روی **احساس** نام‌گذاری شده‌اند، نه روی فایل، چون دو کاراکتر یک احساس را جور دیگری املا می‌کنند: یوآ وقتی خجالت می‌کشد سرش را **پایین** می‌اندازد و هارو **رویش را برمی‌گرداند**، عصبانیتِ او یک **glare** است و مالِ هارو یک **frown**. اسکریپت نباید این را بداند.

### چهره‌های پایه

| # | `Portrait` | یوآ | هارو |
|---|---|---|---|
| ۰ | `Unchanged` | — هرچه روی صفحه است می‌ماند — | |
| ۱ | `Neutral` | `YuaNeutralGentleSmile` | `HaruNeutralGentleSmile` |
| ۲ | `Joyful` | `YuaJoyfulHappyLaugh` | `HaruJoyfulHappyLaugh` |
| ۳ | `Shy` | `YuaShyBlushingLookDown` | `HaruShyBlushingLookAway` |
| ۴ | `Sad` | `YuaSadImploringTearful` | `HaruSadImploringTearful` |
| ۵ | `Angry` | `YuaAnnoyedAngryGlare` | `HaruSeriousAngryFrown` |
| ۶ | `DeadEyes` | `YuaDeadEyesPokerFace` | `HaruDeadEyesPokerFace` |
| ۷ | `Manic` | `YuaInsaneManicSmile` | `HaruInsaneManicSmile` |
| ۸ | `Crying` | `YuaSorrowfulCryingTears` | `HaruSorrowfulCryingTears` |
| ۹ | `Injured` | — ندارد، به `Neutral` می‌افتد — | `HaruInjuredKneeGrimace` |

### بنتو — ده فریم، یک کنشِ مشترک

یک اسمِ `Portrait`، دو نقاشیِ متفاوت از **یک لحظه**. اسکریپت یک بار `LunchFirstLift` می‌گوید و هر دو را یعنی.

| # | `Portrait` | یوآ | هارو |
|---|---|---|---|
| ۱۰ | `LunchOpen` | `YuaBento01HoldClosedBox` | — |
| ۱۱ | `LunchOffer` | `YuaBento02ShowFullFood` | `HaruBento01HoldEmptyLid` |
| ۱۲ | `LunchShared` | `YuaBento03SharedMostlyEmpty` | `HaruBento02FoodReceived` |
| ۱۳ | `LunchFirstLift` | `YuaBento04LiftFirstSushi` | `HaruBento03LiftFirstOctopus` |
| ۱۴ | `LunchFirstBite` | `YuaBento05SavorFirstSushi` | `HaruBento04SavorFirstOctopus` |
| ۱۵ | `LunchSecondLift` | `YuaBento06LiftLastSushi` | `HaruBento05LiftFirstSushi` |
| ۱۶ | `LunchSecondBite` | `YuaBento07SavorLastSushi` | `HaruBento06SavorFirstSushi` |
| ۱۷ | `LunchThirdLift` | `YuaBento08LiftLastOctopus` | `HaruBento07LiftSecondOctopus` |
| ۱۸ | `LunchThirdBite` | `YuaBento09SavorLastOctopus` | `HaruBento08SavorSecondOctopus` |
| ۱۹ | `LunchFinished` | `YuaBento10ClosedFinishedSmile` | — به `Neutral` می‌افتد — |

**هارو یک تصویر عقب‌تر است، در تمامِ توالی.** یوآ باید جعبه را باز کند تا او چیزی برای نگه‌داشتن داشته باشد، پس اولین نقاشیِ او روی دومینِ او می‌افتد؛ و وقتی جعبه بسته می‌شود، دستِ هارو خالی است و تصویری ندارد — که همان چهره‌ی خنثایش است.

> **عدد در دیالوگ = عدد در فریم.** اگر متن می‌گوید «شش تا سوشی، چهار تا سوسیس»، توالی باید ده بار برداشتن داشته باشد — یا عدد گفته نشود.
> محتوای جعبه در آرت: یوآ ۶ سوشی + ۴ سوسیسِ اختاپوسی؛ ۴ سوشی + ۳ سوسیس سهمِ هارو می‌شود.

### نوشیدنی — سه فریم

| # | `Portrait` | یوآ (بابل‌تی) | هارو (ماچا) |
|---|---|---|---|
| ۲۰ | `DrinkFull` | `YuaBobaSipFullCup` | `HaruMatchaHoldFullCup` |
| ۲۱ | `DrinkReluctant` | `YuaBobaHoldEmptyCup` | `HaruMatchaSipReluctant` |
| ۲۲ | `DrinkFinished` | `YuaPeacefulClosedEyesSmile` | `HaruMatchaHoldEmptyCup` |

مالِ او تمام شده و هارو تازه دارد ماچایی را که دوست ندارد شروع می‌کند. آخرش هر دو راضی‌اند، به دو دلیلِ متفاوت.

---

## ۴. کاراکترهای بچه — پرده‌ی ششم

اسمِ فایل‌ها **از الآن قفل است**: دقیقاً اسمِ بزرگسال با `Child` بعد از اسمِ کاراکتر.

```
YuaChildNeutralGentleSmile.png     HaruChildNeutralGentleSmile.png
YuaChildJoyfulHappyLaugh.png       HaruChildJoyfulHappyLaugh.png
YuaChildShyBlushingLookDown.png    HaruChildShyBlushingLookAway.png
YuaChildSadImploringTearful.png    HaruChildSadImploringTearful.png
YuaChildAnnoyedAngryGlare.png      HaruChildSeriousAngryFrown.png
YuaChildDeadEyesPokerFace.png      HaruChildDeadEyesPokerFace.png
YuaChildSorrowfulCryingTears.png   HaruChildSorrowfulCryingTears.png
                                   HaruChildInjuredKneeGrimace.png
```

**هیچ‌کدام هنوز کشیده نشده‌اند** و پرده‌ی ششم بدونشان هم اجرا می‌شود (اسپرایتِ غایب = یک سایه). همین باعث می‌شود بشود پرده را امروز نوشت و قضاوت کرد.

بچه‌ها **لوازم ندارند** — نه بنتو، نه نوشیدنی. `WithoutProps()` در کد این را تضمین می‌کند: فلش‌بک یک حیاطِ مدرسه و یک موتورخانه است.

---

## ۵. جیره‌ی چهره — قانونِ دستورنامه

> **یک چهره برای همه‌ی تک‌گویی‌ها یعنی ترسناک‌ترین تصویرِ پروژه تبدیل می‌شود به کاغذدیواری.**

| چهره | جیره در هر پرده |
|---|---|
| `DeadEyes` | **یک بار** — و برای همان یک حالتی که مالِ اوست |
| `Sad` (چهره‌ی نگران) | **یک بار** — برای بلندترین لحظه |
| `Crying` | فقط جایی که هیچ‌کس در داستان اجازه‌ی دیدنش را ندارد |
| `Joyful` | برای انکار و برای لذت — نه برای شادیِ معمولی در تک‌گویی |
| `Manic` | پرده‌ی پنجم به بعد |

**یک چهره‌ی امضا که دو بار بیاید، نصف می‌شود. سه بار بیاید، صفر می‌شود.**

استثنا: `DeadEyes` یوآ در لحظه‌ی اورراید کردنِ گزینه‌ی آبی جیره ندارد — آن یک مکانیک است، نه یک انتخابِ کارگردانی. بعد از اورراید، صورتش **پوکر می‌ماند** و عمداً همان‌جا رها می‌شود؛ بیتِ بعدی حتماً باید یک `Portrait` بدهد.

---

## ۶. اضافه کردنِ کاراکترِ جدید

پنج جای کد باید عوض شود و صحنه فقط **دو** جایگاه دارد. مراحلِ دقیق: `Guide-Engine.md` بخش «اضافه کردن یک کاراکتر جدید».

کاراکترهایی که DLC ها لازم دارند و هنوز هیچ‌کدام وجود ندارند:
**میزوکی (水月)** · **کایتو (海斗)** · **میناتو (湊)** · **یوما (結真)** · شخصیتِ دایی · شخصیتِ دکتر.
