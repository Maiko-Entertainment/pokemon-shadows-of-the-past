using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveElement : ScriptableObject
{
    public SaveElementId idLegacy;
    [SerializeField] protected string id;
    public string elementName;
    [TextArea] public string descriptionHelp;

    public string GetId()
    {
        return string.IsNullOrEmpty(id) ? idLegacy.ToString() : id;
    }

    public virtual object GetValue()
    {
        return null;
    }

    public virtual void SetValue(object newValue)
    {
        SaveMaster.Instance.SetSaveElementInner(newValue, GetId());
    }

    public virtual SaveValueType GetValueType()
    {
        return SaveValueType.number;
    }
}
