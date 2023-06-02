using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
public class ButtonSysyem : MonoBehaviour
{
    public Button[] buttons;
    public RectTransform arrow;
    public float arrowDistance;
    public GameObject objectToActivate;  // 활성화할 오브젝트
    public GameObject objectToDeactivate;  // 비활성화할 오브젝트
    public AudioClip moveSound; // 화살표가 움직일 때 재생할 효과음
    public AudioMixerGroup audioMixerGroup; // AudioMixerGroup reference
    private AudioSource audioSource; // 효과음을 재생할 오디오 소스
    private int selectedIndex;

    private void Start()
    {
        if (buttons.Length > 0)
        {
            UpdateArrowPosition();
        }
        // AudioSource 컴포넌트를 가져옵니다 (없으면 추가)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 오디오 소스에 AudioMixerGroup을 할당
        audioSource.outputAudioMixerGroup = audioMixerGroup;
    }

    private void Update()
    {
        var keyboard = Keyboard.current; // 현재 키보드를 가져옵니다.

        if (keyboard == null)
            return; // 키보드가 연결되어 있지 않으면 반환

        if (keyboard.xKey.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame)
        {
            objectToActivate.SetActive(true);  // 오브젝트를 활성화
            objectToDeactivate.SetActive(false);  // 오브젝트를 비활성화
        }

        if (keyboard.upArrowKey.wasPressedThisFrame) // 키보드의 상단 화살표 키가 눌렸는지 확인
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = buttons.Length - 1;
            }
            UpdateArrowPosition();
            PlaySound(); // 효과음 재생
        }
        else if (keyboard.downArrowKey.wasPressedThisFrame) // 키보드의 하단 화살표 키가 눌렸는지 확인
        {
            selectedIndex++;
            if (selectedIndex >= buttons.Length)
            {
                selectedIndex = 0;
            }
            UpdateArrowPosition();
            PlaySound(); // 효과음 재생
        }
        else if (keyboard.spaceKey.wasPressedThisFrame || keyboard.zKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame) 
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    private void UpdateArrowPosition()
    {
        Vector2 buttonPosition = buttons[selectedIndex].GetComponent<RectTransform>().anchoredPosition;
        arrow.anchoredPosition = new Vector2(buttonPosition.x - arrowDistance, buttonPosition.y);
    }

    // 효과음 재생 메소드
    private void PlaySound()
    {
        if (moveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(moveSound);
        }
    }
}
