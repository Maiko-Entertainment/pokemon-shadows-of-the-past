using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Moves/Self Hit")]
public class MoveDataSelfHit : MoveData
{
    public override void Execute(BattleEventUseMove battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        HandleStatsChanges(battleEvent.pokemon);
        HandleStatusAdds(battleEvent.pokemon);
        battleEvent.pokemon.ReduceMovePP(this);
        PokemonBattleData pokemonTarget = bm.GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        if (categoryId != MoveCategoryId.status)
        {
            DamageSummary damageSummary = bm.CalculateMoveDamage(battleEvent);
            bm.AddDamageDealtEvent(pokemonTarget, damageSummary);
        }
        bm.AddMoveSuccessEvent(battleEvent);
        BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartPokemonText("Confusion Success", pokemonTarget);
        // Negative values are used for recoil
        if (drainMultiplier != 0)
        {
            BattleMaster.GetInstance().GetCurrentBattle()?.AddTrigger(new BattleTriggerDrainOnMoveDamage(battleEvent.pokemon, this, drainMultiplier));
        }
        HandleAnimations(battleEvent.pokemon, pokemonTarget);
    }
}
