using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerData playerData;
    public float currentHP;
    public float maxHP;
    public float moveSpeed;
    [Header("------Player level------")]
    private int currentLevel = 1;
    private float currentExp = 0f;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerInputAction playerInputAction;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponData weaponData;
    private DashEffectController dashEffect;

    private float fireCoolDown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashEffect = GetComponent<DashEffectController>();

        // Setup for input actions
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        // Move
        playerInputAction.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputAction.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        // Dash
        playerInputAction.Player.Dash.performed += ctx => TryDash();
        // Pause
        playerInputAction.Player.Pause.performed += ctx => Observer.Instance.Broadcast(EventId.OnPauseGame);

        if (playerData != null)
        {
            currentHP = playerData.maxHP;
            maxHP = playerData.maxHP;
            moveSpeed = playerData.moveSpeed;
        }
        else
        {
            Debug.LogError("Player data is not assigned");
        }
    }

    private void Start()
    {
        // Load cac thong so player
        HealthUI.Instance.UpdateHealth(currentHP);
        ExpUI.Instance.UpdateExp(new PlayerExpData(currentExp, playerData.playerLevel.GetExpForLevel(currentLevel)));
        Observer.Instance.Register(EventId.OnEnemyDied, GainExpFromEnemy);
    }

    private void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnEnemyDied, GainExpFromEnemy);
    }

    private void Update()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        AutoAttack();
    }

    private void FixedUpdate()
    {
        if (!dashEffect.isDashing)
        {
            rb.velocity = moveInput.normalized * moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Observer.Instance.Broadcast(EventId.OnHealthChanged, currentHP);
        DamageNumberManager.Instance.SpawnDamageNumber(damage, transform.position, new Color(1, 0.6f, 0, 1));
        if (currentHP <= 0)
        {
            // Handle player death
            gameObject.SetActive(false);
            Observer.Instance.Broadcast(EventId.OnPlayerDied);
        }
    }

    private void AutoAttack()
    {
        fireCoolDown -= Time.deltaTime;
        if (fireCoolDown < 0)
        {
            fireCoolDown = 1f / weaponData.fireRate - PlayerWeaponModifier.Instance.attackSpeed;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        // ScreenToWorldPoint cần z = distance từ camera đến object
        // nếu camera ở z = -10 thì z = 10
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        int totalShots = 1 + PlayerWeaponModifier.Instance.extraWeapon;
        float angleStep = 10f; // goc gia cac phat ban
        float baseAngle = -angleStep * (totalShots - 1) / 2f; // goc ban dau

        for (int i = 0; i < totalShots; i++)
        {
            float angle = baseAngle + i * angleStep;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * direction;
            
            GameObject weapon = MyPoolManager.Instance.GetFromPool(weaponPrefab, this.transform);
            weapon.transform.position = firePoint.position;
            weapon.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
            rb.velocity = dir * weaponData.speed;
            Debug.DrawLine(firePoint.position, firePoint.position + (Vector3)dir * 5f, Color.green, 1f);
        }
    }

    private void TryDash()
    {
        dashEffect.StartDash(moveInput);
    }

    public void GainExp(float exp)
    {
        currentExp += exp;
        float expToLevelUp = playerData.playerLevel.GetExpForLevel(currentLevel);
        while (currentExp >= expToLevelUp && currentLevel < playerData.playerLevel.maxLevel)
        {
            currentExp -= expToLevelUp;
            currentLevel++;
            expToLevelUp = playerData.playerLevel.GetExpForLevel(currentLevel);
            Observer.Instance.Broadcast(EventId.OnLevelUp);
        }

        Observer.Instance.Broadcast(EventId.OnExpChanged, new PlayerExpData(currentExp, playerData.playerLevel.GetExpForLevel(currentLevel)));    
    }

    public void GainExpFromEnemy(object obj)
    {
        float exp = (float)obj;
        GainExp(exp);
    }
}