using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleItemCategoryPicker : MonoBehaviour
{
    public ItemCategory itemCategory;
    public UIBattleItemPickerManager manager;

    public void Load(ItemCategory itemCategory)
    {
        this.itemCategory = itemCategory;
    }

    public void OnClick()
    {
        manager?.LoadItemsByCategory(itemCategory);
    }
}
