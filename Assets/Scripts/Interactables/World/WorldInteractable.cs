using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractable : MonoBehaviour
{
    public bool onStep = false;
    public bool activateOnSpawn = false;
    public WorldInteractableMoveBrain moveBrain;

    protected bool isPlayerInside = false;

    private void Start()
    {
        if (activateOnSpawn)
            OnInteract();
    }

    public virtual void OnInteract()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTriggerEnter(collision);
    }

    public virtual void HandleTriggerEnter(Collider2D collision)
    {
        bool isPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isPlaying)
        {
            if (onStep && collision.tag == "Player")
            {
                isPlayerInside = true;
                OnInteract();
            }
            else if (!onStep && collision.tag == "Touch")
            {
                isPlayerInside = true;
                OnInteract();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isPlaying)
        {
            if (!onStep && collision.collider.tag == "Touch")
            {
                isPlayerInside = true;
                OnInteract();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleTriggerExit(collision);
    }
    public virtual void HandleTriggerExit(Collider2D collision)
    {
        if (onStep && collision.tag == "Player")
        {
            isPlayerInside = false;
        }
        else if (!onStep && collision.tag == "Touch")
        {
            isPlayerInside = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!onStep && collision.collider.tag == "Touch")
        {
            isPlayerInside = false;
        }
    }
}
