using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingBlockCheck : MonoBehaviour
{
    [Header("Variables")]
    public float checkDist;
    private float startYscale;

    [Header("References")]
    public PlayerMovement pm;
    public GameObject player;
    public LayerMask whatIsBlockCrouch;

    private void Start()
    {
        startYscale = transform.localScale.y;
    }

    private void Update()
    {
        if (Physics.Raycast(player.transform.position, player.transform.up, checkDist, whatIsBlockCrouch))
        {
            pm.crouchingBlock = true;
            transform.localScale = new Vector3(transform.localScale.x, pm.crouchYscale, transform.localScale.z);
        }
        else if(pm.state != PlayerMovement.MovmentState.crouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
        { 
            pm.crouchingBlock = false;
        }
    }
}
