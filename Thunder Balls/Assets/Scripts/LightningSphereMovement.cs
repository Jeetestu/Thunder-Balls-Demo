using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;

public class LightningSphereMovement : MonoBehaviour
{
    public int moveTicks;

    public void detonate()
    {
        BarManager.instance.MoveBar(moveTicks);
        Destroy(this.gameObject);
    }

}
