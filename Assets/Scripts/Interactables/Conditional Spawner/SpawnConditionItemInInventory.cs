using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnConditionItemInInventory
{
    public ItemId itemId;
    public SpawnConditionDataValueCheck condition;
    public int amount;

    public bool IsTrue()
    {
        ItemInventory item = InventoryMaster.GetInstance().GetItem(itemId);
        int currentAmount = item.GetAmount();
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
