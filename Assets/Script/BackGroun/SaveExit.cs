using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveExit : MonoBehaviour
{
    public GameObject player;  // 플레이어 오브젝트를 위한 공개 변수. Inspector에서 수동으로 설정해야 합니다.

    void Start()
    {
        // 게임 시작 시에 player 게임 오브젝트를 찾습니다.
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void SaveGame()
    {
        if (player != null) // 플레이어가 존재하는지 확인
        {
            // 플레이어 오브젝트의 위치를 가져옵니다.
            Vector3 playerPosition = player.transform.position;

            Debug.Log("Saving game...");
            Debug.Log($"Scene: {SceneManager.GetActiveScene().name}, Position: {playerPosition}");

            // 현재 씬 이름을 저장
            PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);

            // 플레이어의 위치를 문자열로 변환하여 저장
            PlayerPrefs.SetString("PlayerPosition", $"{playerPosition.x},{playerPosition.y},{playerPosition.z}");

            // 모든 변경사항을 저장
            PlayerPrefs.Save();

            Debug.Log("Game saved.");
        }
        else
        {
            Debug.LogError("Player object is null");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        SaveGame();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        Debug.Log("Game quit.");
    }
}
