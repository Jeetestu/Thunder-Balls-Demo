using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlatformController : MonoBehaviour
{
    public static PlatformController instance;
    public Rigidbody2D rb;
    public Collider2D col;
    public float height;

    public float rightBound;
    public float leftBound;
    public float maxSpeed;
    public Bounds entireBounds;


    float boundWidth;
    float offsetFromCentreOfBounds;
    float actualLeftBound;
    float actualRightBound;

    private void Awake()
    {
        instance = this;
        updateEntireBounds();
    }

    //Updates the bounds to include all attached balls
    public void updateEntireBounds()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        entireBounds = new Bounds(transform.position, Vector3.one);
        entireBounds.Encapsulate(col.bounds);
        foreach (Collider2D c in colliders)
        {
            entireBounds.Encapsulate(c.bounds);
        }

        boundWidth = entireBounds.size.x / 2f;
        offsetFromCentreOfBounds = transform.position.x - entireBounds.center.x;
        actualLeftBound = leftBound + boundWidth + offsetFromCentreOfBounds - 0.1f;
        actualRightBound = rightBound - boundWidth + offsetFromCentreOfBounds + 0.1f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //DEBUG CODE
        //Debug.DrawLine(new Vector2(actualLeftBound, 100f), new Vector2(actualLeftBound, -100f), Color.red);
        //Debug.DrawLine(new Vector2(actualRightBound, 100f), new Vector2(actualRightBound, -100f), Color.red);
        if (LevelManager.instance.gameOver)
            return;
        if (Input.touchCount>0)
        {
            float targetX = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x;


            float actualX = Mathf.Clamp(targetX, actualLeftBound, actualRightBound);
            actualX = Mathf.Clamp(actualX, transform.position.x - maxSpeed, transform.position.x + maxSpeed);
            rb.MovePosition(new Vector2(actualX, height));
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            float targetX = rb.position.x + 2f * Time.fixedDeltaTime;


            float actualX = Mathf.Clamp(targetX, actualLeftBound, actualRightBound);
            actualX = Mathf.Clamp(actualX, transform.position.x - maxSpeed, transform.position.x + maxSpeed);
            rb.MovePosition(new Vector2(actualX, height));
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            float targetX = rb.position.x - 2f * Time.fixedDeltaTime;


            float actualX = Mathf.Clamp(targetX, actualLeftBound, actualRightBound);
            actualX = Mathf.Clamp(actualX, transform.position.x - maxSpeed, transform.position.x + maxSpeed);
            rb.MovePosition(new Vector2(actualX, height));
        }
    }
}
