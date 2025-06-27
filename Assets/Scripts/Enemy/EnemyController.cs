using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour, IDamageable
{
    public EnemyData enemyData;
    public float currentHP;
    public float moveSpeed;
    public float damage;
    public float attackRange;
    public float exp;

    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    public Rigidbody2D rb;
    public PlayerController player;
    private Vector2 direction;

    private float coolDownTime = 1f;
    private float coolDownCounter;
    public bool canAttack = true;

    private IEnemyAttackBehaviors attackBehavior;

    private bool isFrozen = false;
    private Coroutine freezeRoutine;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        if (enemyData != null)
        {
            currentHP = enemyData.maxHP;
            moveSpeed = enemyData.moveSpeed;
            damage = enemyData.damage;
            attackRange = enemyData.attackRange;
            exp = enemyData.exp;
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
        if (!isFrozen)
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
    }

    public void ResetCooldown()
    {
        canAttack = false;
        coolDownCounter = coolDownTime;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        // Hieu ung khi dinh damage
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
            spriteRenderer.DOColor(originalColor, 0.05f));

        // Them hieu ung day lui
        transform.DOMove((Vector2)transform.position + (-direction) * 0.3f, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Neu con song thi tiep tuc di chuyen
                if (currentHP > 0 && !isFrozen)
                {
                    rb.velocity = direction * moveSpeed;
                }
            });

        DamageNumberManager.Instance.SpawnDamageNumber(damage, transform.position, new Color(1, 0.6f, 0, 1));
        if (currentHP <= 0)
        {
            Observer.Instance.Broadcast(EventId.OnEnemyDied, exp);

            if (PlayerSkillManager.Instance.learnedSkill != null)
            {
                foreach (var pair in PlayerSkillManager.Instance.learnedSkill)
                {
                    if (pair.Key.skillType == SkillType.Heal)
                    {
                        int level = pair.Value;
                        int hp = (int)pair.Key.levelStats[level - 1].value;
                        if (player.currentHP < player.maxHP)
                        {
                            hp = Mathf.Min(hp, (int)(player.maxHP - player.currentHP));
                        }
                        player.currentHP += hp;
                        DamageNumberManager.Instance.SpawnDamageNumber(hp, player.transform.position, Color.green);
                        Observer.Instance.Broadcast(EventId.OnHealthChanged, player.currentHP);
                        break;
                    }
                }
            }

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

    public void Freeze(float duration)
    {
        if (isFrozen)
        {
            if (freezeRoutine != null)
            {
                StopCoroutine(freezeRoutine);
            }
        }
        freezeRoutine = StartCoroutine(FreezeCoroutine(duration));
    }
    IEnumerator FreezeCoroutine(float duration)
    {
        isFrozen = true;
        spriteRenderer.DOColor(Color.cyan, 0.2f);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(duration);

        spriteRenderer.DOColor(originalColor, 0.2f);
        isFrozen = false;
        freezeRoutine = null;
    }
}