using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerEndOfRoundUseItem : BattleTriggerOnPokemonRoundEnd
{
    public ItemDataOnPokemon item;
    public BattleTriggerEndOfRoundUseItem(PokemonBattleData pkmn, ItemDataOnPokemon item):
        base(pkmn)
    {
        this.item = item;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        if (pokemon.GetPokemonCurrentHealth() > 0)
        {
            BattleMaster.GetInstance().GetCurrentBattle()?.AddItemPokemonUseEvent(pokemon, item, true);
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
