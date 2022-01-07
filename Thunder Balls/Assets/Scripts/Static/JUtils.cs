using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JUtils
{
    public static class JUtilsClass
    {

        public static Transform closestTransformToPoint (Vector2 point, Transform t1, Transform t2)
        {
            if (Vector2.Distance(point, t1.position) < Vector2.Distance(point, t2.position))
                return t1;
            else
                return t2;
        }
    }
}
