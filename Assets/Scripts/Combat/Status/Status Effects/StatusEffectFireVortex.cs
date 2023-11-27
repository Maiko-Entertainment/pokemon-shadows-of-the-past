using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFireVortex : StatusEffect
{
    public float porcentualDamage = 0.125f;
    
    public StatusEffectFireVortex(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        effectId = StatusEffectId.FireVortex;
        minTurns = 4;
        addedRangeTurns = 1;
        // CaptureRateBonus = 10;
        InmuneTypes.Add(TypesMaster.Instance.GetTypeData("Fire"));
        stopEscape = true;
        gainStatusBlockName = "Fire Vortex Gain";
    }

    public override void Initiate()
    {
        Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
                    pokemon,
                    new DamageSummary(
                        TypesMaster.Instance.GetTypeDataNone(),
                        (int)(pokemon.GetMaxHealth() * porcentualDamage),
                        DamageSummarySource.Status,
                        effectId.ToString()
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
                        "Fire Vortex Damage",
                        new Dictionary<string, string>()
                        {
                            { "pokemon", pokemon.GetName() }
                        }
                    )
                );
        battleTriggers.Add(messageTrigger);
        battleTriggers.Add(animTrigger);
        battleTriggers.Add(statusTrigger);
        base.Initiate();
    }
}
