using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement pm;
    public Transform orientation;
    public Transform cam;
    public Rigidbody rb;
    public StaminaControl SC;

    [Header("Ledge Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabDistance;

    public float minTimeOnLedge;
    private float timeOnLedge;

    public bool holding;

    [Header("Ledge Jumping")]
    public KeyCode jumpKay = KeyCode.Space;
    public float ledgeJumpForwardForce;
    public float ledgeJumpUpwardForce;

    [Header("Ledge Detection")]
    public float ledgeDetectionLenght;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private Transform lastLedge;
    private Transform currLedge;

    private RaycastHit ledgeHit;

    [Header("Exiting")]
    public bool exitingLedge;
    public float exitLedgeTime;
    private float exitLedgeTimer;

    private void Update()
    {
        LedgeDetection();
        SubStateMachine();

        if(SC.stamina <= 0) ExitLedgeHold();
    }

    private void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        // SubState 1 - Holding onto ledge
        if(holding && SC.stamina > 0)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;

            SC.stamina -= Time.deltaTime;

            if (/*timeOnLedge > minTimeOnLedge &&*/ anyInputKeyPressed) ExitLedgeHold();

            if (Input.GetKeyDown(jumpKay)) LedgeJump();
        }
        // SubState 2 - Exiting
        else if(exitingLedge)
        {
            if(exitLedgeTimer > 0) exitLedgeTimer -= Time.deltaTime;
            else exitingLedge = false;
        }
    }

    Vector3 forceToAdd;

    private void LedgeDetection()
    {
        //Normal(first entry or jump from ledge to ledge
        bool ledgeDecetion = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, 
            out ledgeHit, ledgeDetectionLenght, whatIsLedge);
        //FIX ADD CD
        //UP
        if(holding && Input.GetKeyDown(KeyCode.W))
        {
            ledgeDecetion = Physics.SphereCast(transform.position, ledgeSphereCastRadius, currLedge.up, 
                out ledgeHit, ledgeDetectionLenght, whatIsLedge);
            forceToAdd = cam.up * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;
        }
        //Down
        else if (holding && Input.GetKeyDown(KeyCode.S))
        {
            ledgeDecetion = Physics.SphereCast(transform.position, ledgeSphereCastRadius, -(currLedge.up), 
                out ledgeHit, ledgeDetectionLenght, whatIsLedge);
            forceToAdd = -(cam.up) * ledgeJumpForwardForce + -(orientation.up) * ledgeJumpUpwardForce;
        }
        //Right - NEED FIX
        else if (holding && Input.GetKeyDown(KeyCode.D))
        {
            ledgeDecetion = Physics.SphereCast(transform.position, ledgeSphereCastRadius, currLedge.right, 
                out ledgeHit, ledgeDetectionLenght , whatIsLedge);
            forceToAdd = cam.right * ledgeJumpForwardForce + orientation.right * ledgeJumpUpwardForce;
        }
        //Left - NEED FIX
        else if (holding && Input.GetKeyDown(KeyCode.A))
        {
            ledgeDecetion = Physics.SphereCast(transform.position, ledgeSphereCastRadius, -(currLedge.right), 
                out ledgeHit, ledgeDetectionLenght, whatIsLedge);
            forceToAdd = -(cam.right) * ledgeJumpForwardForce + -(orientation.right) * ledgeJumpUpwardForce;
        }


        if (!ledgeDecetion) return;

        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);

        if (ledgeHit.transform == lastLedge) return;

        if (distanceToLedge < maxLedgeGrabDistance && !holding) EnterLedgeHold();
    }

    private void LedgeJump()
    {
        ExitLedgeHold();
        SC.stamina -= 1;
        Invoke(nameof(DeleyedJumpForce), 0.05f);
    }

    private void DeleyedJumpForce()
    {
        
        forceToAdd = cam.forward * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;
        rb.velocity = Vector3.zero;
        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    private void EnterLedgeHold()
    {
        holding = true;

        //Stamina
        SC.staminaRegen = false;

        pm.unlimited = true;
        pm.restricted = true;

        currLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;
        //Set position to move
        Vector3 directionToLedge = currLedge.position - transform.position;
        Vector3 posUnderLEdge = new Vector3(currLedge.position.x, currLedge.position.y - 0.4f, currLedge.position.z);
        float distanceToLedge = Vector3.Distance(transform.position, posUnderLEdge);

        //move player towards ledge
        if(distanceToLedge > 1f)
        {
            if(rb.velocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }
        // Hold onto ledge
        else
        {
            if (!pm.freeze) pm.freeze = true;
            if(pm.unlimited) pm.unlimited = false;
        }
        // Exiting if something goes wrong
        if(distanceToLedge > maxLedgeGrabDistance) ExitLedgeHold();
    }

    private void ExitLedgeHold()
    {
        exitingLedge = true;
        exitLedgeTimer = exitLedgeTime;

        holding = false;
        timeOnLedge = 0f;

        pm.restricted = false;
        pm.freeze = false;

        rb.useGravity = false;

        StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);

        StartCoroutine(StopStamnaRegen());
    }

    private void ResetLastLedge()
    {
        lastLedge = null;
    }

    IEnumerator StopStamnaRegen()
    {
        yield return new WaitForSeconds(5f);
        SC.staminaRegen = true;
    }
}
