using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttackBehaviors 
{
    void Attack(PlayerController player, EnemyController enemy);
}
