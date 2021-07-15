using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public ItemId itemId;
    public string itemName;
    public bool isConsumable;
    public ItemCategory categoryId;
    public AudioClip useSound;
    public Sprite icon;
    public string description;

    public string GetName()
    {
        return itemName;
    }

    public virtual ItemId GetItemId()
    {
        return itemId;
    }
    public virtual ItemTargetType GetItemTargetType()
    {
        return ItemTargetType.None;
    }
    public ItemCategory GetItemCategory()
    {
        return categoryId;
    }

    public virtual CanUseResult CanUse()
    {
        return new CanUseResult(true, "");
    }

    // Used to open menus for selecting or direct item use
    public virtual void Use()
    {

    }

    // Call after item has been successfully been used
    public virtual void HandleAfterUse()
    {
        if (isConsumable)
        {
            InventoryMaster.GetInstance()?.ChangeItemAmount(itemId, -1);
        }
    }
}
