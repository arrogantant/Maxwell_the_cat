using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    public float resetX = 10f; // 이 좌표에 도달하면 오브젝트를 스폰 위치로 리셋
    public Vector3 spawnPosition; // 오브젝트의 스폰 위치

    private void Update()
    {
        // 오브젝트를 오른쪽으로 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 오브젝트가 리셋 지점에 도달하면 스폰 위치로 돌아감
        if (transform.position.x >= resetX)
        {
            transform.position = spawnPosition;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
