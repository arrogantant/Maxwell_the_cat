using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleObjects : MonoBehaviour
{
    public GameObject objectToActivate;  // 활성화할 오브젝트
    public GameObject objectToDeactivate;  // 비활성화할 오브젝트

    private bool hasBeenPressed = false; // 키가 한 번 눌렸는지를 확인하는 플래그

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        // 키가 눌리지 않았고, space 키가 눌렸다면
        if (!hasBeenPressed && keyboard.spaceKey.wasPressedThisFrame)
        {
            objectToActivate.SetActive(true);  // 오브젝트를 활성화
            objectToDeactivate.SetActive(false);  // 오브젝트를 비활성화
            hasBeenPressed = true;  // 키가 눌렸음을 표시
        }
    }
}
