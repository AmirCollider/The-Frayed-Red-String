# راهنمای موتور

**نخِ قرمزِ پوسیده** — کد چطور چیده شده، یک پرده چطور ساخته می‌شود، کات‌سین‌ها چطور کار می‌کنند، و چطور از بازی خروجی بگیری.

> این فایل جایگزینِ `Guide-CSharp.md`، `Guide-Cutscenes.md`، `Guide-StoryEditor.md` و `StoryEditor.md` است.
> **اینجا فقط «چطور»ی موتور است.** «چه» در `Acts.md` و سند روایی است، و «چطور بنویسی» در دستورنامه‌ی دیالوگ.

---

## ۰. قبل از هر چیز

**این مخزن کامپایل نمی‌شود.** اینجا فقط `_C#/` است — آینه‌ی `Assets/_C#/` در پروژه‌ی واقعیِ یونیتی. `Assets/`، `.meta`، `ProjectSettings/`، صدا و ویدیو اینجا نیستند.

یعنی:

* هر تغییرِ کد اینجا **تست‌نشده** است و باید داخل یونیتی اجرا شود.
* تغییرِ کوچک و قابلِ بازبینی بنویس. بازنویسیِ بزرگِ تست‌نشده، هدیه نیست.
* اگر چیزی را نمی‌توانی ثابت کنی، **بنویس که نتوانستی.**

### Story Editor

یک پنجره‌ی گرافیکی داخلِ یونیتی است (`_C#/Editor/StoryEditorWindow.cs`) که پرده‌ها را با ماوس ویرایش می‌کند. **برای انسان است، نه برای مدل.**

| کی هستی | پرده را کجا می‌نویسی |
|---|---|
| انسان، می‌خواهی یک خط را عوض کنی | `The Frayed Red String ▸ Story Editor` ▸ تب Script |
| مدل / از طریق کد | `_C#/Editor/ActNNBuilder.cs` — **این مسیرِ درست است** |

هر دو به یک asset می‌رسند (`Assets/Story/Acts/ActNN.asset`).

> ⚠️ **این دو مسیر همدیگر را پاک می‌کنند.** اگر کسی داخلِ Story Editor دیالوگی را دستی عوض کرده باشد، زدنِ `Build Act NN From The Story Document` کارش را از بین می‌برد.
> `ActAsset.GeneratedSignature` دقیقاً برای همین است: اثرِ انگشتِ اسکریپت، کنارِ بیت‌ها ذخیره می‌شود. اگر با بیت‌های روی دیسک بخواند، کسی دست نزده و بازسازی امن است.

---

## فهرست

