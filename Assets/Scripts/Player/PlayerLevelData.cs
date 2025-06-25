using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelData", menuName = "ScriptableObjects/PlayerLevelData")]
public class PlayerLevelData : ScriptableObject
{
    [Serializable]
    public class LevelData
    {
        public int level;
        public float expRequired;
    }

    public List<LevelData> levels;
    public float GetExpForLevel(int level)
    {
        if (level <= 0 || level > levels.Count) return Mathf.Infinity;
        return levels[level - 1].expRequired;
    }

    public int maxLevel => levels.Count;
}
