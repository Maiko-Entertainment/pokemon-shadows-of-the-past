using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnDamageEffect : BattleTriggerOnPokemonDamage
{
    public List<DamageSummarySource> damageSources = new List<DamageSummarySource>() { DamageSummarySource.Move };
    public bool requiresContact = true; // Only used for moves
    public bool isAbilitySource = false;
    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> moveStatChanges = new List<MoveStatChange>();

    public BattleTriggerOnDamageEffect(PokemonBattleData pokemon, List<MoveStatusChance> statusChances, List<MoveStatChange> moveStatChanges) : base(pokemon, true)
    {
        this.statusChances = statusChances;
        this.moveStatChanges = moveStatChanges;
        maxTriggers = 999999;
    }

    public override bool Execute(BattleEventTakeDamageSuccess battleEvent)
    {
        DamageSummary summary = battleEvent.damageEvent.damageSummary;
        if (damageSources.Contains(summary.damageSource))
        {
            // Checks for required contact requirement which applies only to moves
            if (summary.damageSource == DamageSummarySource.Move)
            {
                MoveData moveData = MovesMaster.Instance.GetMove((MoveId)summary.sourceId);
                // If requires contact but it doesnt have contact exits
                if (requiresContact && !moveData.isContact)
                {
                    return base.Execute(battleEvent);
                }
            }
            HandleStatsChanges(pokemon);
            HandleStatusAdds(pokemon);
        }
        return base.Execute(battleEvent);
    }

    public virtual void HandleStatsChanges(PokemonBattleData pokemon)
    {
        bool wasTriggered = false;
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatChange msc in moveStatChanges)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.changeChance)
            {
                bm.AddStatChangeEvent(pokemonTarget, msc.statsAmountChange);
                if (!wasTriggered && isAbilitySource)
                {
                    wasTriggered = true;
                    BattleMaster.GetInstance().GetCurrentBattle().AddAbilityEvent(pokemon);
                }
            }
        }
    }

    public virtual void HandleStatusAdds(PokemonBattleData pokemon)
    {
        bool wasTriggered = false;
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatusChance msc in statusChances)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.chance)
            {
                bm.AddStatusEffectEvent(pokemonTarget, msc.effectId, false);
                if (!wasTriggered && isAbilitySource)
                {
                    wasTriggered = true;
                    BattleMaster.GetInstance().GetCurrentBattle().AddAbilityEvent(pokemon);
                }
            }
        }
    }
}
