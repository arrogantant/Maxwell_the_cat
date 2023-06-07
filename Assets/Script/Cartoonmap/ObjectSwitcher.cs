using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;

    private PlayerInput playerInput;
    private bool hasJumped = false;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!hasJumped && playerInput.actions["Jump"].triggered)  // 플래그 변수를 체크
        {
            object1.SetActive(false);
            object2.SetActive(true);
            StartCoroutine(WaitAndSwitchObject());

            hasJumped = true;  // 플래그 변수를 true로 설정하여 코드가 다시 실행되지 않도록 함
        }
    }

    IEnumerator WaitAndSwitchObject()
    {
        yield return new WaitForSeconds(2);
        object2.SetActive(false);
        object3.SetActive(true);
    }
}
