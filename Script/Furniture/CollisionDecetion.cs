using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDecetion : MonoBehaviour
{
    //FIXME: ADD TAKE HIT(Target) ANIMATION
    //      

    [Header("References")]
    public WeaponController wc;
    //PARTICLE EFFECT public GameObject HitParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && wc.IsAttacking)
        {
            Debug.Log(other.name);
            //ADD ACTIVE ANIMATION


            //Particle effect
            //Instantiate(HitParticle, new Vector3(other.transform.position.x,
            //    transform.position.y, other.transform.position.z), other.transform.rotation);
        }
    }
}
