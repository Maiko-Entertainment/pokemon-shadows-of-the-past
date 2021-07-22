using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventPokemonBattle : InteractionEvent
{
    public SceneInteractablePokemonEncounter scene;
    public InteractionEventPokemonBattle(SceneInteractablePokemonEncounter scene)
    {
        this.scene = scene;
    }

    public override void Execute()
    {
        base.Execute();
        scene.Execute();
    }
}
