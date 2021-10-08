using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMove : BattleTriggerOnPokemon
{
    protected UseMoveMods useMoveMods;
    public BattleTriggerOnPokemonMove(PokemonBattleData pokemon, UseMoveMods useMoveMods, bool deleteOnLeave) : base(pokemon, deleteOnLeave)
    {
        this.useMoveMods = useMoveMods;
    }

    public virtual bool Execute(BattleEventUseMove battleEvent)
    {
        battleEvent.moveMods.Implement(useMoveMods);
        return base.Execute(battleEvent);
    }
}
