using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnRoundEndDialog : BattleTriggerOnRoundEnd
{
    public BattleTriggerMessageData messageData;
    public int roundNumber;
    public BattleTeamId teamId;

    public BattleTriggerOnRoundEndDialog(BattleTriggerMessageData messageData, int roundNumber, BattleTeamId teamId = BattleTeamId.None) :
    base()
    {
        this.messageData = messageData;
        this.roundNumber = roundNumber;
        this.teamId = teamId;
        maxTriggers = 1;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        int roundNumber = BattleMaster.GetInstance().GetCurrentBattle().turnsPassed;
        if (roundNumber == this.roundNumber)
        {
            if (maxTriggers > 0)
            {
                bool cont = true;
                if (teamId != BattleTeamId.None)
                {
                    PokemonBattleData pkmn = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(teamId);
                    cont = pkmn != null;
                }
                if (cont)
                {
                    BattleAnimatorMaster.GetInstance().AddEvent(
                        new BattleAnimatorEventNarrative(
                                messageData
                        )
                    );
                }
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
