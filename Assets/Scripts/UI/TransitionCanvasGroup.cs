using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvasGroup : TransitionBase
{
    public CanvasGroup canvasGroup;
    public bool destroyAfterFadeOut = false;
    public float min = 0;
    public float max = 1;

    protected bool preventDestroy = false;
    private void Start()
    {
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        if (fading)
        {
            if (speed >= 0)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
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
        preventDestroy = false;
    }

    public virtual void FadeOutTemporary()
    {
        FadeOut();
        preventDestroy = true;
    }

    void Update()
    {
        if (fading)
        {
            canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + speed * Time.deltaTime, min, max);
            if (destroyAfterFadeOut && !preventDestroy)
            {
                if (canvasGroup.alpha <= min && speed < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
