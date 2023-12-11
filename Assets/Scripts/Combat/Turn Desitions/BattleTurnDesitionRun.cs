public class BattleTurnDesitionRun : BattleTurnDesition
{

    public BattleTurnDesitionRun(BattleTeamId team) : base(team)
    {
        priority = 9;
    }

    public override void Execute()
    {
        base.Execute();
        BattleMaster.GetInstance().GetCurrentBattle().AddTryToRunEvent();
    }
}
