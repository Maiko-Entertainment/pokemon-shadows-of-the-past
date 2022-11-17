using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UtilsMaster
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

    public static void SetSelected(GameObject select)
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(select, new BaseEventData(eventSystem));
    }

    public static IEnumerator SetSelectedNextFrame(GameObject select)
    {
        yield return new WaitForEndOfFrame();
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(select, new BaseEventData(eventSystem));
    }

    public static void GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(
            instance.content.localPosition.x,
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        instance.content.localPosition = result;
    }
}
