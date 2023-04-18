using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float respawnTime = 10.0f;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Disappear()
    {
        sr.enabled = false;
        boxCollider.enabled = false;
        StartCoroutine(RespawnPlatform());
    }

    private IEnumerator RespawnPlatform()
    {
        yield return new WaitForSeconds(respawnTime);
        sr.enabled = true;
        boxCollider.enabled = true;
    }
}
