using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMove : BattleTriggerOnPokemon
{
    public bool cancelMoveInstead = false;
    protected UseMoveMods useMoveMods;
    protected TriggerConditionMove triggerConditions = new TriggerConditionMove();
    public BattleTriggerOnPokemonMove(PokemonBattleData pokemon, UseMoveMods useMoveMods, bool deleteOnLeave) : base(pokemon, deleteOnLeave)
    {
        this.useMoveMods = useMoveMods;
        eventId = BattleEventId.pokemonUseMove;
    }

    public BattleTriggerOnPokemonMove(PokemonBattleData pokemon, UseMoveMods useMoveMods, TriggerConditionMove triggerConditions) : base(pokemon, true)
    {
        this.useMoveMods = useMoveMods;
        this.triggerConditions = triggerConditions;
        eventId = BattleEventId.pokemonUseMove;
    }

    public virtual bool Execute(BattleEventUseMove battleEvent)
    {
        PokemonBattleData enemy = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(pokemon, MoveTarget.Enemy);
        bool isEnemyAndTargetingPokemon = battleEvent.pokemon.battleId == enemy.battleId && battleEvent.move.targetType == MoveTarget.Enemy;
        PokemonBattleData targetPokemon = triggerConditions.focusOnEnemiesTargetingPokemonInstead && isEnemyAndTargetingPokemon ?
            enemy :
            pokemon;
        if (battleEvent.pokemon.battleId == targetPokemon.battleId &&
            maxTriggers > 0 &&
            useMoveMods != null &&
            triggerConditions.MeetsConditions(targetPokemon, battleEvent.move))
        {
            // Notify visually that move was cancelled
            if (cancelMoveInstead)
            {
                BattleMaster.GetInstance()?.GetCurrentBattle()?.AddEvent(
                    new BattleEventNarrative(
                        new BattleTriggerMessageData(
                            BattleAnimatorMaster.GetInstance().battleFlowchart,
                            "Inmune"
                        )));
                BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMoveFlowchart(battleEvent));
                return false;
            }
            battleEvent.moveMods.Implement(useMoveMods);
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
