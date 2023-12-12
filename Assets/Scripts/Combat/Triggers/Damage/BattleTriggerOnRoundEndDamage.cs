using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnRoundEndDamage : BattleTriggerOnRoundEnd
{
    public StatusField damageSourceField;
    protected float _damagePercentage;
    protected TriggerCondition _triggerCondition;

    public BattleTriggerOnRoundEndDamage(float damagePercentage, TriggerCondition triggerCondition)
    {
        _damagePercentage = damagePercentage;
        _triggerCondition = triggerCondition;
    }

    public BattleTriggerOnRoundEndDamage(StatusField damageSource)
    {
        _damagePercentage = damageSource.FieldData.percentageDamagePerRound;
        _triggerCondition = damageSource.FieldData.percentageDamageConditions;
        damageSourceField = damageSource;
    }

    public override bool Execute(BattleEventRoundEnd roundEnd)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData team1Active = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData team2Active = bm.GetTeamActivePokemon(BattleTeamId.Team2);

        if (_triggerCondition != null)
        {
            if (_triggerCondition.MeetsConditions(team1Active))
            {
                DamagePokemon(team1Active);
            }
            if (_triggerCondition.MeetsConditions(team2Active))
            {
                DamagePokemon(team2Active);
            }
        }
        else
        {
            DamagePokemon(team1Active);
            DamagePokemon(team2Active);
        }
        return base.Execute(roundEnd);
    }

    public void DamagePokemon(PokemonBattleData pkmn)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        DamageSummary damageSummary = new DamageSummary(
                TypesMaster.Instance.GetTypeDataNone(),
                (int)_damagePercentage * pkmn.GetMaxHealth(),
                DamageSummarySource.Field,
                damageSourceField != null ? damageSourceField.FieldData.id : ""
        );
        bm.AddDamageDealtEvent(pkmn, damageSummary);
    }
}
