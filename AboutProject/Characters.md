# Character Visual & Technical Specifications (Characters.md)
**Project:** The Frayed Red String  
**Engine:** Unity (2D / Visual Novel)

---

## 1. Technical & Canvas Specifications

* **Sprite Dimensions (Native Resolution):** `1200 × 2400 px` (All character sprites share identical canvas bounds).
* **Character Heights:**
  * **Haru:** 173 cm
  * **Yua:** 165 cm
* **Unity Scaling & Transform Rule:**
  * The 8 cm height delta is pre-baked into the image canvas (Yua has built-in top margin and does not occupy the full 2400 px vertical extent).
  * **In-Engine Scale:** Set `Transform.localScale = Vector3(1, 1, 1)` for both Haru and Yua.
  * **No manual downscaling** (e.g., `0.95` for Yua) is required in Unity.

---

## 2. Character: Yua (Height: 165 cm)

| Sprite File Name | Visual Description |
| :--- | :--- |
| `YuaNeutralGentleSmile.png` | Soft and calm smile, direct forward gaze, arms relaxed at her sides. |
| `YuaPeacefulClosedEyesSmile.png` | Serene and relaxed smile with eyes closed, arms resting at her sides. |
| `YuaBobaSipFullCup.png` | Eyes closed in satisfaction while sipping through the straw of a full boba tea cup held at chest level. |
| `YuaBobaHoldEmptyCup.png` | Gentle smile looking forward, holding the finished/empty boba tea cup with both hands at chest level. |
| `YuaJoyfulHappyLaugh.png` | Bright open-mouthed laugh with eyes closed, hands clasped together at chest level. |
| `YuaShyBlushingLookDown.png` | Heavily blushed cheeks, downcast eyes, hands brought up to cover her mouth and nose in bashfulness. |
| `YuaSadImploringTearful.png` | Glistening tearful eyes, furrowed worried brows, hands tightly clasped over her chest in an imploring stance. |
| `YuaAnnoyedAngryGlare.png` | Cold, sharp, annoyed glare with narrowed eyes, flat straight mouth, and arms down at sides. |
| `YuaDeadEyesPokerFace.png` | Completely desaturated, hollow, and lifeless eyes (dead-eyes poker face), void of any emotion. |
| `YuaInsaneManicSmile.png` | Wide unhinged grin showing teeth, dilated golden eyes, hands clasped over chest with red strings taut. |
| `YuaSorrowfulCryingTears.png` | Heavy streaming tears running down blushing cheeks, hands covering her trembling mouth in grief. |
| `YuaBento01HoldClosedBox.png` | Soft smile, holding closed wooden bento with both hands at chest level. |
| `YuaBento02ShowFullFood.png` | Cheerful smile, displaying fully packed bento (6 sushi, 4 octopus sausages). |
| `YuaBento03SharedMostlyEmpty.png` | Gentle smile, bento mostly shared with Haru (2 sushi, 1 octopus remaining). |
| `YuaBento04LiftFirstSushi.png` | Open smile, lifting first sushi with chopsticks to eat (1 sushi, 1 octopus left in box). |
| `YuaBento05SavorFirstSushi.png` | Closed eyes, puffed cheeks chewing first sushi (1 sushi, 1 octopus left in box). |
| `YuaBento06LiftLastSushi.png` | Open smile, lifting second sushi with chopsticks (left side empty, 1 octopus left in box). |
| `YuaBento07SavorLastSushi.png` | Closed eyes, chewing second sushi (left side empty, 1 octopus left in box). |
| `YuaBento08LiftLastOctopus.png` | Open smile, lifting final octopus sausage (box interior completely empty). |
| `YuaBento09SavorLastOctopus.png` | Closed eyes, chewing final octopus sausage (box interior completely empty). |
| `YuaBento10ClosedFinishedSmile.png` | Closed-eye blissful smile, holding finished bento closed again with both hands. |

---

## 3. Character: Haru (Height: 173 cm)

| Sprite File Name | Visual Description |
| :--- | :--- |
| `HaruNeutralGentleSmile.png` | Mild, warm everyday smile, calm direct gaze, relaxed standing posture with arms at his sides. |
| `HaruJoyfulHappyLaugh.png` | Cheerful laugh with open mouth, smiling closed eyes, and a slight blush on his cheeks. |
| `HaruShyBlushingLookAway.png` | Flustered blushing face looking sideways, right hand gripping his left upper arm. |
| `HaruSadImploringTearful.png` | Glossy tearful eyes, worried raised eyebrows, hands clasped together over chest in a pleading gesture. |
| `HaruSeriousAngryFrown.png` | Serious and angry frown with heavy shading over the eyes, intense downward glare, rigid stance. |
| `HaruDeadEyesPokerFace.png` | Blank, lifeless, and empty gaze (dead eyes), completely flat and emotionless facial expression. |
| `HaruInsaneManicSmile.png` | Head sharply tilted sideways, wide manic grin, crazed wide eyes, right hand clutching chest, left hand tensed. |
| `HaruSorrowfulCryingTears.png` | Deep weeping with closed eyes, streaming tears, both hands brought up to cover his mouth. |
| `HaruInjuredKneeGrimace.png` | Painful grimace with gritted teeth and visible sweat, right fist clenched at his side, left hand resting tensely on his thigh above the knee. |
