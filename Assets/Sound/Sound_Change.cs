using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Change : MonoBehaviour
{
    public AudioClip music1; // 첫 번째 음악
    public AudioClip music2; // 두 번째 음악
    public AudioSource audioSource; // 음악을 제어할 AudioSource
    private bool playingFirstClip = true; // 현재 어떤 클립을 재생 중인지 추적

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music1; 
        {

            audioSource.Play(); // 음악 재생 시작
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            audioSource.Stop(); // 현재 재생 중인 음악 중지

            // playingFirstClip 변수를 기반으로 어떤 클립을 재생할지 결정
            if (playingFirstClip)
            {
                audioSource.clip = music2;
            }
            else
            {
                audioSource.clip = music1;
            }

            audioSource.Play(); // 새 클립 재생

            // 다음 번에 다른 클립을 재생하도록 상태 업데이트
            playingFirstClip = !playingFirstClip;
        }
    }
}
