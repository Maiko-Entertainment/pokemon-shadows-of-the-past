using UnityEngine;
using Fungus;
[CommandInfo(
    "Save",
    "Load into local variable save element value",
    ""
)]

public class FungusGetSaveElement : Command
{
    public string saveId;
    public override void OnEnter()
    {
        GetFlowchart().SetStringVariable(saveId, SaveMaster.Instance.GetElementAsString(saveId));
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
