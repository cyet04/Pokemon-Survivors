using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillCardUI[] skillCards;

    [SerializeField] private List<SkillData> allSkills;

    public void Start()
    {
        Observer.Instance.Register(EventId.OnLevelUp, ShowSkillSelection);
    }

    public void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnLevelUp, ShowSkillSelection);
    }

    private void ShowSkillSelection(object obj)
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        var available = PlayerSkillManager.Instance.GetAvailableSkills(allSkills);
        var chosen = GetRandomSkills(available, 3);

        for (int i = 0; i < skillCards.Length; i++)
        {
            skillCards[i].SetUp(chosen[i]);
        }
    }

    private List<SkillData> GetRandomSkills(List<SkillData> list, int count)
    {
        List<SkillData> result = new();
        while (result.Count < count && list.Count > 0)
        {
            int index = Random.Range(0, list.Count);
            result.Add(list[index]);
            list.RemoveAt(index);
        }

        return result;
    }

    public void OnSkillSelected(SkillData skill)
    {
        PlayerSkillManager.Instance.LearnSkill(skill);
        Time.timeScale = 1f; // Resume game
        panel.SetActive(false);
    }
}
