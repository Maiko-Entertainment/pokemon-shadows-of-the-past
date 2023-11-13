using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHideOnTextEmpty : MonoBehaviour
{
    public TextMeshProUGUI text;

    public CanvasGroup canvasGroup;

    private void Start()
    {
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Update()
    {
        canvasGroup.alpha = text && !string.IsNullOrWhiteSpace(text.text) ? 1f : 0f;
    }
}
