using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Canvas : MonoBehaviour
{
    public Color[] colors; // 그라데이션에 사용될 색상 배열
    public float duration = 3.0f; // 각 색상변경에 소요되는 시간

    private Image imageComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        int currentColorIndex = 0;
        while(true) // 반복적으로 실행
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration; // 시간을 경과시킵니다.
                Color startColor = colors[currentColorIndex];
                Color endColor = colors[(currentColorIndex + 1) % colors.Length];
                imageComponent.color = Color.Lerp(startColor, endColor, t); // 색상을 서서히 변경합니다.
                yield return null; // 다음 프레임까지 대기
            }

            currentColorIndex = (currentColorIndex + 1) % colors.Length; // 다음 색상 인덱스로 이동
        }
    }
}
