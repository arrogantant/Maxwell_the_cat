using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerStart : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerPosition;

    public void StartGame()
    {
        if (PlayerPrefs.HasKey("LastScene") && PlayerPrefs.HasKey("PlayerPosition"))
        {
            string lastScene = PlayerPrefs.GetString("LastScene");
            playerPosition = StringToVector3(PlayerPrefs.GetString("PlayerPosition"));

            SceneManager.sceneLoaded += OnSceneLoaded;

            // 지정된 씬을 로드합니다.
            SceneLoader.Instance.LoadScene(lastScene);
        }
        else
        {
            // 저장된 게임이 없는 경우, 기본 씬을 로드합니다.
            SceneLoader.Instance.LoadScene("Cartoon"); 
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 태그를 사용하여 씬에서 플레이어를 찾습니다.
        if (player != null) // 플레이어가 존재하면
        {
            player.transform.position = playerPosition;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private Vector3 StringToVector3(string sVector)
    {
        string[] sArray = sVector.Split(',');
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2])
        );

        return result;
    }
}