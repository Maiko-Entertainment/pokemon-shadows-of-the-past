using UnityEngine;
using Fungus;
[CommandInfo(
    "Save",
    "Load into local variable save element value",
    ""
)]

public class FungusGetSaveElement : Command
{
    public SaveElementId saveId;
    public override void OnEnter()
    {
        SaveElement se = SaveMaster.Instance.GetSaveElementData(saveId);
        GetFlowchart().SetStringVariable(saveId.ToString(), se.ToString());
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
