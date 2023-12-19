using Fungus;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemon : BattleTrigger
{
    public PokemonBattleData pokemon;

    public List<BattleAnimationPokemon> animations = new List<BattleAnimationPokemon>();
    public Flowchart flowchart;
    public string blockName = string.Empty;

    public BattleTriggerOnPokemon(PokemonBattleData pokemon, bool deleteOnLeave)
    {
        this.pokemon = pokemon;
        if (deleteOnLeave)
        {
            BattleMaster.GetInstance().GetCurrentBattle()
            .AddTrigger(new BattleTriggerCleanUp(pokemon, this));
        }
    }

    public virtual bool Execute(BattleEventPokemon battleEvent)
    {
        return base.TryToExecute(battleEvent);
    }

    public override string ToString()
    {
        return ""+pokemon?.pokemon?.GetName();
    }

    public override bool MeetsConditions(BattleEvent ev)
    {
        BattleEventPokemon pokeEvent = ev as BattleEventPokemon;
        if ( pokeEvent != null && pokeEvent.pokemon.battleId == pokemon.battleId)
        {
            return base.MeetsConditions(ev);
        }
        return false;
    }
}
