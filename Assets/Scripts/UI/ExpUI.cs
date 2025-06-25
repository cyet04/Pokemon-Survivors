using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    public static ExpUI Instance { get; private set; }
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text expText;


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
        Observer.Instance.Register(EventId.OnExpChanged, UpdateExp);
    }


    private void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnExpChanged, UpdateExp);
    }

    public void UpdateExp(object obj)
    {
        PlayerExpData playerExpData = (PlayerExpData)obj;
        if (playerExpData == null) return;
        expSlider.maxValue = playerExpData.expRequired;
        expSlider.value = playerExpData.currentExp;
        expText.text = $"{playerExpData.currentExp} / {playerExpData.expRequired}";
    }
}
