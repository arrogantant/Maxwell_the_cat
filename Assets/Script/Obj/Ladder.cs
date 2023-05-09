using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
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
