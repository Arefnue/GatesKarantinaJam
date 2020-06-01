using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    
    public static ScoreManager manager;
    
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

        Application.targetFrameRate = 30;

    }
    

    #endregion


    public float maxBarricadeHealth;
    public float currentBarricadeHealth { get; private set; }

    public int fireCount;

    public int poisonedCount { get; private set; }
    public int deathCount { get; private set; }
    [HideInInspector]public int holyCowScore;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI poisonedCounterText;
    public TextMeshProUGUI deathCounterText;
    public GameObject gameOverPanel;
    public Text scoreText;
    

    private void Start()
    {
        currentBarricadeHealth = maxBarricadeHealth;
        poisonedCount = 0;
        deathCount = 0;
        healthText.text = "x " + currentBarricadeHealth.ToString();
        poisonedCounterText.text = "x " + poisonedCount.ToString();
        deathCounterText.text = "x " + deathCount.ToString();
    }

    public void Damage(float damageValue)
    {
        currentBarricadeHealth -= damageValue;
        if (currentBarricadeHealth <= 0)
        {
            currentBarricadeHealth = 0;
            healthText.text = "x " + currentBarricadeHealth.ToString();
            scoreText.text = CalculateScore();
            Time.timeScale = 0;
            UIManager.manager.pauseButton.SetActive(false);
            gameOverPanel.SetActive(true);
        }
        healthText.text = "x "+currentBarricadeHealth.ToString();
    }

    public string CalculateScore()
    {
        var finalScore = "Score: "+(poisonedCount + deathCount+ holyCowScore).ToString();
        return finalScore;
    }

    public void Heal(float healValue)
    {
        currentBarricadeHealth += healValue;
        if (currentBarricadeHealth >= maxBarricadeHealth)
        {
            currentBarricadeHealth = maxBarricadeHealth;
            healthText.text = "x " + currentBarricadeHealth.ToString();
        }
        healthText.text = "x "+currentBarricadeHealth.ToString();
    }

    public void AddPoisoned()
    {
        poisonedCount++;
        poisonedCounterText.text = "x " + poisonedCount.ToString();
    }

    public void AddHolyCowScore(int value)
    {
        holyCowScore += value;
    }

    public void AddDeath()
    {
        deathCount++;
        deathCounterText.text = "x " + deathCount.ToString();
    }

    public void RemovePoisoned()
    {
        poisonedCount--;
        poisonedCounterText.text = "x " + poisonedCount.ToString();
    }

    public void RemoveDeath()
    {
        deathCount--;
        deathCounterText.text = "x " + deathCount.ToString();
    }
    
    
    
}
