using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISetTextToVariable : MonoBehaviour
{
    public SaveElementId id;
    public TextMeshProUGUI text;

    private void Start()
    {
        if (text)
        {
            SaveElementNumber se = (SaveElementNumber) SaveMaster.Instance.GetSaveElementData(id);
            text.text = ""+se.GetValue();
        }
    }
}
