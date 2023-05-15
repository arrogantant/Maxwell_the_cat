using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacecraft : MonoBehaviour
{
    public float speed = 10f;
    private bool movingRight = true;
    public Transform raycastOrigin;
    public float detectionDistance = 1f;
    public LayerMask wallLayer;

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (Physics2D.Raycast(raycastOrigin.position, Vector2.right, detectionDistance, wallLayer))
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (Physics2D.Raycast(raycastOrigin.position, Vector2.left, detectionDistance, wallLayer))
            {
                movingRight = true;
            }
        }
    }
}
