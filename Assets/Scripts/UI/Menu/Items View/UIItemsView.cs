using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItemsView : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI title;
    public Image icon;
    public TextMeshProUGUI amount;
    public AudioClip onClickSound;

    public delegate void Hover(ItemInventory item);
    public event Hover onHover;
    public delegate void Click(ItemInventory item);
    public event Hover onClick;

    public ItemInventory item;

    public UIItemsView Load(ItemInventory item)
    {
        this.item = item;
        title.text = item.GetName();
        icon.sprite = item.itemData.icon;
        amount.text = item.itemData.isConsumable ? ("x"+item.GetAmount().ToString()) : "";
        return this;
    }

    public void HandleClick()
    {
        if (onClickSound)
        {
            AudioMaster.GetInstance().PlaySfx(onClickSound);
        }
        onClick?.Invoke(item);
    }

    public void HandleHover()
    {
        onHover?.Invoke(item);
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleHover();
    }
}
