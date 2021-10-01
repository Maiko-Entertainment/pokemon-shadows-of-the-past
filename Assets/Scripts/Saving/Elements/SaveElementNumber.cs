using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Save/Save elements/Number")]
public class SaveElementNumber : SaveElement
{
    public float defaultValue = 0;

    public override object GetValue()
    {
        PersistedSaveElement se = SaveMaster.Instance.GetSaveElementInner(id);
        if (se != null)
        {
            float value = (float)se.value;
            return value;
        }
        return defaultValue;
    }
    public override SaveValueType GetValueType()
    {
        return SaveValueType.number;
    }
}
