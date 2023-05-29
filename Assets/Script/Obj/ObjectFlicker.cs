using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlicker : MonoBehaviour
{
    [SerializeField] private Collider2D objectCollider;
    [SerializeField] private SpriteRenderer objectSpriteRenderer;
    [SerializeField] private float flickerInterval = 1f;
    [SerializeField] private float initialDelay = 1f;

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            objectCollider.enabled = !objectCollider.enabled;
            objectSpriteRenderer.enabled = !objectSpriteRenderer.enabled;

            yield return new WaitForSeconds(flickerInterval);
        }
    }
}
