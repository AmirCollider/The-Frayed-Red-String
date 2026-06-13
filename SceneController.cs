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
    // LoadSceneRoutine - Async Coroutine with Optional Fade
    // ==========================================
    private IEnumerator LoadSceneRoutine(string sceneName, float fadeDuration)
    {
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
            elapsed += Time.deltaTime;
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