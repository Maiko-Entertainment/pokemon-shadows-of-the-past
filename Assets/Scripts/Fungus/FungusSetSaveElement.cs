using Fungus;
using UnityEngine;
[CommandInfo(
    "Save",
    "Change a save element to a set value or +/-",
    ""
)]
public class FungusSetSaveElement : Command
{
    public SaveElementId variableId;
    public VariableOperationType operation;
    public float value = 1;

    public override void OnEnter()
    {
        SaveElement se = SaveMaster.Instance.GetSaveElement(variableId);
        SaveElementNumber sen = (SaveElementNumber)se;
        switch (operation)
        {
            case VariableOperationType.change:
                float newValue = (float)sen.GetValue() + value;
                sen.SetValue(newValue);
                break;
            default:
                sen.SetValue(value);
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
        return base.GetSummary() + " - " + variableId.ToString() + " -> "+operation+" "+value;
    }
}
