# پس‌زمینه‌ها

**نخِ قرمزِ پوسیده** · یونیتی (2D / ویژوال ناول)

فایل‌های واقعی در `AboutProject/Backgrounds/` هستند. در پروژه‌ی یونیتی جایشان `Assets/Images/Backgrounds/` است.

---

## ۱. مشخصاتِ فنی

* **رزولوشن:** `1920 × 1080 px` — همه، بدونِ استثنا. فرمت `PNG`.
* **نسبت:** `16:9`
* **تنظیماتِ یونیتی:** `Texture Type: Sprite (2D and UI)` · `Wrap Mode: Clamp`
* روی یک `Image` تمام‌صفحه در `BackgroundCanvas` (sortingOrder 0) کشیده می‌شود.
* **اسم در کد هیچ‌وقت رشته‌ی خام نیست.** همیشه از ثابت‌های `Backgrounds` در `_C#/Narrative/CharacterArt.cs` بیاید.

> **فایلِ غایب بازی را نمی‌شکند.** `ProceduralBackgrounds` از روی خودِ اسم یک تصویرِ جانشین می‌سازد، پس یک پرده را می‌شود امروز نوشت و پخش کرد و قضاوت کرد، وقتی هنوز آرتش وجود ندارد.

---

## ۲. پس‌زمینه‌های موجود

**۳۴ فایل.** ستونِ «پرده‌ها» شمرده شده است، نه حدس — از روی `Backgrounds.` در هر هفت `ActXXBuilder.cs`.

### ۲.۱ سه‌گانه‌های فصلی — مهم‌ترین جدولِ این فایل

بازی **از ۲ سپتامبر ۲۰۲۴ تا ۲۴ مارس ۲۰۲۵** می‌گذرد: پاییز، زمستان، اولِ بهار. **هیچ‌وقت بهار نیست.** پس ثابتِ پیش‌فرضِ هر مکان به **نسخه‌ی پاییزی** اشاره می‌کند و نسخه‌ی بهاری اسمِ صریحِ خودش را دارد و فقط برای فلش‌بک و پایانِ رویاست.

| مکان | پیش‌فرض (پاییز) | زمستان | بهار — فقط خاطره و رویا |
|---|---|---|---|
| مسیرِ مدرسه | `SchoolAlleyDay` → `AutumnSchoolAlleyDay` | — | `SchoolAlleySpring` → `CherryBlossomSchoolAlleyDay` |
| ۱-الف، آفتابی | `ClassroomDay` → `SunnyClassroomAutumnDay` | — | `ClassroomSpringDay` → `SunnyClassroomDay` |
| ۱-الف، بارانی | `ClassroomRainy` → `OvercastClassroomRainyAutumnDay` | — | `ClassroomRainySpring` → `OvercastClassroomRainy` |
| **راه‌پله** | `StairsAutumn` → `AutumnAfternoonSchoolStairs` | `StairsWinter` → `WinterAfternoonSchoolStairs` | `StairsSpring` → `SpringAfternoonSchoolStairs` |
| راهرو | `CorridorSunset` → `SchoolCorridorAutumnSunset` | `CorridorWinterRainyNight` → `SchoolCorridorWinterRainyNight` | `CorridorSunsetSpring` → `SchoolCorridorSunset` |
| پشت‌بام | `RooftopDay` → `SchoolRooftopAutumnSunnyDay` | — | `RooftopSpringDay` → `SchoolRooftopSunnyDay` |
| نونوایی | `BakeryStreetDay` → `UsagiBakeryStreetAutumnDay` | `BakeryStreetWinterDay` → `UsagiBakeryStreetWinterDay` | `BakeryStreetSpringDay` → `UsagiBakeryStreetDay` |
| نبشِ دستگاه، روز | `VendingStreetDay` → `PastelStreetVendingAutumnDay` | — | `VendingStreetSpringDay` → `PastelStreetVendingDay` |
| نبشِ دستگاه، شب | `VendingStreetNight` → `PastelStreetVendingNight` (سپتامبر) | `VendingStreetWinterNight` → `PastelStreetVendingWinterNight` | — |
| کافه | `CafeDay` → `CozyCafeDay` · `CafeRainy` → `CozyCafeDimAutumnDimRainy` | — | `CafeRainySpring` → `CozyCafeDimRainy` |
| زمینِ بازی | `PlaygroundDay` → `PastelPlaygroundAutumnDay` | — | `PlaygroundSpringDay` → `PastelPlaygroundDay` |
| سکوی ایستگاه | `TrainPlatformSunset` → `TrainPlatformAutumnSunset` | `TrainPlatformWinterSunset` → `TrainPlatformWinterSunset` | `TrainPlatformSpringSunset` → `TrainPlatformSunset` |
| اتاقِ یوآ | `YuaRoomDay` → `YuaRoomAutumnDay` | `YuaRoomWinterDay` → `YuaRoomWinterDay` | `YuaRoomSpringDay` → `YuaRoomSunnyDay` |
| کوچه‌ی سنتی، شب | `AlleywayNight` → `TraditionalAlleywayNight` | — | — |
| اتاقِ هارو | `HaruRoomDay` → `HaruRoomSunnyDay` | — | — |

