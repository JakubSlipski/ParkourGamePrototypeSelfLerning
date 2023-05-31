using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    [Header("Vaulting")]
    public float playerHeight = 2f;
    public float playerRadius = 0.5f;
    public bool canAnim = true;

    [Header("References")]
    public PlayerMovement pm;
    private int vaultLayer;
    public CameraMovment cam;
    public Animator anim;

    private void Start()
    {
        vaultLayer = LayerMask.NameToLayer("whatIsVault");
        vaultLayer = ~vaultLayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "vault" && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)) 
        {
            if (Physics.Raycast(cam.footOrientation.transform.position, cam.footOrientation.transform.forward,
                out var firstHit, 1f, vaultLayer))
            {
                if (Physics.Raycast(firstHit.point + (cam.footOrientation.transform.forward * playerRadius)
                    + (Vector3.up * 0.6f * (2 * playerHeight)), Vector3.down, out var secondHit, (2 * playerHeight)))
                {
                    cam.DoFov(90f);
                    pm.unlimited = false;
                    if (canAnim)
                    {
                        anim.SetTrigger("vaultStart");
                        canAnim = false;
                    }
                    StartCoroutine(LerpVault(secondHit.point, 0.0f));
                    cam.DoFov(80f);
                }
                pm.unlimited = false;
                StartCoroutine(CanAnim());
            }
        }  
    }

    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while(time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    IEnumerator CanAnim()
    {
        yield return new WaitForSeconds(1.2f);
        canAnim = true;
    }


}
