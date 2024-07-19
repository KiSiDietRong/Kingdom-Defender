using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ctrl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2;
    private string currentAni;
    private Animator EnemyAni;

    [Header("Attack Attributes")]
    [SerializeField] private float atkRange = 1.0f;
    [SerializeField] private float atkSpeed = 1.0f;
    [SerializeField] private int dmg = 10;
    [SerializeField] private LayerMask playerLayer;

    private Transform target;
    private Transform player;
    private bool isAttacking = false;
    private int pathIndex = 0;
    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
        EnemyAni = GetComponent<Animator>();

    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
        EnemyAtk();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
            ChangeAnimation("walk");
        }
    }
    private void EnemyAtk()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, atkRange, playerLayer);

        if (hitPlayers.Length > 0)
        {
            player = hitPlayers[0].transform;
            rb.velocity = Vector2.zero;
            if (!isAttacking)
            {
                StartCoroutine(PlayAttackAnimation());
            }
        }
        else
        {
            player = null;
        }
    }

    private IEnumerator PlayAttackAnimation()
    {
        isAttacking = true;
        ChangeAnimation("atk"); 
        yield return new WaitForSeconds(atkSpeed);
        //if (player != null)
        //{
        //    player.GetComponent<PlayerHealth>().TakeDamage(dmg);
        //}
        isAttacking = false;
        ChangeAnimation("walk");
    }
    private void ChangeAnimation(string aniName)
    {
        if (currentAni != aniName)
        {
            EnemyAni.ResetTrigger(currentAni);
            currentAni = aniName;
            EnemyAni.SetTrigger(currentAni);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
