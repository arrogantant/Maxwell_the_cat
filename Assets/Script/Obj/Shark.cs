using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform을 참조
    public float speed = 5.0f; // 추적 속도
    public Vector2 minBounds; // 이동 가능한 최소 경계
    public Vector2 maxBounds; // 이동 가능한 최대 경계
    void Start()
    {
        // "Player" 태그가 있는 오브젝트를 찾아서 Transform 컴포넌트를 가져옵니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
    void Update()
    {
        if (player != null)
        {
            // 플레이어를 향해 이동
            Vector2 direction = player.position - transform.position;
            Vector2 newPosition = (Vector2)transform.position + (direction.normalized * speed * Time.deltaTime);

            // 경계 내에서만 이동
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

            transform.position = newPosition;

            // 객체 회전 처리
            RotateTowardsPlayer(direction);
        }
    }

    void RotateTowardsPlayer(Vector2 direction)
    {
        // 두 벡터 간의 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Z축을 기준으로 회전하여 플레이어를 바라보도록 설정
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
    void OnDrawGizmos()
    {
        // 경계를 그리기 위한 Gizmos 설정
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(maxBounds.x, minBounds.y, 0));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
        Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, 0), new Vector3(minBounds.x, maxBounds.y, 0));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0), new Vector3(minBounds.x, minBounds.y, 0));
    }
}
