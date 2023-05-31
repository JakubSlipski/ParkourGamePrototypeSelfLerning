using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealethSystem : MonoBehaviour
{
    [Header("Health")]
    public float health;
    private float maxHealth;
    private float demage;
    private float healthCheck;
    private bool deathAfter = false;

    [Header("References")]
    public GameObject go;
    public WeaponController wc;
    public HealthControl hc;

    void Start()
    {
        maxHealth = health;
        demage = wc.BladeDMG;
    }

    private void Update()
    {
        dmgCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" && wc.IsAttacking)
        {
            if (health > 0)
            {
                TakeDemage(demage);
            }
            else if (health <= 0 || deathAfter)
            {
                Destroy(go);
                //TEST PLAYER -HP
                hc.health -= 15;
            }
        }
    }

    public void TakeDemage(float dmg)
    {
        health -= dmg;
    }

    public void dmgCheck()
    {
        healthCheck = health - (demage+0.1f);
        if (healthCheck <= 0)
            deathAfter = true;
        else
            deathAfter = false;
    }
}
