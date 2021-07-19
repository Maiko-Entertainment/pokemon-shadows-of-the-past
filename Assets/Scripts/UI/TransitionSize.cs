using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSize : TransitionBase
{
    public Vector3 initialSize = new Vector3(0, 0, 1);
    public Vector3 finalSize = new Vector3(1, 1, 1);

    public override void FadeIn()
    {
        base.FadeIn();
        transform.localScale = initialSize;
    }
    void Update()
    {
        timePassed += Time.deltaTime;
        if (fading)
        {
            transform.localScale = Vector3.Lerp(initialSize, finalSize, timePassed * speed);
        }
    }
}
