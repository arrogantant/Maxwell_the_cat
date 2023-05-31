using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GradientText : MonoBehaviour
{
    public TMP_Text text;
    public Color[] colors;
    public float duration = 1.0f;

    void Start()
    {
        if (colors.Length < 2)
        {
            return;
        }
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        int colorIndex = 0;
        float t = 0;

        while (true)
        {
            t += Time.deltaTime / duration;
            text.color = Color.Lerp(colors[colorIndex], colors[(colorIndex + 1) % colors.Length], t);

            if (t >= 1)
            {
                t = 0;
                colorIndex++; 
                if (colorIndex >= colors.Length - 1) 
                {
                    colorIndex = 0;
                }
            }

            yield return null;
        }
    }
}
