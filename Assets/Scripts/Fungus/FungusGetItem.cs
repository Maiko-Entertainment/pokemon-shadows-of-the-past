using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Add/Remove Item to inventory",
    "Adds item to players inventory or removes it if negative."
)]

public class FungusGetItem : Command
{
    [VariableProperty(typeof(StringVariable))]
    [SerializeField] protected StringVariable itemIdString;

    [VariableProperty(typeof(IntegerVariable))]
    [SerializeField] protected IntegerVariable variableAmount;
    public override void OnEnter()
    {
        InventoryMaster.GetInstance().ChangeItemAmount(itemIdString.Value, variableAmount.Value);
        Continue();
    }
}
