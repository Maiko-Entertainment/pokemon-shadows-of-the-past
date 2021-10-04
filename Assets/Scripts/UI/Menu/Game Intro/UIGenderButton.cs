using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGenderButton : MonoBehaviour
{
    public Color maleColor;
    public Color femaleColor;
    public Sprite maleIcon;
    public Sprite femaleIcon;

    public Image icon;
    public Image background;

    public void Load(bool isMale)
    {
        if (isMale)
        {
            icon.sprite = maleIcon;
            background.color = maleColor;
        }
        else
        {
            icon.sprite = femaleIcon;
            background.color = femaleColor;
        }
    }
}
