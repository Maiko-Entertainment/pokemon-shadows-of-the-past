using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectLeechSeed : StatusEffect
{
    public float porcentualDamage = 0.0625f;
    public StatusEffectLeechSeed(PokemonBattleData pokemon): base(pokemon)
    {
        effectId = StatusEffectId.LeechSeed;
        isPrimary = false;
        minTurns = 99999;
        captureRateBonus = 10;
        inmuneTypes.Add(PokemonTypeId.Grass);
    }
public override void Initiate()
    {
        Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
        DamageSummary damageSummary = new DamageSummary(
                        PokemonTypeId.Undefined,
                        (int)(pokemon.GetPokemonHealth() * porcentualDamage),
                        DamageSummarySource.Status,
                        (int)effectId
                    );
        damageSummary.healOpponentByDamage = 1;
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
                    pokemon,
                    damageSummary
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
                        "Leech Damage",
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
