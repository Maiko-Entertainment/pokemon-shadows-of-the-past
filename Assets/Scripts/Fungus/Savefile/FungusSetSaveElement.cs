using Fungus;
using UnityEngine;
[CommandInfo(
    "Save",
    "Change a save element to a set value or +/-",
    ""
)]
public class FungusSetSaveElement : Command
{
    public string saveIdString = "";
    public SaveElementId variableId;
    public VariableOperationType operation;
    public float value = 1;

    public override void OnEnter()
    {
        string varname = string.IsNullOrEmpty(saveIdString) ? variableId.ToString() : saveIdString;
        float val = SaveMaster.Instance.GetSaveElementFloat(varname);
        switch (operation)
        {
            case VariableOperationType.change:
                float newValue = val + value;
                SaveMaster.Instance.SetSaveElement(newValue, varname);
                break;
            default:
                SaveMaster.Instance.SetSaveElement(value, varname);
                break;
        }
        Continue();
    }
    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary() + " - " + (string.IsNullOrEmpty(saveIdString) ? variableId.ToString() : saveIdString) + " -> "+operation+" "+value;
    }
}