> ⚠️ **تله‌ی اسم، سه بار.** اسمِ فایل و اسمِ ثابت با هم نمی‌خوانند و این عمدی است:
> * فایلِ `TrainPlatformSunset.png` نسخه‌ی **بهاری** است؛ ثابتِ `TrainPlatformSunset` به `TrainPlatformAutumnSunset.png` می‌رود.
> * فایلِ `SunnyClassroomDay.png` نسخه‌ی **بهاری** است؛ ثابتِ `ClassroomDay` به `SunnyClassroomAutumnDay.png` می‌رود.
> * فایلِ `SchoolCorridorSunset.png` نسخه‌ی **بهاری** است؛ ثابتِ `CorridorSunset` به `SchoolCorridorAutumnSunset.png` می‌رود.
>
> **هیچ‌وقت از اسمِ فایل حدس نزن. همیشه ثابت را بخوان.**

### ۲.۲ چیزهایی که فقط در یک نسخه هستند — و از هرکدام می‌شود صحنه ساخت

این جدول با چشم از روی خودِ تصویرها درآمده. **قبل از نوشتنِ هر صحنه‌ای که به یک شیء اشاره می‌کند، اینجا را چک کن.**

| شیء | فقط در | یعنی چه |
|---|---|---|
| **دکه‌ی دانگو** | `TrainPlatformAutumnSunset` | در نسخه‌ی زمستان و بهارِ سکو دکه‌ای نیست. صحنه‌ی ۱۹ دسامبر روی سکوی زمستانی می‌گذرد، پس دانگو **دمِ گیت** خریده می‌شود، بیرون از کادر |
| **جا چتری** | `SchoolCorridorWinterRainyNight` | مدرسه جا چتری را زمستان می‌گذارد بیرون و پاییز — که آن هم بارون می‌آید — ورش می‌دارد. **هنوز ازش صحنه ساخته نشده و باید ساخته شود** |
| **نیمکتِ سنگیِ کوتاه کنارِ دستگاه** | `PastelStreetVendingNight` · `PastelStreetVendingWinterNight` · `PastelStreetVendingDay` | در `PastelStreetVendingAutumnDay` **نیست**. صحنه‌ی ۵ نوامبر به همین دلیل ایستاده نوشته شده و نه نشسته |
| **برف** | `PastelStreetVendingWinterNight` | بعد از بازطراحی برف اضافه شد، پس این تصویر **دسامبر به بعد** است، نه نوامبر |
| **تومو (گلدانِ لبِ پنجره)** | `SunnyClassroomDay` · `SunnyClassroomAutumnDay` | در نسخه‌ی بارانی دیده نمی‌شود. هر صحنه‌ای که تومو را نشان می‌دهد باید کلاسِ آفتابی باشد |
| **بچه‌ها روی سرسره** | `PastelPlaygroundAutumnDay` · `PastelPlaygroundDay` | صحنه‌ی شمردن تا پنج روی همین بچه‌ها بنا شده |
| **گربه‌های نیمکت** | هر سه نسخه‌ی نونوایی | زمستان هم هستند، روی برف. آنکو همیشه سرِ جایش است |
| **آینه‌ی ترک‌خورده** | هر سه نسخه‌ی اتاقِ یوآ | هیچ‌وقت در پرده‌های ۱ و ۲ اسم برده نمی‌شود. مالِ پرده‌ی پنجم است |
| **شبکه‌ی کفِ ته راهرو** | `SchoolCorridorAutumnSunset` · `SchoolCorridorSunset` | سکوتِ دومِ پرده‌ی دوم روی همین بنا شده |

---

## ۳. پس‌زمینه‌هایی که هنوز کشیده نشده‌اند

