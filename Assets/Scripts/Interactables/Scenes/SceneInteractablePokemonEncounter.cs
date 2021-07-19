using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractablePokemonEncounter : SceneInteractable
{
    public List<PokemonEncounter> possibleEncounters = new List<PokemonEncounter>();
    public AudioClip battleMusic;
    public ViewTransition transition;


    public override void Interact()
    {
        TransitionMaster.GetInstance().RunPokemonBattleTransition(transition);
        AudioMaster.GetInstance().PlayMusic(battleMusic);
        StartCoroutine(RunBattle(transition.changeTime));
    }

    IEnumerator RunBattle(float delay)
    {
        yield return new WaitForSeconds(delay);
        PokemonCaughtData encounterPokemon = SelectRandomEncounter();
        PokemonBattleData battlePokemon = new PokemonBattleData(encounterPokemon);
        BattleMaster.GetInstance()?.RunPokemonBattle(battlePokemon);
    }

    public PokemonCaughtData SelectRandomEncounter()
    {
        int total = 0;
        foreach (PokemonEncounter encounterPriority in possibleEncounters)
        {
            total += encounterPriority.priority;
        }
        int neededPrioritySum = Random.Range(0, total);
        total = 0;
        foreach (PokemonEncounter encounterPriority in possibleEncounters)
        {
            total += encounterPriority.priority;
            if (total >= neededPrioritySum)
                return encounterPriority.GetPokemonCaught();
        }
        return null;
    }
}
