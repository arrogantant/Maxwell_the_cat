using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour
{
    [SerializeField] private float massIncreaseAmount = 1f;
    private float originalMass;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            originalMass = playerRigidbody.mass;
            playerRigidbody.mass += massIncreaseAmount;
            Player player = collision.transform.GetComponent<Player>();
            player.SetCanDashSwamp(false);
            player.SetIsInSwamp(true);
            player.SetAnimationSpeedInSwamp(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.mass = originalMass;
            Player player = collision.transform.GetComponent<Player>();
            player.SetCanDashSwamp(true);
            player.SetIsInSwamp(false);
            player.SetAnimationSpeedInSwamp(false);
        }
    }
}
