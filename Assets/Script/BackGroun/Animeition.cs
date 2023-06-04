using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animeition : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetTrigger("run"); // 'Run' 애니메이션 상태를 실행
    }
}