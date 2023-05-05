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
            StatusEffectData sed = BattleAnimatorMaster.GetInstance().GetStatusEffectData(status.effectId);
            foreach(BattleAnimation anim in sed.hitAnims)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, anim));
            }
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
