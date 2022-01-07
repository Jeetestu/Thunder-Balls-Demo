using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class StartingMenu : MonoBehaviour
{
    public float timeToFadeMenu;
    public CanvasGroup menuCanvasGroup;
    public TMP_Text highScore;

    private void Awake()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    public void loadMainGame()
    {
        StartCoroutine(loadMainGameRoutine());
    }

    IEnumerator loadMainGameRoutine()
    {
        float count = 0f;
        float progress;
        while (count < timeToFadeMenu)
        {
            count += Time.deltaTime;
            progress = count / timeToFadeMenu;
            menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(1);
    }

    public void loadTutorial()
    {
        StartCoroutine(loadTutorialRoutine());
    }
    IEnumerator loadTutorialRoutine()
    {
        float count = 0f;
        float progress;
        while (count < timeToFadeMenu)
        {
            count += Time.deltaTime;
            progress = count / timeToFadeMenu;
            menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(2);
    }
}
