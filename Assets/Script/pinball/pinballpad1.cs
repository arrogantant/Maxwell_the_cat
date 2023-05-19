using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinballpad1 : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            Vector2 diagonalForce = new Vector2(-1, 1).normalized * bounceForce; // 대각선 방향으로 힘을 적용
            rb.AddForce(diagonalForce, ForceMode2D.Impulse);
        }
    }
}
