
public class BattleTurnDesition
{
    public BattleTeamId team;
    public int priority = 1;
    // Add Tactic id selected

    public BattleTurnDesition(BattleTeamId teamId)
    {
        team = teamId;
    }

    public virtual void Execute()
    {

    }
}
