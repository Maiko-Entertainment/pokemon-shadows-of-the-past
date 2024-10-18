using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UtilsMaster
{
    public static bool IsEnabled(Selectable sel)
    {
        return sel.interactable;
    }

    public static List<Selectable> LineSelectables(List<Selectable> selectables)
    {
        for (int i = 0; i < selectables.Count; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            if (i != 0)
            {
                Selectable target = selectables[i - 1];
                nav.selectOnUp = target.GetComponent<Selectable>();
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
    public static List<Selectable> GridSelectables(List<Selectable> selectables, int rowSize)
    {
        int rows = Mathf.CeilToInt(selectables.Count / (float)rowSize);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < rowSize && selectables.Count > (r * rowSize + c); c++)
            {
                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.Explicit;
                // Left
                if (c > 0)
                {
                    nav.selectOnLeft = selectables[r * rowSize + c - 1].GetComponent<Selectable>();
                }
                // Right
                if (c < rowSize - 1 && selectables.Count > (r * rowSize + c + 1))
                {
                    nav.selectOnRight = selectables[r * rowSize + c + 1].GetComponent<Selectable>();
                }
                // Down
                if (r < (selectables.Count / (float)rowSize - 1) && selectables.Count - 1 >= ((r + 1) * rowSize + c))
                {
                    nav.selectOnDown = selectables[(r + 1) * rowSize + c].GetComponent<Selectable>();
                }
                // Up
                if (r > 0)
                {
                    nav.selectOnUp = selectables[(r - 1) * rowSize + c].GetComponent<Selectable>();
                }
                selectables[r * rowSize + c].GetComponent<Selectable>().navigation = nav;
            }
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

    public static bool ContainsAtLeastOne<T>(List<T> array, List<T> array2)
    {
        foreach(T t in array2)
        {
            if (array.Contains(t))
            {
                return true;
            }
        }
        return false;
    }
}
