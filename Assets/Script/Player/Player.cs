using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public int jumpCount = 0;
    [SerializeField] int maxJumpCount = 2;
    public bool isDashing = false;
    public bool canDash = true;
    [SerializeField] float swampSpeedModifier = 0.5f;
    [SerializeField] float swampAnimationSpeedModifier = 0.5f;
    private bool isInSwamp = false;
    private bool canDashSwamp;
    private int dashCount = 1;
    public Rigidbody2D rb;
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
    //사다리
    [SerializeField] private LayerMask ladderLayer;
    private bool isOnLadder = false;
    private bool isClimbingUp;
    public bool IsClimbingUp { get => isClimbingUp; }
    private float originalGravityScale; //중력
    private float initialDashSpeed; //대쉬
    private bool isMovingToObject = false;
    private GameObject targetObject;
    private Vector2 targetDirection;
    private bool isTouchingDashObject = false;
    [SerializeField] float maxSpeed = 20f;
    private PlayerInput playerInput;
    private GameManager gameManager;
    [SerializeField] private GameObject[] ladderObject;
    private PauseManager pauseManager;

    public static Player instance = null;
    public AudioClip jumpSound; // Inspector에서 설정
    public AudioClip dashSound; // Inspector에서 설정
    public AudioSource audioSource;
    private Coroutine dashCoroutine;
    private float lastDashTime = -999f; // 대쉬를 마지막으로 사용한 시간을 저장하기 위한 변수
    public static Player Instance { get; private set; }
    private bool insideSlimeWall = false; // SlimeWall 안에 있는지 체크
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pauseManager = FindObjectOfType<PauseManager>();
        canDashSwamp = true;
        originalGravityScale = rb.gravityScale;
        initialDashSpeed = speed;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
    }
    void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            ResetGame();
        }

        if (pauseManager != null && pauseManager.IsGamePaused())
        {
            return;
        }
        CheckForPipeTeleport();
        //점프
        if ((GetComponent<PlayerInput>().actions["Jump"].triggered && jumpCount < maxJumpCount) || (isOnLadder && GetComponent<PlayerInput>().actions["Jump"].triggered))
        {
            if(isDashing)
            {
                StopCoroutine(Dash());
                isDashing = false;
            }
            Jump();
            jumpCount++;
            if (insideSlimeWall)
            {
                rb.gravityScale = originalGravityScale;
                insideSlimeWall = false;
            }
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
        
        if (Time.time >= lastDashTime + dashCooldown) // 대쉬 재사용 대기 시간이 지났는지 확인
        {
            canDash = true;
        }

        if (GetComponent<PlayerInput>().actions["Dash"].triggered && !isDashing && canDash && canDashSwamp) // 'canDash' 변수 사용
        {
            if(isDownKeyPressed)
            {
                StartCoroutine(ButtStomp());
            }
            else
            {
                StartCoroutine(Dash());
            }
            canDash = false;
            lastDashTime = Time.time; // 대쉬를 사용한 시간을 저장
            if (insideSlimeWall)
            {
                rb.gravityScale = originalGravityScale;
                insideSlimeWall = false;
            }
        }
        if (GetComponent<PlayerInput>().actions["Interact"].triggered)
        {
            InteractWithObject();
        }
        float climbValue = playerInput.actions["Climb"].ReadValue<float>();
        isClimbingUp = climbValue > 0;

        if (isOnLadder)
        {
            float verticalInput = playerInput.actions["Move"].ReadValue<Vector2>().y;
            bool leftOrRightPressed = Mathf.Abs(playerInput.actions["Move"].ReadValue<Vector2>().x) > 0;
            bool isJumping = playerInput.actions["Jump"].ReadValue<float>() > 0;

            if (isJumping && leftOrRightPressed)
            {
                SetIsOnLadder(false); // 점프하면 사다리에서 떨어짐

                foreach (GameObject ladder in ladderObject)
                {
                    if (ladder != null)
                    {
                        StartCoroutine(TemporarilyDisableCollider(ladder, 0.1f));
                    }
                }
            }
        }
        if (insideSlimeWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, -0.5f); // 천천히 아래로 내려가게 설정
        }
    }
    private IEnumerator TemporarilyDisableCollider(GameObject obj, float time)
    {
        if (obj != null)
        {
            BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
                yield return new WaitForSeconds(time);
                collider.enabled = true;
            }
        }
    }
    void FixedUpdate()
    {
        if (!isDashing)
        {
            bool isJumping = GetComponent<PlayerInput>().actions["Jump"].ReadValue<float>() > 0;
            bool leftOrRightPressed = Mathf.Abs(GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>().x) > 0;

            if (!isOnLadder || (isOnLadder && GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>().y <= 0) || (isOnLadder && isJumping && leftOrRightPressed))
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
            }

            if (Mathf.Abs(rb.velocity.x) < initialDashSpeed)
            {
                isDashing = false;
            }

            float clampedVerticalSpeed = Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed);
            rb.velocity = new Vector2(rb.velocity.x, clampedVerticalSpeed);

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

        if (isOnLadder)
        {
            float verticalInput = GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>().y;
            float horizontalInput = GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>().x;
            bool leftOrRightPressed = Mathf.Abs(horizontalInput) > 0;
            bool isJumping = GetComponent<PlayerInput>().actions["Jump"].ReadValue<float>() > 0;

            if (isJumping && leftOrRightPressed)
            {
                isOnLadder = false;
            }
            else
            {
                jumpCount = 0;
                if (verticalInput > 0)
                {
                    rb.velocity = new Vector2(horizontalInput * speed, speed); 
                }
                else if (verticalInput < 0)
                {
                    rb.velocity = new Vector2(horizontalInput * speed, -speed); 
                }
                else
                {
                    rb.velocity = new Vector2(horizontalInput * speed, 0); 
                }
            }
        }

        if (isMovingToObject)
        {
            MoveTowardObject();
        }

        if (isMovingToObject && !isTouchingDashObject)
        {
            isMovingToObject = false;
        }

        if (isMovingToObject)
        {
            MoveTowardObject();
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
        if (isDashing)
        {
            if(dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            
            isDashing = false;
            canDash = true;
            dashCount++;
            rb.velocity = Vector2.zero; // 대쉬 중이었다면 속도를 초기화합니다.
        }
        Vector2 movement = new Vector2(moveDirection.x * speed, rb.velocity.y);
        rb.velocity = movement;
        PlaySound(jumpSound);

        // 더블 점프를 위한 수정
        if (GetComponent<PlayerInput>().actions["Jump"].triggered && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Y축 속도를 초기화하여 점프 높이를 일정하게 유지
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator Dash()
    {
        // Dash는 ButtStomp이 진행 중일 때에도 수행할 수 있습니다.
        if(!canDash || isDashing) yield break;

        isDashing = true;
        canDash = false;
        float dashStartTime = Time.time;
        Vector2 dashVelocity = new Vector2(dashForce * (sr.flipX ? -1 : 1), rb.velocity.y);
        initialDashSpeed = Mathf.Abs(dashVelocity.x); // 초기 대쉬 속도 저장
        PlaySound(dashSound);

        while (Time.time < dashStartTime + dashDuration && !GetComponent<PlayerInput>().actions["Jump"].triggered)
        {
            rb.velocity = dashVelocity;
            yield return null;
        }

        isDashing = false;
    }
IEnumerator ButtStomp()
{
    isButtStomping = true;
    float stompForce = -jumpForce * 2; // 더 빠르게 내려가도록 힘을 증가시킵니다.
    rb.velocity = new Vector2(rb.velocity.x, 0);
    rb.AddForce(new Vector2(0, stompForce), ForceMode2D.Impulse);

    yield return new WaitUntil(() => isGrounded);
    isButtStomping = false;
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
        if (collision.CompareTag("WarpPipe"))
        {
            isOverPipe = true;
            currentPipe = collision.GetComponent<WarpPipe>();
        }
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
        }
        if (collision.CompareTag("DashObject")&& isDashing)
        {
            isTouchingDashObject = true;
            targetObject = collision.gameObject;
            isMovingToObject = true;
            targetDirection = GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>();
            rb.velocity = Vector2.zero;
        }
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            gameManager.ItemConsumed();
        }
        if (collision.CompareTag("SlimeWall"))
        {
            rb.gravityScale = 0; // 중력 영향을 받지 않게 설정
            insideSlimeWall = true; // SlimeWall 안에 있다고 표시
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
        }
        if (collision.CompareTag("DashObject"))
        {
            isTouchingDashObject = false;
        }
        if (collision.CompareTag("WarpPipe"))
        {
            isOverPipe = false;
            currentPipe = null;
        }
        if (collision.CompareTag("SlimeWall"))
        {
            rb.gravityScale = originalGravityScale; // 중력 스케일을 원래대로 복원
            insideSlimeWall = false; // SlimeWall 안에 없다고 표시
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {   
        if (isGrounded && isButtStomping)
        {
            isButtStomping = false;
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
        if (isOverPipe && (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame))
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
    public void SetIsOnLadder(bool value)
    {
        isOnLadder = value;

        if (!isOnLadder) // 사다리에서 떨어질 때
        {
            rb.gravityScale = originalGravityScale; // 중력을 다시 활성화
        }
        else if (isOnLadder && isClimbingUp)
        {
            rb.gravityScale = 0f; // 사다리를 올라갈 때 중력을 비활성화
        }
    } 
        private void MoveTowardObject()
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + targetDirection * Time.fixedDeltaTime * speed;
        RaycastHit2D hit = Physics2D.Linecast(currentPosition, newPosition);

        if (hit.collider != null && hit.collider.gameObject == targetObject)
        {
            transform.position = hit.point;
            isMovingToObject = false;
        }
        else
        {
            rb.MovePosition(newPosition);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // "Player Start" 태그를 가진 게임 오브젝트를 찾습니다.
        GameObject playerStart = GameObject.FindWithTag("Player Start");
        
        // 플레이어가 없거나 "Player Start" 오브젝트가 없다면, 더 이상 실행하지 않습니다.
        if (playerStart == null || this.gameObject == null)
        {
            return;
        }

        // 플레이어의 위치를 "Player Start" 오브젝트의 위치로 설정합니다.
        this.transform.position = playerStart.transform.position;
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}