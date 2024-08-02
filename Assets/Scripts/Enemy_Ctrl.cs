using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Ctrl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D enemyCollider;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private Slider healthSlider;
    private string currentAni;
    private Animator EnemyAni;
    private int currentHealth;

    [Header("Attack Attributes")]
    [SerializeField] private float atkRange = 1.0f;
    [SerializeField] private float atkSpeed = 1.0f;
    [SerializeField] private int minDamage = 7;
    [SerializeField] private int maxDamage = 12;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask soldierLayer;

    private Transform target;
    private Transform player;
    private Transform soldier;
    private bool isAttacking = false;
    private bool isDead = false;

    private int pathIndex = 0;
    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
        EnemyAni = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
        EnemyAtk();
        AttackSoldier();
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
        if (isDead) return;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, atkRange, playerLayer);
        Collider2D[] hitSoldier = Physics2D.OverlapCircleAll(transform.position, atkRange, soldierLayer);
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
        //if (hitSoldier.Length > 0)
        //{
        //    soldier = hitSoldier[0].transform;
        //    rb.velocity = Vector2.zero;
        //    if (!isAttacking)
        //    {
        //        StartCoroutine(PlayAttackAnimation());
        //    }
        //}
        //else
        //{
        //    soldier = null;
        //}
    }
    private void AttackSoldier()
    {
        if (isDead) return;

        Collider2D[] hitSoldiers = Physics2D.OverlapCircleAll(transform.position, atkRange, soldierLayer);

        if (hitSoldiers.Length > 0)
        {
            Transform soldier = hitSoldiers[0].transform;
            rb.velocity = Vector2.zero;
            if (!isAttacking)
            {
                StartCoroutine(PlayAttackAnimation(soldier));
            }
        }
    }
    private IEnumerator PlayAttackAnimation(Transform soldier)
    {
        isAttacking = true;
        // Trigger attack animation if needed
        yield return new WaitForSeconds(atkSpeed);

        if (soldier != null)
        {
            int damage = Random.Range(minDamage, maxDamage);
            var soldierCtrl = soldier.GetComponent<SoldierScript>();
            if (soldierCtrl != null)
            {
                soldierCtrl.TakeDamage(damage);
            }
        }

        isAttacking = false;
    }


    private IEnumerator PlayAttackAnimation()
    {
        isAttacking = true;
        ChangeAnimation("atk");
        AudioManage.Instance.PlaySFX("EnemyAtk");
        yield return new WaitForSeconds(atkSpeed);
        if (player != null)
        {
            int damage = Random.Range(minDamage, maxDamage);
            player.GetComponent<Player_Ctrl>().TakeDamage(damage);
            
        }
        if (soldier != null)
        {
            int damage = Random.Range(minDamage, maxDamage);
            
            soldier.GetComponent<SoldierScript>().TakeDamage(damage);
        }
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

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        healthSlider.value = currentHealth;
        if (currentHealth < 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        enemyCollider.enabled = false;
        ChangeAnimation("die");
        //EnemySpawner.onEnemyDestroy.Invoke();
        //Destroy(gameObject, 1f);
        StartCoroutine(EnemyDeath());
    }

    private IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(EnemyAni.GetCurrentAnimatorStateInfo(0).length); 
        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }
}
