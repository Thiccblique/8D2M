using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }
    public MovementState state;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Audio")]
    public AudioSource walkSound;
    public AudioSource sprintSound;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Checks where the ground is with raycast by shooting current position down and checks if it hit something
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); 
        
        MyInput();
        SpeedControl();
        StateHandler();
        AudioInputs();

        // Applys the drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

    }
   
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Start Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            // Added downward force so player does not float in the air when shrinked down
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // Stop Crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    private void AudioInputs()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            walkSound.Play();

            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(sprintKey))
            {
                walkSound.Stop();
                sprintSound.Play();
            }
        }
       
        if (Input.GetKeyUp(KeyCode.W))
        {
            walkSound.Stop();

            if (Input.GetKeyUp(sprintKey))
            {
               
                sprintSound.Stop();
            }
        }

    }

    private void StateHandler()
    {
        // MovementState - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // MovementState - Sprinting
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // MovementState - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // MovementState - Air
        else
        {
            state = MovementState.air;
        }
        
    }

    private void MovePlayer()
    {
        // Calculate movement direction, so you'll always walk in the direction you're looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Add force to the player then make it faster
        // On the ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // In the air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);


    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        // If you go faster than your movement speed, you calculate what your max velocity would be and then apply it
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // used Impulse to apply force once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
