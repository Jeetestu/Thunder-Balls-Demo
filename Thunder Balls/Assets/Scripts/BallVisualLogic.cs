using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;
using UnityEngine.Experimental.Rendering.Universal;

public class BallVisualLogic : MonoBehaviour
{

    public GameAssets.BallData data;
    public LightningBoltShapeSphereScript lightning;
    public Light2D light;
    public SpriteRenderer rend;
    public Transform childSprite;
    public Animator sphereAnim;
    public SpriteRenderer lightningSprite;

    float startingGlowIntensity;
    float startingRadius;
    float startingTurbulence;
    float startingIntervalRangeMin;
    float startingIntervalRangeMax;

    [Header("Breaking Visuals")]
    public float breakGlowIntensity;
    public float breakRadius;
    public float breakTurbulence;
    public float breakIntervalRangeMin;
    public float breakIntervalRangeMax;
    public float maxSpriteTremble;


    public void SetBreakageVisual(float percent)
    {
        lightning.GlowIntensity = Mathf.Lerp(startingGlowIntensity, breakGlowIntensity, percent);
        lightning.Radius = Mathf.Lerp(startingRadius, breakRadius, percent);
        lightning.Turbulence = Mathf.Lerp(startingTurbulence, breakTurbulence, percent);
        lightning.IntervalRange.Minimum = Mathf.Lerp(startingIntervalRangeMin, breakIntervalRangeMin, percent);
        lightning.IntervalRange.Maximum = Mathf.Lerp(startingIntervalRangeMax, breakIntervalRangeMax, percent);
        float trembleDistance = Mathf.Lerp(0f, maxSpriteTremble, percent);
        childSprite.transform.localPosition = new Vector3(Random.Range(-trembleDistance, trembleDistance), Random.Range(-trembleDistance, trembleDistance));
    }

    public void breakBallVisual()
    {
        sphereAnim.enabled = true;
    }

    public void SetRandomBallColor()
    {
        SetBallColor((BALLCOLOR)Random.Range(0, System.Enum.GetValues(typeof(BALLCOLOR)).Length));
    }

    public void SetBallColor(BALLCOLOR ballColor)
    {
        data = GameAssets.i.LookupBallData(ballColor);
        rend.sprite = data.sprite;
        lightning.GlowTintColor = data.visualColor;
        lightning.LightningTintColor = data.visualColor;
        light.color = data.visualColor;
        startingGlowIntensity = lightning.GlowIntensity;
        startingRadius = lightning.Radius;
        startingTurbulence = lightning.Turbulence;
        startingIntervalRangeMin = lightning.IntervalRange.Minimum;
        startingIntervalRangeMax = lightning.IntervalRange.Maximum;
        lightningSprite.color = data.visualColor;
    }

    public enum BALLCOLOR
    {
        BLUE,
        GREEN,
        ORANGE,
        PURPLE,
        PINK,
        RED
    }

}
