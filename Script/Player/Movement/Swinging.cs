using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrapple;
    public PlayerMovement pm;
    public StaminaControl SC;

    [Header("Swinging")]
    private float maxSwingDistance = 30f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse1;

    private void Update()
    {
        if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey) || SC.stamina <= 1) StopSwing();

        CheckForSwingPoints();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwing()
    {
        if (predictionHit.point == Vector3.zero) return;

        //Deactive grapple
        if(GetComponent<Grappling>() != null )
            GetComponent<Grappling>().StopGrapple();
        pm.ResetRestrictions();

        pm.swinging = true;
        lr.enabled = true;

        SC.staminaRegen = false;

        SC.stamina -= Time.deltaTime * 4;


            swingPoint = predictionHit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            // the distance grapple will try
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // Settings
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        
    }

    public void StopSwing()
    {
        lr.enabled = false;
        lr.positionCount = 0;
        Destroy(joint);
        pm.swinging = false;
        //Stamina
        StartCoroutine(StopStamnaRegen());
    }

    private Vector3 currentGrapplePosition;

    public void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, swingPoint);
    }

    IEnumerator StopStamnaRegen()
    {
        yield return new WaitForSeconds(5f);
        SC.staminaRegen = true;
    }

    public void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
            out sphereCastHit, maxSwingDistance, whatIsGrapple);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
            out raycastHit, maxSwingDistance, whatIsGrapple);

        Vector3 realHitPoint;

        //Option1 - Direct Hit
        if(raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        //Option2 - Predicted
        else if(sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;
        //Option3 - miss
        else
            realHitPoint = Vector3.zero;

        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }
        //predict point
        predictionHit.point = raycastHit.point == Vector3.zero ? sphereCastHit.point : raycastHit.point;
    }
}
