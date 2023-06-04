using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;

    public Image fadeImage;
    public AnimationCurve fadeCurve;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 이 오브젝트는 씬 전환 시 파괴되지 않습니다.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(Fade(true));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isFadeOut)
    {
        float fadeDuration = 1f;
        float time = 0;

        RectTransform fadeImageRectTransform = fadeImage.GetComponent<RectTransform>();

        // Modify this to match your needs
        Vector2 startPosition = new Vector2(1, 0);
        Vector2 endPosition = new Vector2(-1, 0);

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float a = isFadeOut ? fadeCurve.Evaluate(t) : 1 - fadeCurve.Evaluate(t);
            fadeImage.color = new Color(0f, 0f, 0f, a);

            fadeImageRectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            time += Time.deltaTime;
            yield return null;
        }
    }
}

