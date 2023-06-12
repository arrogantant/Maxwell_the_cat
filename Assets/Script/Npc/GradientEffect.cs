using UnityEngine;
using UnityEngine.UI;
public class GradientEffect : MonoBehaviour
{
    public float duration = 1.0f;  // 페이드에 걸리는 시간
    private Image image;
    private float startTime;

    void Start()
    {
        image = GetComponent<Image>();
        startTime = Time.time;
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;
        Color newColor = image.color;
        newColor.a = Mathf.SmoothStep(0.0f, 1.0f, t);
        image.color = newColor;
    }
}