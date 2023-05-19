using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionRange = 3f;
    public bool canInteract { get; set; } = true;

    private HingeJoint2D hingeJoint2D;

    private void Awake()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();
    }

    public void Interact()
    {
        if (canInteract)
        {
            StartCoroutine(RotateAndReset());
        }
    }

    private IEnumerator RotateAndReset()
    {
        // 모터를 활성화합니다.
        hingeJoint2D.useMotor = true;

        // 0.5초 동안 기다립니다.
        yield return new WaitForSeconds(0.5f);

        // 모터를 비활성화합니다.
        hingeJoint2D.useMotor = false;
    }
}