using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EmergencyCanvas : MonoBehaviour
{
    public bool countDownActive = false;
    public float countdown;
    public TMP_Text countdownText;
    public TMP_Text advertisementChanceText;
    private void Update()
    {
        if (!countDownActive)
            return;
        if (countdown > 0)
        {
            countdown = countdown - Time.deltaTime;
            countdownText.text = ((int)countdown).ToString();
        }
        else
        {
            NoDecision();
        }
    }

    public void NoDecision()
    {
        countDownActive = false;
        LevelManager.instance.emergencyResetNoDecision();
    }

}
