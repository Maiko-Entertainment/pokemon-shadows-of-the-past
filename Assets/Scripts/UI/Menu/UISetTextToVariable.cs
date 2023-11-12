using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISetTextToVariable : MonoBehaviour
{
    public SaveElement saveElement;
    public TextMeshProUGUI text;

    private void Start()
    {
        if (text)
        {
            object value = SaveMaster.Instance.GetSaveElement(saveElement.GetId());
            text.text = value.ToString();
        }
    }
}
