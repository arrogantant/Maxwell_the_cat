using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump_Item : MonoBehaviour
{
    [SerializeField] private float respawnTime = 3f;
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider1;

    private void Start()
    {
        initialPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                player.SetMaxJumpCount(currentMaxJumpCount + 1);
                StartCoroutine(Respawn());
            }
        }
    }

    IEnumerator Respawn()
    {
        spriteRenderer.enabled = false;
        collider1.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        spriteRenderer.enabled = true;
        collider1.enabled = true;
        transform.position = initialPosition;
    }
}
