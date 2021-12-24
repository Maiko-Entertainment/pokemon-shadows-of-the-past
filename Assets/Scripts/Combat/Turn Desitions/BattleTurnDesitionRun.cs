public class BattleTurnDesitionRun : BattleTurnDesition
{

    public BattleTurnDesitionRun(BattleTeamId team) : base(team)
    {
        priority = 10;
    }

    public override void Execute()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddTryToRunEvent();
    }
}
