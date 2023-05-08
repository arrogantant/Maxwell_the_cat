using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rope : MonoBehaviour
{
[SerializeField] private Player playerScript;
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player") && playerScript.IsClimbingUp)
    {
        playerScript.SetIsOnLadder(true);
    }
}

private void OnTriggerStay2D(Collider2D collision)
{
    if (collision.CompareTag("Player") && playerScript.IsClimbingUp)
    {
        playerScript.SetIsOnLadder(true);
    }
}

private void OnTriggerExit2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        playerScript.SetIsOnLadder(false);
    }
}

}
