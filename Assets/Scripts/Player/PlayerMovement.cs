using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    [Header("Keybinds")]
    public KeyCode jumpKey=KeyCode.Space;
    public KeyCode sprintKey=KeyCode.LeftShift;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask IsGround;
    bool grounded;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 movementDirection;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState{
        walking,
        sprinting,
        air
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump=true;
        moveSpeed=walkSpeed;
    }
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down,playerHeight*0.5f+0.2f,IsGround);
        
        PlayerInput();
        SpeedControl();

        StateHandler();
        if(grounded){
            rb.linearDamping= groundDrag;

        }else{
            rb.linearDamping=0;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    } 
    private void PlayerInput(){
        horizontalInput=Input.GetAxis("Horizontal");
        verticalInput=Input.GetAxis("Vertical");
        if(Input.GetKey(jumpKey)&&readyToJump&&grounded){
            Debug.Log("jump activated");
            readyToJump=false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);//continue to jump while holding down key
        }
    }

    private void StateHandler(){
        //sprinting
        if(grounded&& Input.GetKey(sprintKey)){
           state=MovementState.sprinting;
           moveSpeed =sprintSpeed;
        }//walking
        else if(grounded){
            state=MovementState.walking;
            moveSpeed =walkSpeed;
        }
        //air
        else{
            state=MovementState.air;

        }
    }
    private void MovePlayer(){
        movementDirection=orientation.forward * verticalInput+ orientation.right*horizontalInput;
        
        rb.AddForce(movementDirection.normalized*moveSpeed*10f, ForceMode.Force);
        
        if(grounded){
             rb.AddForce(movementDirection.normalized*moveSpeed*10f, ForceMode.Force);
        }else if(!grounded){
            rb.AddForce(movementDirection.normalized*moveSpeed*10f*airMultiplier, ForceMode.Force);
        }
        
    }
    private void SpeedControl(){
        Vector3 flatVel=new Vector3(rb.linearVelocity.x,0f,rb.linearVelocity.z);
        //limit velocity
        if(flatVel.magnitude> moveSpeed){
            Vector3 limitedVel=flatVel.normalized*moveSpeed;
            rb.linearVelocity=new Vector3(limitedVel.x, rb.linearVelocity.y,limitedVel.z);
        }
    }
    private void Jump(){
        //possibly change this in the future for rocket jumping or other types of movement

        //resets y velocity to zero
        rb.linearVelocity = new Vector3(rb.linearVelocity.x,0f,rb.linearVelocity.z);
        rb.AddForce(transform.up*jumpForce,ForceMode.Impulse);
    }
    private void ResetJump(){
        readyToJump=true;
    }
}
