using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{    
    [SerializeField] [Range(1f, 100f)] float rotateSpeed = 50f;
    [SerializeField] [Range(1f, 10f)] float transitionDuration = 2f;
    private SceneLoader sceneLoader;
    private bool inTransition = false;
    void Start() 
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }
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
        var rotatingPartsTransform = rotatingParts.GetComponent<Transform>();

        spriteRenderer.enabled = false;
        rotatingParts.gameObject.SetActive(true);
        rotatingPartsSpriteRenderer.enabled = true; 

        float startTime = Time.time;
        Vector3 startPosition = player.transform.position;
        Vector3 startScale = rotatingPartsTransform.localScale;

        while(Time.time - startTime < transitionDuration)
        {
            float t = (Time.time - startTime) / transitionDuration;

            // Increase rotation speed over time
            float currentRotateSpeed = Mathf.Lerp(rotateSpeed, rotateSpeed * 5f, t);
            rotatingParts.transform.Rotate(0, 0, currentRotateSpeed * Time.deltaTime);
            
            // Decrease size over time
            rotatingPartsTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            player.transform.position = Vector3.Lerp(startPosition, transform.position, t);
            yield return null;
        }

        player.transform.position = transform.position;
        rotatingPartsTransform.localScale = startScale;

        rotatingPartsSpriteRenderer.enabled = false;
        rotatingParts.gameObject.SetActive(false);
        spriteRenderer.enabled = true;

        sceneLoader.LoadScene("one1");
    }
}
