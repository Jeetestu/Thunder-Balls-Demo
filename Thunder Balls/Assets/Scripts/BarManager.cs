using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    public static BarManager instance;

    [Header("Settings")]
    public float maxYPosition;
    public float minYPosition;
    public float targetYPosition;
    public float tickDistance;

    [Header("References")]
    public Transform leftOrigin;
    public Transform rightDestination;
    public Collider2D col;
    public Rigidbody2D rb;
    public ConsoleVisualController upConsole;
    public ConsoleVisualController downConsole;
    public AudioSource audio;

    private bool moving;
    private void Awake()
    {
        instance = this;
        targetYPosition = transform.position.y;
    }
    
    public void MoveBar(int ticks)
    {
        if (LevelManager.instance.gameOver)
            return;
        targetYPosition += ticks * tickDistance;
        targetYPosition = Mathf.Clamp(targetYPosition, minYPosition, maxYPosition);
        moving = true;
        updateVisuals();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(new Vector2(rb.position.x, Mathf.Lerp(rb.position.y, targetYPosition, Time.fixedDeltaTime)));
        if (moving)
            if (Mathf.Abs(rb.position.y - targetYPosition) < 0.1f)
            {
                moving = false;
                updateVisuals();
            }

    }

    private void updateVisuals()
    {
        if (moving)
        {
            SoundManager.i.modifyAudioSource(audio, time: 0.6f, volume: 0.3f);
            if (targetYPosition > rb.position.y)
            {
                upConsole.lightUp();
            }
            else
            {
                downConsole.lightUp();
            }

        }
        else
        {
            SoundManager.i.modifyAudioSource(audio, time: 0.3f,volume: 0);
            upConsole.lightPassive();
            downConsole.lightPassive();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("CaughtBall") && !LevelManager.instance.gameOver)
        {
            LevelManager.instance.GameOverBarHit(collision.transform.position);
        }
    }
}
