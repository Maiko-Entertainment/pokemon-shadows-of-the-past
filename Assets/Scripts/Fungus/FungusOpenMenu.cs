using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Menu",
    "Opens a menu",
    ""
)]
public class FungusOpenMenu : Command
{
    public UIMenuPile menuPile;
    public bool playDefaultSound;
    public override void OnEnter()
    {
        Continue();
        if (menuPile)
            UIPauseMenuMaster.GetInstance().OpenMenu(menuPile, true, playDefaultSound);
    }
    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary();
    }
}
