using UnityEngine;
[System.Serializable]
public class ItemInventory
{
    public ItemData itemData;
    public int amount;

    public ItemInventory(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        this.amount = amount;
    }
    public ItemInventory(ItemElement pe)
    {
        ItemData itemData = ItemMaster.GetInstance().GetItem(pe.id);
        this.itemData = itemData;
        amount = pe.amount;
    }

    public ItemElement GetSave()
    {
        ItemElement pi = new ItemElement();
        pi.amount = amount;
        pi.id = itemData.itemId;
        return pi;
    }

    public string GetName()
    {
        return itemData.GetName();
    }
    public int GetAmount()
    {
        return amount;
    }
    public bool HasEnough()
    {
        if (itemData.isConsumable)
        {
            return amount > 0;
        }
        else
        {
            return true;
        }
    }

    public void ChangeAmount(int change)
    {
        amount = Mathf.Max(0, amount + change);
    }
}
