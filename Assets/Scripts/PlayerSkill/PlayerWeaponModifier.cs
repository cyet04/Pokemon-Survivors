using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponModifier : MonoBehaviour
{
    public static PlayerWeaponModifier Instance { get; private set; }

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

    public int extraWeapon = 0;
    public float attackSpeedMutiplier = 1f;
}
