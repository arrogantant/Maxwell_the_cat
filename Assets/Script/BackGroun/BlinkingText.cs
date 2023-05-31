using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BlinkingText : MonoBehaviour
{
    public TMP_Text text; 
    public float blinkTime = 0.5f;

    void Start()
    {
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            text.alpha = 0;
            yield return new WaitForSeconds(blinkTime);
            text.alpha = 1;
            yield return new WaitForSeconds(blinkTime);
        }
    }
}
