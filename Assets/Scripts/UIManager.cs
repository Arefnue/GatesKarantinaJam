using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    
    public static UIManager manager;
    
    
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 0;

    }
    

    #endregion

    public Animator cameraAnimator;
    public GameObject touchController;
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public Text pauseScoreText;

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        
        cameraAnimator.SetTrigger("Start");
        touchController.SetActive(true);
        startPanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        
        touchController.SetActive(false);
        
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseScoreText.text = ScoreManager.manager.CalculateScore();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        touchController.SetActive(true);
        
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }
    
    
}
