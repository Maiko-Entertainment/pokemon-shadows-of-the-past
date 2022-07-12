using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMoveGameObject : TransitionBase
{
    public Vector3 initialPosition = Vector3.zero;
    public Vector3 finalPosition;

    void Update()
    {
        if (fading)
        {
            timePassed += Time.deltaTime;
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, finalPosition, speed * Time.deltaTime);
            if (Vector3.Distance(gameObject.transform.localPosition, finalPosition) == 0)
            {
                if (pingPong)
                {
                    Vector3 newFinal = initialPosition;
                    initialPosition = finalPosition;
                    finalPosition = newFinal;
                }
                else
                {
                    fading = false;
                }

            }
        }
    }
}
