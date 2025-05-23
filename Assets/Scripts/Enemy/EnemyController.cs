using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public EnemyData enemyData;

    public float currentHP;
    public float moveSpeed;
    public float damage;
    public float attackRange;

    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public PlayerController player;
    private Vector2 direction;

    private float coolDownTime = 1f;
    private float coolDownCounter;
    public bool canAttack = true;

    private IEnemyAttackBehaviors attackBehavior;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        if (enemyData != null)
        {
            currentHP = enemyData.maxHP;
            moveSpeed = enemyData.moveSpeed;
            damage = enemyData.damage;
            attackRange = enemyData.attackRange;
        }
        else
        {
            Debug.LogError("Enemy data is not assigned in the inspector.");
        }
        coolDownCounter = coolDownTime;


        // Sủ dụng strategy pattern để xác định attack behavior
        switch (enemyData.attackType)
        {
            case EnemyAttackType.Melee:
                attackBehavior = new MeleeAttackBehavior();
                break;
            case EnemyAttackType.Ranged:
                attackBehavior = new RangedAttackBehavior();
                break;
            case EnemyAttackType.Explode:
                attackBehavior = new ExplodeBehavior();
                break;
        }
    }

    private void Update()
    {
        // Rotation to player
        direction = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x > 0;

        if (!canAttack)
        {
            coolDownCounter -= Time.deltaTime;
            if (coolDownCounter <= 0)
            {
                canAttack = true;
                coolDownCounter = coolDownTime;
            }
        }
    }

    private void FixedUpdate()
    {
        // Thay đổi tùy loại enemy
        if (enemyData.attackType == EnemyAttackType.Melee || enemyData.attackType == EnemyAttackType.Explode)
        {
            rb.velocity = direction * moveSpeed;
        }
        else if (enemyData.attackType == EnemyAttackType.Ranged)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > attackRange - 0.5f)
            {
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
                attackBehavior?.Attack(player, this);
            }
        }
    }

    public void ResetCooldown()
    {
        canAttack = false;
        coolDownCounter = coolDownTime;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            // Handle enemy death
            if (enemyData.attackType == EnemyAttackType.Explode)
            {
                rb.velocity = Vector2.zero;
                attackBehavior?.Attack(player, this);
            }

            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null && canAttack)
        {
            obj.TakeDamage(damage);
            ResetCooldown();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}