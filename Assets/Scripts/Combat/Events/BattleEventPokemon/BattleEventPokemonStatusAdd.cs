using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonStatusAdd : BattleEventPokemon
{
    public StatusEffectId statusId;
    public BattleEventStatusAddMods mods;
    public BattleEventPokemonStatusAdd(PokemonBattleData pokemon, StatusEffectId statusId, BattleEventStatusAddMods mods=null):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonAddStatus;
        this.statusId = statusId;
        this.mods = mods;
    }

    public override void Execute()
    {
        base.Execute();
        BattleMaster.GetInstance()?.GetCurrentBattle()?.AddStatusEffect(this);
    }
}
