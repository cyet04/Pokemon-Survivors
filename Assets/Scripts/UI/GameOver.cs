using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, ShowGameOverPanel);
    }


    private void OnDisable()
    {
        Observer.Instance.UnRegister(EventId.OnPlayerDied, ShowGameOverPanel);
    }

    public void ShowGameOverPanel(object obj)
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; // Resume game
    }

    public void Home()
    {

    }
}
