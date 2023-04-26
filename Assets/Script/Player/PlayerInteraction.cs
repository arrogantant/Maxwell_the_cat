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
    public float imageHorizontalOffset = 2f; // 이미지의 높이 오프셋
    public float imageHorizontalOffsetY = 2f; // 이미지의 가로 오프셋

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
            nearestInteractable.Interact();
            nearestInteractable.canInteract = false;
        }
        UpdateInteractImagePosition();
    }

    private void FindNearestInteractable()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactionLayer);
        float minDistance = float.MaxValue;

        nearestInteractable = null;
        foreach (Collider2D hitCollider in hitColliders)
        {
            Interactable interactable = hitCollider.GetComponent<Interactable>();
            if (interactable != null && interactable.canInteract)
            {
                float distance = Vector2.Distance(transform.position, interactable.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestInteractable = interactable;
                }
            }
        }
        interactImage.enabled = nearestInteractable != null;
    }
    
    private void UpdateInteractImagePosition()
    {
        if (nearestInteractable != null)
        {
            Vector3 imagePosition = transform.position; // 플레이어의 위치를 기준으로 함
            imagePosition.x += imageHorizontalOffset; // x 축으로 오프셋을 적용
            imagePosition.y += imageHorizontalOffsetY;
            interactImage.transform.position = Camera.main.WorldToScreenPoint(imagePosition);
        }
    }
}
