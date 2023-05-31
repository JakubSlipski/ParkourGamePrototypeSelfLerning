using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPads : MonoBehaviour
{
    [Header("Value")]
    public float jumpForce = 20f;
    private float tmpForce;

    [Header("References")]
    public PlayerMovement pm;

    [Header("CheckJumpPad")]
    public float playerHeight;
    public LayerMask whatIsJumpPad;
    public bool isJumpPad;

    private void Start()
    {
        tmpForce = pm.jumpForce;
    }

    public void Update()
    {
        isJumpPad = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsJumpPad);

        if (isJumpPad)
        {
            
            pm.jumpForce = jumpForce;
        }
        else
        {
            pm.jumpForce = tmpForce;
        }
    }
}
