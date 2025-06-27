using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image icon;
    private SkillData skill;

    public void SetUp(SkillData skillData)
    {
        skill = skillData;
        nameText.text = skillData.skillName;
        icon.sprite = skillData.icon;
    }

    public void OnClick()
    {
        FindObjectOfType<SkillSelectUI>().OnSkillSelected(skill);
    }
}
