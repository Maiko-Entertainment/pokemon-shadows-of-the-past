using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo(
    "Battle",
    "Trigger Trainer Battle",
    "Triggers battle against a trainer"
)]

public class FungusTriggerBattleTrainer : Command
{
    public TrainerCombatData trainer;
    public string onWinBlock;
    public string onLoseBlock;
    public override void OnEnter()
    {
        TransitionMaster.GetInstance().RunTrainerBattleTransition(trainer.transition);
        AudioMaster.GetInstance()?.PlayMusic(trainer.battleSong);
        StartCoroutine(TriggetBattleAfter(trainer.transition.changeTime));
    }

    IEnumerator TriggetBattleAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        BattleMaster.GetInstance()?.RunTrainerBattle(trainer);
        BattleMaster.GetInstance()?.SetPostBattleEvent(new BattleEndEvent(GetFlowchart(), onWinBlock, onLoseBlock));
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(61, 170, 191, 255);
    }
    public override string GetSummary()
    {
        string clipName = trainer != null ? trainer.name : "None";
        return clipName;
    }
}
