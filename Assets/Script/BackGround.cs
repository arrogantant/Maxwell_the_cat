using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{   
    public float speed;
    public Transform[] backgrounds;
    public float overlap = 0.01f;

    [SerializeField] float leftPosX = 0f;
    [SerializeField] float rightPosX = 0f;
    [SerializeField] float xScreenHalfSize;
    [SerializeField] float yScreenHalfSize;

    void Start()
    {
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        leftPosX = -(xScreenHalfSize * 2);
        rightPosX = xScreenHalfSize * 2 * (backgrounds.Length - 1);
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            if (backgrounds[i].position.x < leftPosX)
            {
                float backgroundWidth = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x;
                Vector3 nextPos = backgrounds[(i + backgrounds.Length - 1) % backgrounds.Length].position;
                nextPos.x += backgroundWidth - overlap;
                backgrounds[i].position = nextPos;
            }
        }
    }
}
