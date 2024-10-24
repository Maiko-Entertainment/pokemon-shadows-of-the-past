using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapLocation : MonoBehaviour
{
    public int mapId = 0;

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
}
