using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogueText;
    public GameObject dialogueBox; // 대화창 UI
    public AudioClip textSound; // 텍스트 사운드
    private AudioSource audioSource; // 오디오 소스
    public bool isDialogueRunning = false;
    public bool IsDialogueFullyShown { get; private set; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    public void ShowFullDialogue()
    {
        StopAllCoroutines(); // 현재 실행 중인 모든 코루틴을 중지합니다.
        IsDialogueFullyShown = true; // 대화가 완전히 표시되었음을 나타냅니다.
        // 여기에서 현재 대화를 UI에 완전히 표시하는 코드를 작성합니다.
    }
    // 대화를 표시하는 코루틴
    public IEnumerator ShowDialogue(string dialogue)
    {
        isDialogueRunning = true;
        dialogueBox.SetActive(true); // 대화창 UI를 활성화

        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter; // 문자를 하나씩 추가
            audioSource.PlayOneShot(textSound); // 사운드 재생
            yield return new WaitForSeconds(0.05f); // 각 문자 사이에 약간의 지연 시간
        }
        isDialogueRunning = false;
    }

    // 대화를 숨기는 메서드
    public void HideDialogue()
    {
        dialogueText.text = "";
        dialogueBox.SetActive(false); // 대화창 UI를 비활성화
    }
}
