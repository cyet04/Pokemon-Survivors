using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttackBehavior : IEnemyAttackBehaviors
{
    private float lastFireTime;

    public void Attack(PlayerController player, EnemyController enemy)
    {
        if (!enemy.canAttack) return;
        if (Time.time - lastFireTime < 1 / enemy.enemyData.projectileData.fireRate) return;
        

        GameObject projectile = MyPoolManager.Instance.GetFromPool(enemy.enemyData.projectileData.projectilePrefab, null);
        
        if (projectile != null)
        {
            projectile.transform.position = enemy.transform.position;
            Vector2 direction = (player.transform.position - enemy.transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * enemy.enemyData.projectileData.speed;

            // Set initialize projectile data
            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.Initialize(enemy.damage, enemy.enemyData.projectileData.lifeTime);
            }
        }

        lastFireTime = Time.time;
    }
}