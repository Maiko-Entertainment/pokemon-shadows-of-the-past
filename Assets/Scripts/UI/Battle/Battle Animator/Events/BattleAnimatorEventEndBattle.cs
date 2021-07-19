using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventEndBattle : BattleAnimatorEvent
{
    public BattleEventBattleEnd battleEvent;
    public BattleAnimatorEventEndBattle(BattleEventBattleEnd battleEvent)
    {
        this.battleEvent = battleEvent;
        eventType = BattleAnimatorEventType.PokemonInfoChange;
    }

    public override void Execute()
    {
        float duration = TransitionMaster.GetInstance().RunSceneTransition();
        BattleAnimatorMaster.GetInstance().HideAll();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(duration);
        AudioMaster.GetInstance().StopMusic(false);
        base.Execute();
    }

    public override string ToString()
    {
        return base.ToString() + " - Battle End";
    }
}
