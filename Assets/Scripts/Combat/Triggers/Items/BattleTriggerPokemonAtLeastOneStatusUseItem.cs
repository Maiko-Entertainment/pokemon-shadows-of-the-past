using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerPokemonAtLeastOneStatusUseItem : BattleTriggerOnPokemonGainStatusEffectSuccess
{
    public ItemDataOnPokemon item;
    public List<StatusEffectData> status = new List<StatusEffectData>();
    public BattleTriggerPokemonAtLeastOneStatusUseItem(PokemonBattleData pokemon, ItemDataOnPokemon item) : base(pokemon)
    {
        this.pokemon = pokemon;
        this.item = item;
        status = ((ItemDataOnPokemonRestore) item).statusClears;
    }
    public override bool Execute(BattleEventPokemonStatusAddSuccess battleEvent)
    {

        List<StatusEffect> se = pokemon.GetNonPrimaryStatus().FindAll((se) => status.Contains(se.effectData));
        bool hasPrimary = pokemon.GetCurrentPrimaryStatus() != null && status.Contains(pokemon.GetCurrentPrimaryStatus().effectData);
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
