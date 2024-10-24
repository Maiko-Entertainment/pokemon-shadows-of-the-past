using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventPokemonBattle : InteractionEvent
{
    public WorldInteractablePokemonEncounter worldEncounter;
    public WorldInteractablePokemonEncounter2 worldEncounter2;


    public InteractionEventPokemonBattle(WorldInteractablePokemonEncounter worldEncounter)
    {
        this.worldEncounter = worldEncounter;
    }
    public InteractionEventPokemonBattle(WorldInteractablePokemonEncounter2 worldEncounter2)
    {
        this.worldEncounter2 = worldEncounter2;
    }

    public override void Execute()
    {
        base.Execute();
        worldEncounter?.Execute();
        worldEncounter2?.Execute();
    }
}
