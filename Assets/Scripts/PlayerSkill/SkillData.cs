using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType skillType;
    public int maxLevel = 5;

    public List<SkillLevelStat> levelStats;
}

public enum SkillType
{
    Multishot,
    AttackSpeed,
    Damage,
    Freeze,
    Heal,
    Hp,
    Regen,
    Speed,
    Summon,
}

[System.Serializable]
public class SkillLevelStat
{
    public float value;
}
