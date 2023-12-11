using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/Pokemon/Force Repeat Last Move")]
public class StatusEffectDataForceRepeatMove : StatusEffectData
{
    public override StatusEffect CreateStatusInstance(PokemonBattleData pokemon)
    {
        StatusEffect statusInstance = base.CreateStatusInstance(pokemon);
        int currentTurn = BattleMaster.GetInstance().GetCurrentBattle().turnsPassed;
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        BattleDesitionHistory currentTurnAction = BattleMaster.GetInstance().GetCurrentBattle().GetDesitionHistory(teamId, currentTurn);
        // Pokemon hasnt acted yet, so we check what it did last turn
        if (currentTurnAction == null)
        {
            currentTurnAction = BattleMaster.GetInstance().GetCurrentBattle().GetDesitionHistory(teamId, currentTurn - 1);
        }
        // If the desition exists and it was a move usage, then we create the status
        if (currentTurnAction != null && currentTurnAction.desition is BattleTurnDesitionPokemonMove)
        {
            BattleTurnDesitionPokemonMove moveDesition = currentTurnAction.desition as BattleTurnDesitionPokemonMove;
            // It also has to match the pokemon carrying this status
            if (moveDesition != null && moveDesition.pokemon.battleId == pokemon.battleId)
            {
                BattleTriggerOnMoveCancelChance chanceToCancelMove = new BattleTriggerOnMoveCancelChance(pokemon, 1f);
                chanceToCancelMove.alwaysPlayAnimation = playAnimationEvenIfMoveIsntCanceled;
                chanceToCancelMove.useThisMoveInstead = statusInstance.relatedMove = (moveDesition).move.move;
                chanceToCancelMove.flowchart = flowchart;
                chanceToCancelMove.blockName = onTriggerFlowchartBlock;
                chanceToCancelMove.animations = hitAnims;
                return statusInstance;
            }
            return null;
        }
        return null;
    }
}
