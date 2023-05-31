using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    [Header("Health")]
    public float health;
    public bool healingEnable;
    public float maxHealth;
    public bool medkitOwn;
    public bool medkitEQ = true;

    [Header("Input")]
    public KeyCode healingKey = KeyCode.H;
    public float healingTime;
    public float healingCD;
    private float healingCDTimer;
    public float healingTimer;


    [Header("References")]
    public Slider slider;
    public GameObject bar; // if i need show/hide bar
    public PlayerMovement pm;
    public MedkitController mc;
    public DevMODE dm;

    private void Start()
    {
        medkitEQ = true;
        //Setup health
        maxHealth = health;
        SetMaxHealth(maxHealth);
        //Setup healing timer
        healingEnable = true;
        healingCDTimer = healingCD;
        healingTimer = healingTime;
    }

    private void Update()
    {
        //Update Health
        SetHealth(health);
        if (medkitEQ)
        {//Healing
            Healing();

            // Healing Cooldown
            if (!healingEnable)
                ResetHealingCD();
        }
        //TEST DMG
        if (Input.GetKeyDown(KeyCode.P))
            TakeDemage(25);

        //Health check & reset
        if (health <= 0)
            health = 1;

    }

    public void SetMaxHealth(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void SetHealth(float hp)
    {
        slider.value = hp;
    }

    public void TakeHeal(float heal)
    {
        health += heal;
    }

    public void TakeDemage(float dmg)
    {
        health -= dmg;
    }

    public void Healing()
    {
        if (Input.GetKey(healingKey) && healingEnable && medkitOwn)
        {
            healingTimer -= Time.deltaTime;

            if (healingTimer <= 0)
                healingEnable = false;
        }
        else if (Input.GetKey(healingKey) && !healingEnable && healingTimer <= 0 && medkitOwn)
        {
            TakeHeal(mc.heal);
            ResetHealingTimer();
            mc.CountDown();
            mc.SetAmount();
        }
        else if (health > maxHealth && !dm.infHealth)
        {
            health = maxHealth;
        }
        else
            ResetHealingTimer();
    }

    public void ResetHealingTimer()
    {
        if (healingTimer <= 0)
            healingTimer = healingTime;
        else if (healingEnable)
            healingTimer = healingTime;
    }

    public void ResetHealingCD()
    {
        if (healingCDTimer > 0)
            healingCDTimer -= Time.deltaTime;
        else
        {
            healingCDTimer = healingCD;
            healingEnable = true;
        }
    }

}
