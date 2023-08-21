using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_wall : MonoBehaviour
{
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider1;

    private void Start()
    {
        initialPosition = transform.position;
        collider1 = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (!player.IsGrounded())
            {
                int currentMaxJumpCount = player.GetMaxJumpCount();
                player.SetMaxJumpCount(currentMaxJumpCount + 2);
            }
        }
    }
}

