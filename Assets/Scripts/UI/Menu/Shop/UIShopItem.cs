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
    protected bool isSelling = false;

    public UIShopItem Load(ItemData item, bool isSelling = false)
    {
        this.isSelling = isSelling;
        int money = InventoryMaster.GetInstance().GetMoney();
        bool canBuy = money >= item.price || isSelling;
        this.item = item;
        title.text = item.GetName();
        icon.sprite = item.icon;
        price.text = ((int)(item.price * (isSelling ? InventoryMaster.sellModifier : 1f))) + "$";
        price.color = canBuy ? canBuyColor : cantBuyColor;
        return this;
    }

    public UIShopItem Reload()
    {
        return Load(item, isSelling);
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
