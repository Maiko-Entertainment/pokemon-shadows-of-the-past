using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventPokemonBattle : InteractionEvent
{
    public WorldInteractablePokemonEncounter worldEncounter;

    public InteractionEventPokemonBattle(WorldInteractablePokemonEncounter worldEncounter)
    {
        this.worldEncounter = worldEncounter;
    }

    public override void Execute()
    {
        base.Execute();
        worldEncounter?.Execute();
    }
}
