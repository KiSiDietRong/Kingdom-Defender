using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierScript : MonoBehaviour
{
    [SerializeField] private LayerMask enemies;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float moveSpeed = 2f;
    public int minDamage = 5;
    public int maxDamage = 10;
    public float attackCooldown = 1.5f;
    public Transform barrackTower;
    public float maxDistanceFromBarrack = 10f;
    public int maxHealth = 20;
    private int currentHealth;
    private GameObject targetEnemy;
    private Vector2 spawnPoint;
    private bool canAttack = true;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetSpawnPoint(Vector2 position)
    {
        spawnPoint = position;
    }

    private void Update()
    {
        if (isDead) return;

        if (targetEnemy == null || Vector2.Distance(transform.position, targetEnemy.transform.position) > detectionRange)
        {
            FindTarget();
        }

        if (targetEnemy != null)
        {
            MoveTowardsEnemy();

            if (Vector2.Distance(transform.position, targetEnemy.transform.position) <= attackRange && canAttack)
            {
                StartCoroutine(AttackEnemy());
            }
        }
        else
        {
            ReturnToSpawnPoint();
        }
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        float closestDistance = detectionRange;
        targetEnemy = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemies"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = hit.gameObject;
                }
            }
        }
    }

    private void MoveTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator AttackEnemy()
    {
        canAttack = false;
        if (targetEnemy != null)
        {
            int damage = Random.Range(minDamage, maxDamage);
            var enemyCtrl = targetEnemy.GetComponent<Enemy_Ctrl>();
            if (enemyCtrl != null)
            {
                enemyCtrl.TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        // Xử lý logic chết ở đây (ví dụ: phát âm thanh, animation, v.v.)
        Destroy(gameObject);
    }

    private void ReturnToSpawnPoint()
    {
        if (Vector2.Distance(transform.position, spawnPoint) > 0.1f)
        {
            Vector2 direction = (spawnPoint - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, spawnPoint, moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}