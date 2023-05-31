using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public CameraMovment came;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappling;
    public LineRenderer lr;
    public StaminaControl SC;
    public Swinging sw;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse2;

    private bool grappling;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(grappleKey) && SC.stamina >= 10) StartGrapple();

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        //Deactive swinging
        if (GetComponent<Swinging>() != null)
            GetComponent<Swinging>().StopSwing();
        pm.ResetRestrictions();

        SC.stamina -= 10;

        came.DoFov(65f);

        lr.positionCount = 2;

        grappling = true;

        pm.freeze = true;

        if (sw.predictionHit.point != Vector3.zero)
        {
            grapplePoint = sw.predictionHit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        
        lr.enabled = true;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {

        pm.freeze = false;
        
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        came.DoFov(pm.normalFov);

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;

        lr.positionCount = 0;
    }
}
