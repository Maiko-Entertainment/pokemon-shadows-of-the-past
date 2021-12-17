using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuSectionIcon : TransitionFade
{
    public Image border;
    public Color unselectedColor = Color.clear;
    public Color selectedColor = Color.white;

    private float progress = 0;

    void Update()
    {
        if (fading)
        {
            progress = Mathf.Clamp01(progress + speed * Time.deltaTime);
            border.color = Color.Lerp(unselectedColor, selectedColor, progress);
        }
    }
}
