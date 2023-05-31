using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStep : MonoBehaviour
{
    [Header("AutoStep")]
    public float stepHeight; // The height of the step
    public float stepInterval; // The time interval between steps

    private float nextStepTime; // The time when the next step can be taken
    private bool stepping; // True if the player is currently stepping

    void Start()
    {
        nextStepTime = Time.time;
    }

    void Update()
    {
        // Check if the player is currently stepping
        if (stepping)
        {
            // Check if it is time to take the next step
            if (Time.time > nextStepTime)
            {
                // Reset the stepping flag
                stepping = false;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the player is colliding with an object while moving
        if (hit.moveDirection.y > 0 && !stepping)
        {
            // Check if the player is moving forward or backward
            if (Input.GetAxis("Vertical") != 0)
            {
                // Move the player up by the step height
                transform.position += Vector3.up * stepHeight;

                // Set the stepping flag and update the next step time
                stepping = true;
                nextStepTime = Time.time + stepInterval;
            }
        }
    }
}
