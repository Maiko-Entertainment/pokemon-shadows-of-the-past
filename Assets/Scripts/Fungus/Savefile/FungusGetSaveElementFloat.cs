using Fungus;
using MoonSharp.VsCodeDebugger.SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo(
    "Save",
    "Load into local variable save element FLOAT value",
    "if the string id is not provided the reference to the save element will be used."
)]

public class FungusGetSaveElementFloat : Command
{
    public string saveIdString = "";
    public SaveElementNumber saveElement;

    public string GetId()
    {
        return string.IsNullOrEmpty(saveIdString) ? saveElement?.GetId().ToString() : saveIdString;
    }

    public override void OnEnter()
    {
        string saveIdFinal = GetId();
        float value = SaveMaster.Instance.GetSaveElementFloat(saveIdFinal);
        GetFlowchart().SetFloatVariable(saveIdFinal, value);
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary() + " -> " + GetId();
    }
}
