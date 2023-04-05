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
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTurnStart());
        BattleTeamData team1Data = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1);
        BattleMaster.GetInstance().GetCurrentBattle().SetSelectedTactic(null);
        team1Data.IncreaseTacticGauge(1);
        BattleMaster.GetInstance().GetCurrentBattle().eventManager.RemoveEndOfRoundTriggers();
        BattleMaster.GetInstance().GetCurrentBattle().turnsPassed++;
        PokemonBattleData team1Pokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team1);
        team1Pokemon.roundsInCombat++;
        PokemonBattleData team2Pokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team2);
        team2Pokemon.roundsInCombat++;
        SaveElementNumber tacticsTuto = (SaveElementNumber) SaveMaster.Instance.GetSaveElement(SaveElementId.tacticsTutorial);
        if ((float) tacticsTuto.GetValue() == 0f)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(
                    BattleAnimatorMaster.GetInstance().battleFlowchart,
                    "Tactics Tutorial"
                )
            ));
            tacticsTuto.SetValue(1f);
        }
    }
}
