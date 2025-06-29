using DG.Tweening;
using System.Collections;
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

    public void ItemAnimation(float delay = 0f)
    {
        transform.localScale = Vector3.zero; 
        transform.DOScale(Vector3.one, 1f)
            .SetEase(Ease.OutBounce)
            .SetDelay(delay)
            .SetUpdate(true); // Khi len level thi game pause timescale = 0 nen can SetUpdate(true)
    }
}

