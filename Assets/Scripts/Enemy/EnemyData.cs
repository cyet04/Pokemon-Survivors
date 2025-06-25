using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHP;
    public float moveSpeed;
    public float damage;
    public float attackRange;
    public float exp;
    public EnemyAttackType attackType;

    [Header("------Ranged Attack------")]
    public ProjectileData projectileData;

    [Header("------Exploded------")]
    public GameObject explodeEffect;
}

public enum EnemyAttackType
{
    Melee,
    Ranged,
    Magic,
    Explode,
}