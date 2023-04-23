using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float extensionLength = 0.1f; // 오브젝트가 늘어날 길이
    public float extensionDuration = 3f; // 오브젝트가 늘어나는 데 걸리는 시간

    public void Interact()
    {
        StartCoroutine(ExtendObject());
    }

    private IEnumerator ExtendObject()
    {
        float startTime = Time.time;
        float initialSizeX = transform.localScale.x;
        Vector3 initialPosition = transform.localPosition;

        while (Time.time - startTime < extensionDuration)
        {
            float progress = (Time.time - startTime) / extensionDuration;
            Vector3 newSize = transform.localScale;
            newSize.x = initialSizeX + extensionLength * progress;
            transform.localScale = newSize;

            Vector3 newPosition = initialPosition;
            newPosition.x += (extensionLength * progress) / 2;
            transform.localPosition = newPosition;

            yield return null;
        }

        // 오브젝트의 최종 길이 설정
        Vector3 finalSize = transform.localScale;
        finalSize.x = initialSizeX + extensionLength;
        transform.localScale = finalSize;

        // 오브젝트의 최종 위치 설정
        Vector3 finalPosition = initialPosition;
        finalPosition.x += extensionLength / 2;
        transform.localPosition = finalPosition;
    }
}