1. [نقشه‌ی کد](#۱-نقشهی-کد)
2. [بازی چطور بالا می‌آید](#۲-بازی-چطور-بالا-میآید)
3. [شش قانونِ این کدبیس](#۳-شش-قانونِ-این-کدبیس)
4. [واژگانِ یک پرده — بیت‌ها](#۴-واژگانِ-یک-پرده--بیتها)
5. [نوشتنِ یک پرده با کد](#۵-نوشتنِ-یک-پرده-با-کد)
6. [اضافه کردنِ چیزهای جدید](#۶-اضافه-کردنِ-چیزهای-جدید)
7. [کات‌سین‌ها](#۷-کاتسینها)
8. [پرده‌ی صورتی، خون، و قاب](#۸-پردهی-صورتی-خون-و-قاب)
9. [جای کاراکترها](#۹-جای-کاراکترها)
10. [پایان‌ها و پنج دقیقه](#۱۰-پایانها-و-پنج-دقیقه)
11. [منوی بچگی](#۱۱-منوی-بچگی)
12. [آماده‌سازی، خروجی، تست](#۱۲-آمادهسازی-خروجی-تست)
13. [نقاطِ اتصال](#۱۳-نقاطِ-اتصال)
14. [پیوست — عددها](#۱۴-پیوست--عددها)

---

## ۱. نقشه‌ی کد

`Assets/_C#/` هیچ asmdef ندارد، پس همه‌چیز داخلِ `Assembly-CSharp` است و پوشه‌ی `Editor/` داخلِ `Assembly-CSharp-Editor`.

| پوشه | مسئولِ چیست |
|---|---|
| `Core/` | راه‌اندازی، ثابت‌ها، اسم‌ها، ساعتِ داستان |
| `Narrative/` | داده‌ی داستان و کارگردانش |
| `Presentation/` | صحنه، قاب، فونت، پرده‌ی محو، انتقالِ صحنه |
| `UI/` | باکسِ دیالوگ، انتخاب‌ها، منوی توقف، پنلِ Save |
| `Localization/` | زبان‌ها و جدولِ رشته‌ها |
| `Motion/` | حرکتِ آرامِ همه‌چیز |
| `Tweening/` | موتورِ انیمیشن |
| `Audio/` | سنتزِ صدا و پخش |
| `SaveSystem/` | خواندن و نوشتنِ اسلات‌ها |
| `Scenes/` | یک کنترلر برای هر نوع صحنه |
| `Flow/` | فعل‌های بازی: شروع، لود، رفتن به پرده‌ی بعد |
| `Input/` | تنها جایی که با Input System حرف می‌زند |
| `Editor/` | پنجره‌ی Story Editor، سازنده‌های پرده و Library، ابزارهای تست |

### فایل‌هایی که بیشتر از همه سراغشان می‌روی

| فایل | چیست |
|---|---|
| `Core/GameConfig.cs` | **هر عددِ جادوییِ بازی.** سرعت‌ها، اندازه‌ی فونت‌ها، مدت‌ها |
| `Core/ObjectNames.cs` | اسمِ هر GameObject ای که کد به آن وصل است |
| `Core/SceneNames.cs` | اسمِ صحنه‌ها |
| `Narrative/StoryEnums.cs` | انواعِ بیت، لحنِ انتخاب |
| `Narrative/ActAsset.cs` | ساختارِ داده‌ی یک پرده |
| `Narrative/StoryDirector.cs` | حلقه‌ای که پرده را اجرا می‌کند |
| `Narrative/CharacterArt.cs` | `Speaker`، `Portrait`، و ثابت‌های `Backgrounds` |
| `Narrative/StageSettings.cs` | جای کاراکترها، اندازه‌ی قاب، پرده‌ی صورتی |
| `Localization/LocKeys.cs` + `LocalizationDatabase.cs` | متنِ رابطِ کاربری |
| `Audio/SfxId.cs` + `ProceduralSfxLibrary.cs` | صداها |
| `Editor/ActScriptWriter.cs` | فعل‌هایی که یک پرده با آن‌ها نوشته می‌شود |

---

## ۲. بازی چطور بالا می‌آید

هیچ‌چیزی داخلِ هیچ Scene ای وصل نشده. همه‌اش کد است:

```
[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]
GameBootstrap.ResetStatics()
    └─ هر static ای را صفر می‌کند (چون یونیتی می‌تواند domain reload را رد کند)

[RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
GameBootstrap.Initialize()
    ├─ LocalizationService.Initialize()
    ├─ ServiceHost = یک GameObject با DontDestroyOnLoad
    ├─ TweenRunner، AudioService، MusicService، ScreenFader،
    │  GlobalPointerSfx، LocalizationRefresher روی آن نصب می‌شوند
    └─ SceneInstaller.Enable()      ← به sceneLoaded گوش می‌دهد

[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]
GameBootstrap.InstallFirstScene()

هر بار که یک Scene لود می‌شود:
SceneInstaller.InstallInto(scene)
    ├─ EventSystem و AudioListener را تضمین می‌کند
    ├─ StoryClock.IsPaused = false
    ├─ StoryFonts.ForgetTemplate() + CompleteAuthoredFonts()
    ├─ AmbientMotionInstaller.Install(scene)     ← به همه‌چیز حرکت می‌دهد
    ├─ Light2DAmbientPulse.InstallAll(scene)
    ├─ SceneAudioInstaller.Install(scene)
    ├─ AttachSceneController(scene)              ← طبقِ اسمِ Scene
    └─ LocalizationRefresher.Settle()
```

بعدش، داخلِ یک صحنه‌ی پرده:

```
ActSceneController.Start()
    ├─ ActNumber را از اسمِ Scene می‌خواند
    ├─ ActLibrary.Find(ActNumber)
    ├─ BackgroundCanvas را پیدا/می‌سازد
    ├─ StoryCanvas را می‌سازد (sortingOrder 100)
    │   ├─ VisualNovelStage       ← کاراکترها
    │   ├─ AdvanceCatcher         ← کلیک = خطِ بعد
    │   ├─ StorySafeArea          ← هر چیزی که خوانده می‌شود
    │   │   ├─ DialogueBoxView
    │   │   ├─ StoryOverlayView
    │   │   └─ ChoicePanelView
    │   └─ StoryFrameView         ← چهار نوار، صاحبِ SafeArea
    ├─ PauseCanvas را می‌سازد (sortingOrder 500)
    └─ StoryDirector.Play(act, StorySession.ConsumeResumeLine())
```

**ترتیبِ Canvas ها** در `GameConfig`:

```csharp
BackgroundCanvasOrder = 0;
StoryCanvasOrder      = 100;
PauseCanvasOrder      = 500;
FilmCanvasOrder       = 700;
LetterboxCanvasOrder  = 20000;
// و پرده‌ی محوِ صفحه روی 30000
```

---

## ۳. شش قانونِ این کدبیس

اگر این شش تا را بلد باشی، هر چیزی اضافه کنی با بقیه جور در می‌آید.

### قانون ۱ — `AmbientMotion` تنها صاحبِ transform است

هیچ‌چیزی در این کد `localPosition` یا `localScale` یا `localRotation` شیئی را که `AmbientMotion` دارد نمی‌نویسد. هر سیستمی که می‌خواهد چیزی را تکان بدهد، از کانال‌های آن استفاده می‌کند:

```csharp
motion.ExtraOffset   = new Vector3(0f, -26f, 0f);
motion.ExtraScale    = new Vector3(0.94f, 0.94f, 1f);
motion.ExtraRotation = 3f;
motion.Weight        = 0f;   // فریز کردنِ حرکتِ آرام، بدونِ خاموش کردنِ کامپوننت
```

برای همین است که ورودِ یک کاراکتر، نفس کشیدنش و خم شدنش برای حرف زدن، همزمان و بدونِ تداخل کار می‌کنند.

**استثنا:** اگر والد یک `LayoutGroup` داشته باشد، `AmbientMotion` خودکار کانالِ موقعیت را رها می‌کند (`BorrowsPosition`). دو نویسنده روی یک `anchoredPosition` همان چیزی بود که کلِ منوی توقف را روی هم انباشت.

### قانون ۲ — داستان روی `StoryClock` می‌چرخد، رابطِ کاربری روی زمانِ واقعی

```csharp
StoryClock.IsPaused                 // منوی توقف این را ست می‌کند
StoryClock.DeltaTime                // موقعِ توقف صفر برمی‌گرداند
yield return StoryClock.Wait(2f);   // به‌جای WaitForSecondsRealtime
```

برای تویین‌ها:

```csharp
TweenRunner.Play(duration, t => { … }, EaseType.OutCubic,
    delay: 0f, onComplete: null, owner: this,
    useUnscaledTime: true,
    pausesWithStory: true);   // ← هر چیزی که تصویر را تکان می‌دهد
```

`pausesWithStory: false` (پیش‌فرض) برای پرده‌ی محوِ صفحه، منوی توقف و حرکتِ آرام است. بازیِ متوقفی که منوی متوقف‌کننده‌اش هم یخ بزند، شبیهِ کرش است.

`Time.timeScale` اینجا کار **نمی‌کند** — همه‌چیز عمداً روی زمانِ Unscaled است.

### قانون ۳ — متن فقط از یک در رد می‌شود

```csharp
StoryText.Set(label, text);
```

سه کار می‌کند: متن را می‌نویسد، وزنِ فونت را طبقِ زبان تنظیم می‌کند (لاتین بولد، فارسی نه)، و فونت را **همان فریم** برای زبانِ جدید می‌سازد.

آن آخری اجباری است. `DirectFont` زبان را از `LateUpdate` تشخیص می‌دهد، پس بدونِ این، یک label که متنش از انگلیسی به فارسی عوض شده همان فریم با فونتِ انگلیسی کشیده می‌شود و دیگر هیچ‌وقت اصلاح نمی‌شود.

هر label ساخته‌شده در کد، اول باید فونت بگیرد:

```csharp
StoryFonts.Apply(label);   // بعد از AddComponent<TextMeshProUGUI>()
```

### قانون ۴ — رابطِ کاربری در کد ساخته می‌شود، نه در Scene

هیچ prefab ای نیست. `DialogueBoxView`، `PauseMenuView`، `ChoicePanelView` همه خودشان را از `new GameObject(...)` و شکل‌های کشیده‌شده در `ProceduralUiSprites` می‌سازند.

```csharp
image.sprite = ProceduralUiSprites.RoundedRect(26, fill, border, 3f);
image.type = Image.Type.Sliced;
```

برای همین یک پرده‌ی جدید فقط یک Scene با دوربین است.

### قانون ۵ — چیزهایی که موقعِ اجرا پیدا می‌شوند، با اسم پیدا می‌شوند

هر اسمی که کد به آن وابسته است داخلِ `ObjectNames.cs` است. اگر GameObject ای را داخلِ Scene عوضِ اسم کردی، **فقط همان فایل** باید دنبالش برود.

```csharp
Transform found = UnityUtility.FindInScene(ObjectNames.BackgroundCanvas);
Image bg = UnityUtility.FindDeep<Image>(layer, ObjectNames.StoryBackground);
```

### قانون ۶ — Library ها خودکار ساخته می‌شوند

`StageSpriteLibrary`، `MusicLibrary`، `UiSpriteLibrary`، `ActLibrary` همه asset هایی داخلِ `Resources/TFRS` هستند که سازنده‌های ادیتور از روی محتوای پوشه‌ها می‌سازند. یک فایلِ جدید داخلِ `Assets/Images/Backgrounds` بیندازی، خودش قابلِ استفاده می‌شود.

```csharp
Sprite bg      = StageSpriteLibrary.Load().FindBackground("CozyCafeDay");
AudioClip song = MusicLibrary.Load().Find("MainMenuBackGrungMusic");
ActAsset act   = ActLibrary.Find(3);
```

---

## ۴. واژگانِ یک پرده — بیت‌ها

یک پرده یک `ActAsset` است: یک عنوان، یک آهنگ، و یک فهرست از `BeatData`. هر بیت یک دستور است.

### `StoryBeatKind` — کامل، با عددها

> **عددها ثابت‌اند و باید باشند.** یونیتی یک enum سریالایزشده را به‌صورتِ عددش ذخیره می‌کند، پس asset روی دیسک `14` دارد نه `End`. اضافه کردنِ یک مقدار در **وسطِ** این enum، هر پرده‌ی ذخیره‌شده را بی‌سروصدا بازنویسی می‌کند.
> این یک بار اتفاق افتاده: چهار نوعِ پرده‌ی پنجم وسطِ enum اضافه شدند و هر بیتِ `End` پروژه تبدیل شد به `PullDownDialogue`.
> با نوشته شدنِ عددها، **ترتیبِ فایل آزاد است و نوعِ جدید یعنی عددِ جدید در انتها.** بزرگ‌ترین عددِ فعلی **۲۴** است.

| # | `StoryBeatKind` | کارش | منتظرِ بازیکن می‌ماند؟ |
|---|---|---|---|
| ۰ | `Line` | یک خطِ دیالوگ یا روایت | ✅ |
| ۱ | `Background` | عوض کردنِ مکان، با کراس‌فید | ⏱ منتظرِ انتقال |
| ۲ | `Enter` | آوردنِ کاراکتر / عوض کردنِ چهره | ❌ |
| ۳ | `Exit` | بردنِ یک کاراکتر بیرون | ❌ |
| ۴ | `ClearStage` | بردنِ همه بیرون | ❌ |
| ۵ | `Caption` | نوشتنِ اسمِ مکان در گوشه | ❌ |
| ۶ | `TitleCard` | کارتِ عنوانِ پرده | ⏱ |
| ۷ | `Sound` | پخشِ یک افکتِ صوتی به‌تنهایی | ❌ |
| ۸ | `Music` | شروعِ آهنگ (خالی = قطع با فِید) | ❌ |
| ۹ | `Beat` | سکوت روی تصویر، با باکسِ محوشده | ⏱ |
| ۱۰ | `Choice` | انتخاب | ✅ |
| ۱۱ | `Interlude` | پخشِ یک پرده‌ی کوچک، با احتمال. یک بار تاس می‌خورد | — |
| ۱۲ | `OpenFrame` | باز شدنِ قاب | ⏱ |
| ۱۳ | `CloseFrame` | برگشتنِ قاب | ❌ |
| ۱۴ | `End` | پایانِ پرده | — |
| ۱۵ | `PullDownDialogue` | هارو باکسِ دیالوگ را با دست پایین می‌کشد | ⏱ |
| ۱۶ | `Grade` | شدتِ پرده‌ی صورتیِ روی تصویر (۱ = پرده‌ی ۱ تا ۴، ۰ = بدونِ پرده) | ⏱ |
| ۱۷ | `Stain` | پاشیدنِ یک رنگ روی کلِ صفحه و ماندنش | ⏱ |
| ۱۸ | `CutMusic` | قطعِ آهنگ در یک فریم، بدونِ هیچ فِیدی | ❌ |
| ۱۹ | `EnterCinema` | از اینجا داستان خودش پیش می‌رود؛ بازیکن نمی‌تواند رد کند | ❌ |
| ۲۰ | `ExitCinema` | کنترل را به بازیکن برمی‌گرداند | ❌ |
| ۲۱ | `Video` | پخشِ یک فیلمِ تمام‌صفحه روی همه‌چیز | ⏱ |
| ۲۲ | `EndGame` | بازی تمام شد: سیوها پاک و برگشت به منو | ❌ |
| ۲۳ | `EnterAside` | کاراکتر از صحنه می‌بُرد و با **بازیکن** حرف می‌زند | ❌ |
| ۲۴ | `ExitAside` | صحنه را به اتاق برمی‌گرداند | ❌ |

### فیلدهای مهمِ `BeatData`

| فیلد | برای چه نوعی | نکته |
|---|---|---|
| `Speaker` · `Portrait` | `Line` · `Enter` · `Exit` | `Portrait.Unchanged` یعنی هرچه روی صفحه است بماند |
| `Text` | `Line` | `LocalizedLine` — هر سه زبان کنارِ هم. خالی بودنِ ترجمه = برگشت به انگلیسی، نه کرش |
| `Background` · `Caption` · `FadeSeconds` | `Background` | `FadeSeconds` منفی = کراس‌فیدِ معمول. **صفر یک جوابِ واقعی است** (برش)، برای همین منفی است نه صفر |
| `MusicTrack` | `Music` | خالی = قطع با فِید |
| `PlaySound` · `Sound` · `SoundVolume` | هر نوعی | صدا روی همان فریمِ همان بیت سوار می‌شود |
| `Seconds` | `Beat` · `Grade` · `Stain` · `OpenFrame` | مدت |
| `VoiceClip` | `Line` | اسمِ فایل در `Assets/Audio/Voice`، بدونِ پسوند. فایلِ غایب = تایپ‌رایتر + یک خط در Console |
| `Film` | `Video` | اسم بدونِ پسوند |
| `TypeSpeed` | `Line` | حرف در ثانیه. `0` = سرعتِ معمول. `~140` = سرازیر شدن |
| `Amount` · `Tint` | `Grade` · `Stain` | |
| `Choices[]` | `Choice` | هر گزینه `Text`، `Tone` و `BranchLength` دارد |
| `Interlude` · `Chance` | `Interlude` | |
| **`MeasurePatience`** | `Line` | این خط یکی از سکوت‌های بازی است ← بخش ۱۰ |
| **`YuaOverridesKindness`** · `OverrideLine` | `Choice` | یوآ گزینه‌ی آبی را رد می‌کند ← بخش ۱۰ |
| `Note` | همه | یادداشت برای خودت. هیچ‌وقت به بازیکن نشان داده نمی‌شود |

### `ChoiceTone`

| | رنگ | شمرده می‌شود؟ |
|---|---|---|
| `Kind` | آبی — مهربان و صادقانه | ✅ به‌عنوان آبی، **حتی وقتی یوآ ردش می‌کند** |
| `Cruel` | سبز — کنترل‌گر | ✅ |
| `Neutral` | سفید — بی‌وزن | ❌ اصلاً شمرده نمی‌شود |

### `BranchLength` — انتخابی که واقعاً جواب می‌دهد

`0` (پیش‌فرض) یعنی انتخاب هیچ‌چیز را عوض نمی‌کند. وقتی ست شود، بلوک‌های گزینه‌ها پشتِ سرِ هم بلافاصله بعد از بیتِ انتخاب می‌آیند، به همان ترتیبی که گزینه‌ها فهرست شده‌اند. انتخابِ گزینه‌ی *k* بلوکِ *k* را پخش می‌کند و بقیه را رد می‌کند، و داستان در اولین بیتِ بعد از آخرین بلوک به خودش می‌پیوندد.

> **یک بلوک فقط می‌تواند `Line` داشته باشد.** این محدودیت کارِ واقعی می‌کند: صحنه، پس‌زمینه، آهنگ و قاب هنگامِ لودِ یک سیو با پخشِ دوباره‌ی همه‌ی دستورهای صحنه‌ایِ قبل از نقطه‌ی برگشت، با مدتِ صفر، بازسازی می‌شوند. بلوکی که کسی را جابه‌جا کند یا تصویر را عوض کند، چه انتخاب شده باشد چه نه، پخشِ دوباره می‌شود — و سیوی که بعد از یک انتخاب گرفته شده با صحنه‌ی مسیرِ دیگر برمی‌گردد.

---

## ۵. نوشتنِ یک پرده با کد

### شکلِ یک Builder

`Editor/ActScriptWriter.cs` پایه است. هر پرده سه چیز است:

```csharp
public sealed class Act06Builder : ActScriptWriter
{
    protected override int ActNumber => 6;
    protected override string AssetName => "Act06";
    protected override LocalizedLine Title => L("Rotten Roots", "腐った根", "ریشه‌های پوسیده");

    [MenuItem("The Frayed Red String/Build Act 06 From The Story Document")]
    public static void Build() { new Act06Builder().BuildAsset(); }

    protected override void Write()
    {
        Place(Backgrounds.ElementaryYardDay, "Eight years earlier", "八年前", "هشت سال قبل");
        Hold(2f);
        Enter(Speaker.YuaChild, Portrait.Neutral);
        Say(Speaker.YuaChild, Portrait.Joyful, "You came.", "来てくれたんだ。", "آمدی.");
    }
}
```

`BuildAsset()` بقیه‌اش را انجام می‌دهد: asset را می‌نویسد (اگر از قبل باشد **همان** را استفاده می‌کند تا GUID عوض نشود و هیچ ارجاعی نشکند)، اگر بیت داشته باشد اول می‌پرسد، و آخرش `Rebuild Act Library` را صدا می‌زند.

### فعل‌ها — همه در `ActScriptWriter`

| فعل | بیت |
|---|---|
| `Place(bg, en, ja, fa)` | `Background` + اسمِ مکان |
| `CutTo(bg)` | برشِ آنی، بدونِ فِید و بدونِ صدای زنگ و بدونِ اسمِ مکان |
| `Say(who, face, en, ja, fa)` | `Line` |
| `SayWithSound(who, face, sfx, vol, en, ja, fa)` | `Line` + صدا روی همان فریم |
| `SayVoiced(who, face, clip, en, ja, fa)` | `Line` + اسمِ فایلِ صدا |
| `Narrate(en, ja, fa)` | `Line` با گوینده‌ی `Narrator` |
| `Listen(en, ja, fa)` | `Narrate` + `MeasurePatience` |
| `Hold(seconds)` | `Beat` سکوت |
| `Enter(who, face)` · `Exit(who)` · `ClearStage()` | صحنه |
| `Cel(...)` | توالیِ فریم‌های بنتو و نوشیدنی |
| `Cue(sfx, vol)` | `Sound` تنها |
| `SetMusic(track)` · `StopMusic()` · `CutMusic()` | آهنگ |
| `Decide(blue×3, green×3, refusal×3)` | `Choice` با اورراید یوآ |
| `DecideIdly(a×3, b×3)` | `Choice` سفید، بی‌وزن و شمرده‌نشده |
| `Maybe(LegAche, 0.3f)` | `Interlude` تصادفی |
| `PullDownDialogue(s)` · `OpenFrame(s)` · `CloseFrame()` | شکستنِ رابط |
| `Grade(amount, s)` | پرده‌ی صورتی |
| `Stain(colour, s)` · `ClearStain(s)` | خون |
| `BeginFilm()` · `EndFilm()` | حالتِ فیلم |
| `PlayFilm(name)` | فیلمِ تمام‌صفحه |
| `WriteClosingWords()` | دو جمله‌ی پایانیِ سند |
| `EndGame()` | پاک کردنِ سیوها و برگشت به منو |

`LegAche` و `MachineRoom` خودشان از `Assets/Story/Acts` لود می‌شوند؛ نبودنشان فقط یک Warning است.

> `Maybe(...)` را همیشه **درست قبل از عوض شدنِ صحنه** بگذار. Interlude یک Act است و می‌تواند پس‌زمینه و کاراکترِ خودش را بگذارد؛ هرچه جا بگذارد، بیت‌های بعدی جایگزینش می‌کنند.

### چند نکته که زود به دردت می‌خورند

* **`Enter` با `Portrait.Unchanged` هیچ‌کس را نمی‌آورد.** یک چهره انتخاب کن.
* **`Line` هم می‌تواند چهره عوض کند.** لازم نیست قبل از هر خط یک `Enter` بگذاری.
* **`Beat` (سکوت) کم‌استفاده‌ترین و مهم‌ترین ابزار است.** دو ثانیه سکوت حرفی را می‌زند که یک خطِ روایت خرابش می‌کند.
* **`End` اجباری نیست.** پرده که تمام شود، تمام است. `End` برای وقتی است که می‌خواهی چند بیت را موقتاً از دسترس خارج کنی.

### ساختنِ یک پرده از صفر، بدونِ `ActScriptWriter`

```csharp
ActAsset act = ScriptableObject.CreateInstance<ActAsset>();
act.ActNumber = 3;
act.Title = new LocalizedLine("Serenity", "静けさ", "آرامش");
act.MusicTrack = "Act03Theme";
act.ShowTitleCard = true;

act.Beats.Add(new BeatData
{
    Kind = StoryBeatKind.Background,
    Background = Backgrounds.CafeRainy,
    Caption = new LocalizedLine("The café", "喫茶店", "کافه")
});

act.Beats.Add(new BeatData
{
    Kind = StoryBeatKind.Line,
    Speaker = Speaker.Yua,
    Portrait = Portrait.Neutral,
    Text = new LocalizedLine("You came.", "来てくれたんだ。", "آمدی."),
    PlaySound = true,
    Sound = SfxId.Heartbeat,
    SoundVolume = 0.6f
});

AssetDatabase.CreateAsset(act, "Assets/Story/Acts/Act03.asset");
AssetDatabase.SaveAssets();
TheFrayedRedString.EditorTools.StoryAssetBuilder.Rebuild();   // تا ActLibrary ببیندش
```

> `SfxId` داخلِ `TheFrayedRedString.Audio` است — `using` اش را اضافه کن.

---

## ۶. اضافه کردنِ چیزهای جدید

### ۶.۱ یک نوعِ بیتِ جدید

مثال: `Shake` — لرزاندنِ صفحه.

**قدم ۱** — `Narrative/StoryEnums.cs`. **عددِ جدید در انتها**، هیچ‌وقت وسط:

```csharp
/// <summary>لرزاندنِ تصویر.</summary>
Shake = 25,
```

**قدم ۲** — اگر فیلدِ جدیدی لازم دارد، `Narrative/ActAsset.cs` داخلِ `BeatData`. اگر `Seconds` کافی است، چیزی اضافه نکن.

**قدم ۳** — `Narrative/StoryDirector.cs`، داخلِ `PlayBeat`:

```csharp
case StoryBeatKind.Shake:
    yield return ShakeRoutine(beat.Seconds);
    break;
```

```csharp
private IEnumerator ShakeRoutine(float seconds)
{
    // از کانالِ حرکت استفاده کن، نه از transform — قانون ۱
    AmbientMotion motion = AmbientMotion.GetOrAdd(_stage.gameObject);

    TweenRunner.Play(
        seconds,
        t =>
        {
            float fade = 1f - t;
            motion.ExtraOffset = new Vector3(Mathf.Sin(t * 90f) * 18f * fade, 0f, 0f);
        },
        EaseType.Linear, 0f,
        () => motion.ExtraOffset = Vector3.zero,
        this,
        true,
        true);   // ← pausesWithStory

    yield return StoryClock.Wait(seconds);
}
```

**قدم ۴ — اگر بیت روی صحنه اثر می‌گذارد**، باید داخلِ `ReplayStageBeats` هم یک `case` بگیرد (همان سوییچِ دومِ `StoryDirector` حولِ خط ۱۲۷۰). وگرنه لودِ یک سیو، بازی را با وضعیتِ غلط برمی‌گرداند.

**قدم ۵** — `Editor/StoryEditorWindow.cs`، سه جا، تا انسان هم بتواند ویرایشش کند:

```csharp
// SummaryOf — چه چیزی داخلِ لیست نوشته شود
case StoryBeatKind.Shake:
    return $"≈ shake {beat.FindPropertyRelative("Seconds").floatValue:0.0}s";

// DescriptionOf — جمله‌ای که زیرِ dropdown می‌آید
case StoryBeatKind.Shake:
    return "Shake the picture. Waits for the shake, not for the player.";

// DrawBeatDetail — فیلدهایش
case StoryBeatKind.Shake: DrawSeconds(beat, "For"); break;

// TintFor — رنگش در لیست
case StoryBeatKind.Shake: return FlowTint;
```

اگر حالتِ خرابی دارد، یک `case` هم داخلِ `Check()` بگذار.

**قدم ۶** — یک فعل در `Editor/ActScriptWriter.cs`، تا پرده‌ها بتوانند از آن استفاده کنند.

### ۶.۲ یک صدای جدید

هیچ صدایی فایل نیست — همه سنتز می‌شوند. **هیچ‌وقت اسمِ فایل نده؛ دستورِ ساخت بده.**

**قدم ۱** — `Audio/SfxId.cs`، آخرِ enum:

```csharp
/// <summary>شکستنِ شیشه، پرده‌ی پنجم.</summary>
GlassBreak
```

**قدم ۲** — `Audio/ProceduralSfxLibrary.cs`، داخلِ `Build`:

```csharp
case SfxId.GlassBreak:
    return ProceduralAudioSynth.Render(
        name: "GlassBreak",
        voices: new[]
        {
            ProceduralAudioSynth.Voice.Noise(
                startTime: 0f, duration: 0.35f, amplitude: 0.7f, lowPassHz: 6000f),
            ProceduralAudioSynth.Voice.Bell(
                startTime: 0.01f, duration: 0.9f, frequency: C7, amplitude: 0.4f),
        },
        reverb: 0.25f);
```

> امضای دقیقِ `Voice` را از داخلِ `ProceduralAudioSynth.cs` بخوان — چند شکل دارد (سینوس، ناقوس، نویز، ضربه).

**قدم ۳** — هیچی. `WarmUpStory()` روی کلِ enum حلقه می‌زند.

**تست:** `The Frayed Red String ▸ Export Sound Effects To WAV` و به فایلش گوش بده.

> دستورنامه بخشِ «صدا» یک جدولِ کاملِ پارامتر دارد: هومِ موتورخانه، باد، بلیپ‌ها، زنگِ مدرسه، باران. **از آنجا بردار، از حافظه نساز.**

### ۶.۳ یک کاراکترِ جدید

**قدم ۱** — تصویرها با همان الگوی اسم، `1200 × 2400`، در `Assets/Images/Characters/<Name>/`.

**قدم ۲** — `Narrative/CharacterArt.cs`: مقدارِ جدید **آخرِ** `Speaker`، بعد `SpriteName`، `NameKey` و `HomeSide`.

**قدم ۳** — اسمش در `LocKeys.cs` و `LocalizationDatabase.cs`.

**قدم ۴** — رنگِ پلاکش در `UI/DialogueBoxView.cs` (`PlateColour`).

**قدم ۵ — مهم:** `Presentation/VisualNovelStage.cs` فقط **دو** جایگاه دارد (چپ و راست). کاراکترِ سوم یعنی یک `Slot` سوم و دست بردن در `SlotFor`، `SetUpCharacters` و `SetFocus`.

> اگر کاراکترِ جدید هیچ‌وقت همزمان با دیگری روی صحنه نیست، سرش را نتراش — از همان جایگاهِ چپ استفاده کن.
> و اگر اصلاً بدن لازم ندارد (مثلِ `Classmate`)، `SpriteName` برایش `null` برگرداند: صحنه خودش کارِ درست را می‌کند.

**قدم ۶** — `Rebuild Stage Sprite Library`.

### ۶.۴ یک زبانِ جدید

**قدم ۱** — `Localization/GameLanguage.cs`. **هیچ‌وقت وسطِ enum نه** — عددها در `PlayerPrefs` ذخیره شده‌اند. بعد `Next()` و `IsRightToLeft()`.

هر چیزی که چیدمان دارد — ترازِ متن، سمتِ پلاکِ نام، گوشه‌ی اسمِ مکان، وزنِ فونت — روی `IsRightToLeft` شاخه می‌زند، نه روی خودِ زبان. پس آن یک متد کلِ چیدمان را درست می‌کند.

**قدم ۲** — `LocalizedLine` در `ActAsset.cs` و `LocEntry` در `LocalizationDatabase.cs` یک فیلدِ جدید می‌خواهند.

**قدم ۳** — پرچمش در `UI/UiSpriteLibrary.cs` (`FlagFor`) و تصویرش در `Assets/Images/UI`.

**قدم ۴** — فونتش در `StoryFonts`.

**قدم ۵** — `DrawLocalized` در Story Editor آرایه‌ی سه‌تاییِ زبان‌ها را hardcode کرده؛ به `Enum.GetValues` تبدیلش کن.

### ۶.۵ یک صفحه‌ی جدید

الگویی که همه‌ی صفحه‌های موجود از آن پیروی می‌کنند:

```csharp
public void Initialize(RectTransform layer)
{
    transform.SetParent(layer, false);
    Stretch((RectTransform)transform);

    // گروه روی خودِ این شیء، نه روی Canvas مشترک — وگرنه هر چیزِ
    // دیگری که بعداً داخلِ آن Canvas منتقل شود با این محو می‌شود.
    _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);

    BuildVeil();
    BuildCard();

    _group.alpha = 0f;
    _group.blocksRaycasts = false;
    _group.interactable = false;
}
```

و برای هر label:

```csharp
StoryFonts.Apply(label);                                   // قانون ۳
UnityUtility.GetOrAdd<LocalizedText>(labelHost).Bind(LocKeys.MenuSettings);
```

**دام‌هایی که قبلاً افتاده‌ایم:**

* **دکمه‌ای داخلِ `LayoutGroup` که `AmbientMotion` دارد** → `BorrowsPosition`. وگرنه همه روی هم می‌افتند.
* **`ContentSizeFitter` روی چیزی که والدش `LayoutGroup` دارد** → دو سیستم روی یک rect.
* **`CanvasGroup` روی Canvas مشترک** → هر چیزی که بعداً وارد آن Canvas شود هم محو می‌شود.
* **متنِ مستقیم به `label.text`** → از `StoryText.Set` رد شو.

---

## ۷. کات‌سین‌ها

### ۷.۱ پرده‌ی پنجم — شکستنِ رابط

چهار اتفاق، با چند خط فاصله، به همین ترتیب. همه‌شان بیت‌اند و جابه‌جاشدنی:

| ترتیب | بیت | چه می‌کند |
|---|---|---|
| ۱ | `PullDownDialogue` | هارو باکسِ دیالوگ را **می‌کشد** پایین. محو نمی‌شود، حرکت می‌کند، و تا دوسومِ راه مات است |
| ۲ | `OpenFrame` | قاب باز می‌شود، تصویر کلِ پنجره می‌شود |
| ۳ | `Grade` → `0` | پرده‌ی صورتی برداشته می‌شود |
| ۴ | `SayVoiced` | از اینجا خط‌ها اسمِ فایلِ صدا دارند |

**بعد از `PullDownDialogue` باکس دیگر برنمی‌گردد.** خط‌های بعدی به‌شکلِ **زیرنویسِ ساده** روی تصویر می‌آیند — بدونِ پنلِ صورتی، بدونِ پلاکِ اسم.

> ⚠️ **درست قبل از `PullDownDialogue` هیچ‌وقت `Hold` نگذار.** `Hold` باکس را محو می‌کند، پس هارو یک صفحه‌ی خالی را می‌کشد پایین.

بعدش `Stain` (خون، `Seconds = 0` یعنی برش — و همین لازم است) و `CutMusic`.

> **اولین چیزی که بازیکن با صداپیشگیِ واقعی می‌شنود، جیغِ یوآست.** سند روایی، فکتِ ۱.

### ۷.۲ پرده‌ی ششم — حالتِ فیلم

اولین بیتِ پرده `EnterCinema` است. از آنجا:

* هر خط **به اندازه‌ی خواندنش** می‌ماند و خودش می‌رود (۱٫۶ تا ۹ ثانیه، از روی تعدادِ حرفِ همان زبان)
* کلیک و Space **هیچ کاری نمی‌کنند**
* فلشِ «منتظرِ توام» نمایش داده نمی‌شود
* اگر خط صدای ضبط‌شده داشته باشد، **طولِ فایلِ صدا برنده است**
* **Esc همچنان کار می‌کند** — صحنه‌ای که نشود متوقفش کرد، سینمایی نیست، هَنگ است

عددهایش: `CinemaSecondsPerCharacter = 0.045f` · `CinemaMinimumLineSeconds = 1.60f` · `CinemaMaximumLineSeconds = 9.00f` · `CinemaLineGapSeconds = 0.45f`.

> داخلِ حالتِ فیلم **`Choice` نگذار** و **سکوتِ شمرده‌شده هم نگذار.** تبِ Check هر دو را به‌عنوان خطا می‌گیرد.

**مونتاژِ آخرِ پرده** با `CutTo(...)` ساخته می‌شود: برشِ آنی، بدونِ فِید و بدونِ صدای زنگ، با فاصله‌های هر بار کوتاه‌تر. این فقط چون بازیکن نمی‌تواند کلیک کند کار می‌کند — هر برش دقیقاً روی همان فریمی می‌افتد که برایش نوشته شده، برای همه، هر بار.

### ۷.۳ پرده‌ی هفتم — ویدیو

بیتِ `Video`، فیلدِ `Film` = **`Act07Credits`**. دو جا دنبالش می‌گردد:

1. `Assets/Video/Act07Credits.mp4` — واردِ Build می‌شود
2. `Assets/StreamingAssets/Video/Act07Credits.mp4` — **توصیه‌شده**؛ فایل، فایل می‌ماند و بدونِ Import دوباره می‌شود تدوینش را عوض کرد

**نیاز به ماژولِ Video یونیتی دارد** و این پروژه ندارد:
`Window ▸ Package Manager ▸ Built-in ▸ Video ▸ Enable` · بررسی: `The Frayed Red String ▸ Check The Video Module`

> بدونِ ماژول، بازی **کامپایل می‌شود** و پرده‌ی هفتم بدونِ فیلم اجرا می‌شود. هیچ‌چیزی نمی‌شکند.

پرده‌ی هفتم در دو بیتِ اولش `OpenFrame` و `Grade(0)` دارد — چون قاب و پرده‌ی صورتی برای **هر Scene از نو ساخته می‌شوند** و بدونِ آن‌ها پرده‌ی هفتم با قابِ بسته شروع می‌شد، انگار پرده‌ی پنجم اتفاق نیفتاده.

### ۷.۴ خطاب به بازیکن — `EnterAside`

چهار صحنه: یوآ در اتاقش در پرده‌ی دوم · هارو در خیابان و یوآ در اتاقش در پرده‌ی چهارم · هارو در آخرِ پرده‌ی پنجم.

اتاق تاریک می‌شود (`AsideDim = 0.45f` در `AsideDimDuration = 1.4f` ثانیه)، خط کندتر از هر جای دیگرِ بازی می‌آید (`AsideTypeSpeed = 30f` در برابرِ `45f`)، و باکس روی زمینه‌ی سنگین‌تری می‌نشیند.

هیچ‌کدامِ این‌ها ظریف نیست و قرار هم نیست باشد — این‌ها لحظه‌هایی‌اند که بازی می‌خواهد جدی گرفته شود، و تا الآن با همان لحنی می‌خواست که قهوه سفارش می‌دهد.

> قوانینِ نوشتنِ این صحنه‌ها (کوتاه، دلربا، بدونِ توضیحِ خودش، بدونِ گزینه برای بازیکن، و بعدش هیچ‌کس اشاره نمی‌کند) در دستورنامه بخشِ ۱۳ است.

---

## ۸. پرده‌ی صورتی، خون، و قاب

### پرده‌ی صورتی (Veil / Grade)

**بله عمدی است.** همان چیزی است که سند به آن می‌گوید «شیشه‌ی تار»: پرده‌های ۱ تا ۴ از پشتش دیده می‌شوند و **پرده‌ی پنجم برش می‌دارد.** این کلِ معنیِ «گرافیک واقع‌گرایانه می‌شود» در بازی‌ای است که قرار نیست سه‌بعدی شود — همان آرت، بدونِ چیزی جلویش.

| چه چیزی | کجا |
|---|---|
| خودِ لایه | `_C#/Presentation/StoryGrade.cs` |
| عددهایش | `StageSettings.asset` — `Veil Enabled` · `Veil Strength` (پیش‌فرض `0.55`) · `Veil Colour` |
| مقدارِ اولیه در کد | `GameConfig.StoryGradeDefault = 1f` · `GradeChangeDuration = 3.0f` |
| برداشتنش در پرده ۵ | بیتِ `Grade` با `Amount = 0` |
| لایه‌ی خون | همان‌جا، بیتِ `Stain` |

> قبلاً این عدد در کد ثابت و روی **قدرتِ کامل** بود — به همین دلیل کلِ بازی کمی از فوکوس خارج به‌نظر می‌رسید.

**اگر کاملاً خاموشش کنی هیچی نمی‌شکند.** بیت‌های `Grade` در پرده‌ی پنجم چیزی برای برداشتن ندارند، پس فقط آن یک لحظه‌ی «تصویر شفاف شد» را از دست می‌دهی.

`Grade` و `Stain` بعد از لودِ یک سیو هم درست برمی‌گردند.

### قاب

قاب از **هر چهار طرف** است و با **ضخامت** توصیف می‌شود، نه با نسبتِ تصویر:

| فیلد | پیش‌فرض | یعنی چه |
|---|---|---|
| `Frame Border X` | ۱۴۹ | ضخامتِ نوارِ چپ و راست، در بومِ ۱۹۲۰ |
| `Frame Border Y` | ۶۹ | ضخامتِ نوارِ بالا و پایین، در بومِ ۱۰۸۰ |
| `Frame Colour` | مشکی | |
| `GameConfig.FrameOpenDuration` | ۲٫۴ ثانیه | سرعتِ باز شدن |

باکسِ دیالوگ، انتخاب‌ها و اسمِ مکان همه **داخلِ** قاب چیده می‌شوند و وقتی قاب باز می‌شود با آن پهن‌تر می‌شوند.

از پرده‌ی اول قاب بسته است. هر جا `OpenFrame` بگذاری، همان‌جا باز می‌شود — سند این را برای پرده‌ی پنجم می‌خواهد.

---

## ۹. جای کاراکترها

عددهای رسمی در `Characters.md` بخش ۱ هستند. اینجا فقط تاریخچه‌ی خرابی‌هاست، تا دوباره تکرار نشود.

### چرا خراب می‌شد

**۱. دو منبعِ حقیقت.** جای کاراکتر هم از `StageSettings.asset` می‌آمد هم از Marker های داخلِ Scene، و بازی موقعِ اجرا Scene را ترجیح می‌داد. یک Marker که کسی تکانش داده بود یا از یک Scene کپی‌شده مانده بود، کلِ کاراکترهای آن پرده را جابه‌جا می‌کرد — و در Scene View هیچ نشانه‌ای نداشت.

**حالا:** بازی **فقط** `StageSettings.asset` را می‌خواند. Marker های Scene فقط مرجعِ چشمی‌اند (خاموش). تیکِ `Read Placement From Scene` برش می‌گرداند؛ توصیه نمی‌شود.

**۲. ترتیبِ نصبِ `AmbientMotion`.** لحظه‌ای که فعال می‌شود، جای فعلیِ آبجکت را به‌عنوان «جای استراحت» ذخیره می‌کند و بعد هر فریم همان را دوباره می‌نویسد. یک بار `AmbientMotion` **قبل** از جایگذاری اضافه می‌شد، پس `(0, 0)` را ذخیره می‌کرد و از فریمِ بعد هر دو کاراکتر را می‌کشید وسطِ پایینِ صفحه، با ارتفاعِ کاملِ ۱۲۴۴ واحد. عددهای داخلِ asset تمامِ این مدت درست بودند و شصت بار در ثانیه بازنویسی می‌شدند.

**حالا:** جایگذاری **قبل** از `AmbientMotion` انجام می‌شود، و اگر جای کاراکتری وسطِ پرده عوض شود (پرده‌ی ششم، دو سنِ یک نفر) `RecapturePosition()` صدا زده می‌شود.

**۳. `Reset Character Placement` که `localScale` را روی ۱ می‌گذاشت.** یک Sprite با ارتفاعِ ۲۴۰۰ پیکسل در Scale ۱ برابرِ ۲۴ واحدِ world است، در حالی که دوربین فقط ۱۰ واحد می‌بیند — یعنی خودِ همان دکمه‌ای که برای درست کردن بود، مطمئن‌ترین راهِ خراب کردنش بود. حالا Scale خودِ آن کاراکتر را می‌نشاند.

### ریاضی‌اش

```
SpriteWorldHeight = 2400px / 100 PPU = 24 واحدِ world
CanvasPerWorld    = 108          (دوربینِ Orthographic با size = 5)
HeightFor(Yua)    = 24 × 0.48 × 108 = 1244 واحدِ canvas
HeightFor(Haru)   = 24 × 0.50 × 108 = 1296 واحدِ canvas
```

### وقتی Marker غلط باشد

اگر Marker یک Scene ارتفاعی خارج از بازه‌ی `0.6×` تا `1.5×` عددِ طراحی بدهد، بازی آن را **نادیده می‌گیرد**، کاراکتر را روی Anchor می‌کشد و یک Warning با علتش در Console می‌نویسد.

### دیدنِ حقیقت

```
The Frayed Red String ▸ Diagnose Character Placement
```

هم در حالتِ Play و هم بیرونش کار می‌کند. می‌گوید asset چه می‌گوید، و اگر بازی در حالِ اجراست، Rect واقعاً کجاست — و اگر این دو با هم نخوانند صریح می‌نویسد که چیزی دارد رویش می‌نویسد.

---

## ۱۰. پایان‌ها و پنج دقیقه

شرط‌ها و اسم‌ها در `Acts.md` بخش ۴ هستند. اینجا فقط مکانیکش است.

### چه چیزی شمرده می‌شود

```csharp
StorySession.KindChoices     // آبی
StorySession.CruelChoices    // سبز
StorySession.IsPureKindRun   // ← شرطِ پایانِ رویا
StorySession.PatientMoments  // چند بار بازیکن پنج دقیقه صبر کرده
StorySession.HasBeenPatient
```

`StoryDirector` گزینه را **قبل از** رد کردن ثبت می‌کند، پس فشارِ آبی حتی وقتی یوآ ردش می‌کند هم شمرده می‌شود. **کلِ نکته‌ی این مکانیزم همین است** و هیچ‌جا توضیح داده نمی‌شود.

### اورراید

```csharp
beat.YuaOverridesKindness = true;
beat.OverrideLine = new LocalizedLine("No. I did not say that.", "……ううん。", "نه. من این رو نگفتم.");
```

* مسیرِ داستان **شاخه نمی‌خورد.** بیت‌های بعد از انتخاب را به‌شکلِ مسیرِ سبز بنویس و بگذار جمله‌ی رد کردن آبی را حمل کند.
* `OverrideLine` را خالی بگذاری، فقط چهره را می‌گیرد. **اولین بار باید دقیقاً همین باشد — بدونِ هیچ دیالوگی.**
* بعد از رد کردن، صورتِ یوآ روی `DeadEyes` **می‌ماند** و عمداً آنجا رها می‌شود؛ بیتِ بعدی حتماً باید `Portrait` بدهد.
* `GameConfig.ChoiceOverrideHoldDuration = 1.40f`.

### سکوت‌ها

```csharp
beat.MeasurePatience = true;   // روی یک Line
```

خط‌هایی که بازی در آن‌ها چیزی نمی‌خواهد: جایی که پای هارو نگهش می‌دارد، یا جایی که صدای موتورخانه از زیرِ خیابان به یوآ می‌رسد.

روی صفحه **هیچ‌چیزی** نشان داده نمی‌شود، نه قبلش نه بعدش. **گفتنِ «صبر کن» صبر کردن نیست.**

### در طولِ آن پنج دقیقه بازی یخ نمی‌زند

| چه چیزی | جزئیات |
|---|---|
| **ضربانِ قلب** | از هر ۱۲ ثانیه شروع می‌شود و تا هر ۵ ثانیه نزدیک می‌شود (`PatienceHeartbeatSlowest/Fastest`، حجم `0.35`). برای **همه** پخش می‌شود، حتی اجرای مخلوط |
| **حرکت** | کاراکترها، پس‌زمینه و باکس همه نفس می‌کشند. `AmbientMotion` هیچ‌وقت متوقف نمی‌شود |
| **پرده‌ی صورتی** | `PatienceGradeDrift = 0.06f` — ۶٪ گرم‌تر (رویا در راه) یا سردتر (هدف). روی اجرای مخلوط **هیچ تغییری نمی‌کند** |

> ضربانِ قلب عمداً برای همه پخش می‌شود. اگر فقط برای اجراهای واجدِ شرایط بود، بازیکن از روی سکوت می‌فهمید که اجرایش هنوز شانسی دارد یا نه — و این تنها چیزی است که این مکانیزم نباید لو بدهد.

### تست کردن بدونِ پنج دقیقه انتظار

```
1. The Frayed Red String ▸ Endings ▸ Shorten The Five Minutes To 10 Seconds
2. از یک بیتِ دارای MeasurePatience بازی را شروع کن  (StoryPlaytest.PlayFrom)
3. وسطِ بازی یکی از این سه:
      Endings ▸ Pretend This Run Pressed Only Blue    → باید «رویا» بیاید
      Endings ▸ Pretend This Run Pressed Only Green   → باید «هدف» بیاید
      Endings ▸ Pretend This Run Pressed Both         → باید هیچی نیاید
4. دست به هیچی نزن. ده ثانیه.
5. برای شکِ خودت: Endings ▸ What Ending Is This Run Earning?
6. آخرِ کار: Endings ▸ Use The Real Five Minutes
```

همه‌ی `Pretend…`ها و کوتاه کردنِ زمان داخلِ `#if UNITY_EDITOR` هستند و از Build کاملاً حذف می‌شوند. هر بار Play بزنی یک Warning زرد یادت می‌اندازد که کوتاه‌سازی روشن است.

بدونِ این‌ها باید از پرده‌ی ۳ تا ۵ همه‌ی انتخاب‌ها را یک‌رنگ بزنی و حتی یک بار هم اشتباه نکنی — حدود بیست دقیقه بازیِ دقیق قبل از اینکه چیزی که می‌خواهی تست کنی اصلاً قابلِ دسترس شود.

---

## ۱۱. منوی بچگی

بعد از **هر** پایانی (واقعیت، هدف، رویا):

1. سیوها کاملاً پاک می‌شوند
2. یک فلگ در `PlayerPrefs` ثبت می‌شود — **این سیو نیست**، چیزی نمی‌دهد، و بازیِ بعدی باز هم از صفر شروع می‌شود
3. منوی اصلی عکسِ بچگی را نشان می‌دهد

فایلی که باید بگذاری: `Assets/Images/Backgrounds/MainMenuChildhoodImage.png`
جایگزینِ Sprite روی `MainMenuBackGrundYuaAndHaruImage` می‌شود. تا وقتی نباشد، منو همان عکسِ همیشگی را نگه می‌دارد و یک خط در Console می‌نویسد.

برگرداندنش به حالتِ اول: `StoryProgress.Forget()` یا پاک کردنِ `PlayerPrefs`.

> **چرا سیوها پاک می‌شوند:** شرطِ پایانِ مخفی یک واقعیت درباره‌ی **کلِ یک اجراست**، و یک سیوِ وسطِ راه اجازه می‌داد فارم شود. سند هم همین را می‌خواهد.

---

## ۱۲. آماده‌سازی، خروجی، تست

### یک دکمه که همه‌چیز را آماده می‌کند

```
The Frayed Red String ▸ Prepare The Whole Game
```

به همین ترتیب:

| # | کار |
|---|---|
| ۱ | ساختنِ پوشه‌های لازم (`Story/Acts`، `Audio/Voice`، `Video`، `StreamingAssets/Video` و …) |
| ۲ | ساختنِ دوباره‌ی همه‌ی کتابخانه‌ها: تصویر، صدا، ویس، ویدیو، UI |
| ۳ | برگرداندنِ جای کاراکترها روی عددهای طراحی |
| ۴ | باز کردنِ **تک‌تکِ** Scene های پرده‌ها، درست کردنشان، و مرتب کردنِ Marker ها |
| ۵ | ساختنِ اسکریپتِ پرده‌های ۲ تا ۷ + هر دو پایان |
| ۶ | ساختنِ دوباره‌ی Act Library |
| ۷ | درست کردنِ Build Settings |
| ۸ | درست کردنِ Player Settings (اسمِ بازی، رزولوشن، …) |
| ۹ | بررسیِ ماژولِ Video |

**یک بار می‌پرسد** و فقط یک چیز: اگر پرده‌ای از قبل دیالوگ داشته باشد، نگهشان دارد یا از روی کد دوباره بنویسد. پیش‌فرض **نگه داشتن** است.

> دوباره زدنش هیچ ضرری ندارد. هر وقت چیزی خراب به‌نظر رسید، اول این را بزن.

بعدش `The Frayed Red String ▸ Is The Game Ready?` — لیستِ زنده‌ی هر فایلی که هنوز نیست.

### خروجی

```
1. Prepare The Whole Game
2. Is The Game Ready?                         ← لیستِ فایل‌های غایب
3. File ▸ Build Settings ▸ WarningScene باید Index 0 باشد
4. Build
```

`Prepare` این‌ها را خودش تنظیم می‌کند: اسمِ بازی `The Frayed Red String` · رزولوشنِ `1920 × 1080` · پنجره‌ی قابلِ تغییرِ اندازه · **Run In Background روشن** (بازی روی زمانِ Unscaled است؛ اگر با از دست دادنِ فوکوس متوقف شود، وسطِ یک خط می‌ایستد).

> **بازی بدونِ هیچ فایلِ هنری‌ای هم خروجی می‌گیرد و از اول تا آخر اجرا می‌شود.** پس‌زمینه‌های غایب تصویرِ جانشین می‌شوند، اسپرایت‌های غایب سایه، ویس‌های غایب تایپ‌رایتر، ویدیوی غایب رد می‌شود.
> این تصادفی نیست: یعنی می‌شود پرده را نوشت، پخش کرد و قضاوت کرد **قبل از** اینکه آرتش وجود داشته باشد.

### تست

| چه چیزی | چطور |
|---|---|
| کلِ بازی از اول | `Play The Whole Game` (یا `Alt+G`) |
| یک صحنه‌ی مشخص | `StoryPlaytest.PlayFrom(act, beat)` — یا در Story Editor دکمه‌ی `▶ Play from here` |
| هر سه پایان | بخش ۱۰ |
| وضعیتِ اجرای فعلی | `Endings ▸ What Ending Is This Run Earning?` |
| چه چیزی کم است | `Is The Game Ready?` |
| اشکالاتِ یک پرده | Story Editor ← تبِ **Check** (و `Editor/StoryDiagnostics.cs`) |
| صداها | `Export Sound Effects To WAV` |

### ابزارهای ادیتور که مستقیم صدا می‌زنی

```csharp
StoryAssetBuilder.Rebuild();
StageSpriteLibraryBuilder.Rebuild();
AudioLibraryBuilder.Rebuild();
BuildSettingsAutoConfigurator.Repair();
StoryPlaytest.PlayFrom(act, beat);
ActSceneSetup.ResetCharacterPlacement();
ActSceneSetup.AdoptSceneCharacters();
Act01Builder.Build();  …  Act07Builder.Build();
EndingsBuilder.BuildBoth();
SfxWavExporter.ExportAll();
```

---

## ۱۳. نقاطِ اتصال

### رویدادها

```csharp
LocalizationService.LanguageChanged += lang => { … };
SaveService.SlotsChanged           += () => { … };
director.Finished                  += () => { … };
pauseMenu.Resumed                  += () => { … };
typewriter.Completed               += () => { … };
```

### فعل‌های بازی

```csharp
GameFlowService.StartNewGame();
GameFlowService.LoadSlot(2);
GameFlowService.AdvanceToAct(4);
GameFlowService.EnterMainMenu();
GameFlowService.EraseAllProgress();   // بعد از یک پایان
GameFlowService.QuitGame();
```

### وضعیتِ بازیِ جاری

```csharp
StorySession.ActNumber
StorySession.LineIndex
StorySession.PlaySeconds
StorySession.KindChoices / CruelChoices
StorySession.IsPureKindRun
StorySession.PatientMoments / HasBeenPatient
StorySession.ActiveSlot       // فقط یادداشت؛ هیچ‌چیزی خودکار روی آن نمی‌نویسد
StorySession.BackgroundName
StorySession.WriteTo(slot);
StorySession.BeginAt(act, beat);
```

### صدا و آهنگ و انتقالِ صحنه

```csharp
AudioService.Play(SfxId.Confirm, volumeScale: 0.8f, pitch: 1.05f);
MusicService.PlayLoop("Act03Theme");
MusicService.Stop(fadeOutSeconds: 1f);

SceneTransitionService.LoadScene(SceneNames.MainMenu);
SceneTransitionService.LoadScene("Act04", new Color(0.10f, 0.04f, 0.09f, 1f));
ScreenFader.FadeOut(1.2f, () => { … });
```

---

## ۱۴. پیوست — عددها

همه در `Core/GameConfig.cs`:

```csharp
// خواندن و انتقال
TypeSpeedNormal            = 45f;    // حرف در ثانیه
AsideTypeSpeed             = 30f;    // موقعِ حرف زدن با بازیکن
DialogueFadeDuration       = 0.35f;
BackgroundFadeDuration     = 1.10f;
BackgroundCutThreshold     = 0.15f;  // کمتر از این = برش، بدونِ زنگ
CharacterFadeDuration      = 0.50f;
CharacterFocusDuration     = 0.30f;
CaptionHoldDuration        = 2.40f;
TitleCardHoldDuration      = 2.60f;
SceneFadeOutDuration       = 1.25f;
SceneFadeInDuration        = 1.50f;

// قاب و پرده
FrameOpenDuration          = 2.40f;
DialoguePullDownDuration   = 2.00f;
StoryGradeDefault          = 1f;
GradeChangeDuration        = 3.00f;
AsideDim                   = 0.45f;
AsideDimDuration           = 1.40f;

// پایان‌ها
DesignedPatienceSeconds    = 300f;   // پنج دقیقه
PatienceSeconds                      // ← خاصیت. در ادیتور قابلِ کوتاه شدن
PatienceGradeDrift         = 0.06f;
PatienceHeartbeatSlowest   = 12f;
PatienceHeartbeatFastest   = 5f;
PatienceHeartbeatVolume    = 0.35f;
ChoiceOverrideHoldDuration = 1.40f;

// حالتِ فیلم
CinemaSecondsPerCharacter  = 0.045f;
CinemaMinimumLineSeconds   = 1.60f;
CinemaMaximumLineSeconds   = 9.00f;
CinemaLineGapSeconds       = 0.45f;

// فونت
DialogueFontSize = 40f;   SpeakerFontSize = 32f;   ChoiceFontSize = 36f;
CaptionFontSize  = 30f;   SeasonFontSize  = 40f;   ActTitleFontSize = 88f;

// صدا
SfxVoiceCount      = 10;
DefaultSfxVolume   = 0.65f;
SfxPitchJitter     = 0.045f;
DefaultMusicVolume = 0.45f;
MusicFadeInDuration  = 2.2f;
MusicFadeOutDuration = 1.0f;
DefaultVoiceVolume = 0.85f;
```

اندازه‌ی باکسِ دیالوگ بالای `UI/DialogueBoxView.cs` است، و اندازه‌ی قاب و جای کاراکترها در `StageSettings.asset`.
