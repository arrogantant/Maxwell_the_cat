using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public BoxCollider2D solidCollider;
    public BoxCollider2D triggerCollider;
    public GameObject DashObj;
    public GameObject GoodObj;
    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 참조 추가

    public void Break()
    {
        solidCollider.enabled = false; // 콜라이더 비활성화
        triggerCollider.enabled = true; // 트리거 콜라이더 활성화
        DashObj.SetActive(false);
        GoodObj.SetActive(true);
        spriteRenderer.enabled = false; // 스프라이트 렌더러 비활성화
        Destroy(gameObject);
    }
}
