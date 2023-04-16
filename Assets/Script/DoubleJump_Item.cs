using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump_Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (!player.IsGrounded())
            {
                int currentMaxJumpCount = player.GetMaxJumpCount();
                player.SetMaxJumpCount(currentMaxJumpCount + 1);
                Destroy(gameObject);
            }
        }
    }
}
