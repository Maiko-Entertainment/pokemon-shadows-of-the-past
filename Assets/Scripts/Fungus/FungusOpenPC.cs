using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Menu",
    "Open PC",
    "Opens the PC to exchange stored pokemon"
)]
public class FungusOpenPC : Command
{
    public override void Execute()
    {
        UIPauseMenuMaster.GetInstance().OpenPcMenu();
        Continue();
    }
}
