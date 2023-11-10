﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : MonoBehaviour
{
    public static ItemMaster Instance;

    private Dictionary<string, ItemData> itemDataBase = new Dictionary<string, ItemData>();

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
        string categoryPath = ResourceMaster.Instance.GetItemCategoryPath();
        ItemData[] baseDatas = Resources.LoadAll<ItemData>(categoryPath);
        foreach (ItemData itemData in baseDatas)
        {
            itemDataBase.Add(itemData.GetItemId(), itemData);
        }
    }

    public static ItemMaster GetInstance() { return Instance; }

    public ItemData GetItem(string id)
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
