using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierScript : MonoBehaviour
{
    public float detectionRange = 5f; // Phạm vi phát hiện kẻ địch
    public float attackRange = 1f; // Phạm vi tấn công
    public float moveSpeed = 2f; // Tốc độ di chuyển của lính
    private GameObject targetEnemy; // Kẻ địch hiện tại mà lính nhắm đến

    private void Update()
    {
        // Tìm kiếm kẻ địch trong phạm vi phát hiện
        FindTarget();

        if (targetEnemy != null)
        {
            // Di chuyển về phía kẻ địch
            MoveTowardsEnemy();

            // Kiểm tra nếu đã trong phạm vi tấn công
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
            {
                // Thực hiện hành động tấn công (cần thêm logic tấn công ở đây)
                AttackEnemy();
            }
        }
    }

    private void FindTarget()
    {
        // Tìm tất cả các đối tượng có lớp "Enemy" trong phạm vi phát hiện
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
            // Di chuyển về phía kẻ địch
            Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void AttackEnemy()
    {
        // Logic tấn công kẻ địch (có thể gây sát thương, làm giảm máu, v.v.)
        Debug.Log("Attacking the enemy!");
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi phát hiện trong Editor để dễ kiểm tra
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
