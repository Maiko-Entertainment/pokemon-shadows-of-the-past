using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerPokemonHalfHealthUseItem : BattleTriggerOnPokemonDamage
{
    public ItemData item;
    public BattleTriggerPokemonHalfHealthUseItem(PokemonBattleData pokemon, ItemData item, bool deleteOnLeave=true): base(pokemon, deleteOnLeave)
    {
        this.pokemon = pokemon;
        this.item = item;
    }

    public override bool Execute(BattleEventTakeDamageSuccess battleEvent)
    {
        if (battleEvent.pokemon == pokemon &&
            !battleEvent.pokemon.IsFainted() &&
            battleEvent.damageEvent.resultingHealth / (float)battleEvent.pokemon.GetMaxHealth() <= 0.5f)
        {
            if (maxTriggers > 0)
            {
                BattleMaster.GetInstance().GetCurrentBattle().AddItemPokemonUseEvent(battleEvent.pokemon, (ItemDataOnPokemon) item, true);
                battleEvent.pokemon.pokemon.equippedItem = null;
            }
        }
        else
        {
            // Adds a trigger so that it compensates the -1 trigger in the base execute
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
