using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleItem : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Image icon;
    public TextMeshProUGUI amount;

    public delegate void Hover(ItemInventory item);
    public event Hover onHover;

    private ItemInventory item;

    public UIBattleItem Load(ItemInventory item)
    {
        this.item = item;
        title.text = item.GetName();
        icon.sprite = item.itemData.icon;
        amount.text = "x"+ (item.itemData.isConsumable ? item.GetAmount().ToString() : "");
        return this;
    }

    public void HandleUse()
    {
        item.itemData.Use();
    }

    public void HandlePreview()
    {
        onHover?.Invoke(item);
    }
}
