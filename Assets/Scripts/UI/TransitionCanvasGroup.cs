using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvasGroup : TransitionBase
{
    public CanvasGroup canvasGroup;

    private void Start()
    {
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        if (fading)
        {
            canvasGroup.alpha += speed * Time.deltaTime;
        }
    }
}
