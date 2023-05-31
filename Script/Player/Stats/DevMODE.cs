using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevMODE : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement pm;
    public StaminaControl sc;
    public HealthControl hc;
    public GameObject stam;
    public GameObject hp;

    [Header("Stats")]
    public bool infStamina = false;
    public bool infHealth = false;
    public float infStaminaValue;
    public float infHealthValue;

    [Header("Inputs")]
    public KeyCode resetGrounded = KeyCode.F1;
    public KeyCode stamina = KeyCode.F2;
    public KeyCode health = KeyCode.F3;

    private void Start()
    {
        stam.SetActive(false);
        hp.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(resetGrounded)) //Reset buged state
        {
            pm.unlimited = false;
        }
        else if (Input.GetKeyDown(stamina)) //Stamina
        {
            if (infStamina)
            {
                sc.stamina = sc.maxStamina;
                sc.SetMaxStamina(sc.maxStamina);
                stam.SetActive(false);
                infStamina = false;
            }
            else
            {
                sc.stamina = infStaminaValue;
                sc.SetMaxStamina(infStaminaValue);
                stam.SetActive(true);
                infStamina = true;
            }
        }
        else if(Input.GetKeyDown(health)) //Health
        {
            if (infHealth)
            {
                hc.health = hc.maxHealth;
                hc.SetMaxHealth(hc.maxHealth);
                hp.SetActive(false);
                infHealth = false;
            }
            else
            {
                hc.health = infHealthValue;
                hc.SetMaxHealth(infHealthValue);
                hp.SetActive(true);
                infHealth = true;
            }
        }
    }
}
