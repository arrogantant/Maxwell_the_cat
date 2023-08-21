using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    private bool isAttached; // 풍선이 플레이어에 붙었는지 여부
    public float offsetY = 2.0f; // 풍선이 플레이어 위로 얼마나 떨어져 붙을지의 높이
    private Vector3 initialPosition; // 풍선의 초기 위치 저장을 위한 변수

    private void Start()
    {
        initialPosition = transform.position; // 풍선의 초기 위치 저장
    }

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
                playerScript.AttachToBalloon(this); // 현재 풍선 스크립트 인스턴스를 인수로 전달
            }
        }
    }

    private void Update()
    {
        if (isAttached && player != null)
        {
            // 풍선을 플레이어와 같이 움직임
            transform.position = new Vector3(player.position.x, player.position.y + offsetY, player.position.z);
        }
    }

    public void DetachAndReset()
    {
        isAttached = false;
        transform.position = initialPosition; // 초기 위치로 풍선 재설정
        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.canDash = true; // 대쉬 가능
            }
        }
    }
}
