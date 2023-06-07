using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtStomp : MonoBehaviour
{   
    public BoxCollider2D solidCollider;
    public BoxCollider2D triggerCollider;
    public GameObject DashObj;
    public GameObject GoodObj;
    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 참조 추가
    private Player playerScript; // Player 스크립트 참조

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // Player 스크립트 인스턴스를 가져옵니다.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == playerScript.gameObject && playerScript.isButtStomping)
        {
            Break();
        }
    }

    public void Break()
    {
        solidCollider.enabled = false; // 콜라이더 비활성화
        triggerCollider.enabled = true; // 트리거 콜라이더 활성화
        DashObj.SetActive(false);
        GoodObj.SetActive(true);
        spriteRenderer.enabled = false; // 스프라이트 렌더러 비활성화
    }
}
