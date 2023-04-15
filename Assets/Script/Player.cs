using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float checkDistance = 0.1f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded = false;
    private Vector2 moveDirection;
    private bool jumpRequest = false;
    Animator myAnimator;
    [SerializeField] Vector2 groundCheckOffset;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GetComponent<PlayerInput>().actions["Jump"].triggered && isGrounded)
        {
            jumpRequest = true;
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(moveDirection.x * speed, rb.velocity.y);
        rb.velocity = movement;
        
        Vector2 checkPosition = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        isGrounded = Physics2D.Raycast(checkPosition, Vector2.down, checkDistance, groundLayer);

        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
        if (moveDirection.x != 0)
        {
            myAnimator.SetBool("run", true);
            sr.flipX = moveDirection.x < 0;
        }
        else
        {
            myAnimator.SetBool("run", false);
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector2 checkPosition = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(checkPosition, Vector2.down * checkDistance);
        }
    }
}