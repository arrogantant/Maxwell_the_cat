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
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    // 대화를 표시하는 코루틴
    public IEnumerator ShowDialogue(string dialogue)
    {
        dialogueBox.SetActive(true); // 대화창 UI를 활성화

        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter; // 문자를 하나씩 추가
            audioSource.PlayOneShot(textSound); // 사운드 재생
            yield return new WaitForSeconds(0.05f); // 각 문자 사이에 약간의 지연 시간
        }
    }

    // 대화를 숨기는 메서드
    public void HideDialogue()
    {
        dialogueText.text = "";
        dialogueBox.SetActive(false); // 대화창 UI를 비활성화
    }
}
