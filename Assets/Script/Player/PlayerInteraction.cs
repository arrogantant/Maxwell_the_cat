using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionRange = 3f;

    private PlayerInput playerInput;
    private Interactable nearestInteractable;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        FindNearestInteractable();

        if (playerInput.actions["Interact"].triggered && nearestInteractable != null)
        {
            Debug.Log("Starting interaction with " + nearestInteractable.gameObject.name);
            nearestInteractable.Interact();
        }
    }

    private void FindNearestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, interactionLayer);
        float minDistance = float.MaxValue;

        nearestInteractable = null;
        foreach (Collider hitCollider in hitColliders)
        {
            Interactable interactable = hitCollider.GetComponent<Interactable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, interactable.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestInteractable = interactable;
                }
            }
        }
    }
}


