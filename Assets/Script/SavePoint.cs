using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
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
