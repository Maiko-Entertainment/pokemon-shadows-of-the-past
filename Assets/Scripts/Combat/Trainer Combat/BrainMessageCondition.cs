using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BrainMessageCondition
{
    public BrainMessageConditionType type;
    public float value = 0;
    public string blockName = "";

    public void Initiate(BattleManager bm, Flowchart flowchart)
    {
        switch (type)
        {
            case BrainMessageConditionType.trainerPokemonsLeft:
                BattleTriggerOnPokemonFaintNumberLeftDialog pl = new BattleTriggerOnPokemonFaintNumberLeftDialog(
                    new BattleTriggerMessageData(flowchart, blockName),
                    BattleTeamId.Team2,
                    (int)value
                    );
                pl.priority = -3;
                bm.AddTrigger(pl);
                break;
            case BrainMessageConditionType.playersPokemonsLeft:
                bm.AddTrigger(new BattleTriggerOnPokemonFaintNumberLeftDialog(
                    new BattleTriggerMessageData(flowchart, blockName),
                    BattleTeamId.Team1,
                    (int)value
                    ));
                break;
            case BrainMessageConditionType.roundNumber:
                if (value <= 0)
                {
                    BattleAnimatorEventNarrative eventNarrative = new BattleAnimatorEventNarrative(new BattleTriggerMessageData(flowchart, blockName));
                    eventNarrative.priority = -3;
                    BattleAnimatorMaster.GetInstance().AddEvent(eventNarrative);
                }
                else
                {
                    bm.AddTrigger(new BattleTriggerOnRoundEndDialog(
                        new BattleTriggerMessageData(flowchart, blockName),
                        (int)value-1,
                        BattleTeamId.Team2
                        ));
                }
                break;
        }
    }
}
