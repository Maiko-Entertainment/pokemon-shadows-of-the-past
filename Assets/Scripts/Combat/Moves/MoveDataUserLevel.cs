using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Moves/User Level")]
public class MoveDataUserLevel : MoveData
{
    public override DamageSummary GetMoveDamageSummary(BattleEventUseMove battleEvent)
    {
        PokemonBattleData user = battleEvent.pokemon;
        DamageSummary summary = new DamageSummary(
            moveType,
            user.GetPokemonCaughtData().GetLevel(),
            DamageSummarySource.Move,
            battleEvent.move.GetId(),
            BattleTypeAdvantageType.normal,
            user);
        summary.move = battleEvent.move;
        return summary;
    }
    public override int GetPower(PokemonBattleData user)
    {
        return user.GetPokemonCaughtData().GetLevel();
    }
}
