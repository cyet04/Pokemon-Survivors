using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    private float damage;
    private float fireRate;
    private float range;
    private float speed;
    private float lifeTime;

    private Coroutine lifeCoroutine;

    private void OnEnable()
    {
        if (weaponData != null)
        {
            damage = weaponData.damage;
            fireRate = weaponData.fireRate;
            range = weaponData.range;
            speed = weaponData.speed;
            lifeTime = range / speed;
        }
        else
        {
            Debug.LogError("Weapon data is not assigned in the inspector.");
        }

        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
        }
        lifeCoroutine = StartCoroutine(DisableAfterLifeTime(lifeTime));
    }

    private void OnDisable()
    {
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
        }
    }

    private IEnumerator DisableAfterLifeTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null && PlayerSkillManager.Instance.learnedSkill != null)
        {
            foreach (var pair in PlayerSkillManager.Instance.learnedSkill)
            {
                if (pair.Key.skillType == SkillType.Freeze)
                {
                    int level = pair.Value;
                    float freezeTime = pair.Key.levelStats[level - 1].value;
                    enemy.Freeze(freezeTime);
                    break;
                }
            }
        }

        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null)
        {
            obj.TakeDamage(damage + PlayerWeaponModifier.Instance.damage);
        }
        gameObject.SetActive(false);
    }
}
