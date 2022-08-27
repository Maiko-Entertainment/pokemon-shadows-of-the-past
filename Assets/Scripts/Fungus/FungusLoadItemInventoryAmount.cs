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
    public ItemId idItem;
    public string variableName;

    public override void OnEnter()
    {
        ItemInventory item = InventoryMaster.GetInstance().GetItem(idItem);
        if (GetFlowchart().GetVariable(variableName))
            GetFlowchart().SetIntegerVariable(variableName, item.amount);
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return "Load "+idItem+" into "+variableName;
    }
}
