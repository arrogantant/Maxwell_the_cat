using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleObjects : MonoBehaviour
{
    public GameObject objectToActivate;  // 활성화할 오브젝트
    public GameObject objectToDeactivate;  // 비활성화할 오브젝트

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            objectToActivate.SetActive(true);  // 오브젝트를 활성화
            objectToDeactivate.SetActive(false);  // 오브젝트를 비활성화
        }
    }
}
