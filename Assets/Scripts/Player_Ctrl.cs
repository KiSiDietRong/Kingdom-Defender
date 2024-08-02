using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ctrl : MonoBehaviour
{
    #region Enemy_Code
    //[Header("References")]
    //[SerializeField] private Rigidbody2D rb;

    //[Header("Attributes")]
    //[SerializeField] private float moveSpeed = 2;

    //private Transform target;
    //private int pathIndex = 0;
    //private void Start()
    //{
    //    target = LevelManager.main.path[pathIndex];
    //}

    //private void Update()
    //{
    //    if (Vector2.Distance(target.position, transform.position) <= 0.1f)
    //    {
    //        pathIndex++;

    //        if (pathIndex == LevelManager.main.path.Length)
    //        {
    //            Destroy(gameObject);
    //            return;
    //        }
    //        else
    //        {
    //            target = LevelManager.main.path[pathIndex];
    //        }
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    Vector2 dir = (target.position - transform.position).normalized;

    //    rb.velocity = dir * moveSpeed;
    //}
    #endregion

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private Animator PlayerAnim;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private LayerMask enemies;

    private Rigidbody2D rb;
    private string currentAni;
    private string previousAni;

    private Vector3 targetPosition;

    private bool isAttack = false;
    private bool isMoving = false;
    //public AudioClip walkClip;
    //AudioSource playerSource;

<<<<<<< HEAD
    private int maxHealth = 100;
    private int currentHealth;
    private Transform enemy;

    private Vector3 respawn;

    private Coroutine regenCoroutine;

    private Collider2D[] colliders;
=======
>>>>>>> Toàn
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
        targetPosition = transform.position;
<<<<<<< HEAD

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        AudioManage.Instance.PlayMusic("BattleTheme");

        respawn = transform.position;
        colliders = GetComponents<Collider2D>();
=======
>>>>>>> Toàn
    }
    void Update()
    {
        if(!isAttack)
        {
            MovePlayer(moveSpeed);
        }
<<<<<<< HEAD
        if (!isDead)
        {
            Attack();
        }
        if (!isMoving && !isAttack && !isDead)
        {
            if (regenCoroutine == null)
            {
                regenCoroutine = StartCoroutine(RegenerateHealth());
            }
        }
        else
        {
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }
=======
        Attack();
>>>>>>> Toàn
    }

    private void MovePlayer(float moveSpeed)
    {
        if (isDead) return;

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
        if (isDead) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemies);
        if(hitEnemies.Length > 0 && !isAttack)
        {
            StartCoroutine(AttackAnimation());
        }
    }
    private IEnumerator AttackAnimation()
    {
        if (isDead) yield break;
        isAttack = true;
        ChangeAnimation("player1_atk");
        AudioManage.Instance.PlaySFX("Attack");
        yield return new WaitForSeconds(PlayerAnim.GetCurrentAnimatorStateInfo(0).length);
<<<<<<< HEAD
        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemies);
        //foreach (Collider2D enemyCol in hitEnemies)
        //{
        //    enemyCollider.GetComponent<Enemy_Ctrl>().TakeDamage(UnityEngine.Random.Range(minDMG, maxDMG + 1));
        //}
        //yield return new WaitForSeconds(PlayerAnim.GetCurrentAnimatorStateInfo(0).length - 0.2f);
        if (enemy != null && !isDead)
        {
            int damage = UnityEngine.Random.Range(minDMG, maxDMG);
            enemy.GetComponent<Enemy_Ctrl>().TakeDamage(damage);
        }
=======
>>>>>>> Toàn
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
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
<<<<<<< HEAD

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthSlider.value = currentHealth;
        if(currentHealth < 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        //ChangeAnimation("player1_die");
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        ChangeAnimation("player1_die");
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        yield return new WaitForSeconds(PlayerAnim.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(10);
        Respawn();
    }

    private IEnumerator FadeOut()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= 0.05f)
        {
            Color newColor = sprite.color;
            newColor.a = alpha;
            sprite.color = newColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator FadeIn()
    {
        for (float alpha = 0; alpha <= 1f; alpha += 0.05f)
        {
            Color newColor = sprite.color;
            newColor.a = alpha;
            sprite.color = newColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void Respawn()
    {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        transform.position = respawn;
        isDead = false;
        StartCoroutine(FadeIn());
        ChangeAnimation("player1_ani");
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }
    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (currentHealth < maxHealth && !isDead)
            {
                currentHealth += 1;
                healthSlider.value = currentHealth;
            }
            yield return new WaitForSeconds(1f);
        }
    }
=======
>>>>>>> Toàn
}
