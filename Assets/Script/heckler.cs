using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heckler : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float pushForce = 30f;
    private Transform playerTransform;

    private void FixedUpdate()
    {
        // 방해꾼이 오른쪽으로 이동하는 코드
        transform.position += Vector3.left * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            playerRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }
}

