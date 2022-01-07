using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerLogic : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaunchingBall"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("LaunchedBall");
        }
    }
}