اسمشان در کد قفل شده است. فایل را با همین اسم داخل `Assets/Images/Backgrounds` بینداز و بدونِ هیچ تغییرِ کدی جایگزین می‌شود.

### پرده‌ی ششم — فلش‌بک

| ثابتِ کد | اسمِ فایل | چیست |
|---|---|---|
| `ElementaryClassroomDay` | `ElementaryClassroomDay.png` | کلاسِ دبستان، هشت سال قبل‌تر |
| `ElementaryHallwayDay` | `ElementaryHallwayDay.png` | راهروی بیرونش |
| `ElementaryYardDay` | `ElementaryYardDay.png` | حیاطی که شش سال هر روز در آن بازی کردند. **۷ بار استفاده می‌شود — پرکاربردترین پس‌زمینه‌ی پرده** |
| `RiverbankChildhoodDusk` | `RiverbankChildhoodDusk.png` | کنارِ رودخانه در راهِ برگشت |
| `SpiderLilyGardenDusk` | `SpiderLilyGardenDusk.png` | **باغچه‌ی سوسنِ عنکبوتیِ قرمز.** مهم‌ترین پس‌زمینه‌ی بازی — کلِ پرده‌ی سوم به آن اشاره می‌کند و دو نفر دو بار از گفتنِ دلیلش طفره می‌روند |
| `BackLaneDusk` | `BackLaneDusk.png` | کوچه‌ای که او را از آن بردند |
| `MachineRoomDoorDusk` | `MachineRoomDoorDusk.png` | بیرونِ درِ موتورخانه — جایی که پای هارو شکست |
| `MachineRoomDark` | `MachineRoomDark.png` | داخلش. یک بار، در تاریکی، **بدونِ هیچ چیزی در کادر** |

### بقیه

| ثابتِ کد | اسمِ فایل | چیست |
|---|---|---|
| `AlleywayAftermath` | `TraditionalAlleywayAftermath.png` | تنها پس‌زمینه‌ای که نسخه‌ی دیگری از یک پس‌زمینه است. همان دیوار و همان فانوس، با اینکه چیزی جلویشان اتفاق افتاده |
| — | `MainMenuChildhoodImage.png` | عکسِ بچگی‌شان کنارِ عروسکِ سالم. بعد از **هر** پایانی جای تصویرِ منوی اصلی می‌نشیند |

---

## ۴. فصل و برگ‌ریزان

بازی **از ۲ سپتامبر ۲۰۲۴ تا ۲۴ مارس ۲۰۲۵** می‌گذرد: پاییز، زمستان، اولِ بهار. **هیچ‌وقت فصلِ شکوفه‌ی گیلاس نیست.**

این تصادفی نیست و اسمِ پرده‌ی اول خودش آن را می‌گوید: **«سرابِ شکوفه‌های گیلاس».** شکوفه‌ای در کار نیست. شکوفه‌ها فقط در انتظارِ بازیکن از این ژانر وجود دارند.

### فصلِ هر پرده

| پرده | تاریخ | فصل | افکتِ ریزش |
|---|---|---|---|
| ۱ | سپتامبر ۲۰۲۴ | اوایلِ پاییز | برگِ پاییزی، **کم و پراکنده** |
| ۲ | اکتبر تا دسامبر ۲۰۲۴ | پاییزِ عمیق ← اوایلِ زمستان | برگِ پاییزی، پرتر · آخرِ پرده تقریباً هیچ |
| ۳ | از ۲۵ دسامبر ۲۰۲۴ | زمستان | بدونِ ریزش (یا برفِ خیلی سبک) |
| ۴ | از ۴ ژانویه ۲۰۲۵ | زمستان | بدونِ ریزش |
| ۵ | مارس ۲۰۲۵ | اولِ بهار | بدونِ ریزش تا قبل از شکستنِ قاب |
| ۶ | ژانویه ۲۰۱۷ | زمستان | بدونِ ریزش |
| **فلش‌بک‌های شاد / پایانِ رویا** | — | — | **شکوفه‌ی گیلاس** — تنها جایی که مجاز است |

### قانون

> **شکوفه‌ی گیلاس در این بازی یک دروغ است، نه یک فصل.**
> هر جا شکوفه ریخت، بازیکن دارد چیزی را می‌بیند که وجود ندارد — خاطره، رویا، یا سرابِ پرده‌ی اول.
> **هیچ‌کس هیچ‌وقت درباره‌ی این اشاره نمی‌کند.**

