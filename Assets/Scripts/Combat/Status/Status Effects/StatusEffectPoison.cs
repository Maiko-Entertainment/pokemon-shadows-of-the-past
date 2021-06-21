using Fungus;
using System.Collections.Generic;

public class StatusEffectPoison : StatusEffect
{
    public float porcentualDamage = 0.125f;

    public StatusEffectPoison(PokemonBattleData pokemon, Flowchart message) : 
        base(pokemon, message)
    {
        effectId = StatusEffectId.Poison;
        isPrimary = true;
        minTurns = 99999;
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
        BattleTrigger messageTrigger = new BattleTriggerOnPokemonTurnEndMessage(
                    pokemon,
                    new BattleTriggerMessageData(
                        message,
                        "Poison Damage",
                        new Dictionary<string, string>()
                        {
                            { "pokemon", pokemon.GetName() }
                        }
                    )
                );
        battleTriggers.Add(messageTrigger);
        battleTriggers.Add(statusTrigger);
        BattleMaster.GetInstance()?
            .GetCurrentBattle()?.AddTrigger(
                messageTrigger
            );
        BattleMaster.GetInstance()?
            .GetCurrentBattle()?.AddTrigger(
                statusTrigger
            );
        base.Initiate();
    }
}
