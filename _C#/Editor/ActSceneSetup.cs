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
        /// Puts both characters back on the anchors the game is designed around,
        /// in the stage settings and in the open scene at once.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A placement can come from two places: the stage settings asset, which
        /// every act reads, and marker sprites dropped into a scene, which
        /// override it for that scene alone. Fixing one and not the other is why
        /// act one and act two could disagree about where the same two people
        /// stand.
        /// </para>
        /// <para>
        /// So this does both. Any Yua or Haru marker in the open scene is moved
        /// onto the anchor and set to the scale that character is drawn at, and
        /// then measured — which writes the anchor into the settings together
        /// with the sprite's real height, so the two can no longer drift apart.
        /// A scene with no markers just gets the built-in numbers written into
        /// the asset.
        /// </para>
        /// </remarks>
        [MenuItem("The Frayed Red String/Reset Character Placement")]
        public static void ResetCharacterPlacement()
        {
            StageSettings settings = StoryAssetBuilder.EnsureStageSettings();

            if (settings == null)
            {
                return;
            }

            int markers = 0;

            markers += PlaceMarkerOnAnchor(Speaker.Yua) ? 1 : 0;
            markers += PlaceMarkerOnAnchor(Speaker.Haru) ? 1 : 0;

            UnityEngine.SceneManagement.Scene scene =
                UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (markers > 0 && scene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(scene);
            }

            int adopted = markers > 0 ? AdoptSceneCharacters() : 0;

            if (adopted > 0)
            {
                Debug.Log(
                    $"[Stage] {adopted} marker(s) in '{scene.name}' were moved onto the anchors " +
                    $"(Yua x {StageSettings.Placement.YuaAnchorX} scale {StageSettings.Placement.YuaScale}, " +
                    $"Haru x {StageSettings.Placement.HaruAnchorX} scale {StageSettings.Placement.HaruScale}, " +
                    $"both y {StageSettings.Placement.AnchorY}) and recorded for every act. Save the scene.");

                return;
            }

            settings.ResetPlacementsToDefault();

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            Debug.Log(
                markers > 0
                    ? "[Stage] The markers were moved onto the anchors but could not be measured — the scene " +
                      "needs an orthographic camera tagged MainCamera — so the built-in placement was written " +
                      "into the stage settings instead. Save the scene."
                    : $"[Stage] Both characters are back on the anchors: Yua x {StageSettings.Placement.YuaAnchorX} " +
                      $"at scale {StageSettings.Placement.YuaScale}, Haru x {StageSettings.Placement.HaruAnchorX} " +
                      $"at scale {StageSettings.Placement.HaruScale}, both y {StageSettings.Placement.AnchorY}. " +
                      "The open scene has no character markers, so every act now reads these numbers.");
        }

        /// <summary>
        /// Moves one character's marker sprite onto its anchor, at the size the
        /// game is designed around.
        /// </summary>
        /// <returns>False when this scene has no marker for that character.</returns>
        /// <remarks>
        /// <para>
        /// Three numbers, and the third one is the one this used to get wrong.
        /// It set the scale back to one, on the reasoning that the eight
        /// centimetres between Haru and Yua are baked into the artwork and a
        /// scaled marker has that correction applied twice. The reasoning is
        /// sound and the conclusion was not: at a scale of one a 2400-pixel
        /// sprite is twenty-four world units tall against a camera that sees
        /// ten, so both of them came out at roughly twice their proper size and
        /// the menu item advertised as the fix for a broken placement was itself
        /// the most reliable way to break one.
        /// </para>
        /// <para>
        /// Yua goes to 0.48 and Haru to 0.50, which is the difference the
        /// artwork does not carry. See <see cref="StageSettings.Placement.YuaScale"/>.
        /// </para>
        /// </remarks>
        private static bool PlaceMarkerOnAnchor(Speaker speaker)
        {
            SpriteRenderer marker = VisualNovelStage.FindAuthoredAnchor(speaker);

            if (marker == null)
            {
                return false;
            }

            Transform pose = marker.transform;

            Undo.RecordObject(pose, "Reset Character Placement");

            pose.position = new Vector3(
                StageSettings.Placement.AnchorXFor(speaker),
                StageSettings.Placement.AnchorY,
                pose.position.z);

            pose.localScale = Vector3.one * StageSettings.Placement.ScaleFor(speaker);

            return true;
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

            // Recording a placement writes it into every act at once, so a
            // marker that is obviously wrong is worth refusing here rather than
            // finding six scenes later.
            if (!StageSettings.Placement.IsPlausible(speaker, measured, out string complaint))
            {
                Debug.LogWarning(
                    $"[Stage] '{marker.name}' was not recorded because {complaint}. Nothing has been " +
                    $"changed. Put it on x {StageSettings.Placement.AnchorXFor(speaker):0.00}, " +
                    $"y {StageSettings.Placement.AnchorY:0.00} at a scale of " +
                    $"{StageSettings.Placement.ScaleFor(speaker):0.00} — or press Reset to the anchors, " +
                    "which does exactly that — and try again.");

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
