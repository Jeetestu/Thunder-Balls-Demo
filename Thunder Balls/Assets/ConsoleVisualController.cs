using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ConsoleVisualController : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer rend1;
    public SpriteRenderer rend2;
    public SpriteRenderer rend3;

    public Light2D ambienceLights;

    [Header("Settings")]
    public float passiveAlphaLevel;
    public float litAlphaLevel;
    public float passiveIntensityLevel;
    public float litIntensityLevel;

    private void Awake()
    {
        modifyAlpha(passiveAlphaLevel);
    }

    public void lightUp()
    {
        modifyAlpha(litAlphaLevel);
        modifyIntensity(litIntensityLevel);
    }

    public void lightPassive()
    {
        modifyAlpha(passiveAlphaLevel);
        modifyIntensity(passiveIntensityLevel);
    }

    public void lightOff()
    {
        modifyAlpha(0f);
        modifyIntensity(0f);
    }

    private void modifyAlpha(float alpha)
    {
        Color newRendColor = rend1.color;
        newRendColor.a = alpha;
        rend1.color = newRendColor;
        rend2.color = newRendColor;
        rend3.color = newRendColor;
    }

    private void modifyIntensity (float intensity)
    {
        ambienceLights.intensity = intensity;
    }
}
