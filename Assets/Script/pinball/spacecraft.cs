using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacecraft : MonoBehaviour
{
    public float speed = 10f;
    private bool movingRight = true;
    public Transform raycastOrigin;
    public float detectionDistance = 1f;
    public LayerMask wallLayer;
    private GameObject player;
    private Rigidbody2D playerRigidbody;
    private float nextThrowTime;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float throwInterval = 1f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (player != null && playerRigidbody != null && Time.time >= nextThrowTime)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= throwRange)
            {
                nextThrowTime = Time.time + throwInterval;
                ShootLaser();
            }
        }

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (Physics2D.Raycast(raycastOrigin.position, Vector2.right, detectionDistance, wallLayer))
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (Physics2D.Raycast(raycastOrigin.position, Vector2.left, detectionDistance, wallLayer))
            {
                movingRight = true;
            }
        }
    }

    private void ShootLaser()
    {
        // 우주선 위치의 y축 -2에 레이저 생성
        Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
        GameObject laser = ObjectPool.Instance.GetLaser();
        laser.transform.position = laserPosition;
        laser.SetActive(true);
    }
}

