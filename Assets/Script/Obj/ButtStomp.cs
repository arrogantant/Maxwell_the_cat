using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtStomp : MonoBehaviour
{       
    public GameObject object1;
    public GameObject object2;
    public BoxCollider2D solidCollider;
    public BoxCollider2D triggerCollider;

    public void Break()
    {
        solidCollider.enabled = false; // 콜라이더 비활성화
        triggerCollider.enabled = true; // 트리거 콜라이더 활성화
        object1.SetActive(false);
        object2.SetActive(true);
        Destroy(gameObject);
    }
}
