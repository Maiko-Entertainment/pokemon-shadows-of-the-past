using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFrostbite : StatusEffect
{
    public UseMoveMods moveMods = new UseMoveMods(null);
    public float porcentualDamage = 0.0625f;
    public StatusEffectFrostbite(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        // Only for Sp Attack
        effectId = StatusEffectId.Frostbite;
        // IsPrimary = true;
        minTurns = 99999;
        // CaptureRateBonus = 10;
        InmuneTypes.Add(TypesMaster.Instance.GetTypeData("Ice"));
        moveMods.powerMultiplier = 0.5f;
        gainStatusBlockName = "Frostbite Gain";
    }

    public override void Initiate()
    {
        Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
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
                 "Frostbite Damage",
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
            MoveCategoryId.special,
            mods
           );
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
            pokemon,
            new DamageSummary(
                TypesMaster.Instance.GetTypeDataNone(),
                (int)(pokemon.GetMaxHealth() * porcentualDamage),
                DamageSummarySource.Status,
                effectId.ToString()
            )
        );
        battleTriggers.Add(triggerMove);
        battleTriggers.Add(statusTrigger);
        battleTriggers.Add(animTrigger);
        battleTriggers.Add(messageTrigger);
        base.Initiate();
    }
}
