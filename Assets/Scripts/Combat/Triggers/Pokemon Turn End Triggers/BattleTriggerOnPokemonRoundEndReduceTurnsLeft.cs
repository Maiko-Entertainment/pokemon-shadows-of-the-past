using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonRoundEndReduceTurnsLeft : BattleTriggerOnPokemonRoundEnd
{
    public StatusEffect status;

    public BattleTriggerOnPokemonRoundEndReduceTurnsLeft(
        PokemonBattleData pokemon,
        StatusEffect status) :
        base(pokemon)
    {
        this.status = status;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        status.PassTurn();
        return base.Execute(battleEvent);
    }

    public override string ToString()
    {
        return base.ToString() + " - " + status.ToString();
    }
}
