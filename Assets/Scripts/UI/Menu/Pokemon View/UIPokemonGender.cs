using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonGender : MonoBehaviour
{
    public Image genderIcon;
    public Image genderBackground;

    public Sprite genderMaleIcon;
    public Sprite genderFemaleIcon;
    public Color maleColor = Color.blue;
    public Color femaleColor = Color.cyan;

    protected bool isMale = true;

    public UIPokemonGender Load(bool isMale)
    {
        genderIcon.sprite = isMale ? genderMaleIcon : genderFemaleIcon;
        genderBackground.color = isMale ? maleColor : femaleColor;
        return this;
    }
}
