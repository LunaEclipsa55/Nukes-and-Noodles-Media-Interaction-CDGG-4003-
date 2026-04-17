/*TODO:
 - make movement more satifying by improving the speed acceleration etc
 - add animations
 - create levitating ability 
 */


// FOR THIS SCRIPT TO WORK THE GROUND THAT THE PLAYER IS ON NEED TO BE ON THE GROUND LAYER
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerData Data;
    public Rigidbody2D rb { get; private set; }
    
    private Vector2 moveDirection;
    public float LastOnGroundTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }

    private bool isJumping;
    private bool isGrounded;

    private bool isLevitating; 
    private bool canLeviateJump;
    public static bool isFacingRight = true;

    [Header("Input")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference levitate;


    
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask groundLayer;

    //Animator 
    [SerializeField] private Animator _animator;

    private void FixedUpdate()
    {
        Run();
        
        //Test Levitating
        if (isLevitating && !isGrounded)
        {
            if (rb.linearVelocity.y < -2f) // only when falling
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

    private void Awake()
    {
        
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
        
        // Read movement input
        moveDirection = move.action.ReadValue<Vector2>();

        if (moveDirection != Vector2.zero)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }

        //Read levitation input
        if (levitate.action.ReadValue<float>() > 0.0)
        {
            isLevitating = true;
        } else
        {
            isLevitating = false;   
        }

        // Tick timers
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;

        // Ground check
        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
        {
            LastOnGroundTime = Data.coyoteTime;
            isJumping = false;
            isGrounded = true; 

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
            
        if (moveDirection.x < 0 && isFacingRight)
        {
            Flip();
        }
        else if (moveDirection.x > 0 && !isFacingRight)
        {
            Flip();
        }

        
    }

    private void OnLevitationStart(InputAction.CallbackContext ctx)
    {
        
    } 

    private void OnLevitationCanceled(InputAction.CallbackContext ctx)
    {
        
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
        
        isJumping = true;
        isGrounded = false;
    }
    
    private void LevitateJump()
    {
        float force = Data.jumpForce * 0.8f; // slightly weaker than normal jump

        // Reset downward velocity so jump feels responsive
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping && LastPressedJumpTime > 0;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnJump");
        if (ctx.performed)
        {
            LastPressedJumpTime = Data.jumpInputBufferTime;
            
            //Ability to boost while levitating
            if (isLevitating && !isGrounded)
            {
                canLeviateJump = true; 
            }

        }
    }
    
    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "Horizontal")
        {
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }
}
