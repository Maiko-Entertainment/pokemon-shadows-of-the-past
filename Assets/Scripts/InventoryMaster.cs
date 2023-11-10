﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryMaster : MonoBehaviour
{
    public static InventoryMaster Instance;
    public static float sellModifier = 0.75f;

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
    public void Load(SaveFile save)
    {
        inventory = new List<ItemInventory>();
        foreach (PersistedItem pi in save.persistedItems)
        {
            ChangeItemAmount(pi.GetId(), pi.amount);
        }
    }
    public void HandleSave()
    {
        List<PersistedItem> persistedItem = new List<PersistedItem>();
        foreach (ItemInventory item in inventory)
        {
            persistedItem.Add(item.GetSave());
        }
        SaveMaster.Instance.activeSaveFile.persistedItems = persistedItem;
    }

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

    public void ChangeItemAmount(string id, int changeAmount)
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
        ItemInventory newInventoryInstance = new ItemInventory(newInstance, changeAmount);
        inventory.Add(newInventoryInstance);

        // Tries to add move to party if its a TM
        if (newInstance.categoryId == ItemCategory.TM)
        {
            foreach (PokemonCaughtData p in PartyMaster.GetInstance().GetParty())
            {
                p.CheckForLearnedMoves(p.GetLevel());
            }
        }
    }

    public ItemInventory GetItem(string id)
    {
        foreach (ItemInventory i in inventory)
        {
            if (i.itemData.GetItemId() == id)
            {
                return i;
            }
        }
        ItemData newInstance = ItemMaster.GetInstance().GetItem(id);
        ItemInventory newInventoryInstance = new ItemInventory(newInstance, 0);
        inventory.Add(newInventoryInstance);
        return newInventoryInstance;
    }

    public List<ItemInventory> GetItems()
    {
        return inventory.ToList();
    }

    public int GetMoney()
    {
        float money = SaveMaster.Instance.GetSaveElementFloat(SaveElementId.money.ToString());
        return (int)money;
    }

    public int ChangeMoney(int change)
    {
        int moneyAmount = GetMoney();
        int amountAfter = Mathf.Max(0, moneyAmount + change);
        SaveMaster.Instance.SetSaveElement((float)amountAfter, SaveElementId.money.ToString());
        return amountAfter;
    }

    public bool HasTMForMove(string moveId)
    {
        return inventory.Find((item =>
        {
            if (item.itemData.GetItemCategory() == ItemCategory.TM && item.amount > 0)
            {
                try
                {
                    ItemDataTM tmItem = (ItemDataTM)item.itemData;
                    return tmItem.moveLearned.GetId() == moveId;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        })) != null;
    }
}
