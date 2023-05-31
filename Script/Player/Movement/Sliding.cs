using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;
    public CameraMovment cam;
    public StaminaControl SC;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYscale;
    private float startYscale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYscale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) 
            && SC.stamina > SC.minStamina) //Remove !pm.swinging if need enable to build speed up on rope
            StartSlide();

        if(Input.GetKeyUp(slideKey) && pm.sliding || (SC.stamina <= 0 && pm.sliding))
            StopSlide();
    }

    private void FixedUpdate()
    {
        if(pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        //Stamina
        SC.staminaRegen = false;

        cam.DoFov(90f);
        // if need change hitbox height
       playerObj.localScale = new Vector3(playerObj.localScale.x, slideYscale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        if (pm.activeGrapple) StopSlide();

        Vector3 inputDirection = orientation.forward * verticalInput + orientation.forward * horizontalInput;

        SC.stamina -= Time.deltaTime * 4;

        // sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }



        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        cam.DoFov(pm.normalFov);

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYscale, playerObj.localScale.z);

        StartCoroutine(StopStamnaRegen());
    }

    IEnumerator StopStamnaRegen()
    {
        yield return new WaitForSeconds(5f);
        SC.staminaRegen = true;
    }
}
