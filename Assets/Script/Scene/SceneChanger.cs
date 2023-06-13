using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{    
    [SerializeField] [Range(1f, 100f)] float rotateSpeed = 50f;
    [SerializeField] [Range(1f, 10f)] float transitionDuration = 2f;

    private bool inTransition = false;

    void Update() 
    {
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !inTransition)
        {
            inTransition = true;
            StartCoroutine(PlayerTransition(collision.gameObject));
        }
    }

    IEnumerator PlayerTransition(GameObject player)
    {
        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        var rotatingParts = player.transform.Find("RotatingParts");
        var rotatingPartsSpriteRenderer = rotatingParts.GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
        rotatingParts.gameObject.SetActive(true);
        rotatingPartsSpriteRenderer.enabled = true; 

        float startTime = Time.time;
        Vector3 startPosition = player.transform.position;

        while(Time.time - startTime < transitionDuration)
        {
            rotatingParts.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            player.transform.position = Vector3.Lerp(startPosition, transform.position, (Time.time - startTime) / transitionDuration);
            yield return null;
        }

        player.transform.position = transform.position;

        rotatingPartsSpriteRenderer.enabled = false;
        rotatingParts.gameObject.SetActive(false);
        spriteRenderer.enabled = true;

        SceneManager.LoadScene("one1");
    }
}
