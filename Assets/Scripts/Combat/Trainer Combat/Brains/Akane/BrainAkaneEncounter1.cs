using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trainer Brains/Akane/Encounter 1")]
public class BrainAkaneEncounter1 : TrainerBrainDataAdvanced
{
    public override void Initialize(BattleManager currentBattle)
    {
        base.Initialize(currentBattle);

        PokemonBattleData charmander = currentBattle.GetTeamData(BattleTeamId.Team2).pokemon[0];
        currentBattle.AddTrigger(
            new BattleTriggerOnPokemonFaintDialogue(
                charmander, 
                new BattleTriggerMessageData(
                    flowchartInstance,
                    "Charmander Faint",
                    new Dictionary<string, string>()
                    )
                )
            );
        currentBattle.AddTrigger(
            new BattleTriggerOnPokemonEnterDialogue(
                charmander,
                 new BattleTriggerMessageData(
                    flowchartInstance,
                    "Start",
                    new Dictionary<string, string>()
                    )
                )
            );
    }
}
