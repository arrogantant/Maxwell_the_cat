using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinballpad : MonoBehaviour
{
    public float bounceForce = 10f;
    public AudioSource audioSource;
    public AudioClip jumpSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            Vector2 diagonalForce = new Vector2(1, 1).normalized * bounceForce; // 대각선 방향으로 힘을 적용
            rb.AddForce(diagonalForce, ForceMode2D.Impulse);
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
