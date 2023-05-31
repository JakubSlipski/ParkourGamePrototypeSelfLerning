using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MedkitController : MonoBehaviour
{
    //FIXME: SHOW COOLDOWAN FOR USE MEDKIT

    [Header("Medkit")]
    private float castTimer;
    public int count = 20; 
    public float heal = 15;

    [Header("References")]
    public Slider slider;
    public GameObject bar;
    public HealthControl hp;
    public PlayerAction pa;
    public Text textCast;

    [Header("References Inv")]
    public Image medkit;
    public Text counter;
    public ItemObject item;
    public InventoryObject inventory;

    private void Start()
    {
        SetIcon();
        SetCastTime();
        SetMaxCast(hp.healingTime);
        SetHealValue();

        count = pa.amountStart;
        castTimer = hp.healingTime;
        hp.healingTimer = hp.healingTime;
        SetMaxCast(hp.healingTime);
        if(count > 0) hp.medkitOwn = true;
    }

    private void Update()
    {
        if (hp.medkitEQ)
        {
            textCast.text = "Healing";
            SetIcon();
            SetCastTime();
            SetMaxCast(hp.healingTime);
            SetHealValue();
            CastProggresBar();
            SetCountMK();
        }
        if (count > 0) hp.medkitOwn = true;
        else hp.medkitOwn = false;
    }

    public void SetMaxCast(float cast)
    {
        slider.maxValue = cast;
        slider.value = 0;
    }

    public void SetCast(float cast)
    {
        slider.value = cast;
    }

    public void SetCountMK()
    {
        counter.text = count.ToString();
    }

    public void CountDown()
    {
        if (count > 0 && !hp.healingEnable && hp.medkitOwn) count -= 1;
    }

    public void SetCount()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
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
                medkit.sprite = inventory.Container[i].item.icon;
            }
        }
    }

    public void SetCastTime()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                hp.healingTime = inventory.Container[i].item.castTime;
            }
        }
    }

    public void SetHealValue()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                heal = inventory.Container[i].item.healing;
            }
        }
    }

    public void CastProggresBar()
    {
        if (Input.GetKey(hp.healingKey) && hp.healingEnable 
            && castTimer <= hp.healingTime && hp.healingTimer >= 0 && hp.medkitOwn)
        {
            castTimer += Time.deltaTime;
            bar.SetActive(true);
            SetCast(castTimer);
        }
        else
        {
            bar.SetActive(false);
        }

        if (castTimer > hp.healingTime)
        {
            castTimer = 0;
            bar.SetActive(false);
        }
    }

}
