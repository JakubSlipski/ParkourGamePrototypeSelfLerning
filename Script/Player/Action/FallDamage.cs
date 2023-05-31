using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    [Header("FallDamage")]
    public float fallDamageThreshold = 15f; // The minimum fall height to trigger fall damage
    public float fallDamageMultiplier = 3f; // The amount of damage to apply based on the fall height
    private float fallStartHeight;

    [Header("References")]
    public Rigidbody rb;
    public HealthControl hc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is falling
        if (rb.velocity.y < 0)
        {
            // Check if the player has not yet started falling
            if (fallStartHeight == 0)
            {
                // Record the starting height for the fall
                fallStartHeight = transform.position.y;
            }
        }
        else
        {
            // Player is not falling, check if they have fallen far enough to trigger fall damage
            if (fallStartHeight - transform.position.y > fallDamageThreshold)
            {
                // Calculate fall damage
                float fallHeight = fallStartHeight - transform.position.y;
                float fallDamage = fallHeight * fallDamageMultiplier;

                // Apply fall damage
                hc.TakeDemage(fallDamage);
            }

            // Reset fall start height
            fallStartHeight = 0;
        }
    }
}
