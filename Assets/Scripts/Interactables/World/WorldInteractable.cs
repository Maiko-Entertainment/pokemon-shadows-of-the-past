using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractable : MonoBehaviour
{
    public bool onStep = false;
    public virtual void OnInteract()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onStep && collision.tag == "Player")
        {
            OnInteract();
        }
    }
}
