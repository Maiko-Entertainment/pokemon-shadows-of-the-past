using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSpin : TransitionBase
{
    public Vector3 initialRotation;
    public Vector3 rotationAdd = new Vector3(0, 360, 0);

    protected Vector3 vSpeed = Vector3.zero;

    Vector3 currentValues;

    void Update()
    {
        timePassed += Time.deltaTime;
        if (fading)
        {
            Vector3 target = initialRotation + rotationAdd;
            currentValues = Vector3.SmoothDamp(currentValues, target, ref vSpeed, 1 / speed);
            transform.localEulerAngles = currentValues;
            if (Vector3.Distance(currentValues, target) == 0)
            {
                if (pingPong)
                {
                    rotationAdd *= -1;
                }
            }
            //transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, initialRotation + rotationAdd, ref vSpeed, 1 / speed);
        }
    }
}
