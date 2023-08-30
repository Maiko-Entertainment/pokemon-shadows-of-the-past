using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISetVariable : MonoBehaviour
{
    public SaveElementId element;
    public TextMeshProUGUI varName;
    public TMP_InputField inputField;

    private void Start()
    {
        Load(element);
    }

    public UISetVariable Load(SaveElementId element)
    {
        this.element = element;
        SaveElement se = SaveMaster.Instance.GetSaveElementData(element);
        varName.text = se.elementName;
        inputField.text = se.GetValue().ToString();
        return this;
    }

    public void SetVariable(string value)
    {
        SaveElement se = SaveMaster.Instance.GetSaveElementData(element);
        switch (se.GetValueType())
        {
            case SaveValueType.number:
                se.SetValue(float.Parse(value));
                break;
            default:
                se.SetValue(value);
                break;
        }
    }
}
