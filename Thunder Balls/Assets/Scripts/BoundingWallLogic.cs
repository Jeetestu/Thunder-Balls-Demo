using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingWallLogic : MonoBehaviour
{
    List<BallCollisionLogic> ballsTouchingBounds;

    private void Awake()
    {
        ballsTouchingBounds = new List<BallCollisionLogic>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CaughtBall"))
            ballsTouchingBounds.Add(other.gameObject.GetComponent<BallCollisionLogic>());
    }
    private void Update()
    {
        for (int i = ballsTouchingBounds.Count - 1; i >= 0; i--)
            if (!ballsTouchingBounds[i].caught)
                ballsTouchingBounds.RemoveAt(i);
            else
            {
                //takes 1.5 seconds to break
                ballsTouchingBounds[i].setBreakagePercent(ballsTouchingBounds[i].breakage + Time.deltaTime / 1.5f);
            }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CaughtBall"))
        {
            BallCollisionLogic logic = other.gameObject.GetComponent<BallCollisionLogic>();
            logic.setBreakagePercent(0f);
            ballsTouchingBounds.Remove(logic);
        }

    }
}
