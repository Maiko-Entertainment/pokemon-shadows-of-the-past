﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleType : MonoBehaviour
{
    public TextMeshProUGUI typeName;
    public Image icon;
    public Image background;

    public UIBattleType Load(TypeData typeData)
    {
        typeName.text = typeData.typeName;
        icon.sprite = typeData.icon;
        background.color = typeData.color;
        return this;
    }
}
