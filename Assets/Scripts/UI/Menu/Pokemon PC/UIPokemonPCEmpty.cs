using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPokemonPCEmpty : MonoBehaviour, ISelectHandler
{
    public delegate void Hover();
    public event Hover onHover;
    public delegate void Click();
    public event Click onClick;

    public void HandleClick()
    {
        onClick?.Invoke();
    }

    public void HandleHover()
    {
        onHover?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleHover();
    }
}
