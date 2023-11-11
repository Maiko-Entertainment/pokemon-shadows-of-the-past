using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveElement : ScriptableObject
{
    [SerializeField] protected string id;
    public string elementName;
    [TextArea] public string descriptionHelp;

    public string GetId()
    {
        return id;
    }

    public virtual object GetValue()
    {
        return null;
    }

    public virtual void SetValue(object newValue)
    {
        SaveMaster.Instance.SetSaveElement(newValue, GetId());
    }

    public virtual SaveValueType GetValueType()
    {
        return SaveValueType.number;
    }
}
