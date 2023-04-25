using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionRange = 3f;
    public Image interactImage; // 상호작용 이미지

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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactionLayer);
        float minDistance = float.MaxValue;

        nearestInteractable = null;
        foreach (Collider2D hitCollider in hitColliders)
        {
            Interactable interactable = hitCollider.GetComponent<Interactable>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, interactable.transform.position);
                Debug.Log("Distance to " + interactable.name + ": " + distance); // 로그 추가
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestInteractable = interactable;
                }
            }
        }

        if (nearestInteractable != null)
        {
            interactImage.enabled = true; // 이미지 활성화
        }
        else
        {
            interactImage.enabled = false; // 이미지 비활성화
        }
    }
}
