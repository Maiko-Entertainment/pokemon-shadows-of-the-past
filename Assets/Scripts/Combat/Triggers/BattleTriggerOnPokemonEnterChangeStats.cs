using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonEnterChangeStats : BattleTriggerOnPokemonEnter
{
    PokemonBattleStats statsLevelChange;
    MoveTarget target;
    public BattleTriggerOnPokemonEnterChangeStats(PokemonBattleData pbd, PokemonBattleStats statsLevelChange, MoveTarget target, bool deleteOnLeave = true) : 
        base(pbd, deleteOnLeave)
    {
        this.statsLevelChange = statsLevelChange;
        this.target = target;
    }

    public override bool Execute(BattleEventEnterPokemon battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            if (pokemon == battleEvent.pokemon)
            {
                PokemonBattleData pkmnTarget = bm.GetTarget(pokemon, target);
                bm.AddStatChangeEvent(pkmnTarget, statsLevelChange);
            }
        }
        return base.Execute(battleEvent);
    }

    public override string ToString()
    {
        return base.ToString() + " - ChangeStats - "+ target;
    }
}
