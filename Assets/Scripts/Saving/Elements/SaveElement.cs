using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveElement : ScriptableObject
{
    public SaveElementId id;
    public string elementName;

    public virtual object GetValue()
    {
        return null;
    }

    public virtual void SetValue(object newValue)
    {
        SaveMaster.Instance.SetSaveElementInner(newValue, id);
    }

    public virtual SaveValueType GetValueType()
    {
        return SaveValueType.number;
    }
}
