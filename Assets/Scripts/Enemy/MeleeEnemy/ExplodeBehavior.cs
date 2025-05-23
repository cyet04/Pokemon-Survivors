using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplodeBehavior : IEnemyAttackBehaviors
{   
    public void Attack(PlayerController player, EnemyController enemy)
    {
        enemy.StartCoroutine(PrepareBeforeExplode(enemy));
    }

    IEnumerator PrepareBeforeExplode(EnemyController enemy)
    {
        SpriteRenderer spriteRenderer = enemy.spriteRenderer;
        Color originalColor = spriteRenderer.color;
        Color flashColor = Color.red;

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.DOColor(flashColor, 0.1f).OnComplete(() =>
            {
                spriteRenderer.DOColor(originalColor, 0.1f);
            });
            yield return new WaitForSeconds(0.2f);
        }

        GameObject explosion = MyPoolManager.Instance.GetFromPool(enemy.enemyData.explodeEffect, null);
        if (explosion != null)
        {
            explosion.transform.position = enemy.transform.position;
            explosion.SetActive(true);
        }

        enemy.gameObject.SetActive(false);
    }
}
