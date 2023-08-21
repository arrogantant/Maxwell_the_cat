using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_wall : MonoBehaviour
{
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider1;

    private bool playerTouched; // 플레이어가 이 오브젝트에 닿았는지 추적
    private float resetDelay = 1f; // 리셋 지연 시간 (초 단위)

    private void Start()
    {
        initialPosition = transform.position;
        collider1 = GetComponent<Collider2D>();
        playerTouched = false; // 초기에는 닿지 않았으므로 false로 설정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerTouched) // 플레이어가 닿았고, 아직 이 오브젝트에 닿지 않았다면
        {
            Player player = collision.GetComponent<Player>();

            if (!player.IsGrounded())
            {
                int currentMaxJumpCount = player.GetMaxJumpCount();
                player.SetMaxJumpCount(currentMaxJumpCount + 2);

                playerTouched = true; // 이제 플레이어가 이 오브젝트에 닿았으므로 true로 설정
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 오브젝트에서 벗어났을 때
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ResetPlayerTouched()); // 코루틴으로 지연 후 리셋
        }
    }

    private IEnumerator ResetPlayerTouched()
    {
        yield return new WaitForSeconds(resetDelay); // 지정한 시간 동안 대기
        playerTouched = false; // 다시 닿을 수 있게 false로 설정
    }
}
