using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;

    public static SceneLoader Instance
    {
        get
        {
            if (instance != null) return instance;
            var i = FindObjectOfType<SceneLoader>();
            if (i != null)
            {
                instance = i;
                return instance;
            }
            i = Instantiate(Resources.Load<SceneLoader>("SceneLoader"));
            instance = i;
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this) Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public Image blackBG;
    public CanvasGroup group;

    public void LoadScene(string sceneName)
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        StartCoroutine(nameof(Load), sceneName);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        StartCoroutine(nameof(OutTransition));
    }

    private IEnumerator OutTransition()
    {
        group.blocksRaycasts = true;
        blackBG.fillOrigin = 0;
        blackBG.fillAmount = 1;
        var timer = 1f;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime * 1.5f;
            blackBG.fillAmount = timer;
            yield return new WaitForEndOfFrame();
        }

        group.blocksRaycasts = false;
    }

    private IEnumerator Load(string sceneName)
    {
        group.blocksRaycasts = true;
        blackBG.fillOrigin = 1;
        blackBG.fillAmount = 0;

        var timer = 0f;

        while (timer < 1)
        {
            timer += Time.unscaledDeltaTime * 1.5f;
            blackBG.fillAmount = timer;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.2f);
        var async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = true;
    }
}
