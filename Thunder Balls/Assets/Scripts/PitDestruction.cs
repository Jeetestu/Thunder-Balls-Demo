using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitDestruction : MonoBehaviour
{
    public static PitDestruction instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform downRight;
    public Transform downLeft;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaunchedBall"))
        {
            collision.gameObject.GetComponent<BallCollisionLogic>().destroyBallNegativeCause();
        }
    }
}
