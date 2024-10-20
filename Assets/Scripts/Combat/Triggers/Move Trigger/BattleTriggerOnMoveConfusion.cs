using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnMoveConfusion : BattleTriggerOnPokemonMove
{
    public StatusEffectConfusion status;
    float selfHitChange = 0.333333f;
    public BattleTriggerOnMoveConfusion(PokemonBattleData pokemon, StatusEffectConfusion status): base(pokemon, new UseMoveMods(PokemonTypeId.Unmodify), true)
    {
        this.status = status;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        if (pokemon == battleEvent.pokemon)
        {
            Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(
                battleFlow,
                "Confusion Warning",
                new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } }))
            );
            StatusEffectData sed = BattleAnimatorMaster.GetInstance().GetStatusEffectData(status.effectId);
            foreach (BattleAnimation anim in sed.hitAnims)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, anim));
            }
            float random = Random.value;
            if (random < selfHitChange)
            {
                MoveData move = MovesMaster.Instance.GetMove(MoveId.SelfHit);
                battleEvent.move = move;
                battleEvent.moveMods.moveTypeId = PokemonTypeId.Undefined;
            }
        }
        return base.Execute(battleEvent);
    }
}
