using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Save",
    "Load into local variable save element FLOAT value",
    ""
)]

public class FungusGetSaveElementFloat : Command
{
    public string saveId;
    public override void OnEnter()
    {
        GetFlowchart().SetFloatVariable(saveId, SaveMaster.Instance.GetElementAsFloat(saveId));
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
