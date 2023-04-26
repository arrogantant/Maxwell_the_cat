using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private LayerMask swampLayer;
    [SerializeField] private float sinkingSpeed = 1.0f;
    [SerializeField] float checkDistance = 0.1f;
    [SerializeField] float acceleration = 15.0f;
    [SerializeField] float deceleration = 40.0f;
    [SerializeField] float groundCheckRadius = 0.15f;
    [SerializeField] float dashForce = 10.0f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCooldown = 0.5f;
    public bool isButtStomping = false;
    private int jumpCount = 0;
    [SerializeField] int maxJumpCount = 2;
    public bool isDashing = false;
    private bool canDash = true;
    [SerializeField] float swampSpeedModifier = 0.5f;
    [SerializeField] float swampAnimationSpeedModifier = 0.5f;
    private bool isInSwamp = false;
    private bool canDashSwamp;
    private int dashCount = 1;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded = false;
    private Vector2 moveDirection;
    Animator myAnimator;
    [SerializeField] Vector2 groundCheckOffset;
    private bool isOverPipe = false;
    private WarpPipe currentPipe;
    [SerializeField] private float maxPushForce = 30f;
    public float interactionRange = 2f;
    private Interactable nearestInteractable;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        canDashSwamp = true;

        //세이브
        if (PlayerPrefs.HasKey("SavedX") && PlayerPrefs.HasKey("SavedY") && PlayerPrefs.HasKey("SavedZ"))
        {
            Vector3 savedPosition = new Vector3(PlayerPrefs.GetFloat("SavedX"), PlayerPrefs.GetFloat("SavedY"), PlayerPrefs.GetFloat("SavedZ"));
            transform.position = savedPosition;
        }
    }

    void Update()
    {
        CheckForPipeTeleport();
        //점프
        if (GetComponent<PlayerInput>().actions["Jump"].triggered && jumpCount < maxJumpCount)
        {
            Jump();
            jumpCount++;
        }
           if (isGrounded)
        {
            myAnimator.SetBool("Jump", false);
            maxJumpCount = 1;
        }
            else
        {
            myAnimator.SetBool("Jump", true);
        }
        var keyboard = Keyboard.current;
        bool isDownKeyPressed = keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed;
        
        if (GetComponent<PlayerInput>().actions["Dash"].triggered && !isDashing && dashCount > 0 && canDash && canDashSwamp) // 'canDash' 변수 사용
        {
            if(isDownKeyPressed)
            {
                StartCoroutine(ButtStomp());
            }
            else
            {
                StartCoroutine(Dash());
            }
            dashCount--; // 대쉬 횟수 감소
            canDash = false; // 대쉬를 사용한 후 다시 사용할 수 없도록 함
        }
        if (GetComponent<PlayerInput>().actions["Interact"].triggered)
        {
            InteractWithObject();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            float targetSpeed = moveDirection.x * speed;
            float currentSpeed = rb.velocity.x;

            if (Mathf.Abs(moveDirection.x) > 0)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.fixedDeltaTime);
            }

            Vector2 movement = new Vector2(currentSpeed, rb.velocity.y);
            rb.velocity = movement;
            Vector2 checkPosition = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
            bool isInSwamp = Physics2D.OverlapCircle(checkPosition, groundCheckRadius, swampLayer);

            if (isInSwamp)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - sinkingSpeed * Time.fixedDeltaTime);
            }
        }  
        isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y), groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = 0;
            if (isButtStomping)
            {
                isButtStomping = false;
            }
        }
    }
    private void InteractWithObject()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        float minDistance = float.MaxValue;

        foreach (Collider2D collider in collidersInRange)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestInteractable = interactable;
                }
            }
        }

        if (nearestInteractable != null)
        {
            nearestInteractable.Interact();
        }
    
    }
    void Jump()
    {
        Vector2 movement = new Vector2(moveDirection.x * speed, rb.velocity.y);
        rb.velocity = movement;

        // 더블 점프를 위한 수정
        if (GetComponent<PlayerInput>().actions["Jump"].triggered && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Y축 속도를 초기화하여 점프 높이를 일정하게 유지
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false; // 대쉬 중에는 대쉬를 사용할 수 없도록 함
        float dashStartTime = Time.time;
        Vector2 originalVelocity = rb.velocity;
        Vector2 dashVelocity = new Vector2(dashForce * (sr.flipX ? -1 : 1), originalVelocity.y);

        while (Time.time < dashStartTime + dashDuration)
        {
            rb.velocity = dashVelocity;
            yield return null;
        }

        rb.velocity = originalVelocity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown); // 대쉬 쿨다운 대기
        canDash = true; // 쿨다운이 끝나면 대쉬를 다시 사용할 수 있음
        dashCount++; // 대쉬 쿨타임이 끝나면 대쉬 횟수를 증가시킴
    }
    IEnumerator ButtStomp()
    {
        isButtStomping = true;
        canDash = false;
        float stompForce = -jumpForce * 2; // 더 빠르게 내려가도록 힘을 증가시킵니다.
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, stompForce), ForceMode2D.Impulse);

        yield return new WaitUntil(() => isGrounded);
        canDash = true;
        dashCount++;
    }

    void OnMove(InputValue value)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Breakable") && isDashing)
        {
            Breakable breakable = collision.gameObject.GetComponent<Breakable>();
            if (breakable != null)
            {
                breakable.Break();
            }
        }
        if (collision.gameObject.CompareTag("ButtStomp") && isButtStomping)
        {
            ButtStomp buttStomp = collision.gameObject.GetComponent<ButtStomp>();
            if (buttStomp != null)
            {
                buttStomp.Break();
            }
        }
        if (collision.tag == "WarpPipe")
        {
            isOverPipe = true;
            currentPipe = collision.GetComponent<WarpPipe>();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {   
        if (isGrounded && isButtStomping)
        {
            isButtStomping = false;
        }

        if (collision.gameObject.CompareTag("WarpPipe"))
        {
            isOverPipe = false;
            currentPipe = null;
        }
        
        if (collision.gameObject.CompareTag("DisappearingPlatform"))
        {
            PlatformDisappear platform = collision.gameObject.GetComponent<PlatformDisappear>();
            if (platform != null)
            {
                StartCoroutine(PlatformDisappearCoroutine(platform));
            }
        }
        
    }
    private void CheckForPipeTeleport()
    {
        var keyboard = Keyboard.current;
        if (isOverPipe && (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame))
        {
            currentPipe.TeleportPlayer(transform);
        }
    }

    private IEnumerator PlatformDisappearCoroutine(PlatformDisappear platform)
    {
        yield return new WaitForSeconds(0.1f);
        platform.Disappear();
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
    public void SetIsInSwamp(bool isInSwamp)
    {
        this.isInSwamp = isInSwamp;
        if (isInSwamp)
        {
            speed *= swampSpeedModifier;
        }
        else
        {
            speed /= swampSpeedModifier;
        }
    }
    public void SetAnimationSpeedInSwamp(bool isInSwamp)
    {
        if (isInSwamp)
        {
            myAnimator.speed *= swampAnimationSpeedModifier;
        }
        else
        {
            myAnimator.speed /= swampAnimationSpeedModifier;
        }
    }

    public int GetMaxJumpCount()
    {
        return maxJumpCount;
    }

    public void SetMaxJumpCount(int newMaxJumpCount)
    {
        maxJumpCount = newMaxJumpCount;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool IsDashing()
    {
        return isDashing;
    }
    public void SetCanDashSwamp(bool value)
    {
        canDashSwamp = value;
    }
    public float GetMaxPushForce()
{
    return maxPushForce;
}
    
}