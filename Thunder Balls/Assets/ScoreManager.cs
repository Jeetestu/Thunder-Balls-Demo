using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("References")]
    public TMP_Text inGameText;
    public TMP_Text retryMenuScoreText;
    public TMP_Text highScoreText;
    public GameObject highScoreAlertText;
    public int currentScore;
    public int highScore;
    private void Awake()
    {
        instance = this;
        currentScore = 0;
    }

    public void addScore(int amount)
    {
        currentScore = currentScore + amount;
        inGameText.text = currentScore.ToString();
    }

    public void setupRetryMenuScore()
    {
        retryMenuScoreText.text = currentScore.ToString();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            highScoreAlertText.SetActive(true);
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        highScoreText.text = highScore.ToString();
    }
}
