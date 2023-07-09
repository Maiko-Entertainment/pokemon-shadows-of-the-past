using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Combat",
    "Check if battle is already over",
    "if the battle end event was already fired it sets true to the variable."
)]
public class FungusIsBattleOver : Command
{
    public string variableToLoadTo = "isBattleOver";
    public override void OnEnter()
    {
        bool isOver = BattleMaster.GetInstance().GetCurrentBattle().IsBattleSetToOver();
        if (GetFlowchart().GetVariable(variableToLoadTo)) GetFlowchart().SetBooleanVariable(variableToLoadTo, isOver);
        Continue();
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
