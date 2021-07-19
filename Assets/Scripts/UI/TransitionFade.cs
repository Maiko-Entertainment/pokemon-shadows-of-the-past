using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFade : MonoBehaviour
{
    public float speed = 1;
    public bool fading;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void FadeIn()
    {
        fading = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        speed = Mathf.Abs(speed);
    }

    public virtual void FadeOut()
    {
        fading = true;
        speed = -1f * Mathf.Abs(speed);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Hide()
    {
        fading = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }

    void Update()
    {
        if (fading)
        {
            canvasGroup.alpha += speed * Time.deltaTime;
        }
    }
}
