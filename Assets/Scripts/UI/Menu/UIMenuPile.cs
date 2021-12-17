using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuPile : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TransitionCanvasGroup transition;
    public GameObject firstSelected;

    public void Open()
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstSelected, new BaseEventData(eventSystem));
        if (transition)
        {
            transition.FadeIn();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        if (transition)
        {
            transition.FadeOut();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeactivateMenu()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void ReactivateMenu()
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstSelected, new BaseEventData(eventSystem));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

}
