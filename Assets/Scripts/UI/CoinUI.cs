using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public static CoinUI Instance { get; private set; }
    [SerializeField] private TMP_Text coinText;

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
        Observer.Instance.Register(EventId.OnCoinCollected, UpdateCoinCount);
    }

    private void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnCoinCollected, UpdateCoinCount);
    }

    private void UpdateCoinCount(object obj)
    {

    }
}
