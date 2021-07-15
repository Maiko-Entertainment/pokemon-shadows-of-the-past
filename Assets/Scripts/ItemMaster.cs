using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : MonoBehaviour
{
    public static ItemMaster Instance;

    public List<ItemDatabaseCategory> itemDatabaseCategory = new List<ItemDatabaseCategory>();
    private Dictionary<ItemId, ItemData> itemDataBase = new Dictionary<ItemId, ItemData>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            InstantiateDatabase();
        }
    }

    public void InstantiateDatabase()
    {
        foreach(ItemDatabaseCategory itemDatabaseCategory in itemDatabaseCategory)
        {
            foreach (ItemData item in itemDatabaseCategory.itemList)
            {
                itemDataBase.Add(item.GetItemId(), item);
            }
        }
    }

    public static ItemMaster GetInstance() { return Instance; }

    public ItemData GetItem(ItemId id)
    {
        return itemDataBase[id];
    }
}
