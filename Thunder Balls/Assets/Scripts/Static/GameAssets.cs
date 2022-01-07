using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    [System.Serializable]
    public struct BallData
    {
        public BallVisualLogic.BALLCOLOR colorEnum;
        public Color visualColor;
        public Sprite sprite;
    }

    public BallData LookupBallData(BallVisualLogic.BALLCOLOR ballColor)
    {
        foreach (BallData b in ballData)
            if (b.colorEnum == ballColor)
                return b;
        return ballData[0];
    }


    public GameObject connectedBallLightningPrefab;
    public List<BallData> ballData;


}
