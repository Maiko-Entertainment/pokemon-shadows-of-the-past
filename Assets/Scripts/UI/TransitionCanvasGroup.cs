using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvasGroup : TransitionBase
{
    public CanvasGroup canvasGroup;
    public bool destroyAfterFadeOut = false;
    public float min = 0;
    public float max = 1;
    private void Start()
    {
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public override void FadeIn()
    {
        base.FadeIn();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public override void FadeOut()
    {
        base.FadeOut();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (fading)
        {
            canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + speed * Time.deltaTime, min, max);
            if (destroyAfterFadeOut)
            {
                if (canvasGroup.alpha <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
