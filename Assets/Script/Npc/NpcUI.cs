using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogueText; // 대화 내용을 표시할 텍스트
    public GameObject dialoguePanel; // 대화 UI 패널
    private void Start()
    {
        // 초기에 대화 패널을 비활성화합니다.
        dialoguePanel.SetActive(false);
    }
    // 대화 UI를 표시하는 메서드
    public void ShowDialogue(string dialogue)
    {
        dialoguePanel.SetActive(true); // 패널 활성화
        dialogueText.text = dialogue; // 텍스트 설정
    }

    // 대화 UI를 숨기는 메서드
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false); // 패널 비활성화
        dialogueText.text = string.Empty; // 텍스트 내용 비우기
    }
}
