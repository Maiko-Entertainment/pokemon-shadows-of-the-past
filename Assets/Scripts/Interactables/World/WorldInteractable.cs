using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractable : MonoBehaviour
{
    public bool onStep = false;
    public bool activateOnSpawn = false;
    public WorldInteractableMoveBrain moveBrain;


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
        bool isPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isPlaying)
        {
            print(collision.transform.position + " - " + collision.name + " touched " + gameObject.name);
            if (onStep && collision.tag == "Player")
            {
                OnInteract();
            }
            else if (!onStep && collision.tag == "Touch")
            {
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
                OnInteract();
            }
        }
    }
}
