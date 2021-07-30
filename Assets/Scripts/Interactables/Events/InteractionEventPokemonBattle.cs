using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventPokemonBattle : InteractionEvent
{
    public SceneInteractablePokemonEncounter scene;
    public WorldInteractablePokemonEncounter worldEncounter;
    public InteractionEventPokemonBattle(SceneInteractablePokemonEncounter scene)
    {
        this.scene = scene;
    }

    public InteractionEventPokemonBattle(WorldInteractablePokemonEncounter worldEncounter)
    {
        this.worldEncounter = worldEncounter;
    }

    public override void Execute()
    {
        base.Execute();
        scene?.Execute();
        worldEncounter?.Execute();
    }
}
