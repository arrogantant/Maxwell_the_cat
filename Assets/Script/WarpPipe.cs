using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPipe : MonoBehaviour
{
    [SerializeField] private GameObject connectedPipe;
    [SerializeField] private float yOffset = 1.0f;

    public void TeleportPlayer(Transform playerTransform)
    {
        Vector2 newPosition = new Vector2(connectedPipe.transform.position.x, connectedPipe.transform.position.y + yOffset);
        playerTransform.position = newPosition;
    }
}