using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowYCoord : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target == null)
            Destroy(this);
        else
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
    }

}
