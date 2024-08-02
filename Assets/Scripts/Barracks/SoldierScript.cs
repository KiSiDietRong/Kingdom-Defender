using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierScript : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float moveSpeed = 2f;
    public int minDamage = 5;
    public int maxDamage = 10;
    public float attackCooldown = 1.5f;
    public int maxHealth = 20;
    public Slider healthSliderPrefab; // Slider prefab để hiển thị thanh máu

    private int currentHealth;
    private GameObject targetEnemy;
    private Vector2 spawnPoint;
    private bool canAttack = true;
    private bool isDead = false;
    private Slider healthSlider; // Slider thực tế được tạo ra

    private void Start()
    {
        currentHealth = maxHealth; // Set initial health

        // Tạo và cấu hình thanh máu
        if (healthSliderPrefab != null)
        {
            healthSlider = Instantiate(healthSliderPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            healthSlider.transform.SetParent(transform);
            healthSlider.transform.localScale = new Vector3(1, 1, 1);
            healthSlider.value = (float)currentHealth / maxHealth;
            healthSlider.gameObject.SetActive(true);
        }
    }

    public void SetSpawnPoint(Vector2 position)
    {
        spawnPoint = position;
    }

    private void Update()
    {
        if (isDead) return; // Do nothing if soldier is dead

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

        // Cập nhật vị trí thanh máu
        if (healthSlider != null)
        {
            healthSlider.transform.position = transform.position + new Vector3(0, 1, 0);
            healthSlider.value = (float)currentHealth / maxHealth;
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
        if (healthSlider != null)
        {
            Destroy(healthSlider.gameObject); // Xóa thanh máu khi lính chết
        }
        Destroy(gameObject); // Xóa lính khỏi game
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