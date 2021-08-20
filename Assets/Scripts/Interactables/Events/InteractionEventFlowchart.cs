using Fungus;

public class InteractionEventFlowchart : InteractionEvent
{
    public Flowchart flowchart;
    public string blockName;

    public InteractionEventFlowchart(Flowchart flowchart, string blockName)
    {
        this.flowchart = flowchart;
        this.blockName = blockName;
    }

    public override void Execute()
    {
        base.Execute();
        flowchart?.ExecuteBlock(blockName);
    }
}
