using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnConditionItemInInventory
{
    public ItemData item;
    public SpawnConditionDataValueCheck condition;
    public int amount;

    public bool IsTrue()
    {
        ItemInventory itemInventory = InventoryMaster.GetInstance().GetItem(item.GetItemId());
        int currentAmount = itemInventory.GetAmount();
        switch (condition)
        {
            case SpawnConditionDataValueCheck.IsLessThan:
                return currentAmount <= amount;
            case SpawnConditionDataValueCheck.IsMoreThan:
                return currentAmount >= amount;
            case SpawnConditionDataValueCheck.IsDifferent:
                return currentAmount != amount;
            default:
                return currentAmount == amount;
        }
    }
}
