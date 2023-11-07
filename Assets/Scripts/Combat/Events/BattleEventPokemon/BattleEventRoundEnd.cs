using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventRoundEnd : BattleEvent
{
    public BattleEventRoundEnd() : base()
    {
        eventId = BattleEventId.roundEnd;
    }

    public override void Execute()
    {
        base.Execute();
        //Restart Turn
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTurnStart());
        // Restart Tactic
        BattleTeamData team1Data = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1);
        BattleMaster.GetInstance().GetCurrentBattle().SetSelectedTactic(null);
        team1Data.IncreaseTacticGauge(1);
        // Remove End of turn triggers
        BattleMaster.GetInstance().GetCurrentBattle().eventManager.RemoveEndOfRoundTriggers();
        BattleMaster.GetInstance().GetCurrentBattle().turnsPassed++;
        // Increase rounds in combat count
        PokemonBattleData team1Pokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team1);
        team1Pokemon.roundsInCombat++;
        PokemonBattleData team2Pokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team2);
        team2Pokemon.roundsInCombat++;

        //Tactics Tutorial Activation
        float tacticsTuto = SaveMaster.Instance.GetSaveElementFloat(SaveElementId.tacticsTutorial.ToString());
        float storyProgress = SaveMaster.Instance.GetSaveElementFloat(SaveElementId.storyProgress.ToString());
        if (tacticsTuto == 0f && storyProgress >= 10)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(
                    BattleAnimatorMaster.GetInstance().battleFlowchart,
                    "Tactics Tutorial"
                )
            ));
            SaveMaster.Instance.SetSaveElement(1f, SaveElementId.tacticsTutorial.ToString());
        }
    }
}
