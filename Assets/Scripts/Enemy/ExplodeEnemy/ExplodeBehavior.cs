using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

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

        Vector2 boxSize = new Vector2(3.5f, 2f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(enemy.transform.position + new Vector3(0, 2, 0), boxSize, 0f);
        foreach (var hit in hits)
        {
            var obj = hit.gameObject.GetComponent<IDamageable>();
            if (obj != null)
            {
                obj.TakeDamage(enemy.enemyData.damage);
            }
        }

        enemy.gameObject.SetActive(false);
    }
}
