using UnityEngine;
using Fungus;
[CommandInfo(
    "Save",
    "Load into local variable save element value that's string",
    ""
)]

public class FungusGetSaveElement : Command
{
    public string saveId;
    public SaveElementText saveElement;

    public string GetId()
    {
        return string.IsNullOrEmpty(saveId) ? saveElement?.GetId() : saveId;
    }

    public override void OnEnter()
    {
        string se = SaveMaster.Instance.GetSaveElementString(GetId());
        GetFlowchart().SetStringVariable(GetId(), se);
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
