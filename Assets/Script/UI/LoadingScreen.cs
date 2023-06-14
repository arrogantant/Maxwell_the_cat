using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenObject;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ShowLoadingScreen()
    {
        loadingScreenObject.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreenObject.SetActive(false);
    }
}