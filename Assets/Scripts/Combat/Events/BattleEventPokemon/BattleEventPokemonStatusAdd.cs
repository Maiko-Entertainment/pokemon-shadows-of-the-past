using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonStatusAdd : BattleEventPokemon
{
    public StatusEffectData status;
    public BattleEventStatusAddMods mods;

    // Possible Sources
    public MoveData moveSource;

    bool isStatus = false;
    public BattleEventPokemonStatusAdd(PokemonBattleData pokemon, StatusEffectData status, bool isStatus=false, BattleEventStatusAddMods mods=null):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonAddStatus;
        this.status = status;
        this.mods = mods;
        this.isStatus = isStatus;
    }

    public override void Execute()
    {
        base.Execute();
        BattleMaster.GetInstance()?.GetCurrentBattle()?.AddStatusEffect(this, isStatus);
    }
}
