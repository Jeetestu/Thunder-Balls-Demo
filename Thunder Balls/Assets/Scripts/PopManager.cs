using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopManager : MonoBehaviour
{
    public static PopManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PopBalls(HashSet<BallCollisionLogic> chainHash)
    {
        //copying to list which allows iteration
        List<BallCollisionLogic> chain = new List<BallCollisionLogic>();
        foreach (BallCollisionLogic b in chainHash)
            chain.Add(b);
        for (int i = chain.Count - 1; i >=0; i--)
        {
            chain[i].destroyBallPositiveCause();
        }
        PlatformController.instance.updateEntireBounds();
    }

}
