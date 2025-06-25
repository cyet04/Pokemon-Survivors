using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public float maxHP;
    public float moveSpeed;
    public PlayerLevelData playerLevel;
    //public List<Skill>
}
