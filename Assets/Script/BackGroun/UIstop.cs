using UnityEngine;
using UnityEngine.InputSystem;

public class UIstop : MonoBehaviour
{
    public GameObject objectToDeactivate; // 비활성화할 게임 오브젝트

    void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            objectToDeactivate.SetActive(false); // 게임 오브젝트를 비활성화
        }
    }
}
