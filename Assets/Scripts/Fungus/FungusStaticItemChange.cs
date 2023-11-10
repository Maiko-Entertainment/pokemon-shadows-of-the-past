using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusStaticItemChange : Command
{
    public int changeAmount = 1;
    public string idItem;

    public override void OnEnter()
    {
        InventoryMaster.GetInstance().ChangeItemAmount(idItem, changeAmount);
        Continue();
    }
}
