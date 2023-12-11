
using UnityEngine.UIElements;

public class BattleTurnDesition
{
    public BattleTeamId team;
    // 1 Move, 3 Item, 5 Switch, 7 Pokeball, 9 Tactic
    public int priority = 1;
    public TacticData tactic;

    public BattleTurnDesition(BattleTeamId teamId)
    {
        team = teamId;
    }

    public virtual void Execute()
    {
        int currentTurn = BattleMaster.GetInstance().GetCurrentBattle().turnsPassed;
        BattleMaster.GetInstance().GetCurrentBattle().battleDesitionHistories.Add(new BattleDesitionHistory(this, currentTurn));
    }

    public virtual float GetTiebreakerPriority()
    {
        return 0;
    }
}
