using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPool
{
    private Stack<GameObject> stack = new Stack<GameObject>();
    private GameObject prefab;

    private Transform parent;
    private GameObject tmpObject;
    private ReturnToPool returnToPool;

    public MyPool(GameObject baseObject, Transform parent = null)
    {
        this.prefab = baseObject;
        this.parent = parent;
    }

    public GameObject GetObject()
    {
        if (stack.Count > 0)
        {
            tmpObject = stack.Pop();
            tmpObject.SetActive(true);
            return tmpObject;
        }
        tmpObject = GameObject.Instantiate(prefab, parent);
        returnToPool = tmpObject.GetComponent<ReturnToPool>();
        returnToPool.pool = this;
        return tmpObject;
    }

    public void AddToPool(GameObject obj)
    {
        stack.Push(obj);
    }
}

