using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Save/Save elements/Text")]
public class SaveElementText : SaveElement
{
    public string defaultValue = "";

    public override object GetValue()
    {
        PersistedSaveElement se = SaveMaster.Instance.GetSaveElementFromSavefile(GetId());
        if (se != null)
        {
            string value = se.value.ToString();
            return value;
        }
        return defaultValue;
    }
    public override SaveValueType GetValueType()
    {
        return SaveValueType.text;
    }
    public override void SetValue(object newValue)
    {
        SaveMaster.Instance.SetSaveElement(""+newValue, GetId());
    }
}
