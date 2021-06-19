public class BattleAnimatorEventTypeAdvantage : BattleAnimatorEvent
{
    public BattleTypeAdvantageType advantageType;

    public BattleAnimatorEventTypeAdvantage(BattleTypeAdvantageType advantageType)
    {
        this.advantageType = advantageType;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.ExecuteEffectivenessFlowchart(advantageType);
        base.Execute();
    }
}
