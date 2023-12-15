using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFieldStatus : MonoBehaviour
{
    public TextMeshProUGUI categoryText;
    public TextMeshProUGUI nameText;

    public Image icon;

    public UIMenuAnim anim;

    protected StatusField _statusField;

    public UIFieldStatus Load(StatusField status)
    {
        _statusField = status;
        if (categoryText) categoryText.text = status.FieldData.fieldCategory.GetCategoryName();
        if (nameText) nameText.text = status.FieldData.GetStatusName();
        icon.sprite = status.FieldData.GetIcon();
        return this;
    }

    public void Open()
    {
        anim?.OpenDialog();
    }

    public void Close()
    {
        anim?.CloseDialog();
    }

    public StatusField GetStatusField()
    {
        return _statusField;
    }
}
