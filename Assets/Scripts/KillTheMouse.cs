using UnityEngine;
using UnityEngine.EventSystems;

public class KillTheMouse : MonoBehaviour
{
    GameObject lastselect;

    void Start()
    {
        lastselect = new GameObject();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }
}