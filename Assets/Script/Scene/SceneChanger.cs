using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] [Range(1f,100f)] float rotateSpeed =  50f;
    void Update() 
    {
        transform.Rotate(0,0,Time.deltaTime * rotateSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("one1");
        }
    }
}
