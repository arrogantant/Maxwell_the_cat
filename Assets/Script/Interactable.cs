using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float rotateZ = 45f;
    public float rotationSpeed = 2f;
    public float bounceForce = 10f;
    public float interactionRange = 3f;
    public bool canInteract { get; set; } = true;
    public GameObject rotatingObject; // 이것이 Sprite Renderer를 가진 GameObject입니다.

    // ...

    public void Interact()
    {
        if (canInteract)
        {
            StartCoroutine(RotateAndBounce());
        }
    }

    private IEnumerator RotateAndBounce()
    {
        canInteract = false;

        // 회전하기
        Quaternion startRotation = rotatingObject.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, rotateZ);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            rotatingObject.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        // 플레이어를 튕겨냅니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerRigidbody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        // 원래대로 돌아가기
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            rotatingObject.transform.localRotation = Quaternion.Lerp(endRotation, startRotation, t);
            yield return null;
        }

        // 상호작용 다시 허용
        canInteract = true;
    }
}