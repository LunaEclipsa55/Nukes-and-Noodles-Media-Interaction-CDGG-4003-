using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerData Data;
    public Rigidbody2D rb { get; private set; }
    public float moveSpeed;
    Vector2 moveDirection;
    public float LastOnGroundTime { get; private set; }


    [SerializeField] private InputActionReference move;
    
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask _groundLayer;

    

    private void FixedUpdate()
    {
        Run();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        
        LastOnGroundTime -= Time.deltaTime;
        
        //Ground Check
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
            LastOnGroundTime = 0.1f;
    }

    private void Run()
    {
        float targetSpeed = moveDirection.x * Data.runMaxSpeed;

        float accelRate;
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        
        // if(Data.doConserveMomentum && Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        // {
        //     //Prevent any deceleration from happening, or in other words conserve are current momentum
        //     //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
        //     accelRate = 0; 
        // }
        
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float movement = speedDif * accelRate;        
        rb.AddForce(movement * Vector2.right);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * Data.jumpForce, ForceMode2D.Impulse);
        LastOnGroundTime = 0;
        lastJumpTime = 0;
        isJumping = true;
        jumpInputReleased = false;
    }
    
}
