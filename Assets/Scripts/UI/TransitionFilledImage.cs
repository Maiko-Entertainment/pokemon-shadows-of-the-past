using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionFilledImage : MonoBehaviour
{
    public float speed = 1;
    public float fadeSpeed = 1;
    public bool fading;
    public Image image;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        fading = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        fadeSpeed = Mathf.Abs(fadeSpeed);
        speed = Mathf.Abs(speed);
    }

    public void FadeOut()
    {
        fading = true;
        fadeSpeed = -1 * Mathf.Abs(fadeSpeed);
        speed = -1 * Mathf.Abs(speed);
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
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            image.fillAmount = Mathf.Clamp01(image.fillAmount + speed * Time.deltaTime);
        }
    }
}
