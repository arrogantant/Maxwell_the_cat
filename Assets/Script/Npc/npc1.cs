using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npc1 : MonoBehaviour
{
    public string dialogue; // NPC의 대사
    private NpcUI dialogueUI; // 대화 UI 스크립트

    private void Start()
    {
        dialogueUI = GameObject.FindObjectOfType<NpcUI>(); // 대화 UI 스크립트 인스턴스를 가져옵니다.
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
