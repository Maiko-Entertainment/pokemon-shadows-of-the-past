using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Target went first")]
public class TriggerConditionMoveTargetWentFirst : TriggerConditionData
{
    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData move)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData target = bm.GetTarget(pokemon, move.targetType);
        List<BattleDesitionHistory> desitions = bm.GetDesitionHistory(bm.turnsPassed);
        bool foundTargetFirst = false;
        foreach(BattleDesitionHistory desition in desitions)
        {
            if (desition.desition is BattleTurnDesitionPokemon && desition.desition is not BattleTurnDesitionPokemonSwitch)
            {
                BattleTurnDesitionPokemon pokemoveDesition = desition.desition as BattleTurnDesitionPokemon;
                // The target registered it's move first
                if (pokemoveDesition.pokemon.battleId == target.battleId)
                {
                    foundTargetFirst = true;
                    break;
                }
                // The player registered its move first
                else if (pokemoveDesition.pokemon.battleId == pokemon.battleId)
                {
                    break;
                }
            }
        }
        return invertCondition ? !foundTargetFirst : foundTargetFirst;
    }
}
