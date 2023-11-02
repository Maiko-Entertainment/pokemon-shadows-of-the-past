using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISetTextToVariable : MonoBehaviour
{
    public string name;
    public TextMeshProUGUI text;

    private void Start() {
        if (text) {
            text.text = SaveMaster.Instance.GetElementAsString(name);
        }
    }
}
