using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PinballInteractable : MonoBehaviour
{
    public float rotateZ = 45f; // 핀볼 오브젝트가 회전할 각도입니다.
    public float bounceForce = 10f; // 플레이어를 튕겨내는 힘의 크기입니다.
    public float interactionRange = 3f; // 상호작용 가능한 거리입니다.
    public bool canInteract { get; set; } = true; // 상호작용 가능 여부

    private PlayerInput playerInput;
    private Quaternion originalRotation;

    private void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (playerInput.actions["Interact"].triggered && IsPlayerInRange())
        {
            Interact();
        }
    }

    private bool IsPlayerInRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return Vector3.Distance(player.transform.position, transform.position) < interactionRange;
    }

    public void Interact()
    {
        if (canInteract)
        {
            // 핀볼 오브젝트를 회전시킵니다.
            transform.Rotate(0, 0, rotateZ);

            // 플레이어를 위로 튕겨냅니다.
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
            playerRigidbody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

            // 1초 후에 핀볼 오브젝트의 회전을 원래대로 돌립니다.
            StartCoroutine(ResetRotation());

            // 다음 상호작용을 위해 canInteract를 false로 설정합니다.
            canInteract = false;
        }
    }

    private IEnumerator ResetRotation()
    {
        yield return new WaitForSeconds(1f);
        transform.rotation = originalRotation;

        // 회전이 초기화된 후에 다시 상호작용이 가능하도록 canInteract를 true로 설정합니다.
        canInteract = true;
    }
}
