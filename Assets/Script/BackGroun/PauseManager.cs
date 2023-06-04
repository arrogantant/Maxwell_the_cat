using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // 일시정지 메뉴 UI

    private bool isPaused = false; // 게임 일시정지 상태인지 표시하는 변수
    private bool zKeyPressed = false; // Z키가 눌렸는지 표시하는 변수
    private int xKeyCount = 0; // X키를 누른 횟수를 저장하는 변수

    private void Update()
    {
        // ESC 키를 눌렀는지 확인
        if (Keyboard.current[Key.Escape].wasPressedThisFrame) 
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // Z 키를 눌렀는지 확인
        if (Keyboard.current[Key.Z].wasPressedThisFrame && isPaused)
        {
            zKeyPressed = true;
        }

        // X 키를 눌렀는지 확인
        if (Keyboard.current[Key.X].wasPressedThisFrame && isPaused)
        {
            if(zKeyPressed && xKeyCount < 1)
            {
                xKeyCount++;
            }
            else if (!zKeyPressed || (zKeyPressed && xKeyCount >= 1))
            {
                Resume();
                xKeyCount = 0;
                zKeyPressed = false;
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // 일시정지 메뉴 UI를 비활성화
        Time.timeScale = 1f; // 게임 속도를 원래대로 복원
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // 일시정지 메뉴 UI를 활성화
        Time.timeScale = 0f; // 게임 속도를 0으로 설정하여 게임을 일시 중지
        isPaused = true;
    }
    public bool IsGamePaused()
    {
        return isPaused;
    }
}
