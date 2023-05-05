using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerPokemonAtLeastOneStatusUseItem : BattleTriggerOnPokemonGainStatusEffectSuccess
{
    public ItemDataOnPokemon item;
    public List<StatusEffectId> status = new List<StatusEffectId>();
    public BattleTriggerPokemonAtLeastOneStatusUseItem(PokemonBattleData pokemon, ItemDataOnPokemon item) : base(pokemon)
    {
        this.pokemon = pokemon;
        this.item = item;
        status = ((ItemDataOnPokemonRestore) item).statusClears;
    }
    public override bool Execute(BattleEventPokemonStatusAddSuccess battleEvent)
    {

        List<StatusEffect> se = pokemon.GetNonPrimaryStatus().FindAll((se) => status.Contains(se.effectId));
        bool hasPrimary = pokemon.GetCurrentPrimaryStatus() != null && status.Contains(pokemon.GetCurrentPrimaryStatus().effectId);
        PokemonBattleData pokemonInEvent = battleEvent.statusEvent.pokemon;
        if (pokemonInEvent.battleId == pokemon.battleId &&
            !pokemonInEvent.IsFainted() &&
            (hasPrimary || se.Count > 0))
        {
            if (maxTriggers > 0)
            {
                BattleMaster.GetInstance().GetCurrentBattle().AddItemPokemonUseEvent(pokemonInEvent, item, true);
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
