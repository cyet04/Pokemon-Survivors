using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance { get; private set; }

    [SerializeField] private GameObject numberPrefab;
    [SerializeField] private Transform numberParent;

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

    public void SpawnDamageNumber(float amount, Vector3 position, Color color)
    {
        GameObject number = MyPoolManager.Instance.GetFromPool(numberPrefab, numberParent);
        if (number != null)
        {
            number.transform.position = position;
            number.gameObject.SetActive(true);
            DamageNumber dmgNumberComponent = number.GetComponent<DamageNumber>();
            dmgNumberComponent.Initialize(amount, color);
        }
    }
}
