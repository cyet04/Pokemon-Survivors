using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance { get; private set; }

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PlayerController player;


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

    private void Start()
    {
        Observer.Instance.Register(EventId.OnHealthChanged, UpdateHealth);
    }


    private void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnHealthChanged, UpdateHealth);
    }

    public void UpdateHealth(object obj)
    {
        healthSlider.maxValue = player.playerData.maxHP;
        healthSlider.value = (float)obj;
        healthText.text = $"{(float)obj} / {player.playerData.maxHP}";
    }
}
