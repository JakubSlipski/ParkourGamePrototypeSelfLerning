using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PloeSwing : MonoBehaviour
{
    //PoleSwing :D Need FIX NAME && This File is reforge version of Swinging.cs
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public PlayerMovement pm;
    public StaminaControl SC;
    private Rigidbody rb;
    
    [Header("Swinging")]
    private Vector3 swingPoint;
    private SpringJoint joint;
    private bool swingingPole = false;
    private Collision colid;

    //[Header("Prediction")]
    //public RaycastHit predictionHit;
    //public float predictionSphereCastRadius;
    //public Transform predictionPoint;

    [Header("Input")]
    public KeyCode swingJumpKey;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        //List<GameObject> list = new List<GameObject>("Pole");
    }

    private void Update()
    {
        if (swingingPole)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rb.AddForce(gunTip.forward * 2f, ForceMode.Acceleration);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-(gunTip.forward * 2f), ForceMode.Acceleration);
            }
        }

        if ((Input.GetKeyUp(swingJumpKey) && swingingPole) || SC.stamina <= 1)
        {
            rb.AddForce(gunTip.forward * 8f, ForceMode.Impulse);
            rb.AddForce(gunTip.up * 2.5f, ForceMode.Impulse);
            pm.unlimited = false;
            StopSwing();
        }

        if(!swingingPole)
        {
            

        }
    }

    private void LateUpdate()
    {
        //DrawRopeDebug();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Pole")
        {
            colid = collision;
            colid.collider.isTrigger = true;
            //Function for set swingPoint from collider HERE;
            Vector3 point = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            swingPoint = point;   
            //Start swing
            StartSwing();
        }
    }

    private void StartSwing()
    {
        // if (predictionHit.point == Vector3.zero) return;
        swingingPole = true;

        pm.ResetRestrictions();

        pm.swinging = true;
        lr.enabled = true;

        SC.staminaRegen = false;

        SC.stamina -= Time.deltaTime * 4;


       // swingPoint = predictionHit.point;
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
        colid.collider.isTrigger = false;
        swingingPole = false;
        lr.enabled = false;
        lr.positionCount = 0;
        Destroy(joint);
        pm.swinging = false;

        pm.unlimited = false;

        //Stamina
        StartCoroutine(StopStamnaRegen());
    }

    private Vector3 currentGrapplePosition;

    public void DrawRopeDebug()
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

}

