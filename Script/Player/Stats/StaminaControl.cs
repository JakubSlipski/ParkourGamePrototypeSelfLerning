using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StaminaControl : MonoBehaviour
{
    [Header("Stamina")]
    public float stamina;
    public float minStamina;
    public float maxStamina;
    private float tmpStamina;
    public Gradient gradient;
    public bool staminaRegen;

    public float staminaTime;
    public float castTimer;
    public float staminaCD;
    private float staminaCDTimer;
    public float staminaTimer;
    public bool staminBoostOwn = false;
    public bool staminaBoostEnable = true;
    public bool staminaBoostActive = false;
    public bool staminaBoostEQ = false;

    [Header("References")]
    public HealthControl hp;
    public DevMODE dm;
    public Slider slider;
    public Slider sliderCast;
    public Image fill;
    public GameObject bar;
    public GameObject barCast;
    public Text textCast;

    [Header("References Inv")]
    public int count = 0;
    public InventoryObject inventory;
    public ItemObject item;
    public Image medicImg;
    public Text counter;

    private void Start()
    {
        SetCastTime();
        tmpStamina = stamina;
        maxStamina = stamina;
        SetMaxStamina(maxStamina);
        fill.color = gradient.Evaluate(1f);
    }

    public void Update()
    {
        if (staminaBoostEQ)
        {
            textCast.text = "Stamina Boost";
            SetIcon();
            SetCastTime();
            SetMaxCast(staminaTime);
            CastProggresBar();
            SetCountMK();
            SetCount();
            SetAmount();

            //StaminaBoost
            StaminaBoost();

            // Stamina Cooldown
            if (!staminaBoostEnable)
                ResetStaminaCD();
        }
        SetMaxMaxStamina(maxStamina);
        SetStamina(stamina);
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (count > 0) staminBoostOwn = true;
        else staminBoostOwn = false;

        ShowBar();
    }

    public void SetMaxStamina(float stam)
    {
        slider.maxValue = stam;
        slider.value = stam;
    }

    public void SetMaxMaxStamina(float stam)
    {
        slider.maxValue = stam;
    }

    public void SetStamina(float stam)
    {
        slider.value = stam;
    }

    public void ShowBar()
    {
        if (stamina >= maxStamina)
            bar.SetActive(false);
        else
            bar.SetActive(true);
    }

    public void SetMaxCast(float cast)
    {
        sliderCast.maxValue = cast;
        sliderCast.value = 0;
    }

    public void SetCast(float cast)
    {
        sliderCast.value = cast;
    }

    public void SetCountMK()
    {
        counter.text = count.ToString();
    }

    public void SetCount()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                count = inventory.Container[i].amount;
            }
        }
    }

    public void SetAmount()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                inventory.Container[i].amount = count;
            }
        }
    }

    public void SetIcon()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                medicImg.sprite = inventory.Container[i].item.icon;
            }
        }
    }

    public void CountDown()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                inventory.Container[i].amount -= 1;
                count -= 1;
            }
        }
    }

    public void StaminaBoost()
    {
        if (Input.GetKey(hp.healingKey) && staminaBoostEnable && staminBoostOwn && staminaBoostEQ)
        {
            staminaTimer -= Time.deltaTime;

            if (staminaTimer <= 0)
                staminaBoostEnable = false;
        }
        else if (Input.GetKey(hp.healingKey) && !staminaBoostEnable && staminaTimer <= 0 && staminBoostOwn)
        {
            staminaBoostActive = true;
            ResetStaminaTimer();
            CountDown();

            if (staminaBoostActive) ActiveBoost();
        }
        else if (stamina > maxStamina && !dm.infStamina)
        {
            stamina = maxStamina;
        }
        else
            ResetStaminaTimer();
    }

    public void SetCastTime()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                staminaTime = inventory.Container[i].item.castTime;
            }
        }
    }

    public void CastProggresBar()
    {
        if (Input.GetKey(hp.healingKey) && staminaBoostEnable
            && castTimer <= staminaTime && staminaTimer >= 0 && staminBoostOwn)
        {
            castTimer += Time.deltaTime;
            barCast.SetActive(true);
            SetCast(castTimer);
        }
        else
        {
            barCast.SetActive(false);
        }

        if (castTimer > staminaTime)
        {
            castTimer = 0;
            barCast.SetActive(false);
        }
    }

    public void ResetStaminaTimer()
    {
        if (staminaTimer <= 0)
            staminaTimer = staminaTime;
        else if (staminaBoostEnable)
            staminaTimer = staminaTime;
    }

    public void ResetStaminaCD()
    {
        if (staminaCDTimer > 0)
            staminaCDTimer -= Time.deltaTime;
        else
        {
            staminaCDTimer = staminaCD;
            staminaBoostEnable = true;
        }
    }


    int second;
    public void ActiveBoost()
    {
        
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                if(maxStamina < 2*tmpStamina)
                    maxStamina += inventory.Container[i].item.stamina;

                second = inventory.Container[i].item.secondForActiveEff;
            }
        }
        stamina = maxStamina;
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        yield return new WaitForSeconds(second);
        staminaBoostActive = false;
        maxStamina = tmpStamina;
        if (stamina > maxStamina) stamina = maxStamina;
    }
}
