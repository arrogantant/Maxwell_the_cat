using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [SerializeField] private LoadingScreen loadingScreen;

    private void Awake()
    {
        gameObject.SetActive(true);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.ShowLoadingScreen();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        StartCoroutine(ShowLoadingScreenAndHide());
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ShowLoadingScreenAndHide());
    }
    void OnSceneUnloaded(Scene scene)
    {
        // 씬이 언로드되면 로딩 화면을 표시합니다.
        loadingScreen.ShowLoadingScreen();
    }
    private IEnumerator ShowLoadingScreenAndHide()
    {
        // 씬이 로드되면 로딩 화면을 표시합니다.
        loadingScreen.ShowLoadingScreen();

        // 1초 동안 대기합니다.
        yield return new WaitForSeconds(1f);

        // 1초 후에 로딩 화면을 숨깁니다.
        loadingScreen.HideLoadingScreen();
    }
}