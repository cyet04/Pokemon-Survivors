using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class DamageNumber : MonoBehaviour
{
    private TMP_Text dmgText;

    private void Awake()
    {
        dmgText = GetComponent<TMP_Text>();
    }

    public void Initialize(float damage)
    {
        dmgText.text = damage.ToString("0");
        dmgText.color = new Color(1f, 0.6f, 0f, 1f); // Màu đỏ nhạt

        // Sequence hiệu ứng
        Sequence sq = DOTween.Sequence();
        sq.Append(transform.DOMoveY(transform.position.y + 0.5f, 1f).SetEase(Ease.OutQuad));
        sq.Join(transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack));
        sq.Join(dmgText.DOFade(0f, 1f));
        sq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
