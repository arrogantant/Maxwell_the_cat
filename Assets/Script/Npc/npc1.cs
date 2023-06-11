using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npc1 : MonoBehaviour
{
    public string dialogue; // NPC의 대사
    private NpcUI dialogueUI; // 대화 UI 스크립트
    public GameObject player; // 플레이어 게임오브젝트
    private SpriteRenderer spriteRenderer; // NPC의 SpriteRenderer 컴포넌트
    Animator myAnimator;

    private void Start()
    {
        dialogueUI = GameObject.FindObjectOfType<NpcUI>(); // 대화 UI 스크립트 인스턴스를 가져옵니다.
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 게임오브젝트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>(); // NPC의 SpriteRenderer 컴포넌트를 가져옵니다.
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 플레이어가 NPC의 오른쪽에 있으면
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true; // 이미지를 원래대로 놓습니다.
        }
        // 플레이어가 NPC의 왼쪽에 있으면
        else if (player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false; // 이미지를 가로 방향으로 뒤집습니다.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어가 NPC의 범위에 들어왔을 때
        {
            dialogueUI.ShowDialogue(dialogue); // 대화 UI를 보여줍니다.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어가 NPC의 범위에서 나갔을 때
        {
            Debug.Log("Player exited the trigger area");
            dialogueUI.HideDialogue(); // 대화 UI를 숨깁니다.
        }
    }
}
