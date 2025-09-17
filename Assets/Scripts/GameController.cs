using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public GameObject loadCanvas;
    public Slider progressSlider;
    public GameObject player;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;
    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedLevelIsCount;

    public static event Action OnReset;

    void Start()
    {
        progressAmount = 0;

        // Subscribe to events
        PlayerHealth.OnPlayedDied += GameOverScreen;
        Coin.OnCoinCollect += IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;

        // Make sure UI is hidden at the start
        if (loadCanvas != null) loadCanvas.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);

        progressSlider.value = 0;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent callbacks on destroyed objects
        PlayerHealth.OnPlayedDied -= GameOverScreen;
        Coin.OnCoinCollect -= IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete -= LoadNextLevel;
    }

    void LoadLevel(int level, bool wantSurvivedIncrease)
    {
        if (loadCanvas != null) loadCanvas.SetActive(true);

        if (levels[currentLevelIndex] != null)
            levels[currentLevelIndex].SetActive(false);

        if (levels[level] != null)
            levels[level].SetActive(true);

        if (player != null)
            player.transform.position = Vector3.zero;

        currentLevelIndex = level;
        if (wantSurvivedIncrease) survivedLevelIsCount++;
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
    }

    void GameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Debug.LogWarning("⚠ GameOverScreen is missing or destroyed!");
        }
    }

    public void ResetGame()
    {   
        OnReset?.Invoke();
        Time.timeScale = 1;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        survivedLevelIsCount = 0;
        LoadLevel(0, false);
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        if (progressSlider != null)
            progressSlider.value = progressAmount;
    }
}
