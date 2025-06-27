using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class DamageNumber : MonoBehaviour
{
    private TMP_Text Text;

    private void Awake()
    {
        Text = GetComponent<TMP_Text>();
    }

    public void Initialize(float amount, Color color)
    {
        Text.text = amount.ToString("0");
        Text.color = color;

        // Sequence hiệu ứng
        Sequence sq = DOTween.Sequence();
        sq.Append(transform.DOMoveY(transform.position.y + 0.5f, 1f).SetEase(Ease.OutQuad));
        sq.Join(transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack));
        sq.Join(Text.DOFade(0f, 1f));
        sq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
