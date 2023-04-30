using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SavedX"))
        {
            PlayerPrefs.SetFloat("SavedX", transform.position.x);
            PlayerPrefs.SetFloat("SavedY", transform.position.y);
            PlayerPrefs.SetFloat("SavedZ", transform.position.z);
            PlayerPrefs.Save();
        }
        else
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("SavedX"), PlayerPrefs.GetFloat("SavedY"), PlayerPrefs.GetFloat("SavedZ"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("SavedX", transform.position.x);
            PlayerPrefs.SetFloat("SavedY", transform.position.y);
            PlayerPrefs.SetFloat("SavedZ", transform.position.z);
            PlayerPrefs.Save();
        }
    }
}
