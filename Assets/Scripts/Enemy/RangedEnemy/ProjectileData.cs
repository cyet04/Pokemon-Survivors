
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public GameObject projectilePrefab;
    public float speed;
    public float fireRate;
    public float lifeTime;
}
