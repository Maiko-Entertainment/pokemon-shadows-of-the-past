using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Moves/Empower By Faint")]
public class MoveDataEmpowerByFaint : MoveData
{
    public int passedTurnsRange = 1;
    public List<BattleTeamId> teams = new List<BattleTeamId>();
    public int maxMatches = 1;
    public UseMoveMods modsPerMatch = new UseMoveMods(PokemonTypeId.Unmodify);

    public UseMoveMods GetFinalMods()
    {
        UseMoveMods modsPerMatchFinal = new UseMoveMods(PokemonTypeId.Unmodify);
        List<BattleFaintHistory> history = BattleMaster.GetInstance().GetCurrentBattle().faintHistory;
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        int currentTurn = bm.turnsPassed;
        int matchesLeft = maxMatches;
        foreach (BattleFaintHistory h in history)
        {
            if (matchesLeft <= 0)
                break;
            PokemonBattleData pkmn = h.pokemon;
            int turn = h.turn;
            bool meetsTurn = currentTurn - turn <= passedTurnsRange;
            bool meetsTeam = teams.Count == 0 || teams.Contains(bm.GetTeamId(pkmn));
            if (meetsTurn && meetsTeam)
            {
                matchesLeft--;
                modsPerMatchFinal.Implement(modsPerMatch);
            }
        }
        return modsPerMatchFinal;
    }

    public override int GetCritLevel()
    {
        return base.GetCritLevel() + GetFinalMods().criticalLevelChange;
    }

    public override int GetPower(PokemonBattleData user)
    {
        return (int)(base.GetPower(user) * GetFinalMods().powerMultiplier);
    }

    public override float GetAccuracy(PokemonBattleData user)
    {
        return base.GetAccuracy(user) * GetFinalMods().powerMultiplier;
    }

}
