using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnMoveParalyzedCancel : BattleTriggerOnPokemonMove
{
    public StatusEffectParalyzed status;
    public BattleTriggerOnMoveParalyzedCancel(PokemonBattleData pokemon, StatusEffectParalyzed status) : base(pokemon, new UseMoveMods(PokemonTypeId.Unmodify), true)
    {
        this.status = status;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        float random = Random.value;
        
        if (pokemon == battleEvent.pokemon && random < status.chance)
        {
            Flowchart flowchart = BattleAnimatorMaster.GetInstance().battleFlowchart;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(
                    flowchart,
                    status.onWarningFlowchartBlock,
                    new Dictionary<string, string> { { "pokemon", pokemon.GetName() } }
                )));
            return false;
        }
        return base.Execute(battleEvent);
    }
}
