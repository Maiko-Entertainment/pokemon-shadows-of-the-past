using System;
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
        foreach (string name in Enum.GetNames(typeof(ItemCategory)))
        {
            string categoryPath = ResourceMaster.Instance.GetItemCategoryPath(name);
            ItemData[] baseDatas = Resources.LoadAll<ItemData>(categoryPath);
            foreach (ItemData itemData in baseDatas)
            {
                itemDataBase.Add(itemData.GetItemId(), itemData);
            }
        }
    }

    public static ItemMaster GetInstance() { return Instance; }

    public ItemData GetItem(ItemId id)
    {
        if (itemDataBase.ContainsKey(id))
        {
            return itemDataBase[id];
        }
        else
        {
            return null;
        }
    }
}
