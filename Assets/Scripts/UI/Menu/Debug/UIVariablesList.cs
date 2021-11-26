using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVariablesList : MonoBehaviour
{
    public UISetVariable variablePreafb;
    public Transform variableList;

    public void LoadVaraibles()
    {
        foreach (Transform e in variableList)
        {
            Destroy(e.gameObject);
        }
        List<SaveElement> elements = SaveMaster.Instance.saveElements;
        foreach (SaveElement e in elements)
        {
            Instantiate(variablePreafb, variableList).Load(e.id);
        }
    }

    private void OnEnable()
    {
        LoadVaraibles();
    }
}
