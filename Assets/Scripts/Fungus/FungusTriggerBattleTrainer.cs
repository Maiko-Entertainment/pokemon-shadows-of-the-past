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
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(61, 170, 191, 255);
    }
}
