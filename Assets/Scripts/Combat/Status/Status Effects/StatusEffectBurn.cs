using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBurn : StatusEffect
{
    public float porcentualDamage = 0.0625f;
    public StatusEffectBurn(PokemonBattleData pokemon, StatusEffectData seData): base(pokemon, seData, null)
    {
        effectId = StatusEffectId.Burn;
        // IsPrimary = true;
        minTurns = 99999;
        // CaptureRateBonus = 10;
        InmuneTypes.Add(TypesMaster.Instance.GetTypeData("Fire"));
        gainStatusBlockName = "Burn Gain";
    }

    public override void Initiate()
    {
        Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
                    pokemon,
                    new DamageSummary(
                        TypesMaster.Instance.GetTypeDataNone(),
                        Mathf.Max(1, (int)(pokemon.GetMaxHealth() * porcentualDamage)),
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
                        "Burn Damage",
                        new Dictionary<string, string>()
                        {
                            { "pokemon", pokemon.GetName() }
                        }
                    )
                );
        UseMoveMods mods = new UseMoveMods(null);
        mods.powerMultiplier = 0.5f;
        BattleTriggerOnPokemonMoveCategory triggerMove = new BattleTriggerOnPokemonMoveCategory(
            pokemon,
            MoveCategoryId.physical,
            mods
           );
        // Needs trigger to reduce physical attack damage
        battleTriggers.Add(messageTrigger);
        battleTriggers.Add(animTrigger);
        battleTriggers.Add(statusTrigger);
        battleTriggers.Add(triggerMove);
        base.Initiate();
    }
}
