using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Inventory")]
    public InventoryObject inventory;
    public ItemObject itemMK;
    public int amountStart;
    public KeyCode invKey = KeyCode.I;
    public KeyCode swapMedic = KeyCode.Tab;

    [Header("Usable props")]
    public float MaxUseDistance = 5f;
    public KeyCode useKey = KeyCode.F;
    public KeyCode menuKey = KeyCode.Escape;


    [Header("Flashlight")]
    public KeyCode flashLightKey = KeyCode.T;
    public bool useFlashLight = false;

    [Header("References")]
    public PlayerMovement pm;
    public MedkitController mc;
    public HealthControl hp;
    public StaminaControl sc;
    public TextMeshPro UseText;
    public Transform Camera;
    public GameObject fl;
    public GameObject flOutline;
    public LayerMask UseLayers;
    public TimeControll tc;


    private void Start()
    {
        //Clear inventory
        inventory.Container.Clear();
        //Start pack medkits
        inventory.AddItem(itemMK, amountStart);
        //Setup off flashlight
        fl.SetActive(false);
        flOutline.SetActive(false);
    }

    private void Update()
    {
        DoorToolTip();
        PickUpToolTip();
        FlashLight();
        SwapMedic();
    }

    public void OnUseDoor()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.isOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(transform.position);
                }
            }
        }
    }

    private void DoorToolTip()
    {
        if (Input.GetKey(useKey)) OnUseDoor();

        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers)
            && hit.collider.TryGetComponent<Door>(out Door door))
        {
            if (door.isOpen)
            {
                UseText.SetText("Close \"" + useKey + "\"");
            }
            else
            {
                UseText.SetText("Open \"" + useKey + "\"");
            }
            UseText.gameObject.SetActive(true);
            UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.1f;
            UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);
        }
        else
        {
            UseText.gameObject.SetActive(false);
        }
    }

    public void PickUp()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.tag == "item")
            {
                var item = hit.collider.GetComponent<Item>();
                if (item)
                {
                    inventory.AddItem(item.item, 1);
                    Destroy(hit.collider.gameObject);
                    mc.SetCount();
                }
            }
        }
    }

    private void PickUpToolTip()
    {
        if (Input.GetKey(useKey)) PickUp();

        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            //Check type
            if (hit.collider.tag == "item")
                UseText.SetText("Pickup \""+useKey+"\"");

            //Show tool tip
            UseText.gameObject.SetActive(true);
            //Position tool tip
            UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.1f;
            UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);
        }
        else
        {
            UseText.gameObject.SetActive(false);
        }
    }

    public void FlashLight()
    {
        if (Input.GetKeyDown(flashLightKey))
        {
            //Turn off
            if (useFlashLight)
            {
                fl.SetActive(false);
                flOutline.SetActive(false);
                useFlashLight = false;
            }
            //Turn on 
            else if (!useFlashLight)
            {
                StartCoroutine(FlashlightStart());
                useFlashLight = true;
            }
        }
    }

    IEnumerator FlashlightStart()
    {
        yield return new WaitForSeconds(0.2f);
        fl.SetActive(true);
        flOutline.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        flOutline.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        flOutline.SetActive(true);
    }

    void SwapMedic()
    {
        if (Input.GetKeyDown(swapMedic))
        {
            if (hp.medkitEQ)
            {
                hp.medkitEQ = false;
                sc.staminaBoostEQ = true;
            }
            else if (sc.staminaBoostEQ)
            {
                sc.staminaBoostEQ = false;
                hp.medkitEQ = true;
            }
        }
    }

}
