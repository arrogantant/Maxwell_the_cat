using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Audio_Magane : MonoBehaviour
{
    public AudioMixer audioMixer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        // 게임이 시작될 때 저장된 볼륨 값을 가져와 적용
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);

        // 볼륨 값들을 로그 스케일로 변환
        float dbMusicVolume = Mathf.Log10(musicVolume + 0.001f) * 20;
        float dbSFXVolume = Mathf.Log10(sfxVolume + 0.001f) * 20;

        // 변환된 볼륨 값들을 오디오 믹서에 설정
        audioMixer.SetFloat("MusicVolume", dbMusicVolume);
        audioMixer.SetFloat("SFXVolume", dbSFXVolume);
    }
}
