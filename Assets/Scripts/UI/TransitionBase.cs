using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBase : MonoBehaviour
{
    public float speed = 1;
    public bool fading;
    public bool pingPong = false;

    protected float timePassed;

    public virtual void FadeIn()
    {
        fading = true;
        speed = Mathf.Abs(speed);
    }

    public virtual void FadeOut()
    {
        fading = true;
        speed = -1f * Mathf.Abs(speed);
    }

    public virtual void Hide()
    {
        fading = false;
    }
}
