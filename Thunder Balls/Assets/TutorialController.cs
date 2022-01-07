using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;
public class TutorialController : MonoBehaviour
{
    public float fadeTime;
    public List<Light2D> firstLights;
    public List<Light2D> secondLights;
    public List<Light2D> thirdLights;
    public List<Light2D> fourthLights;
    public List<Light2D> fifthLights;
    public TMP_Text firstText;
    public TMP_Text secondText;
    public TMP_Text thirdText;
    public TMP_Text fourthText;
    public TMP_Text fifthText;

    List<Light2D> fadeIn;
    List<Light2D> fadeOut;
    TMP_Text fadeInText;
    TMP_Text fadeOutText;

    private void Awake()
    {
        foreach (Light2D light in firstLights)
            light.intensity = 0f;
        foreach (Light2D light in secondLights)
            light.intensity = 0f;
        foreach (Light2D light in thirdLights)
            light.intensity = 0f;
        foreach (Light2D light in fourthLights)
            light.intensity = 0f;
        foreach (Light2D light in fifthLights)
            light.intensity = 0f;
        secondText.alpha = 0f;
        thirdText.alpha = 0f;
        fourthText.alpha = 0f;
        fifthText.alpha = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeIn = new List<Light2D>();
        fadeIn.AddRange(firstLights);
        fadeOut = new List<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Light2D light in fadeIn)
        {
            if (light.intensity <= 1f && !fadeOut.Contains(light))
                light.intensity = Mathf.Min(1f, light.intensity + Time.deltaTime * fadeTime);
        }
        foreach (Light2D light in fadeOut)
        {
            if (light.intensity >= 0f)
                light.intensity = Mathf.Max(0f, light.intensity - Time.deltaTime * fadeTime);
        }

        if (fadeOutText != null)
        {
            if (fadeOutText.alpha <= 0)
                fadeOutText = null;
            else
                fadeOutText.alpha = Mathf.Max(0f, fadeOutText.alpha - Time.deltaTime * fadeTime);
        }
        if (fadeInText != null)
        {
            if (fadeInText.alpha >= 1)
                fadeInText = null;
            else
                fadeInText.alpha = Mathf.Min(1f, fadeInText.alpha + Time.deltaTime * fadeTime);
        }
    }

    public void ShiftToSecondLights()
    {
        fadeOut.AddRange(firstLights);
        fadeIn.AddRange(secondLights);
        fadeOutText = firstText;
        fadeInText = secondText;
    }

    public void ShiftToThirdLights()
    {
        fadeOut.AddRange(secondLights);
        fadeIn.AddRange(thirdLights);
        firstText.alpha = 0f;
        fadeInText = thirdText;
        fadeOutText = secondText;
    }

    public void ShiftToFourthLights()
    {
        fadeOut.AddRange(thirdLights);
        fadeIn.AddRange(fourthLights);
        secondText.alpha = 0f;
        fadeInText = fourthText;
        fadeOutText = thirdText;
    }

    public void ShiftToFifthLights()
    {
        fadeOut.AddRange(fourthLights);
        fadeIn.AddRange(fifthLights);
        thirdText.alpha = 0f;
        fadeInText = fifthText;
        fadeOutText = fourthText;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
