// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ActSceneSetup.cs  (Editor only)
//
//  Making a new act need no setting up.
//
//  An act scene contains a camera and a background image. Everything else — the
//  dialogue box, the characters, the frame, the pause menu, the save panel — is
//  built at load. That is the design, and it works; what it does not do on its
//  own is create the camera and the background image, so every new act still
//  began with the same twenty clicks and the same chance of naming something
//  slightly wrong.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Builds the two things an act scene has to contain.</summary>
    public static class ActSceneSetup
    {
        /// <summary>
        /// Gives the open scene a camera and a background canvas, if it has not
        /// got them.
        /// </summary>
        /// <remarks>
        /// Additive and idempotent. Anything already there by the right name is
        /// left exactly as it is — this cannot overwrite a scene somebody has
        /// arranged, only complete one nobody has started.
        /// </remarks>
        [MenuItem("The Frayed Red String/Prepare This Scene As An Act")]
        public static void PrepareScene()
        {
            UnityEngine.SceneManagement.Scene scene =
                UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (!scene.IsValid())
            {
                Debug.LogWarning("[Story] There is no open scene to prepare.");
                return;
            }

            int added = 0;

            added += EnsureCamera() ? 1 : 0;
            added += EnsureBackgroundCanvas() ? 1 : 0;

            EditorSceneManager.MarkSceneDirty(scene);

            string act = SceneNames.IsActScene(scene.name)
                ? $"It will play act {scene.name.Substring(3).TrimStart('0')}."
                : $"Note that '{scene.name}' is not named like an act — an act scene has to be called " +
                  "Act01, Act02 and so on, or nothing will play in it.";

            Debug.Log(
                added == 0
                    ? $"[Story] '{scene.name}' already had everything an act needs. {act}"
                    : $"[Story] Added {added} thing(s) '{scene.name}' was missing. {act} Save the scene.");
        }

        /// <summary>
        /// Writes the character positions in this scene into the stage settings,
        /// so every act uses them.
        /// </summary>
        /// <returns>How many characters were recorded.</returns>
        /// <remarks>
        /// The scene's own marker sprites win over the settings asset at runtime,
        /// which is what makes dragging them around a way of placing a character.
        /// This is the other half of that: once the placement is right, it stops
        /// being this scene's and becomes the game's, and the next act needs no
        /// markers at all.
        /// </remarks>
        public static int AdoptSceneCharacters()
        {
            StageSettings settings = StoryAssetBuilder.EnsureStageSettings();

            if (settings == null)
            {
                return 0;
            }

            Camera camera = Camera.main;

            if (camera == null || !camera.orthographic)
            {
                Debug.LogWarning(
                    "[Stage] This scene has no orthographic camera tagged MainCamera, so there is nothing " +
                    "to measure the characters against.");
                return 0;
            }

            int adopted = 0;

            adopted += Adopt(settings, camera, Speaker.Yua) ? 1 : 0;
            adopted += Adopt(settings, camera, Speaker.Haru) ? 1 : 0;

            if (adopted > 0)
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }

            return adopted;
        }

        private static bool Adopt(StageSettings settings, Camera camera, Speaker speaker)
        {
            SpriteRenderer marker = VisualNovelStage.FindAuthoredAnchor(speaker);

            if (marker == null)
            {
                return false;
            }

            StageSettings.Placement placement = settings.PlacementFor(speaker);

            if (!placement.TryMeasureFrom(marker, camera, out StageSettings.Placement measured))
            {
                return false;
            }

            measured.Speaker = speaker;
            settings.SetPlacement(measured);

            Debug.Log(
                $"[Stage] {speaker} is now placed at X {measured.OffsetX:0}, feet {measured.FeetY:0}, " +
                $"height {measured.Height:0} for every act, taken from '{marker.name}'.");

            return true;
        }

        private static bool EnsureCamera()
        {
            Camera existing = Object.FindAnyObjectByType<Camera>(FindObjectsInactive.Include);

            if (existing != null)
            {
                // A perspective camera cannot be measured against, and every
                // placement in this game is measured against one.
                if (!existing.orthographic)
                {
                    existing.orthographic = true;
                    existing.orthographicSize = 5f;

                    Debug.LogWarning(
                        $"[Story] '{existing.name}' was a perspective camera and has been switched to " +
                        "orthographic, which is what a 2D scene here needs.");
                }

                return false;
            }

            GameObject host = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            host.tag = "MainCamera";

            Camera camera = host.GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;

            host.transform.position = new Vector3(0f, 0f, -10f);

            Undo.RegisterCreatedObjectUndo(host, "Prepare Act Scene");
            return true;
        }

        private static bool EnsureBackgroundCanvas()
        {
            if (GameObject.Find(TheFrayedRedString.Core.ObjectNames.BackgroundCanvas) != null)
            {
                return false;
            }

            GameObject canvasHost = new GameObject(
                TheFrayedRedString.Core.ObjectNames.BackgroundCanvas,
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

            Canvas canvas = canvasHost.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = GameConfig.BackgroundCanvasOrder;

            CanvasScaler scaler = canvasHost.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            GameObject imageHost = new GameObject(TheFrayedRedString.Core.ObjectNames.StoryBackground, typeof(Image));
            imageHost.transform.SetParent(canvasHost.transform, false);

            RectTransform rect = (RectTransform)imageHost.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image = imageHost.GetComponent<Image>();
            image.color = Color.black;
            image.raycastTarget = false;

            Undo.RegisterCreatedObjectUndo(canvasHost, "Prepare Act Scene");
            return true;
        }
    }
}
