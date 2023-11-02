using Fungus;
using UnityEngine;
[CommandInfo(
    "Save",
    "Change a save element to a set value or +/-",
    ""
)]
public class FungusSetSaveElement : Command
{
    public string variableId;
    public VariableOperationType operation;
    public int value = 1;

    public override void OnEnter()
    {
        int se = SaveMaster.Instance.GetElementAsInt(variableId);
        switch (operation)
        {
            case VariableOperationType.change:
                SaveMaster.Instance.SaveElement(variableId, se + value);
                break;
            default:
                SaveMaster.Instance.SaveElement(variableId, value);
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
        return base.GetSummary() + " - " + variableId + " -> "+operation+" "+value;
    }
}
