using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherVisualController : MonoBehaviour
{
    public float reloadTime;
    public float launchTime;
    public Vector3 originPosition;
    public Vector3 launchedPosition;

    bool launching;

    private float launchStartTime;
    private float launchEndTime;
    private Rigidbody2D rb;

    private bool isActivated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void runLaunchVisual()
    {
        isActivated = true;
        launching = true;
        launchStartTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (launching)
        {
            rb.position = Vector3.Lerp(originPosition, launchedPosition, (Time.time - launchStartTime) / launchTime);
            if (transform.position == launchedPosition)
            {
                launching = false;
                launchEndTime = Time.time;
            }

        }

        if (!launching && isActivated)
        {
            rb.position = Vector3.Lerp(launchedPosition, originPosition, (Time.time - launchEndTime) / reloadTime);
        }
    }

}
