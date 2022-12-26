using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI title;
    public Image icon;
    public TextMeshProUGUI price;
    public AudioClip onClickSound;

    public Color canBuyColor = Color.white;
    public Color cantBuyColor = Color.red;

    public delegate void Hover(ItemData item);
    public event Hover onHover;
    public delegate void Click(ItemData item);
    public event Hover onClick;

    protected ItemData item;

    public UIShopItem Load(ItemData item)
    {
        int money = InventoryMaster.GetInstance().GetMoney();
        bool canBuy = money >= item.price;
        this.item = item;
        title.text = item.GetName();
        icon.sprite = item.icon;
        price.text = item.price + "$";
        price.color = canBuy ? canBuyColor : cantBuyColor;
        return this;
    }

    public UIShopItem Reload()
    {
        return Load(item);
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

    public ItemData GetItem()
    {
        return item;
    }
}
