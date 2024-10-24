using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractablePlayParticleOnTouch : WorldInteractable
{
    public ParticleSystem ParticleSystem;
    public override void OnInteract()
    {
        base.OnInteract();
        ParticleSystem?.Play();
    }
}
