using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Get Item",
    "Adds item to players inventory."
)]

public class FungusGetItem : Command
{
    public string variableItemId = "itemId";
    public string variableNameAmount = "amount";
    public override void OnEnter()
    {
        ItemId item = (ItemId)GetFlowchart().GetIntegerVariable(variableItemId);
        int amount = GetFlowchart().GetIntegerVariable(variableNameAmount);
        InventoryMaster.GetInstance().ChangeItemAmount(item, amount);
        Continue();
    }
}
