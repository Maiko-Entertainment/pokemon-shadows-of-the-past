using Fungus;
using System.Collections.Generic;

public class StatusEffectPoison : StatusEffect
{
    public float porcentualDamage = 0.125f;

    public StatusEffectPoison(PokemonBattleData pokemon) : 
        base(pokemon)
    {
        effectId = StatusEffectId.Poison;
        isPrimary = true;
        minTurns = 99999;
        captureRateBonus = 10;
        inmuneTypes.Add(PokemonTypeId.Poison);
        gainStatusBlockName = "Poison Gain";
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
        BattleTrigger statusTrigger = new BattleTriggerRoundEndDamage(
                    pokemon,
                    new DamageSummary(
                        PokemonTypeId.Undefined,
                        (int)(pokemon.GetPokemonHealth() * porcentualDamage),
                        DamageSummarySource.Status,
                        effectId.ToString()
                    )
                );
        BattleTrigger messageTrigger = new BattleTriggerOnPokemonTurnEndMessage(
                    pokemon,
                    new BattleTriggerMessageData(
                        battleFlow,
                        "Poison Damage",
                        new Dictionary<string, string>()
                        {
                            { "pokemon", pokemon.GetName() }
                        }
                    )
                );
        battleTriggers.Add(messageTrigger);
        battleTriggers.Add(statusTrigger);
        battleTriggers.Add(animTrigger);
        base.Initiate();
    }
}
