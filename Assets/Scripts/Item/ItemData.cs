using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public ItemId itemId;
    public string itemName;
    public bool isConsumable;
    public bool canBeUsedInBattle = true;
    public ItemCategory categoryId;
    public AudioClip useSound;
    public Sprite icon;
    public string description;
    public int price = 100;
    public List<BattleAnimation> animations = new List<BattleAnimation>();

    public string GetName()
    {
        return itemName;
    }
    public virtual string GetDescription()
    {
        return description;
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
        bool isInBattle = BattleMaster.GetInstance().IsBattleActive();
        if (isInBattle)
        {
            return new CanUseResult(canBeUsedInBattle, canBeUsedInBattle ? "" : "This cannot be used in battle!");
        }
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
