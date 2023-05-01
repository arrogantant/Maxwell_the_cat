using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundGradient : MonoBehaviour
{
    public GameObject player;
    public Color[] colors;
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
        int colorIndex = Mathf.FloorToInt(t * (colors.Length - 1));
        Color startColor = colors[colorIndex];
        Color endColor = colors[Mathf.Min(colorIndex + 1, colors.Length - 1)];
        float colorT = (t - colorIndex * (1.0f / (colors.Length - 1))) * (colors.Length - 1);
        mainCamera.backgroundColor = Color.Lerp(startColor, endColor, colorT);
    }
}
