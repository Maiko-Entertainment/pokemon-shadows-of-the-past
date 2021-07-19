using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneInteractable : Clickable2D
{

    public virtual void TryToInteract()
    {
        if (CanInteract())
        {
            Interact();
        }
    }

    public virtual void Interact()
    {

    }

    public virtual bool CanInteract()
    {
        bool isInteractionFree = !InteractionsMaster.GetInstance().IsInteractionPlaying();
        return isInteractionFree;
    }

    protected override void DoPointerClick()
    {
        TryToInteract();
    }
}
