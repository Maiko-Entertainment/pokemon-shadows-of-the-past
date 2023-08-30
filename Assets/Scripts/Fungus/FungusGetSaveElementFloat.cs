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
    public SaveElementId saveId;
    public override void OnEnter()
    {
        SaveElement se = SaveMaster.Instance.GetSaveElementData(saveId);
        GetFlowchart().SetFloatVariable(saveId.ToString(), (float)se.GetValue());
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
