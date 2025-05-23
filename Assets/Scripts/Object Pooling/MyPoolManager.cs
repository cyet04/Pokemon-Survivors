using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoolManager : MonoBehaviour
{
    public static MyPoolManager Instance { get; private set; }

    private Dictionary<GameObject, MyPool> pools = new Dictionary<GameObject, MyPool>();
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

    public GameObject GetFromPool(GameObject baseObject, Transform parent)
    {
        if (!pools.ContainsKey(baseObject))
        {
            pools.Add(baseObject, new MyPool(baseObject, parent));
        }
        return pools[baseObject].GetObject();
    }
}
