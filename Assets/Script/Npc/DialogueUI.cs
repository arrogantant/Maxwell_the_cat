using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogueText; // 대화를 표시할 Text 컴포넌트

    // 대화를 표시하는 메서드
    public void ShowDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    // 대화를 숨기는 메서드
    public void HideDialogue()
    {
        dialogueText.text = "";
    }
}
