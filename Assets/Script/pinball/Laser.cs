using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    public float forceOnHit = 5f;

    private void OnEnable()
    {
        Invoke("DisableLaser", lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(direction * forceOnHit, ForceMode2D.Impulse);
            }
        }
    }

    private void DisableLaser()
    {
        ObjectPool.Instance.ReturnLaser(gameObject);
    }
}
