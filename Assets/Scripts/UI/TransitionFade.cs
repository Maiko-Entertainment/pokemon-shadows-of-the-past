using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFade : MonoBehaviour
{
    public float speed = 1;
    public bool fading;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        fading = true;
        speed = Mathf.Abs(speed);
    }

    public void FadeOut()
    {
        fading = true;
        speed = -1 * Mathf.Abs(speed);
    }

    public void Hide()
    {
        fading = false;
        canvasGroup.alpha = 0;
    }

    void Update()
    {
        if (fading)
        {
            canvasGroup.alpha += speed * Time.deltaTime;
        }
    }
}
