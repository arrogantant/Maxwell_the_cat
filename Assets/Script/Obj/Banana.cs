using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Collider2D exceptionCollider;

    private bool isRotating = true;

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == exceptionCollider)
        {
            return;
        }

        isRotating = false;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;
                playerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
