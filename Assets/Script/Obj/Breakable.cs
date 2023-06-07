using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D solidCollider;
    public BoxCollider2D triggerCollider;
    public GameObject DashObj;
    public GameObject GoodObj;

    public void Break()
    {
        solidCollider.enabled = false; // 콜라이더 비활성화
        triggerCollider.enabled = true; // 트리거 콜라이더 활성화
        animator.SetTrigger("Break"); // 파편 애니메이션 실행
        DashObj.SetActive(false);
        GoodObj.SetActive(true);
        StartCoroutine(DestroyAfterDelay(3f)); // 3초 후에 오브젝트 파괴
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(2);
        GoodObj.SetActive(false);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
