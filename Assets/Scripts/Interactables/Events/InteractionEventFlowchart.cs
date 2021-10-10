using Fungus;
using System.Collections.Generic;

public class InteractionEventFlowchart : InteractionEvent
{
    public Flowchart flowchart;
    public string blockName;
    public Dictionary<string, string> variables = new Dictionary<string, string>();

    public InteractionEventFlowchart(Flowchart flowchart, string blockName, Dictionary<string, string> variables=null)
    {
        if (variables != null)
        {
            this.variables = variables;
        }
        this.flowchart = flowchart;
        this.blockName = blockName;
    }

    public override void Execute()
    {
        base.Execute();
        foreach(string varName in variables.Keys)
        {
            flowchart.SetStringVariable(varName, variables[varName]);
        }
        flowchart?.ExecuteBlock(blockName);
    }
}
