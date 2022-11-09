using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventStatChange : BattleAnimatorEvent
{
    public BattleEventPokemonChangeStat battleEvent;

    public BattleAnimatorEventStatChange(BattleEventPokemonChangeStat battleEvent)
    {
        this.battleEvent = battleEvent;
        eventType = BattleAnimatorEventType.PokemonInfoChange;
    }

    public override void Execute()
    {
        PokemonBattleStats battleStats = battleEvent.statsLevelChange;
        HandleStatUpDown("Attack", battleStats.attack, battleEvent.pokemon);
        HandleStatUpDown("Defense", battleStats.defense, battleEvent.pokemon);
        HandleStatUpDown("Special Attack", battleStats.spAttack, battleEvent.pokemon);
        HandleStatUpDown("Special Defense", battleStats.spDefense, battleEvent.pokemon);
        HandleStatUpDown("Speed", battleStats.speed, battleEvent.pokemon);
        HandleStatUpDown("Evasion", battleStats.evasion, battleEvent.pokemon);
        HandleStatUpDown("Accuracy", battleStats.accuracy, battleEvent.pokemon);
        HandleStatUpDown("Critical Rate", battleStats.critical, battleEvent.pokemon);
        base.Execute();
    }

    public void HandleStatUpDown (string statName, int change, PokemonBattleData pokemon){
        if (change > 0)
        {
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartPokemonText(
                "Stat Up", pokemon,
                new Dictionary<string, string>() { { "stat", statName } }
            );
        }
        else if (change < 0)
        {
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartPokemonText(
                "Stat Down", pokemon,
                new Dictionary<string, string>() { { "stat", statName } }
            );
        }
    }

    public override string ToString()
    {
        return "Damage - " + battleEvent.ToString(); ;
    }
}
