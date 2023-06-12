using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinballbauns : MonoBehaviour
{
    public float bounceForce = 10f;
    public AudioSource audioSource;
    public AudioClip jumpSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 bounceDirection = -collision.relativeVelocity.normalized; // 플레이어가 오브젝트에 부딪힌 방향의 반대
            rb.velocity = Vector2.zero; // 플레이어의 현재 속도를 초기화
            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse); // 반대 방향으로 힘을 적용
        }
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
