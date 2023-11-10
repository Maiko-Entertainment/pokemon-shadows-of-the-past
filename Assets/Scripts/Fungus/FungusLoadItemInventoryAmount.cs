using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo(
    "Save",
    "Load Item Inventory into variable INT",
    ""
)]
public class FungusLoadItemInventoryAmount : Command
{
    public ItemData item;
    [VariableProperty(typeof(IntegerVariable))]
    [SerializeField] protected IntegerVariable itemAmountVariable;

    public override void OnEnter()
    {
        ItemInventory itemInv = InventoryMaster.GetInstance().GetItem(item.GetItemId());
        GetFlowchart().SetIntegerVariable(itemAmountVariable.Key, itemInv.GetAmount());
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return "Load "+item?.GetName()+" into "+ itemAmountVariable?.Key;
    }
}
