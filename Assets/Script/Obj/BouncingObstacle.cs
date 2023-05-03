using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    private bool isMovingToEnd = true;

    void Update()
    {
        if (isMovingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, endPosition.position) < 0.01f)
            {
                isMovingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPosition.position) < 0.01f)
            {
                isMovingToEnd = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Player"))
    {
        collision.collider.transform.SetParent(transform);
    }
}

private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Player"))
    {
        collision.collider.transform.SetParent(null);
    }
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;
            playerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        }
    }
}
