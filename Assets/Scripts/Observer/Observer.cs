using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public static Observer Instance { get; private set; }
    public Dictionary<EventId, Action<object>> dictionaries = new Dictionary<EventId, Action<object>>();

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

    public void Register(EventId eventId, Action<object> action)
    {
        if (!dictionaries.ContainsKey(eventId))
        {
            dictionaries[eventId] = action;
        }
        else
        {
            dictionaries[eventId] += action;
        }
    }

    public void UnRegister(EventId eventId, Action<object> action)
    {
        if (!dictionaries.ContainsKey(eventId)) return;
        dictionaries[eventId] -= action;
    }

    public void Broadcast(EventId eventId, object obj = null)
    {
        if (!dictionaries.ContainsKey(eventId)) return;
        dictionaries[eventId]?.Invoke(obj);
    }
}

public enum EventId
{
    OnHealthChanged,
    OnExpChanged,
    OnLevelUp,
    OnPlayerDied,
    OnEnemyDied,
    OnPauseGame,
}