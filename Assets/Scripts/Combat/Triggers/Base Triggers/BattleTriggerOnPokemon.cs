using Fungus;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemon : BattleTrigger
{
    public PokemonBattleData pokemon;

    public List<BattleAnimation> animations = new List<BattleAnimation>();
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
        return base.Execute(battleEvent);
    }

    public override string ToString()
    {
        return ""+pokemon?.pokemon?.GetName();
    }
}
