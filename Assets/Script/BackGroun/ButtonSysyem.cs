using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonSysyem : MonoBehaviour
{
    public Button[] buttons;
    public RectTransform arrow;
    public float arrowDistance;

    private int selectedIndex;

    private void Start()
    {
        if (buttons.Length > 0)
        {
            UpdateArrowPosition();
        }
    }

    private void Update()
    {
        var keyboard = Keyboard.current; // 현재 키보드를 가져옵니다.

        if (keyboard == null)
            return; // 키보드가 연결되어 있지 않으면 반환

        if (keyboard.upArrowKey.wasPressedThisFrame) // 키보드의 상단 화살표 키가 눌렸는지 확인
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = buttons.Length - 1;
            }
            UpdateArrowPosition();
        }
        else if (keyboard.downArrowKey.wasPressedThisFrame) // 키보드의 하단 화살표 키가 눌렸는지 확인
        {
            selectedIndex++;
            if (selectedIndex >= buttons.Length)
            {
                selectedIndex = 0;
            }
            UpdateArrowPosition();
        }
        else if (keyboard.spaceKey.wasPressedThisFrame || keyboard.zKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame) 
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    private void UpdateArrowPosition()
    {
        Vector2 buttonPosition = buttons[selectedIndex].GetComponent<RectTransform>().anchoredPosition;
        arrow.anchoredPosition = new Vector2(buttonPosition.x - arrowDistance, buttonPosition.y);
    }
}