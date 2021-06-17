using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerCleanUp : BattleTriggerOnPokemon
{
    public BattleTrigger removeOnLeaveTrigger;
    public bool cleanUpDone = false;
    public BattleTriggerCleanUp(PokemonBattleData pokemon, BattleTrigger removeOnLeaveTrigger) : 
        base(pokemon, false)
    {
        eventId = BattleEventId.pokemonLeaveCleanUp;
        this.removeOnLeaveTrigger = removeOnLeaveTrigger;
    }

    public virtual bool Execute(BattleEventPokemon battleEvent)
    {
        if (battleEvent.pokemon == pokemon)
        {
            BattleMaster.GetInstance().GetCurrentBattle()?
                .RemoveTrigger(removeOnLeaveTrigger);
            cleanUpDone = true;
        }
        return base.Execute(battleEvent);
    }
}
