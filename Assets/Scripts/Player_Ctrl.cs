using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ctrl : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private Animator PlayerAnim;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private HealthControl health;
    [SerializeField] private Transform healthbarFollow;

    private Rigidbody2D rb;
    private string currentAni;
    private string previousAni;

    private Vector3 targetPosition;

    private bool isAttack = false;
    private bool isMoving = false;
    private bool isDead = false;

    private int maxHealth = 100;
    private int currentHealth;

    private Vector3 respawn;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
        targetPosition = transform.position;

        currentHealth = maxHealth;
        health.SetMaxHealth(maxHealth);

        AudioManage.Instance.PlayMusic("BattleTheme");

        respawn = transform.position;
    }
    void Update()
    {
        if(!isAttack && !isDead)
        {
            MovePlayer(moveSpeed);
        }
        Attack();
        UpdateHealthBarPosition();
    }

    private void MovePlayer(float moveSpeed)
    {
        float xValue = (Input.GetAxis("Horizontal")) * moveSpeed * Time.deltaTime;
        float yValue = (Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;

        Vector2 move = new Vector2(xValue, yValue).normalized;
        transform.rotation = Quaternion.Euler(new Vector3(0, (xValue > 0.1f) ? 0 : 180, 0));

        if(!isAttack)
        {
            if (Math.Abs(xValue) > 0.1f || Math.Abs(yValue) > 0.1f)
            {
                isMoving = true;
                ChangeAnimation("player1_walk");
                rb.velocity = move * moveSpeed * Time.deltaTime;
            }
            else
            {
                isMoving = false;
                ChangeAnimation("player1_ani");
                rb.velocity = Vector2.zero;
            }
        }
        
    }
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemies);
        if(hitEnemies.Length > 0 && !isAttack)
        {
            StartCoroutine(AttackAnimation());
        }
    }
    private IEnumerator AttackAnimation()
    {
        isAttack = true;
        ChangeAnimation("player1_atk");
        AudioManage.Instance.PlaySFX("Attack");
        yield return new WaitForSeconds(PlayerAnim.GetCurrentAnimatorStateInfo(0).length);
        isAttack = false;
        ChangeAnimation("player1_ani");
    }
    private void ChangeAnimation(string AniName)
    {
        if (currentAni != AniName)
        {
            if(!string.IsNullOrEmpty(currentAni))
            {
                PlayerAnim.ResetTrigger(currentAni);
            }
            previousAni = currentAni;
            currentAni = AniName;
            PlayerAnim.SetTrigger(currentAni);
            Debug.Log("Doi hoat anh thanh: " + currentAni);
            
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        health.SetHealth(currentHealth);

        if(currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        ChangeAnimation("player1_die");
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10);
        isDead = false;
        transform.position = respawn;
        currentHealth = maxHealth;
        health.SetHealth(maxHealth);
        ChangeAnimation("player1_ani");
    }

    private void UpdateHealthBarPosition()
    {
        health.transform.position = healthbarFollow.position;
    }
}
