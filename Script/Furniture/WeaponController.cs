using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //FIXME: ADD AUDIO
    //       

    [Header("Weapon")]
    public GameObject Blade; //Can add more
    public float BladeDMG = 10;

    [Header("Varables")]
    public bool CanAttack = true;
    public float AttackCooldown = 1.0f;
    public bool IsAttacking = false;

    [Header("References")]
    public StaminaControl sc;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && sc.stamina > 0)
        {
            if (CanAttack)
            {
                BladeAttack();
            }
        }
    }

    public void BladeAttack()
    {
        CanAttack = false;
        IsAttacking = true;

        //Stamina
        sc.stamina -= 5;
        sc.staminaRegen = false;
        //Anim
        Animator anim = Blade.GetComponent<Animator>();
        anim.SetTrigger("AttackNow");
        //Reset value
        StartCoroutine(ResetAttackCooldown());
        StartCoroutine(StopStamnaRegen());
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(1f);
        IsAttacking = false;
    }

    IEnumerator StopStamnaRegen()
    {
        yield return new WaitForSeconds(5f);
        sc.staminaRegen = true;
    }
}