افکت ساخته شده و با بیتِ `Fall` کنترل می‌شود: `Guide-Engine.md` بخشِ ۱۵.۲.

---

## ۵. نقص‌های شناخته‌شده‌ی آرت

خلاصه‌شان اینجاست؛ شرحِ کامل و ترتیبِ کارشان در `Roadmap.md`.

| # | نقص | وضعیت |
|---|---|---|
| ۲ | پس‌زمینه‌ی کوچه‌ی شکوفه‌دار در سپتامبر | **برطرف شد** — `AutumnSchoolAlleyDay` |
| ۳ | `PastelStreetVendingDay` و `TrainPlatformSunset` شکوفه دارند و در پاییز استفاده می‌شدند | **برطرف شد** — نسخه‌ی پاییزی و زمستانیِ هر دو ساخته شد و پرده‌های ۱ و ۲ به آن‌ها منتقل شدند. نسخه‌ی شکوفه‌دار می‌ماند، برای خاطره و رویا |
| ۴ | اتاقِ هارو با کمد | **برطرف شد** |
| ۵ | هشت پس‌زمینه‌ی پرده‌ی ششم و اسپرایت‌های بچه هنوز کشیده نشده‌اند | باز |
| ۶ | **مرزِ کاراکتر و پس‌زمینه:** هر دو در یک کلیدِ رنگیِ پاستلی کشیده شده‌اند و ژاکتِ کرمِ هارو جلوی دیوارِ کرمِ کلاس لبه نداشت | **برطرف شد در کد، نه در آرت** — `StageSettings ▸ Separation`: پس‌زمینه کمی سردتر و تیره‌تر عقب می‌رود (`SceneryRecede`) و کاراکترها یک کانتورِ نرمِ تیره می‌گیرند (`CharacterEdge`). هر دو صفر می‌شوند و بازی دقیقاً مثلِ قبل می‌شود |
| ۷ | **برگ‌ها نمی‌ریختند.** لایه‌ی ریزش درست کار می‌کرد و از اولین `Place()` پرده‌ی اول زیرِ خودِ پس‌زمینه دفن می‌شد | **برطرف شد** — `VisualNovelStage.SwapBackgroundLayers` |
| ۸ | ریزشِ برگ در دو صحنه‌ی **داخلی** اجرا می‌شد: کافه‌ی پرده ۱ (۰٫۰۵) و راهروی نوامبرِ پرده ۲ (۰٫۴۲ — سنگین‌ترین ریزشِ کلِ بازی، زیرِ سقف) | **برطرف شد** — قانون: برگ فقط بیرون و فقط در روشنایی |
| ۹ | **صحنه‌ی راه‌پله تصویر نداشت** و روی عکسِ پشت‌بام اجرا می‌شد، در حالی که راوی می‌گفت روی پاگردِ بینِ طبقه‌ی سوم و دومند | **برطرف شد** — سه `…AfternoonSchoolStairs` ساخته شد و صحنه جدا شد |
| ۱۰ | همه‌ی فضاهای داخلی و بیرونیِ پرده‌های ۱ و ۲ نسخه‌ی **بهاری**شان را نشان می‌دادند (کلاس، پشت‌بام، نونوایی، کافه، زمینِ بازی، اتاقِ یوآ) | **برطرف شد** — نسخه‌ی پاییزی و زمستانیِ همه ساخته شد و ثابت‌ها به آن‌ها نقطه‌زنی شدند |

---

## ۶. آرتی که هنوز لازم است

| # | فایل | چرا |
|---|---|---|
| ۱ | `PastelStreetVendingAutumnNight.png` | **تنها موردِ ضروری.** نبشِ دستگاه در شبِ پاییز: باغچه‌ها ساقه‌ی خشک، **بدونِ برف**، نیمکتِ سنگی سرِ جایش. صحنه‌ی ۵ نوامبر به این نیاز دارد. الآن روی `PastelStreetVendingAutumnDay` (غروبِ طلایی) نوشته شده که کار می‌کند ولی صحنه واقعاً شب می‌خواهد |
| ۲ | `SunnyClassroomWinterDay.png` | خوب است، ضروری نیست. صحنه‌ی ۱۹ دسامبر در کلاس می‌گذرد و نسخه‌ی زمستانیِ کلاس وجود ندارد؛ فعلاً نسخه‌ی پاییزی استفاده می‌شود |
