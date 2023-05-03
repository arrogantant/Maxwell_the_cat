using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10f;
    public float maxHeight = 5f; // 플레이어의 최대 높이 제한

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            float currentHeight = other.transform.position.y;

            if (currentHeight < maxHeight)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
