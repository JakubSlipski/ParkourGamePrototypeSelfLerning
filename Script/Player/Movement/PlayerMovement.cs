using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;
    public float climbSpeed;
    public float swingSpeed;
    public float dashSpeed;
    public float airMinSpeeed;
    public float vaultSpeed;
    
    private float dashSpeedChangeFactor;
    public float maxYSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;


    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYscale;
    private float startYscale;
    public bool crouchingBlock;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Camera Effects")]
    public CameraMovment cam;
    public float normalFov = 80f;
    public float grappleFov = 90f;

    [Header("References")]
    public Climbing climbingScript;
    public LedgeGrabbing lg;
    public StaminaControl SC;
    public HealthControl HC;
    public Animator anim;
    public GameObject character;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovmentState state;

    public enum MovmentState
    {
        stay,
        freeze,
        unlimited,
        walking,
        sprinting,
        wallrunning,
        climbing,
        vaulting,
        crouching,
        sliding,
        swinging,
        dashing,
        monkeybar,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;
    public bool climbing;
    public bool vaulting;
    public bool dashing;
    public bool swinging;
    public bool isSwingingPole;

    public bool freeze;
    public bool unlimited;
    public bool activeGrapple;

    public bool restricted;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYscale = transform.localScale.y;

        cam.DoFov(normalFov);
    }

    private void Update()
    {

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if ((state == MovmentState.walking || state == MovmentState.sprinting || state == MovmentState.crouching) && !activeGrapple)
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
        if (lg.holding)
        {
            horizontalInput = 0;
            verticalInput = 0;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey) && grounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crousch
        if(Input.GetKeyUp(crouchKey)) 
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
    }

    bool keepMomentum;

    public void StateHandler()
    {

        //Mode - Stay FIXME: CHECK STATUS
        /*if(grounded)
        {
            state = MovmentState.stay;
            desiredMoveSpeed = walkSpeed;
            if (SC.stamina < SC.maxStamina)
                SC.stamina += Time.deltaTime * 8;

            if(HC.healingTime > 0.5)
                HC.healingEnable = true;
        }*/
        if (isSwingingPole && SC.stamina >= 0)
        {
            state = MovmentState.monkeybar;
            desiredMoveSpeed = swingSpeed;
            SC.stamina -= Time.deltaTime * 4;
            HC.healingEnable = false;
        }
        //Mode - Swinging
        else if (swinging && SC.stamina >= 0)
        {
            state = MovmentState.swinging;
            desiredMoveSpeed = swingSpeed;
            SC.stamina -= Time.deltaTime * 4;
            HC.healingEnable = false;

        }
        //Mode - Dashing
        else if (dashing && SC.stamina >= SC.minStamina)
        {
            state = MovmentState.dashing;
            desiredMoveSpeed = dashSpeed;
            SC.stamina -= SC.minStamina;
            HC.healingEnable = false;

        }
        //Mode - freeze
        else if(freeze)
        {
            state = MovmentState.freeze;
            desiredMoveSpeed = 0f;
            rb.velocity = Vector3.zero;
            HC.healingEnable = false;

        }
        //Mode - Unlimted
        else if (unlimited)
        {
            state = MovmentState.unlimited;
            desiredMoveSpeed = walkSpeed;

            return;
        }
        //Mode - Vaulting
        else if (vaulting)
        {
            state = MovmentState.vaulting;
            desiredMoveSpeed = vaultSpeed;

        }
        //Mode - Climbing
        else if(climbing && SC.stamina > 0)
        {
            state = MovmentState.climbing;
            desiredMoveSpeed = climbSpeed;
            SC.stamina -= Time.deltaTime * 4;
            HC.healingEnable = false;

        }
        //Mode - Wallrunning
        else if (wallrunning && SC.stamina > 0)
        {
            state = MovmentState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
            SC.stamina -= Time.deltaTime * 4;
            HC.healingEnable = false;

        }
        //Mode - sliding
        else if(sliding && SC.stamina > 0)
        {
            state = MovmentState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
                keepMomentum = true;
            }
            else
                desiredMoveSpeed = sprintSpeed;
        
            SC.stamina -= Time.deltaTime * 4;

        }

        //Mode - crouching
        else if (Input.GetKey(crouchKey) && grounded || crouchingBlock)
        {
            state = MovmentState.crouching;
            desiredMoveSpeed = crouchSpeed;
            if (SC.stamina < SC.maxStamina && SC.staminaRegen)
                SC.stamina += Time.deltaTime * 8;

        }
        //Mode - spirnt
        else if (grounded && Input.GetKey(sprintKey)) // && SC.stamina > 0 - ADD THIS IF NEED STAMINA CONTROL
        {
            state = MovmentState.sprinting;
            desiredMoveSpeed = sprintSpeed;

            //SPEND STAMIAN DISABELE FOR TEST VAULTING OR BETTER PLAN FOR SPRINTING
            //SC.staminaRegen = false;
            //SC.stamina -= Time.deltaTime * 4;
        }
        //Mode - walk
        else if(grounded)
        {
            state = MovmentState.walking;
            desiredMoveSpeed = walkSpeed;
            if (SC.stamina < SC.maxStamina && SC.staminaRegen)
                SC.stamina += Time.deltaTime * 8;

            if(horizontalInput != 0 || verticalInput != 0)
                anim.SetBool("Walk", true);
            else
                anim.SetBool("Walk", false);
            
        }
        //Mode - air
        else
        {
            state = MovmentState.air;
            desiredMoveSpeed = airMinSpeeed;
        }

        bool desiredMoveSpeedChange = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedChange)
        {
            if(keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        //Check if desiredMoveSpeed has change
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementspeed to desired value
        float time = 0;
        float difference =  Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while( time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time/difference);
            time += Time.deltaTime;
            yield return null;
        }
    }
    private void MovePlayer()
    {
        if (restricted) return;
        if (state == MovmentState.dashing) return;
        if (activeGrapple) return;
        if (swinging) return;


        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // trun graviti off while on slope
        if(!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (activeGrapple) return;

        // limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        // limit speed on ground or in air
        else 
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        // limit y velo
        if(maxYSpeed !=0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);

    }

    private void Jump()
    {

        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool enableMovementOnNextTouch;

    public void JumpToPosition(UnityEngine.Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculatedJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        cam.DoFov(grappleFov);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        cam.DoFov(normalFov);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    // on slope/ on '<'
    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle&& angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public Vector3 CalculatedJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    IEnumerator StopStamnaRegen()
    {
        yield return new WaitForSeconds(5f);
        SC.staminaRegen = true;
    }
}
