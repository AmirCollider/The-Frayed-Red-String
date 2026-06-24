// ==========================================
// SceneController - Scene Loading & Transition Wrapper
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    // ==========================================
    // Singleton Instance
    // ==========================================
    public static SceneController Instance { get; private set; }

    // ==========================================
    // Fade Overlay (assign a full-screen black Image via Inspector — optional)
    // ==========================================
    [Header("Fade Overlay")]
    [SerializeField] private Image fadeOverlay;

    // ==========================================
    // Inspector — Transition SFX (Transition AudioEvent)
    // ==========================================
    [Header("Transition SFX")]
    [SerializeField] private AudioEvent transitionSfx;

    // ==========================================
    // Awake - Singleton Enforcement & Persistence
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ==========================================
    // LoadScene - Validate, Fade Out, Load, Fade In
    // ==========================================
    public void LoadScene(string sceneName, float fadeDuration = ConstantsConfig.FADE_DURATION_DEFAULT)
    {
        if (!SceneExistsInBuild(sceneName))
        {
            Debug.LogWarning($"[SceneController] Scene '{sceneName}' is not in Build Settings. Add it first.");
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName, fadeDuration));
    }

    // ==========================================
    // LoadAct - Convenience Loader by Act Number
    // ==========================================
    public void LoadAct(int actNumber)
    {
        switch (actNumber)
        {
            case 1: LoadScene(ConstantsConfig.SCENE_ACT_1); break;
            case 2: LoadScene(ConstantsConfig.SCENE_ACT_2); break;
            case 3: LoadScene(ConstantsConfig.SCENE_ACT_3); break;
            case 4: LoadScene(ConstantsConfig.SCENE_ACT_4); break;
            case 5: LoadScene(ConstantsConfig.SCENE_ACT_5); break;
            default: Debug.LogWarning($"[SceneController] Act {actNumber} has no mapped scene."); break;
        }
    }

    // ==========================================
    // LoadMainMenu - Return to Main Menu
    // ==========================================
    public void LoadMainMenu()
    {
        LoadScene(ConstantsConfig.SCENE_MAIN_MENU);
    }

    // ==========================================
    // LoadActOrMenu - Load Act if Its Scene is Built, Else Return to Main Menu
    // (prevents Act 3 climax dead-ending on a black screen when Act 4 is not yet built)
    // ==========================================
    public void LoadActOrMenu(int actNumber)
    {
        string scene = SceneNameForAct(actNumber);
        if (!string.IsNullOrEmpty(scene) && IsSceneInBuild(scene))
            LoadScene(scene);
        else
            LoadMainMenu();
    }

    // ==========================================
    // SceneNameForAct - Map Act Number to Scene Name
    // ==========================================
    private string SceneNameForAct(int actNumber)
    {
        switch (actNumber)
        {
            case 1: return ConstantsConfig.SCENE_ACT_1;
            case 2: return ConstantsConfig.SCENE_ACT_2;
            case 3: return ConstantsConfig.SCENE_ACT_3;
            case 4: return ConstantsConfig.SCENE_ACT_4;
            case 5: return ConstantsConfig.SCENE_ACT_5;
            default: return null;
        }
    }

    // ==========================================
    // IsSceneInBuild - Public Build-Settings Membership Check
    // ==========================================
    public bool IsSceneInBuild(string sceneName) => SceneExistsInBuild(sceneName);

    // ==========================================
    // LoadSceneRoutine - Async Coroutine with Optional Fade
    // ==========================================
    private IEnumerator LoadSceneRoutine(string sceneName, float fadeDuration)
    {
        // ==========================================
        // Unfreeze Before Transition - A Load Can Fire From the Paused Stop Menu
        // (Time.timeScale == 0); a time-scaled fade would stall and stick on black.
        // ==========================================
        Time.timeScale = 1f;

        transitionSfx?.Play();

        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (op != null && !op.isDone)
            yield return null;

        if (fadeOverlay != null)
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    // ==========================================
    // Fade - Alpha Tween Coroutine for Overlay
    // ==========================================
    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = fadeOverlay.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / duration);
            fadeOverlay.color = c;
            yield return null;
        }

        c.a = to;
        fadeOverlay.color = c;
    }

    // ==========================================
    // SceneExistsInBuild - Validate Against Build Settings
    // ==========================================
    private bool SceneExistsInBuild(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Contains(sceneName))
                return true;
        }
        return false;
    }
}