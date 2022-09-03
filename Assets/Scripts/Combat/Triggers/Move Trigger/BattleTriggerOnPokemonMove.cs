using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMove : BattleTriggerOnPokemon
{
    protected UseMoveMods useMoveMods;
    public BattleTriggerOnPokemonMove(PokemonBattleData pokemon, UseMoveMods useMoveMods, bool deleteOnLeave) : base(pokemon, deleteOnLeave)
    {
        this.useMoveMods = useMoveMods;
        eventId = BattleEventId.pokemonUseMove;
    }

    public virtual bool Execute(BattleEventUseMove battleEvent)
    {
        if (battleEvent.pokemon == pokemon && maxTriggers > 0 && useMoveMods != null)
        {
            battleEvent.moveMods.Implement(useMoveMods);
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
