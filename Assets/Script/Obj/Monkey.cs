using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    [SerializeField] private GameObject bananaPrefab;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwInterval = 1f;
    [SerializeField] private float bananaLifeTime = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D monkeyCollider;

    private GameObject player;
    private float nextThrowTime;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            nextThrowTime = Time.time;
        }
    }

    private void Update()
    {
        if (player != null && playerRigidbody != null && Time.time >= nextThrowTime)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= throwRange)
            {
                ThrowBanana();
                nextThrowTime = Time.time + throwInterval;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null && playerScript.isDashing)
            {
                monkeyCollider.isTrigger = true;
                StartCoroutine(PlayDeathAnimationAndDestroy());
            }
        }
    }

    private void ThrowBanana()
    {
        GameObject banana = Instantiate(bananaPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = banana.GetComponent<Rigidbody2D>();
        Vector2 predictedPlayerPosition = (Vector2)player.transform.position + playerRigidbody.velocity * (Vector2.Distance(transform.position, player.transform.position) / throwForce);
        Vector2 direction = (predictedPlayerPosition - (Vector2)transform.position).normalized;

        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);

        StartCoroutine(DestroyBananaAfterTime(banana, bananaLifeTime));
    }

    private IEnumerator DestroyBananaAfterTime(GameObject banana, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(banana);
    }

    private IEnumerator PlayDeathAnimationAndDestroy()
    {
        animator.SetTrigger("monDie");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}

