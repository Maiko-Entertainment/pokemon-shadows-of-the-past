using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerCleanUp : BattleTriggerOnPokemon
{
    public BattleTrigger removeOnLeaveTrigger;
    public bool cleanUpDone = false;

    public delegate void OnExecute(BattleEventPokemon battleEvent);
    public OnExecute onExecute;

    public BattleTriggerCleanUp(PokemonBattleData pokemon) :
        base(pokemon, false)
    {
        eventId = BattleEventId.pokemonLeaveCleanUp;
    }

    public BattleTriggerCleanUp(PokemonBattleData pokemon, BattleTrigger removeOnLeaveTrigger) : 
        base(pokemon, false)
    {
        eventId = BattleEventId.pokemonLeaveCleanUp;
        this.removeOnLeaveTrigger = removeOnLeaveTrigger;
    }

    public override bool Execute(BattleEventPokemon battleEvent)
    {
        if (battleEvent.pokemon == pokemon)
        {
            if (removeOnLeaveTrigger != null)
            {
                BattleMaster.GetInstance().GetCurrentBattle()?
                    .RemoveTrigger(removeOnLeaveTrigger);
            }
            cleanUpDone = true;
            onExecute?.Invoke(battleEvent);
        }
        return base.Execute(battleEvent);
    }
}
