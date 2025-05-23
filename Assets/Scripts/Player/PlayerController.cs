using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerData playerData;
    private float curentHP;
    private float moveSpeed;

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
        playerInputAction.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputAction.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInputAction.Player.Dash.performed += ctx => TryDash();

        if (playerData != null)
        {
            curentHP = playerData.maxHP;
            moveSpeed = playerData.moveSpeed;
        }
        else
        {
            Debug.LogError("Player data is not assigned in the inspector.");
        }
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
        curentHP -= damage;
        if (curentHP <= 0)
        {
            // Handle player death
            Debug.Log("Player is dead");
        }
    }

    private void AutoAttack()
    {
        fireCoolDown -= Time.deltaTime;
        if (fireCoolDown < 0)
        {
            fireCoolDown = 1f / weaponData.fireRate;
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
        GameObject weapon = MyPoolManager.Instance.GetFromPool(weaponPrefab, null);
        weapon.transform.position = firePoint.position;
        weapon.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        WeaponController weaponController = weapon.GetComponent<WeaponController>();
        weaponController.weaponData = weaponData;

        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        rb.velocity = direction * weaponData.speed;
        Debug.DrawLine(firePoint.position, firePoint.position + (Vector3)direction * 5f, Color.green, 1f);
    }

    private void TryDash()
    {
        dashEffect.StartDash(moveInput);
    }
}

