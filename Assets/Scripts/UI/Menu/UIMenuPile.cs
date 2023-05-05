using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuPile : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TransitionCanvasGroup transition;
    public GameObject firstSelected;
    public Transform optionsList;
    public bool fadeOutWhenNotFocused = false;

    public delegate void OnActivateMenu();
    public event OnActivateMenu onActivateMenu;

    public void Open()
    {
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
        HandleFirstSelected();
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
        if (fadeOutWhenNotFocused)
        {
            transition.FadeOutTemporary();
        }
    }
    public void ReactivateMenu()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        if (fadeOutWhenNotFocused)
        {
            transition.FadeIn();
        }
        HandleFirstSelected();
    }

    public void HandleFirstSelected()
    {
        if (firstSelected)
        {
            UtilsMaster.SetSelected(firstSelected);
        }
        else if (optionsList)
        {
            if (optionsList.childCount > 0)
            {
                UtilsMaster.SetSelected(optionsList.GetChild(0).gameObject);
            }
        }
        onActivateMenu?.Invoke();
    }

}
