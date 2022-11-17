using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBurn : StatusEffect
{
    public float porcentualDamage = 0.0625f;
    public StatusEffectBurn(PokemonBattleData pokemon): base(pokemon)
    {
        effectId = StatusEffectId.Burn;
        isPrimary = true;
        minTurns = 99999;
        captureRateBonus = 10;
        inmuneTypes.Add(PokemonTypeId.Fire);
    }

    public override void Initiate()
    {
        Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
                    pokemon,
                    new DamageSummary(
                        PokemonTypeId.Undefined,
                        Mathf.Max(1, (int)(pokemon.GetPokemonHealth() * porcentualDamage)),
                        DamageSummarySource.Status,
                        (int)effectId
                    )
                );

        List<BattleAnimation> animations = BattleAnimatorMaster.GetInstance().GetStatusEffectData(effectId).hitAnims;
        BattleTrigger animTrigger = new BattleTriggerRoundEndAnimations(
                       pokemon,
                       pokemon,
                       animations
                   );
        
        BattleTrigger messageTrigger = new BattleTriggerOnPokemonTurnEndMessage(
                    pokemon,
                    new BattleTriggerMessageData(
                        battleFlow,
                        "Burn Damage",
                        new Dictionary<string, string>()
                        {
                            { "pokemon", pokemon.GetName() }
                        }
                    )
                );
        BattleTriggerOnPokemonBurn triggerMove = new BattleTriggerOnPokemonBurn(
            pokemon,
            MoveCategoryId.physical
           );
        // Needs trigger to reduce physical attack damage
        battleTriggers.Add(messageTrigger);
        battleTriggers.Add(animTrigger);
        battleTriggers.Add(statusTrigger);
        battleTriggers.Add(triggerMove);
        base.Initiate();
    }
}
