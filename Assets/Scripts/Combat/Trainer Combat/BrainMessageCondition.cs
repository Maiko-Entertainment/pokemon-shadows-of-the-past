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
                bm.AddTrigger(new BattleTriggerOnPokemonFaintNumberLeftDialog(
                    new BattleTriggerMessageData(flowchart, blockName),
                    BattleTeamId.Team2,
                    (int) value
                    ));
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
                    BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(flowchart, blockName)));
                }
                else
                {
                    bm.AddTrigger(new BattleTriggerOnPokemonFaintNumberLeftDialog(
                        new BattleTriggerMessageData(flowchart, blockName),
                        BattleTeamId.Team1,
                        (int)value-1
                        ));
                }
                break;
        }
    }
}
