using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Save/Save elements/Text")]
public class SaveElementText : SaveElement
{
    public string defaultValue = "";

    public override object GetValue()
    {
        PersistedSaveElement se = SaveMaster.Instance.GetSaveElementInner(id);
        if (se != null)
        {
            string value = (string)se.value;
            return value;
        }
        return defaultValue;
    }
    public override SaveValueType GetValueType()
    {
        return SaveValueType.text;
    }
}
