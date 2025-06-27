using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance { get; private set; }
    public  PlayerController player;

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
        player = FindObjectOfType<PlayerController>();
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
                PlayerWeaponModifier.Instance.attackSpeed += value;
                break;
            case SkillType.Damage:
                PlayerWeaponModifier.Instance.damage += value;
                break;
            case SkillType.Heal:
                //
                break;
            case SkillType.Hp:
                player.currentHP += value;
                player.maxHP += value;
                Observer.Instance.Broadcast(EventId.OnHealthChanged, player.currentHP);
                break;
            case SkillType.Regen:
                //
                break;
            case SkillType.Speed:
                player.moveSpeed *= value;
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
