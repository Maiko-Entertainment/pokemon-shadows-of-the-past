using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonStatusAdd : BattleEventPokemon
{
    public StatusEffectId statusId;
    public BattleEventStatusAddMods mods;
    bool isStatus = false;
    public BattleEventPokemonStatusAdd(PokemonBattleData pokemon, StatusEffectId statusId, bool isStatus=false, BattleEventStatusAddMods mods=null):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonAddStatus;
        this.statusId = statusId;
        this.mods = mods;
        this.isStatus = isStatus;
    }

    public override void Execute()
    {
        base.Execute();
        BattleMaster.GetInstance()?.GetCurrentBattle()?.AddStatusEffect(this, isStatus);
    }
}
