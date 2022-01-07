using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public static GameObject ambience;
    private void Awake()
    {
        if (ambience != null)
            Destroy(this.gameObject);
        else
            ambience = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }
}
