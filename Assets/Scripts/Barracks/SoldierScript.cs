using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierScript : MonoBehaviour
{
    public float detectionRange = 5f; 
    public float attackRange = 1f; 
    public float moveSpeed = 2f; 
    private GameObject targetEnemy;

    private void Update()
    {
        FindTarget();

        if (targetEnemy != null)
        {
            MoveTowardsEnemy();

            if (Vector2.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
            {
                AttackEnemy();
            }
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

    private void AttackEnemy()
    {
        Debug.Log("Attacking the enemy!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
