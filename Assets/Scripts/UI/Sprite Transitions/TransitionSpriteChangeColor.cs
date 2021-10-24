using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSpriteChangeColor : TransitionBase
{
    public Color initialColor = Color.white;
    public Color finalColor = Color.white;
    public SpriteRenderer sprite;

    public float initialDelay = 0;

    private void Update()
    {
        if (fading)
        {
            initialDelay -= Time.deltaTime;
            if (initialDelay < 0)
            {
                timePassed += Time.deltaTime;
                sprite.color = Color.Lerp(initialColor, finalColor, speed * timePassed);
                if (sprite.color.Equals(finalColor))
                {
                    if (pingPong)
                    {
                        Color aux = initialColor;
                        initialColor = finalColor;
                        finalColor = aux;
                    }
                }
            }
        }
    }
}
