using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatform : MonoBehaviour
{
    public float blinkTime = 2f;
    public float inactiveTime = 2f;
    public float activeTime = 5f;
    public float blinkInterval = 0.5f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        StartCoroutine(BlinkAndDeactivate());
    }

    IEnumerator BlinkAndDeactivate()
    {
        while (true)
        {
            yield return StartCoroutine(Blink());
            yield return StartCoroutine(Deactivate());
            yield return StartCoroutine(Activate());
        }
    }

    IEnumerator Blink()
    {
        float endTime = Time.time + blinkTime;
        while (Time.time < endTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        spriteRenderer.enabled = true;
    }

    IEnumerator Deactivate()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(inactiveTime);
    }

    IEnumerator Activate()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        yield return new WaitForSeconds(activeTime);
    }
}
