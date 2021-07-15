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
