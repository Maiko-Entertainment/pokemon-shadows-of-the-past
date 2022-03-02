
public class BattleTurnDesition
{
    public BattleTeamId team;
    public int priority = 1;
    public TacticData tactic;

    public BattleTurnDesition(BattleTeamId teamId)
    {
        team = teamId;
    }

    public virtual void Execute()
    {
    }

    public virtual float GetTiebreakerPriority()
    {
        return 0;
    }
}
