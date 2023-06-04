using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class oneScenesStop : MonoBehaviour
{
    
    public GameObject pauseMenuUI; // 일시정지 메뉴 UI
    public GameObject player; // Player 오브젝트

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트에 메서드를 추가
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 오브젝트가 파괴될 때 메서드를 제거
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0) // 첫번째 씬이 아닐 때만
        {
            pauseMenuUI.SetActive(true); // 일시정지 메뉴 UI를 활성화
            player.SetActive(true); // Player 오브젝트를 활성화
        }
        else
        {
            pauseMenuUI.SetActive(false); // 첫번째 씬일 때는 비활성화
            player.SetActive(false); // Player 오브젝트를 비활성화
        }
    }
}
