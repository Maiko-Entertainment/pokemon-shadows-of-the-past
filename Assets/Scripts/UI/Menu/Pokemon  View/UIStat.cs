using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI value;

    public Color defaultColor;
    public Color boostedColor;

    public UIStat Load(string title, string value, bool isBoosted = false)
    {
        this.title.text = title;
        this.value.text = value;
        if (isBoosted)
        {
            this.value.color = boostedColor;
            this.title.color = boostedColor;
        }
        else
        {
            this.value.color = defaultColor;
            this.title.color = defaultColor;
        }
        return this;
    }
}
