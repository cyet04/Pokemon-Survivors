using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance { get; private set; }

    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private Transform damageNumberParent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnDamageNumber(float damage, Vector3 position)
    {
        GameObject damageNumber = MyPoolManager.Instance.GetFromPool(damageNumberPrefab, damageNumberParent);
        if (damageNumber != null)
        {
            damageNumber.transform.position = position;
            damageNumber.gameObject.SetActive(true);
            DamageNumber dmgNumberComponent = damageNumber.GetComponent<DamageNumber>();
            dmgNumberComponent.Initialize(damage);
        }
    }
}
