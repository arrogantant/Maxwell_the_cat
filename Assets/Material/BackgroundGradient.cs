using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundGradient : MonoBehaviour
{
    public GameObject player;
    public Color startColor;
    public Color endColor;
    public float minHeight;
    public float maxHeight;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        float t = Mathf.InverseLerp(minHeight, maxHeight, player.transform.position.y);
        mainCamera.backgroundColor = Color.Lerp(startColor, endColor, t);
    }
}
