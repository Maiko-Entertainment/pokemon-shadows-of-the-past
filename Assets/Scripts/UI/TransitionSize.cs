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
        if (fading)
        {
            timePassed += Time.deltaTime;
            if (speed < 0)
            {
                transform.localScale = Vector3.Lerp(finalSize, initialSize, timePassed * speed * -1);
            }
            else
            {
                transform.localScale = Vector3.Lerp(initialSize, finalSize, timePassed * speed);
            }
            if (pingPong && Vector3.Distance(transform.localScale, finalSize) < 0.001f)
            {
                Vector3 newFinal = initialSize;
                initialSize = finalSize;
                finalSize = newFinal;
                timePassed = 0;
            }
        }
    }
}
