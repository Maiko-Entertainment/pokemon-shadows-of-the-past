using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBurn : StatusEffect
{
    public float porcentualDamage = 0.0625f;
    public StatusEffectBurn(PokemonBattleData pokemon, Flowchart message): base(pokemon, message)
    {
        effectId = StatusEffectId.Burn;
        isPrimary = true;
        minTurns = 99999;
        captureRateBonus = 10;
    }

    public override void Initiate()
    {
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
                    pokemon,
                    new DamageSummary(
                        PokemonTypeId.Undefined,
                        (int)(pokemon.GetPokemonHealth() * porcentualDamage),
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
                        message,
                        "Burn Damage",
                        new Dictionary<string, string>()
                        {
                            { "pokemon", pokemon.GetName() }
                        }
                    )
                );
        // Needs trigger to reduce physical attack damage
        battleTriggers.Add(messageTrigger);
        battleTriggers.Add(animTrigger);
        battleTriggers.Add(statusTrigger);
        BattleMaster.GetInstance()?
            .GetCurrentBattle()?.AddTrigger(
                messageTrigger
            );
        BattleMaster.GetInstance()?
            .GetCurrentBattle()?.AddTrigger(
                animTrigger
            );
        BattleMaster.GetInstance()?
            .GetCurrentBattle()?.AddTrigger(
                statusTrigger
            );
        base.Initiate();
    }
}
