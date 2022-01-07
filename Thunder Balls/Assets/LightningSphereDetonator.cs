using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphereDetonator : MonoBehaviour
{
    public AudioSource audio;
    private void OnTriggerStay2D(Collider2D collision)
    {
       // SoundManager.i.PlayAudioSource(audio);
        collision.GetComponent<LightningSphereMovement>().detonate();
    }
}
