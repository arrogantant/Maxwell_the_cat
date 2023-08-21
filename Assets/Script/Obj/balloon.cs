using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    private bool isAttached; // 풍선이 플레이어에 붙었는지 여부
    public float offsetY = 2.0f; // 풍선이 플레이어 위로 얼마나 떨어져 붙을지의 높이

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttached)
        {
            player = collision.transform;
            isAttached = true;

            // 플레이어 스크립트에 접근
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                // 플레이어 스크립트의 함수를 호출하여 상태를 변경
                playerScript.AttachToBalloon();
            }
        }
    }

    private void Update()
    {
        if (isAttached && player != null)
        {
            // 풍선을 플레이어와 같이 움직임
            // 위치를 플레이어의 위치에 offsetY만큼 높게 설정
            transform.position = new Vector3(player.position.x, player.position.y + offsetY, player.position.z);
        }
    }
}
