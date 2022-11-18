using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableKey : WorldInteractable
{
    public float delayBetweenPresses = 1f;
    public bool interacted = false;

    private float timeLeft = 0;
    virtual public bool CanInteract()
    {
        bool isInteracting = InteractionsMaster.GetInstance().IsInteractionPlaying();
        return !isInteracting && !interacted;
    }

    public void MarkInteracted()
    {
        interacted = true;
        timeLeft = delayBetweenPresses;
    }

    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            interacted = false;
        }
    }
}
