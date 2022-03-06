using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusMinor : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI statusName;

    public StatusEffectData status;

    public UIStatusMinor Load(StatusEffectData status)
    {
        this.status = status;
        if (status.icon)
            icon.sprite = status.icon;
        statusName.text = status.statusName;
        return this;
    }
}
