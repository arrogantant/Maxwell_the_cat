using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class AudioSettings : MonoBehaviour
{
    public Slider musicSlider;  // 배경음악을 조절하는 슬라이더
    public Slider sfxSlider;  // 효과음을 조절하는 슬라이더
    public AudioMixer audioMixer; // 오디오 믹서

    private void Start()
    {
        // 슬라이더의 초기 값 설정
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.1f);
    }
    private bool isMusicSelected = true; // 기본적으로 음악 슬라이더를 선택

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // 위/아래 방향키를 눌렀을 때 슬라이더 선택 변경
        if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
        {
            isMusicSelected = !isMusicSelected; // 선택된 슬라이더 변경
        }

        // 좌우 방향키를 눌렀을 때 선택된 슬라이더의 값 조절
        if (keyboard.leftArrowKey.wasPressedThisFrame)
        {
            if (isMusicSelected)
            {
                musicSlider.value -= 0.1f; // 음악 슬라이더 값을 감소
                UpdateMusicVolume(); // 볼륨 업데이트
            }
            else
            {
                sfxSlider.value -= 0.1f; // 효과음 슬라이더 값을 감소
                UpdateSFXVolume(); // 볼륨 업데이트
            }
        }
        else if (keyboard.rightArrowKey.wasPressedThisFrame)
        {
            if (isMusicSelected)
            {
                musicSlider.value += 0.1f; // 음악 슬라이더 값을 증가
                UpdateMusicVolume(); // 볼륨 업데이트
            }
            else
            {
                sfxSlider.value += 0.1f; // 효과음 슬라이더 값을 증가
                UpdateSFXVolume(); // 볼륨 업데이트
            }
        }
    }

    public void UpdateMusicVolume()
    {
        // 배경음악 슬라이더의 값에 따라 오디오 믹서의 음량 변경
        float volume = Mathf.Log10(musicSlider.value + 0.001f) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void UpdateSFXVolume()
    {
        // 효과음 슬라이더의 값에 따라 오디오 믹서의 음량 변경
        float volume = Mathf.Log10(sfxSlider.value + 0.001f) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}
