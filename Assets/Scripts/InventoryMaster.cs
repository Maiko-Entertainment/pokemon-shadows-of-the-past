﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMaster : MonoBehaviour
{
    public static InventoryMaster Instance;

    public List<ItemInventory> inventory = new List<ItemInventory>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static InventoryMaster GetInstance() { return Instance; }

    public List<ItemInventory> GetItemsFromCategory(ItemCategory category)
    {
        List<ItemInventory> filtered = new List<ItemInventory>();
        foreach(ItemInventory i in inventory)
        {
            if (i.itemData.GetItemCategory() == category)
            {
                filtered.Add(i);
            }
        }
        return filtered;
    }

    public void ChangeItemAmount(ItemId id, int changeAmount)
    {
        foreach (ItemInventory i in inventory)
        {
            if (i.itemData.GetItemId() == id)
            {
                i.ChangeAmount(changeAmount);
                return;
            }
        }
        ItemData newInstance = ItemMaster.GetInstance().GetItem(id);
        ItemInventory newInventoryInstance = new ItemInventory(newInstance, 0);
        newInventoryInstance.ChangeAmount(changeAmount);
        inventory.Add(newInventoryInstance);
    }
}