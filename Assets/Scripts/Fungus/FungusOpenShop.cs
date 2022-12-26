using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Menu",
    "Open Shop Menu",
    "Opens the shop menu with. Recieves a list of items to sell"
)]
public class FungusOpenShop : Command
{
    public List<ItemData> sellableItems = new List<ItemData>();

    public override void Execute()
    {
        UIPauseMenuMaster.GetInstance().OpenShopMenu(sellableItems);
        Continue();
    }
}
