# راهنمای کامل C#

**The Frayed Red String** — همان کارهای Story Editor، از طریق کد، به اضافه‌ی چیزهایی که فقط با کد میشود.

> اگر فقط میخواهی داستان بنویسی، `Guide-StoryEditor.md` را بخوان. این فایل برای وقتی است که میخواهی چیز **جدیدی** به بازی اضافه کنی.

---

## فهرست

1. [نقشه کد](#۱-نقشه-کد)
2. [بازی چطور بالا می‌آید](#۲-بازی-چطور-بالا-می‌آید)
3. [شش قانون این کدبیس](#۳-شش-قانون-این-کدبیس)
4. [ساختن یک پرده با کد](#۴-ساختن-یک-پرده-با-کد)
5. [اضافه کردن یک نوع Beat جدید](#۵-اضافه-کردن-یک-نوع-beat-جدید)
6. [اضافه کردن یک صدای جدید](#۶-اضافه-کردن-یک-صدای-جدید)
7. [اضافه کردن یک کاراکتر جدید](#۷-اضافه-کردن-یک-کاراکتر-جدید)
8. [اضافه کردن یک زبان جدید](#۸-اضافه-کردن-یک-زبان-جدید)
9. [ساختن یک صفحه‌ی جدید](#۹-ساختن-یک-صفحه‌ی-جدید)
10. [نقاط اتصال](#۱۰-نقاط-اتصال)

---

## ۱. نقشه کد

`Assets/_C#/` — هیچ asmdef ای ندارد، پس همه‌چیز داخل `Assembly-CSharp` است و پوشه‌ی `Editor` داخل `Assembly-CSharp-Editor`.

| پوشه | مسئول چیست |
|---|---|
| `Core/` | راه‌اندازی، ثابت‌ها، اسم‌ها، ساعت داستان |
| `Narrative/` | داده‌ی داستان و کارگردانش |
| `Presentation/` | صحنه، قاب، فونت، پرده‌ی محو، انتقال صحنه |
| `UI/` | باکس دیالوگ، انتخاب‌ها، منوی توقف، پنل Save |
| `Localization/` | زبان‌ها و جدول رشته‌ها |
| `Motion/` | حرکت آرام همه چیز |
| `Tweening/` | موتور انیمیشن |
| `Audio/` | سنتز صدا و پخش |
| `SaveSystem/` | خواندن و نوشتن اسلات‌ها |
| `Scenes/` | یک کنترلر برای هر نوع صحنه |
| `Flow/` | فعل‌های بازی: شروع، لود، رفتن به پرده بعد |
| `Input/` | تنها جایی که با Input System حرف میزند |
| `Editor/` | پنجره‌ی Story Editor، سازنده‌های Library، ابزارهای تست |

### فایل‌هایی که بیشتر از همه سراغشان میروی

| فایل | چیست |
|---|---|
| `Core/GameConfig.cs` | **هر عدد جادویی بازی**. سرعت‌ها، اندازه فونت‌ها، مدت‌ها |
| `Core/ObjectNames.cs` | اسم هر GameObject ای که کد به آن وصل است |
| `Core/SceneNames.cs` | اسم صحنه‌ها |
| `Narrative/StoryEnums.cs` | انواع Beat، لحن انتخاب |
| `Narrative/ActAsset.cs` | ساختار داده‌ی یک پرده |
| `Narrative/StoryDirector.cs` | حلقه‌ای که پرده را اجرا میکند |
| `Narrative/StageSettings.cs` | جای کاراکترها و اندازه قاب |
| `Localization/LocKeys.cs` + `LocalizationDatabase.cs` | متن رابط کاربری |
| `Audio/SfxId.cs` + `ProceduralSfxLibrary.cs` | صداها |

---

## ۲. بازی چطور بالا می‌آید

هیچ چیزی داخل هیچ Scene ای وصل نشده. همه‌اش کد است:

```
[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]
GameBootstrap.ResetStatics()
    └─ هر static ای را صفر میکند (چون یونیتی میتواند domain reload را رد کند)

[RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
GameBootstrap.Initialize()
    ├─ LocalizationService.Initialize()
    ├─ ServiceHost = یک GameObject با DontDestroyOnLoad
    ├─ TweenRunner، AudioService، MusicService، ScreenFader،
    │  GlobalPointerSfx، LocalizationRefresher روی آن نصب میشوند
    └─ SceneInstaller.Enable()      ← به sceneLoaded گوش میدهد

[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]
GameBootstrap.InstallFirstScene()

هر بار که یک Scene لود میشود:
SceneInstaller.InstallInto(scene)
    ├─ EventSystem و AudioListener را تضمین میکند
    ├─ StoryClock.IsPaused = false
    ├─ StoryFonts.ForgetTemplate() + CompleteAuthoredFonts()
    ├─ AmbientMotionInstaller.Install(scene)     ← به همه چیز حرکت میدهد
    ├─ Light2DAmbientPulse.InstallAll(scene)
    ├─ SceneAudioInstaller.Install(scene)
    ├─ AttachSceneController(scene)              ← طبق اسم Scene
    └─ LocalizationRefresher.Settle()
```

بعدش، داخل یک صحنه‌ی پرده:

```
ActSceneController.Start()
    ├─ ActNumber را از اسم Scene میخواند
    ├─ ActLibrary.Find(ActNumber)
    ├─ BackgroundCanvas را پیدا/میسازد
    ├─ StoryCanvas را میسازد (sortingOrder 100)
    │   ├─ VisualNovelStage       ← کاراکترها
    │   ├─ AdvanceCatcher         ← کلیک = خط بعد
    │   ├─ StorySafeArea          ← هر چیزی که خوانده میشود
    │   │   ├─ DialogueBoxView
    │   │   ├─ StoryOverlayView
    │   │   └─ ChoicePanelView
    │   └─ StoryFrameView         ← چهار نوار، صاحب SafeArea
    ├─ PauseCanvas را میسازد (sortingOrder 500)
    │   ├─ PauseMenuView
    │   │   └─ دکمه زبان (از MenuCanvas قدیمی)
    │   └─ LoadPanelController    (از MenuCanvas قدیمی)
    └─ StoryDirector.Play(act, StorySession.ConsumeResumeLine())
```

**ترتیب Canvas ها** داخل `GameConfig`:

```csharp
BackgroundCanvasOrder = 0;
StoryCanvasOrder      = 100;
PauseCanvasOrder      = 500;
// و پرده‌ی محو صفحه روی 30000
```

---

## ۳. شش قانون این کدبیس

اگر این شش تا را بلد باشی، هر چیزی اضافه کنی با بقیه جور در می‌آید.

### قانون ۱ — `AmbientMotion` تنها صاحب transform است

هیچ چیزی داخل این کد `localPosition` یا `localScale` یا `localRotation` یک شیء را که `AmbientMotion` دارد نمینویسد. هر سیستمی که میخواهد چیزی را تکان بدهد، از کانال‌های آن استفاده میکند:

```csharp
motion.ExtraOffset = new Vector3(0f, -26f, 0f);   // جابجایی
motion.ExtraScale  = new Vector3(0.94f, 0.94f, 1f);
motion.ExtraRotation = 3f;
motion.Weight = 0f;   // فریز کردن حرکت آرام، بدون خاموش کردن کامپوننت
```

برای همین است که ورود یک کاراکتر، نفس کشیدنش و خم شدنش برای حرف زدن، همزمان و بدون تداخل کار میکنند.

**استثنا:** اگر والد یک `LayoutGroup` داشته باشد، `AmbientMotion` خودکار کانال موقعیت را رها میکند (`BorrowsPosition`). دو نویسنده روی یک `anchoredPosition` همان چیزی بود که کل منوی توقف را روی هم انباشت.

### قانون ۲ — داستان روی `StoryClock` میچرخد، رابط کاربری روی زمان واقعی

```csharp
StoryClock.IsPaused        // منوی توقف این را ست میکند
StoryClock.DeltaTime       // موقع توقف صفر برمیگرداند
yield return StoryClock.Wait(2f);   // به جای WaitForSecondsRealtime
```

برای تویین‌ها:

```csharp
TweenRunner.Play(duration, t => { … }, EaseType.OutCubic,
    delay: 0f, onComplete: null, owner: this,
    useUnscaledTime: true,
    pausesWithStory: true);   // ← هر چیزی که تصویر را تکان میدهد
```

`pausesWithStory: false` (پیش‌فرض) برای پرده‌ی محو صفحه، منوی توقف و حرکت آرام است. بازی متوقفی که منوی متوقف‌کننده‌اش هم یخ بزند، شبیه کرش است.

`Time.timeScale` اینجا کار **نمیکند** — همه چیز عمدا روی زمان Unscaled است.

### قانون ۳ — متن فقط از یک در رد میشود

```csharp
StoryText.Set(label, text);
```

این سه کار میکند: متن را مینویسد، وزن فونت را طبق زبان تنظیم میکند (لاتین بولد، فارسی نه)، و فونت را **همان فریم** برای زبان جدید میسازد.

آن آخری اجباری است. `DirectFont` زبان را از `LateUpdate` تشخیص میدهد، پس بدون این، یک label که متنش از انگلیسی به فارسی عوض شده همان فریم با فونت انگلیسی کشیده میشود و دیگر هیچ‌وقت اصلاح نمیشود.

هر label ساخته‌شده در کد، اول باید فونت بگیرد:

```csharp
StoryFonts.Apply(label);   // بعد از AddComponent<TextMeshProUGUI>()
```

### قانون ۴ — رابط کاربری در کد ساخته میشود، نه در Scene

هیچ prefab ای نیست. `DialogueBoxView`، `PauseMenuView`، `ChoicePanelView` همه خودشان را از `new GameObject(...)` و شکل‌های کشیده‌شده در `ProceduralUiSprites` میسازند.

```csharp
image.sprite = ProceduralUiSprites.RoundedRect(26, fill, border, 3f);
image.type = Image.Type.Sliced;
```

برای همین یک پرده‌ی جدید فقط یک Scene با دوربین است.

### قانون ۵ — چیزهایی که موقع اجرا پیدا میشوند، با اسم پیدا میشوند

هر اسمی که کد به آن وابسته است داخل `ObjectNames.cs` است. اگر GameObject ای را داخل Scene عوض اسم کردی، **فقط همان فایل** باید دنبالش برود.

```csharp
Transform found = UnityUtility.FindInScene(ObjectNames.BackgroundCanvas);
Image bg = UnityUtility.FindDeep<Image>(layer, ObjectNames.StoryBackground);
```

### قانون ۶ — Library ها خودکار ساخته میشوند

`StageSpriteLibrary`، `MusicLibrary`، `UiSpriteLibrary`، `ActLibrary` همه asset هایی داخل `Resources/TFRS` هستند که سازنده‌های ادیتور از روی محتوای پوشه‌ها میسازند. یک فایل جدید داخل `Assets/Images/Backgrounds` بیندازی، خودش قابل استفاده میشود.

```csharp
Sprite bg = StageSpriteLibrary.Load().FindBackground("CozyCafeDay");
AudioClip music = MusicLibrary.Load().Find("MainMenuBackGrungMusic");
ActAsset act = ActLibrary.Find(3);
```

---

## ۴. ساختن یک پرده با کد

گاهی نوشتن صد خط در یک اسکریپت راحت‌تر از صد بار کلیک است. یک اسکریپت ادیتور:

```csharp
using TheFrayedRedString.Localization;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

public static class BuildActThree
{
    [MenuItem("The Frayed Red String/Build Act 03 From Code")]
    public static void Build()
    {
        ActAsset act = ScriptableObject.CreateInstance<ActAsset>();
        act.ActNumber = 3;
        act.Title = new LocalizedLine("Stillness", "静けさ", "آرامش");
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
            Kind = StoryBeatKind.Enter,
            Speaker = Speaker.Yua,
            Portrait = Portrait.Sad
        });

        act.Beats.Add(new BeatData
        {
            Kind = StoryBeatKind.Line,
            Speaker = Speaker.Yua,
            Portrait = Portrait.Unchanged,
            Text = new LocalizedLine(
                "You came.",
                "来てくれたんだ。",
                "آمدی."),
            PlaySound = true,
            Sound = SfxId.Heartbeat,
            SoundVolume = 0.6f
        });

        act.Beats.Add(new BeatData { Kind = StoryBeatKind.Beat, Seconds = 2f });

        AssetDatabase.CreateAsset(act, "Assets/Story/Acts/Act03.asset");
        AssetDatabase.SaveAssets();

        // تا ActLibrary این را ببیند
        TheFrayedRedString.EditorTools.StoryAssetBuilder.Rebuild();
    }
}
```

بعدش داخل Story Editor بازش کن و ادامه بده. هر دو مسیر به یک asset میرسند.

> `SfxId` داخل `TheFrayedRedString.Audio` است — `using` اش را اضافه کن.

---

## ۵. اضافه کردن یک نوع Beat جدید

مثال: `Shake` — لرزاندن صفحه.

**قدم ۱** — `Narrative/StoryEnums.cs`، آخر enum اضافه کن (**هیچ‌وقت وسطش نه** — شماره‌ها داخل asset ها ذخیره شده‌اند):

```csharp
public enum StoryBeatKind
{
    Line, Background, Enter, Exit, ClearStage, Caption, TitleCard,
    Sound, Music, Beat, Choice, Interlude, OpenFrame, CloseFrame, End,

    /// <summary>لرزاندن تصویر.</summary>
    Shake
}
```

**قدم ۲** — اگر فیلد جدیدی لازم دارد، `Narrative/ActAsset.cs` داخل `BeatData`. اگر `Seconds` کافی است، چیزی اضافه نکن.

**قدم ۳** — `Narrative/StoryDirector.cs`، داخل `PlayBeat`:

```csharp
case StoryBeatKind.Shake:
    yield return ShakeRoutine(beat.Seconds);
    break;
```

```csharp
private IEnumerator ShakeRoutine(float seconds)
{
    // از کانال حرکت استفاده کن، نه از transform — قانون ۱
    AmbientMotion motion = AmbientMotion.GetOrAdd(_stage.gameObject);

    TweenRunner.Play(
        seconds,
        t =>
        {
            float fade = 1f - t;
            motion.ExtraOffset = new Vector3(
                Mathf.Sin(t * 90f) * 18f * fade, 0f, 0f);
        },
        EaseType.Linear, 0f,
        () => motion.ExtraOffset = Vector3.zero,
        this,
        true,
        true);   // ← pausesWithStory

    yield return StoryClock.Wait(seconds);
}
```

**قدم ۴** — `Editor/StoryEditorWindow.cs`، سه جا:

```csharp
// SummaryOf — چه چیزی داخل لیست نوشته شود
case StoryBeatKind.Shake:
    return $"≈ shake {beat.FindPropertyRelative("Seconds").floatValue:0.0}s";

// DescriptionOf — جمله‌ای که زیر dropdown می‌آید
case StoryBeatKind.Shake:
    return "Shake the picture. Waits for the shake, not for the player.";

// DrawBeatDetail — فیلدهایش
case StoryBeatKind.Shake: DrawSeconds(beat, "For"); break;
```

اگر میخواهی داخل شش دکمه‌ی سریع هم باشد، به آرایه‌ی `QuickAdd` اضافه‌اش کن. و اگر حالت خرابی دارد، یک `case` هم داخل `Check()` بگذار.

**قدم ۵** — رنگش داخل لیست، `TintFor`:

```csharp
case StoryBeatKind.Shake: return FlowTint;
```

---

## ۶. اضافه کردن یک صدای جدید

هیچ صدایی فایل نیست — همه سنتز میشوند.

**قدم ۱** — `Audio/SfxId.cs`، آخر enum:

```csharp
/// <summary>شکستن شیشه، پرده پنجم.</summary>
GlassBreak
```

**قدم ۲** — `Audio/ProceduralSfxLibrary.cs`، داخل `Build`:

```csharp
case SfxId.GlassBreak:
    return ProceduralAudioSynth.Render(
        name: "GlassBreak",
        voices: new[]
        {
            ProceduralAudioSynth.Voice.Noise(
                startTime: 0f, duration: 0.35f, amplitude: 0.7f,
                lowPassHz: 6000f),
            ProceduralAudioSynth.Voice.Bell(
                startTime: 0.01f, duration: 0.9f,
                frequency: C7, amplitude: 0.4f),
        },
        reverb: 0.25f);
```

> امضای دقیق `Voice` را از داخل `ProceduralAudioSynth.cs` بخوان — چند شکل دارد (سینوس، ناقوس، نویز، ضربه).

**قدم ۳** — هیچی. `WarmUpStory()` روی کل enum حلقه میزند، و Story Editor خودش آن را داخل dropdown نشان میدهد.

**تست:** `The Frayed Red String ▸ Export Sound Effects To WAV` و به فایلش گوش بده.

---

## ۷. اضافه کردن یک کاراکتر جدید

**قدم ۱** — تصویرها را با همان الگوی اسم بگذار داخل `Assets/Images/Characters/<Name>/`:

```
KaedeNeutralGentleSmile.png
KaedeJoyfulHappyLaugh.png
…
```

اندازه‌شان `1200 × 2400` باشد تا با بقیه جور در بیایند.

**قدم ۲** — `Narrative/CharacterArt.cs`:

```csharp
public enum Speaker
{
    Narrator, Yua, Haru, Player,
    Kaede            // ← آخر اضافه کن
}
```

```csharp
public static string SpriteName(Speaker speaker, Portrait portrait)
{
    switch (speaker)
    {
        case Speaker.Yua:   return "Yua" + YuaSuffix(portrait);
        case Speaker.Haru:  return "Haru" + HaruSuffix(portrait);
        case Speaker.Kaede: return "Kaede" + KaedeSuffix(portrait);
        default: return null;
    }
}

public static string NameKey(Speaker speaker)
{
    switch (speaker)
    {
        case Speaker.Yua:   return LocKeys.SpeakerYua;
        case Speaker.Haru:  return LocKeys.SpeakerHaru;
        case Speaker.Kaede: return LocKeys.SpeakerKaede;
        default: return null;
    }
}

public static StageSide HomeSide(Speaker speaker)
{
    return speaker == Speaker.Haru ? StageSide.Right : StageSide.Left;
}
```

**قدم ۳** — اسمش داخل `LocKeys.cs` و `LocalizationDatabase.cs`:

```csharp
public const string SpeakerKaede = "speaker.kaede";
```

```csharp
table[LocKeys.SpeakerKaede] = new LocEntry("Kaede", "楓", "کائده");
```

**قدم ۴** — رنگ پلاکش داخل `UI/DialogueBoxView.cs`:

```csharp
private static readonly Color KaedePlate = new Color(0.62f, 0.85f, 0.66f, 0.96f);

private static Color PlateColour(Speaker speaker)
{
    switch (speaker)
    {
        case Speaker.Yua:   return YuaPlate;
        case Speaker.Haru:  return HaruPlate;
        case Speaker.Kaede: return KaedePlate;
        default: return PlayerPlate;
    }
}
```

**قدم ۵ — مهم:** `Presentation/VisualNovelStage.cs` فقط **دو** جایگاه دارد (چپ و راست). کاراکتر سوم یعنی یک `Slot` سوم:

```csharp
private Slot _left;
private Slot _centre;   // ← جدید
private Slot _right;
```

و `SlotFor` و `SetUpCharacters` و `SetFocus` باید سومی را هم ببینند. اگر کائده هیچ‌وقت همزمان با دیگری روی صحنه نیست، سرش را نتراش — از همان جایگاه چپ استفاده کن و `StageSettings` را برایش تنظیم کن.

**قدم ۶** — `Rebuild Stage Sprite Library`.

---

## ۸. اضافه کردن یک زبان جدید

**قدم ۱** — `Localization/GameLanguage.cs`. **هیچ‌وقت وسط enum اضافه نکن** — شماره‌ها داخل PlayerPrefs ذخیره شده‌اند:

```csharp
public enum GameLanguage
{
    English = 0,
    Japanese = 1,
    Persian = 2,
    Arabic = 3        // ← آخر
}
```

```csharp
public static GameLanguage Next(this GameLanguage language)
{
    switch (language)
    {
        case GameLanguage.English:  return GameLanguage.Japanese;
        case GameLanguage.Japanese: return GameLanguage.Persian;
        case GameLanguage.Persian:  return GameLanguage.Arabic;
        default: return GameLanguage.English;
    }
}

public static bool IsRightToLeft(this GameLanguage language)
{
    return language == GameLanguage.Persian || language == GameLanguage.Arabic;
}
```

هر چیزی که چیدمان دارد — تراز متن، سمت پلاک نام، گوشه‌ی اسم مکان، وزن فونت — روی `IsRightToLeft` شاخه میزند، نه روی خود زبان. پس این یک متد کل چیدمان را درست میکند.

**قدم ۲** — `LocalizedLine` داخل `ActAsset.cs` یک فیلد جدید میخواهد:

```csharp
[TextArea(2, 8)] public string Arabic;

public string For(GameLanguage language)
{
    switch (language)
    {
        case GameLanguage.Japanese: return string.IsNullOrEmpty(Japanese) ? English : Japanese;
        case GameLanguage.Persian:  return string.IsNullOrEmpty(Persian)  ? English : Persian;
        case GameLanguage.Arabic:   return string.IsNullOrEmpty(Arabic)   ? English : Arabic;
        default: return English;
    }
}
```

همینطور `LocEntry` داخل `LocalizationDatabase.cs`.

**قدم ۳** — پرچمش داخل `UI/UiSpriteLibrary.cs` (`FlagFor`) و تصویرش داخل `Assets/Images/UI`.

**قدم ۴** — فونتش. اگر عربی است، `StoryFonts.PersianFontNames` همان را پیدا میکند. اگر خط جدیدی است، یک آرایه‌ی اسم جدید و یک خانه‌ی جدید داخل `AssignLanguageSlots` لازم است.

**قدم ۵** — Story Editor خودکار زبان جدید را نشان میدهد، چون روی `Enum.GetValues` و `_language.ToString()` کار میکند. فقط `DrawLocalized` را نگاه کن که آرایه‌ی سه‌تایی زبان‌های دیگر را hardcode کرده — آنجا را هم به `Enum.GetValues` تبدیل کن.

---

## ۹. ساختن یک صفحه‌ی جدید

الگویی که همه‌ی صفحه‌های موجود از آن پیروی میکنند. مثال: صفحه‌ی تنظیمات.

```csharp
using TheFrayedRedString.Core;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.UI
{
    [DisallowMultipleComponent]
    public sealed class SettingsPanelView : MonoBehaviour
    {
        private CanvasGroup _group;

        public void Initialize(RectTransform layer)
        {
            transform.SetParent(layer, false);
            Stretch((RectTransform)transform);

            // گروه روی خودِ این شیء، نه روی Canvas مشترک —
            // وگرنه هر چیز دیگری که بعدا داخل آن Canvas منتقل شود
            // با این محو میشود.
            _group = UnityUtility.GetOrAdd<CanvasGroup>(gameObject);

            BuildVeil();
            BuildCard();

            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
        }

        private void BuildCard()
        {
            GameObject host = new GameObject("SettingsCard", typeof(RectTransform));
            host.transform.SetParent(transform, false);

            Image card = host.AddComponent<Image>();
            card.sprite = ProceduralUiSprites.RoundedRect(30,
                new Color(0.14f, 0.09f, 0.13f, 0.90f),
                new Color(0.98f, 0.71f, 0.80f, 0.75f), 3f);
            card.type = Image.Type.Sliced;

            // …

            GameObject labelHost = new GameObject("Title", typeof(RectTransform));
            labelHost.transform.SetParent(host.transform, false);

            TMP_Text label = labelHost.AddComponent<TextMeshProUGUI>();
            label.fontSize = GameConfig.ChoiceFontSize;
            label.alignment = TextAlignmentOptions.Midline;
            label.raycastTarget = false;

            StoryFonts.Apply(label);                                   // قانون ۳
            UnityUtility.GetOrAdd<LocalizedText>(labelHost)
                        .Bind(LocKeys.MenuSettings);
        }

        public void Open()
        {
            _group.blocksRaycasts = true;
            _group.interactable = true;
            _group.FadeTo(1f, 0.28f, EaseType.OutCubic);
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
```

بعد از داخل کنترلر صحنه صدایش بزن، مثل `BuildPauseLayer`.

**دام‌هایی که قبلا افتاده‌ایم:**

- **دکمه‌ای داخل `LayoutGroup` که `AmbientMotion` دارد** → `motion.BorrowsPosition = true` (یا بگذار خودکار تشخیص بدهد). وگرنه همه روی هم می‌افتند.
- **`ContentSizeFitter` روی چیزی که والدش `LayoutGroup` دارد** → دو سیستم روی یک rect.
- **`CanvasGroup` روی Canvas مشترک** → هر چیزی که بعدا وارد آن Canvas شود هم محو میشود.
- **متن مستقیم به `label.text`** → از `StoryText.Set` رد شو.

---

## ۱۰. نقاط اتصال

### رویدادها

```csharp
LocalizationService.LanguageChanged += lang => { … };
SaveService.SlotsChanged += () => { … };
director.Finished += () => { … };
pauseMenu.Resumed += () => { … };
typewriter.Completed += () => { … };
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

### وضعیت بازی جاری

```csharp
StorySession.ActNumber
StorySession.LineIndex
StorySession.PlaySeconds
StorySession.KindChoices / CruelChoices
StorySession.IsPureKindRun    // ← شرط پایان مخفی
StorySession.PatientMoments   // ← چند بار بازیکن پنج دقیقه صبر کرده
StorySession.HasBeenPatient
StorySession.ActiveSlot       // ← فقط یادداشت؛ هیچ چیزی خودکار روی آن نمینویسد
StorySession.BackgroundName
StorySession.WriteTo(slot);
StorySession.BeginAt(act, beat);   // ← همان چیزی که Play from here استفاده میکند
```

### صدا و آهنگ

```csharp
AudioService.Play(SfxId.Confirm, volumeScale: 0.8f, pitch: 1.05f);
MusicService.PlayLoop("Act03Theme");
MusicService.Stop(fadeOutSeconds: 1f);
```

### انتقال صحنه

```csharp
SceneTransitionService.LoadScene(SceneNames.MainMenu);
SceneTransitionService.LoadScene("Act04", new Color(0.10f, 0.04f, 0.09f, 1f));
ScreenFader.FadeOut(1.2f, () => { … });
```

### دو فیلد Beat که پرده دوم اضافه کرد

```csharp
beat.MeasurePatience = true;      // روی یک Line: پنج دقیقه صبر یک بار ثبت میشود

beat.YuaOverridesKindness = true; // روی یک Choice: آبی رد میشود
beat.OverrideLine = new LocalizedLine("No. I did not say that.", "……ううん。", "نه. من این رو نگفتم.");
```

`StoryDirector` گزینه را **قبل از** رد کردن ثبت میکند، پس `IsPureKindRun` همچنان درست میماند. مسیر داستان شاخه نمیخورد — Beat های بعد از انتخاب را به شکل مسیر سبز بنویس.

بعد از رد کردن، صورت یوآ روی `DeadEyes` میماند و عمدا آنجا رها میشود؛ Beat بعدی باید `Portrait` بدهد.

### ساختن یک پرده با یک اسکریپت — الگوی واقعی پروژه

`Editor/ActScriptWriter.cs` پایه است، و `Act02Builder.cs` و `Act03Builder.cs` دو پرده‌ی کامل روی آن.

یک پرده جدید سه چیز است:

```csharp
public sealed class Act04Builder : ActScriptWriter
{
    protected override int ActNumber => 4;
    protected override string AssetName => "Act04";
    protected override LocalizedLine Title => L("To Deepen", "深まる", "عمیق شدن");

    [MenuItem("The Frayed Red String/Build Act 04 From The Story Document")]
    public static void Build() { new Act04Builder().BuildAsset(); }

    protected override void Write()
    {
        Place(Backgrounds.CafeRainy, "The café", "喫茶店", "کافه");
        Hold(2f);
        Enter(Speaker.Yua, Portrait.Neutral);
        Say(Speaker.Yua, Portrait.Joyful, "You came.", "来てくれたんだ。", "آمدی.");
    }
}
```

`BuildAsset()` بقیه‌اش را انجام میدهد: asset را مینویسد (اگر از قبل باشد **همان** را استفاده میکند تا GUID عوض نشود و هیچ ارجاعی نشکند)، اگر Beat داشته باشد اول میپرسد، و آخرش `Rebuild Act Library` را صدا میزند.

**فعل‌ها** — همه داخل `ActScriptWriter`، هر کدام یک Beat:

| فعل | Beat |
|---|---|
| `Place(bg, en, ja, fa)` | Background + اسم مکان |
| `Say(who, face, en, ja, fa)` | Line |
| `SayWithSound(who, face, sfx, vol, en, ja, fa)` | Line + صدا روی همان فریم |
| `Narrate(en, ja, fa)` | Line با گوینده Narrator |
| `Listen(en, ja, fa)` | Narrate + `MeasurePatience` |
| `Hold(seconds)` | Beat سکوت |
| `Enter(who, face)` / `Exit(who)` / `ClearStage()` | صحنه |
| `Cue(sfx, vol)` | Sound تنها |
| `StopMusic()` | Music خالی |
| `Decide(blue×3, green×3, refusal×3)` | Choice با اورراید یوآ |
| `DecideIdly(a×3, b×3)` | Choice سفید، بی‌وزن و شمرده‌نشده |
| `Maybe(LegAche, 0.3f)` | Interlude تصادفی |

`LegAche` و `MachineRoom` خودشان از `Assets/Story/Acts` لود میشوند؛ نبودنشان فقط یک Warning است.

> `Maybe(...)` را همیشه **درست قبل از عوض شدن صحنه** بگذار. Interlude یک Act است و میتواند بک‌گراند و کاراکتر خودش را بگذارد؛ هر چه جا بگذارد، Beat های بعدی جایگزینش میکنند.

### ابزارهای ادیتور که خودت میتوانی صدا بزنی

```csharp
StoryAssetBuilder.Rebuild();
StageSpriteLibraryBuilder.Rebuild();
AudioLibraryBuilder.Rebuild();
BuildSettingsAutoConfigurator.Repair();
StoryPlaytest.PlayFrom(act, beat);
ActSceneSetup.ResetCharacterPlacement();
Act02Builder.Build();
Act03Builder.Build();
ActSceneSetup.AdoptSceneCharacters();
SfxWavExporter.ExportAll();
```

---

## پیوست — عددهایی که احتمالا میخواهی عوض کنی

همه داخل `Core/GameConfig.cs`:

```csharp
TypeSpeedNormal        = 45f;    // حرف در ثانیه
DialogueFadeDuration   = 0.35f;
BackgroundFadeDuration = 1.10f;
CharacterFadeDuration  = 0.50f;
CaptionHoldDuration    = 2.40f;
TitleCardHoldDuration  = 2.60f;
FrameOpenDuration      = 2.40f;
ChoiceOverrideHoldDuration = 1.40f;
PatienceSeconds        = 300f;   // پنج دقیقه
SceneFadeOutDuration   = 1.25f;
SceneFadeInDuration    = 1.50f;

DialogueFontSize = 40f;
SpeakerFontSize  = 32f;
ChoiceFontSize   = 36f;
CaptionFontSize  = 30f;
SeasonFontSize   = 40f;
ActTitleFontSize = 88f;

DefaultSfxVolume   = 0.65f;
DefaultMusicVolume = 0.45f;
```

اندازه‌ی باکس دیالوگ داخل `UI/DialogueBoxView.cs` بالای فایل، و اندازه‌ی قاب داخل تب Stage.

---

## پیوست ۲ — جای ایستادن کاراکترها

عددهای مرجع، داخل `Narrative/StageSettings.cs` (`Placement`):

```csharp
YuaAnchorX  = -4.55f;   // موقعیت world کاراکتر داخل Scene
HaruAnchorX =  4.70f;
AnchorY     = -1.50f;   // برای هر دو
```

اینها **مختصات world** هستند، نه canvas. بقیه از رویشان حساب میشود (`CanvasPerWorld = 108`، دوربین Orthographic با `size = 5`). قبلا برعکس بود — عددهای canvas دستی نوشته شده بودند و از Scene فاصله گرفته بودند، که هر دو کاراکتر را در هر پرده‌ای که Marker نداشت دو سوم واحد بالاتر مینشاند.

سه جا میتوانند جای کاراکتر را تعیین کنند:

1. `Placement.Default()` داخل کد — وقتی هیچ چیز دیگری نباشد
2. `Assets/Resources/TFRS/StageSettings.asset` — چیزی که همه‌ی پرده‌ها میخوانند
3. Marker های Scene (یک `Yua…` یا `Haru…` با SpriteRenderer) — فقط برای همان Scene، و بر بقیه غالب

`The Frayed Red String ▸ Reset Character Placement` هر سه را یکی میکند: Marker های Scene باز را روی Anchor میگذارد (و `localScale` را به ۱ برمیگرداند)، بعد اندازه‌گیریشان میکند و همان را — با ارتفاع واقعی خودِ Sprite — داخل asset مینویسد. Scene بدون Marker فقط عددهای پیش‌فرض را میگیرد.

همان دکمه بالای تب Stage هم هست: **Reset to the anchors**.
