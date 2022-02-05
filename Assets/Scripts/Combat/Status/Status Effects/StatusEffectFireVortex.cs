using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFireVortex : StatusEffect
{
    public float porcentualDamage = 0.125f;
    
    public StatusEffectFireVortex(PokemonBattleData pokemon, Flowchart message): base(pokemon, message)
    {
        effectId = StatusEffectId.Burn;
        isPrimary = true;
        minTurns = 4;
        addedRangeTurns = 1;
        captureRateBonus = 10;
        inmuneTypes.Add(PokemonTypeId.Fire);
        stopEscape = true;
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
        foreach (BattleTrigger bt in battleTriggers)
        {
            BattleMaster.GetInstance()?
                .GetCurrentBattle()?.AddTrigger(
                    bt
                );
        }
        base.Initiate();
    }
}
