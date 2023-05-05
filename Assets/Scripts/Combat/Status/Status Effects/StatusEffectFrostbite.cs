using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFrostbite : StatusEffect
{
    public UseMoveMods moveMods = new UseMoveMods(PokemonTypeId.Unmodify);
    public float porcentualDamage = 0.0625f;
    public StatusEffectFrostbite(PokemonBattleData pokemon): base(pokemon)
    {
        // Only for Sp Attack
        effectId = StatusEffectId.Frostbite;
        isPrimary = true;
        minTurns = 99999;
        captureRateBonus = 10;
        inmuneTypes.Add(PokemonTypeId.Ice);
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
        BattleTriggerOnPokemonBurn triggerMove = new BattleTriggerOnPokemonBurn(
            pokemon,
            MoveCategoryId.special
           );
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
            pokemon,
            new DamageSummary(
                PokemonTypeId.Undefined,
                (int)(pokemon.GetPokemonHealth() * porcentualDamage),
                DamageSummarySource.Status,
                (int)effectId
            )
        );
        battleTriggers.Add(triggerMove);
        battleTriggers.Add(statusTrigger);
        battleTriggers.Add(animTrigger);
        battleTriggers.Add(messageTrigger);
        base.Initiate();
    }
}
