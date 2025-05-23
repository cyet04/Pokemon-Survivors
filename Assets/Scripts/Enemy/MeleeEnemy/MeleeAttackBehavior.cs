using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehavior : IEnemyAttackBehaviors
{
    public void Attack(PlayerController player, EnemyController enemy)
    {
        if (enemy.canAttack)
        {
            player.TakeDamage(enemy.damage);
            enemy.ResetCooldown();
        }
    }
}
