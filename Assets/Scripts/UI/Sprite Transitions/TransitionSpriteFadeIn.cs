using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSpriteFadeIn : TransitionBase
{
    public SpriteRenderer spriteRenderer;
    public float startingAlpha = 0;
    public float finalAlpha = 1;

    void Update()
    {
        timePassed += Time.deltaTime;
        if (fading)
        {
            Color c = spriteRenderer.color;
            float alpha = Mathf.SmoothStep(startingAlpha, finalAlpha, timePassed * speed);
            spriteRenderer.color = new Color(c.r,c.g,c.b, alpha);
        }
    }
}
