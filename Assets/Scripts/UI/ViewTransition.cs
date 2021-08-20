using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTransition : MonoBehaviour
{
    public float changeTime = 1.5f;
    public float totalDuration = 2f;
    public AudioClip sound;

    void Start()
    {
        if (sound != null)
            AudioMaster.GetInstance().PlaySfx(sound);
        Destroy(gameObject, totalDuration);
    }
}
