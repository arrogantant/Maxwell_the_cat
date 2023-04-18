using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heckler : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float pushForce = 30f;
    private Transform playerTransform;
    private float lastPushTime;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        // 방해꾼이 오른쪽으로 이동하는 코드
        transform.position += Vector3.left * speed * Time.fixedDeltaTime;
    }

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            Vector2 newForce = pushDirection * pushForce;

            // 적용될 힘을 계산하고 최대 힘을 초과하지 않도록 제한합니다.
            Vector2 resultingForce = playerRigidbody.velocity + newForce;
            resultingForce = Vector2.ClampMagnitude(resultingForce, player.GetMaxPushForce());

            playerRigidbody.velocity = resultingForce;
        }
    }
}
}

