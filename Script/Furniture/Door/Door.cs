using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door")]
    public bool isOpen = false;
    public bool IsRotatingDoor = true;
    private float Speed = 1f;


    [Header("Roatation Configs")]
    public float RotationAmount = 90f;
    public float ForwardDirection = 0;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        // SInce "Forward" actually is pointing into the door frame, choose a direction
        Forward = transform.right;
    }

    public void Open(Vector3 userPosition)
    {
        if(!isOpen)
        {
            if(AnimationCoroutine!= null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                float forwardAmount = Vector3.Dot(Forward, (userPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(forwardAmount));
                Invoke(nameof(Close), 10f);
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(forwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }

        isOpen = true;

        float time = 0;

        while(time<1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void Close()
    {
        if(isOpen)
        {
            if(AnimationCoroutine!= null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            
            if(IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        isOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }
}
