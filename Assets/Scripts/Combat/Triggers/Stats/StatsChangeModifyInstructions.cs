using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatsChangeModifyInstructions
{
    public int attackMultiplier = 1;
    public int defenseMultiplier = 1;
    public int spAttackMultiplier = 1;
    public int spDefenseMultiplier = 1;
    public int speedMultiplier = 1;
    public int accuracyMultiplier = 1;
    public int evasionMultiplier = 1;
    public int criticalMultiplier = 1;

    public bool onlyOnNegativeChanges = true;

    public BattleEventPokemonChangeStat Apply(BattleEventPokemonChangeStat changeEvent)
    {
        PokemonBattleStats levelChange = changeEvent.statsLevelChange;
        levelChange.attack = onlyOnNegativeChanges && levelChange.attack >= 0 ? levelChange.attack : levelChange.attack * attackMultiplier;
        levelChange.defense = onlyOnNegativeChanges && levelChange.defense >= 0 ? levelChange.defense : levelChange.defense * defenseMultiplier;
        levelChange.spAttack = onlyOnNegativeChanges && levelChange.spAttack >= 0 ? levelChange.spAttack : levelChange.spAttack * spAttackMultiplier;
        levelChange.spDefense = onlyOnNegativeChanges && levelChange.spDefense >= 0 ? levelChange.spDefense : levelChange.spDefense * spDefenseMultiplier;
        levelChange.speed = onlyOnNegativeChanges && levelChange.speed >= 0 ? levelChange.speed : levelChange.speed * speedMultiplier;
        levelChange.critical = onlyOnNegativeChanges && levelChange.critical >= 0 ? levelChange.critical : levelChange.critical * criticalMultiplier;
        levelChange.accuracy = onlyOnNegativeChanges && levelChange.accuracy >= 0 ? levelChange.accuracy : levelChange.accuracy * accuracyMultiplier;
        levelChange.evasion = onlyOnNegativeChanges && levelChange.evasion >= 0 ? levelChange.evasion : levelChange.evasion * evasionMultiplier;
        return changeEvent;
    }

    public bool IsApplicable(BattleEventPokemonChangeStat changeEvent)
    {
        PokemonBattleStats levelChange = changeEvent.statsLevelChange;
        if (attackMultiplier != 1 && levelChange.attack != 0 && (onlyOnNegativeChanges ? levelChange.attack < 0 : true))
        {
            return true;
        }
        if (defenseMultiplier != 1 && levelChange.defense != 0 && (onlyOnNegativeChanges ? levelChange.defense < 0 : true))
        {
            return true;
        }
        if (spAttackMultiplier != 1 && levelChange.spAttack != 0 && (onlyOnNegativeChanges ? levelChange.spAttack < 0 : true))
        {
            return true;
        }
        if (spDefenseMultiplier != 1 && levelChange.spDefense != 0 && (onlyOnNegativeChanges ? levelChange.spDefense < 0 : true))
        {
            return true;
        }
        if (speedMultiplier != 1 && levelChange.speed != 0 && (onlyOnNegativeChanges ? levelChange.speed < 0 : true))
        {
            return true;
        }
        if (criticalMultiplier != 1 && levelChange.critical != 0 && (onlyOnNegativeChanges ? levelChange.critical < 0 : true))
        {
            return true;
        }
        if (accuracyMultiplier != 1 && levelChange.accuracy != 0 && (onlyOnNegativeChanges ? levelChange.accuracy < 0 : true))
        {
            return true;
        }
        if (evasionMultiplier != 1 && levelChange.evasion != 0 && (onlyOnNegativeChanges ? levelChange.evasion < 0 : true))
        {
            return true;
        }
        return false;
    }
}
