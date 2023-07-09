using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEndEvent
{
    public Flowchart flowchart;
    public string onWinBlockName = "Win";
    public string onLoseBlockName = "Lose";
    public string onDrawBlockName = "Draw";

    public BattleEndEvent(Flowchart flowchart, string onWinBlockName, string onLoseBlockName, string onDrawBlockName)
    {
        this.flowchart = flowchart;
        this.onWinBlockName = onWinBlockName;
        this.onLoseBlockName = onLoseBlockName;
        this.onDrawBlockName = onDrawBlockName;
    }

    public void Execute(BattleEventBattleEnd e)
    {
        if (e.winningTeamId == BattleTeamId.Team1)
        {
            if (flowchart && onWinBlockName!= "")
            {
                InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, onWinBlockName));
                InteractionsMaster.GetInstance().ExecuteNext();
            }
        }
        else if (e.winningTeamId == BattleTeamId.Team2)
        {
            if (flowchart && onLoseBlockName != "")
            {
                InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, onLoseBlockName));
                InteractionsMaster.GetInstance().ExecuteNext();
            }
        }
        else
        {
            if (flowchart)
            {
                InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, "Draw"));
            }
            InteractionsMaster.GetInstance().ExecuteNext();
        }
    }
}
