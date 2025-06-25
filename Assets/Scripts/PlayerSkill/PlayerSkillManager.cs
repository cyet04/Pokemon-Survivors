using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance { get; private set; }

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

    public Dictionary<SkillData, int> learnedSkill = new();

    public void LearnSkill(SkillData skill)
    {
        if (!learnedSkill.ContainsKey(skill))
        {
            learnedSkill[skill] = 1;
        }
        else
        {
            if (learnedSkill[skill] < skill.maxLevel)
            {
                learnedSkill[skill]++;
            }
        }

        ApplySkillEffect(skill);
    }

    public int GetSkillLevel(SkillData skill)
    {
        return learnedSkill.TryGetValue(skill, out int level) ? level : 0;
    }

    private void ApplySkillEffect(SkillData skill)
    {
        int level = GetSkillLevel(skill);
        float value = skill.levelStats[level - 1].value;

        switch (skill.skillType)
        {
            case SkillType.Multishot:
                PlayerWeaponModifier.Instance.extraWeapon += (int)value;
                break;
            case SkillType.AttackSpeed:
                //
                break;
            case SkillType.Damage:
                //
                break;
            case SkillType.Freeze:
                //
                break;
            case SkillType.Heal:
                //
                break;
            case SkillType.Hp:
                //
                break;
            case SkillType.Regen:
                //
                break;
            case SkillType.Speed:
                //
                break;
            case SkillType.Summon:
                //
                break;

        }
    }

    public List<SkillData> GetAvailableSkills(List<SkillData> allSkills)
    {
        List<SkillData> candidates = new();
        foreach (var skill in allSkills)
        {
            if (!learnedSkill.ContainsKey(skill) || learnedSkill[skill] < skill.maxLevel)
            {
                candidates.Add(skill);
            }

        }
        return candidates;
    }
}
