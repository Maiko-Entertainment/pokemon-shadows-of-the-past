using Fungus;
using UnityEngine;
[CommandInfo(
    "Save",
    "Change a save element to a set value or +/-",
    "If saveIdString is empty then it uses saveElement, if it's empty it uses variableId"
)]
public class FungusSetSaveElement : Command
{
    public string saveIdString = "";
    public SaveElementNumber saveElement;
    public SaveElementId variableId;
    public VariableOperationType operation;
    public float value = 1;

    public string GetId()
    {
        if (string.IsNullOrEmpty(saveIdString))
        {
            if (saveElement != null)
            {
                return saveElement?.GetId();
            }
            return variableId.ToString();
        }
        return saveIdString;
    }

    public override void OnEnter()
    {
        string varId = GetId();
        float val = SaveMaster.Instance.GetSaveElementFloat(varId);
        switch (operation)
        {
            case VariableOperationType.change:
                float newValue = val + value;
                SaveMaster.Instance.SetSaveElement(newValue, varId);
                break;
            default:
                SaveMaster.Instance.SetSaveElement(value, varId);
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
        return base.GetSummary() + " - " + GetId() + " -> "+operation+" "+value;
    }
}
