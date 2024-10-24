using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonChangeStatsModify : BattleTriggerOnPokemonChangeStats
{
    public StatsChangeModifyInstructions instructions;
    public bool showAbility = false;
    public ItemDataOnPokemon relatedItem = null;
    public BattleTriggerOnPokemonChangeStatsModify(PokemonBattleData pokemon, StatsChangeModifyInstructions instructions, bool showAbility) :
        base(pokemon, true)
    {
        this.instructions = instructions;
        this.showAbility = showAbility;
        eventId = BattleEventId.pokemonChangeStats;
    }

    public override bool Execute(BattleEventPokemonChangeStat battleEvent)
    {
        if (battleEvent.pokemon.battleId == pokemon.battleId && maxTriggers > 0 && instructions.IsApplicable(battleEvent))
        {
            if (showAbility)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(battleEvent.pokemon));
            }
            if (relatedItem)
            {
                BattleMaster.GetInstance().GetCurrentBattle().AddItemPokemonUseEvent(battleEvent.pokemon, (ItemDataOnPokemon)relatedItem, true);
            }
            battleEvent = instructions.Apply(battleEvent);
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
