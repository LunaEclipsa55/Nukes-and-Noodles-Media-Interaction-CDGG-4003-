using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip walkSound;

    private float footstepTimer;
    public float footstepInterval = 0.4f;

    public PlayerData Data;
    public Rigidbody2D rb { get; private set; }

    private Vector2 moveDirection;
    public float LastOnGroundTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }

    private bool isJumping;
    private bool isGrounded;

    private bool isLevitating;
    private bool canLeviateJump;

    public static bool levitateAbility { get; set; }
    public static bool isFacingRight = true;

    [Header("Input")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference levitate;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        levitateAbility = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        jump.action.Enable();
        jump.action.performed += OnJump;

        levitate.action.Enable();
        levitate.action.started += OnLevitationStart;
        levitate.action.canceled += OnLevitationCanceled;
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJump;
        jump.action.Disable();

        levitate.action.started -= OnLevitationStart;
        levitate.action.canceled -= OnLevitationCanceled;
        levitate.action.Disable();
    }

    void Update()
    {
        
        moveDirection = move.action.ReadValue<Vector2>();

        // Animator
        _animator.SetBool("isRunning", moveDirection != Vector2.zero);

        // Levitation input
        isLevitating = levitate.action.ReadValue<float>() > 0.0f && levitateAbility;

        // Timers
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;

        // Ground check
        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
        {
            LastOnGroundTime = Data.coyoteTime;
            isJumping = false;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Reset jump state when falling
        if (isJumping && rb.linearVelocity.y < 0)
        {
            isJumping = false;
        }

        // Attempt jump
        if (LastOnGroundTime > 0 && LastPressedJumpTime > 0 && !isJumping && isGrounded)
        {
            Jump();
        }

        // Flip
        if (moveDirection.x < 0 && isFacingRight) Flip();
        else if (moveDirection.x > 0 && !isFacingRight) Flip();

      
        HandleFootsteps();
    }

    private void FixedUpdate()
    {
        Run();

        if (isLevitating && !isGrounded)
        {
            if (rb.linearVelocity.y < -2f)
            {
                rb.AddForce(Vector2.up * 50f);
            }
        }

        if (canLeviateJump)
        {
            LevitateJump();
            canLeviateJump = false;
        }
    }

    void HandleFootsteps()
    {
        bool isMovingHorizontally = Mathf.Abs(moveDirection.x) > 0.1f;

        if (isMovingHorizontally && isGrounded)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstep();

               
                footstepTimer = footstepInterval + UnityEngine.Random.Range(-0.05f, 0.05f);
            }
        }
        else
        {
            footstepTimer = 0.1f;
        }
    }

    void PlayFootstep()
    {
        if (walkSound != null)
        {
            AudioSource.PlayClipAtPoint(walkSound, transform.position);
        }
    }

    private void Run()
    {
        float targetSpeed = moveDirection.x * Data.runMaxSpeed;

        float accelRate;
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;

        float speedDif = targetSpeed - rb.linearVelocity.x;
        float movement = speedDif * accelRate;
        rb.AddForce(movement * Vector2.right);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private void Jump()
    {
        LastOnGroundTime = 0;
        LastPressedJumpTime = 0;

        float force = Data.jumpForce;

        if (rb.linearVelocity.y < 0)
        {
            force -= rb.linearVelocity.y * moveDirection.y;
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        
        if (jumpSound != null)
        {
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }

        isJumping = true;
        isGrounded = false;
    }

    private void LevitateJump()
    {
        float force = Data.jumpForce * 0.8f;

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            LastPressedJumpTime = Data.jumpInputBufferTime;

            if (isLevitating && !isGrounded)
            {
                canLeviateJump = true;
            }
        }
    }

    private void OnLevitationStart(InputAction.CallbackContext ctx) { }
    private void OnLevitationCanceled(InputAction.CallbackContext ctx) { }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }
}