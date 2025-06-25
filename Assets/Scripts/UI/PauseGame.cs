using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        Observer.Instance.Register(EventId.OnPauseGame, ShowPausePanel);
    }

    private void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnPauseGame, ShowPausePanel);
    }

    public void ShowPausePanel(object obj)
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }
}
