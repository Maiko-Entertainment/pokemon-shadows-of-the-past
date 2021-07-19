using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTransition : MonoBehaviour
{
    public float changeTime = 1.5f;
    public float totalDuration = 2f;

    void Start()
    {
        Destroy(gameObject, totalDuration);
    }
}
