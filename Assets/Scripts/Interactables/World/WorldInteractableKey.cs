using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableKey : WorldInteractable
{
    // Start is called before the first frame update
    virtual public bool CanInteract()
    {
        bool isInteracting = InteractionsMaster.GetInstance().IsInteractionPlaying();
        return !isInteracting;
    }
}
