using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilsMaster : MonoBehaviour
{
    public static List<Selectable> LineSelectables(List<Selectable> selectables)
    {
        for (int i = 0; i < selectables.Count; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            if (i != 0)
            {
                nav.selectOnUp = selectables[i - 1].GetComponent<Selectable>();
            }
            else
            {
                nav.selectOnUp = selectables[selectables.Count - 1].GetComponent<Selectable>();
            }
            if (i != selectables.Count - 1)
            {
                nav.selectOnDown = selectables[i + 1].GetComponent<Selectable>();
            }
            else
            {
                nav.selectOnDown = selectables[0].GetComponent<Button>();
            }
            selectables[i].GetComponent<Selectable>().navigation = nav;
        }
        return selectables;
    }
}
