using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector2 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = new Vector2(moveDirection.x * speed, rb.velocity.y);
        rb.velocity = movement;
        if (GetComponent<PlayerInput>().actions["Jump"].triggered && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}