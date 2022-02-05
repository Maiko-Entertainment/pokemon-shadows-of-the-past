using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractablePokemonEncounter : SceneInteractable
{
    public List<PokemonEncounter> possibleEncounters = new List<PokemonEncounter>();
    public float activeChance = 0.3f;
    public BattleData battleData;
    public ViewTransition transition;
    public Transform pokemonSpawn;
    public Material pokemonSpriteMaterial;

    protected bool isActive = false;
    protected PokemonAnimationController activeGameoBject;
    protected PokemonCaughtData pokemon;
    private void Start()
    {
        TryToActivate();
    }

    public void TryToActivate()
    {
        float random = Random.value;
        if (random <= activeChance && !isActive && GetPosibleEncounters().Count > 0)
        {
            isActive = true;
            StartCoroutine(CreateDelay(0f));
        }
        else
        {
            isActive = false;
            Destroy(activeGameoBject);
        }
    }

    IEnumerator CreateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pokemon = SelectRandomEncounter();
        activeGameoBject = Instantiate(pokemon.GetPokemonBaseData().battleAnimation, pokemonSpawn);
        if (pokemonSpriteMaterial != null)
        {
            activeGameoBject.SetPokemonSpriteMaterial(pokemonSpriteMaterial);
        }
    }

    public override void Interact()
    {
        if (isActive)
            InteractionsMaster.GetInstance().AddEvent(new InteractionEventPokemonBattle(this));
    }

    public virtual void Execute()
    {
        TransitionMaster.GetInstance().RunPokemonBattleTransition(transition);
        AudioMaster.GetInstance().PlayMusic(battleData.battleMusic);
        StartCoroutine(RunBattle(transition.changeTime));
    }

    IEnumerator RunBattle(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(activeGameoBject.gameObject);
        PokemonBattleData battlePokemon = new PokemonBattleData(pokemon, 100);
        BattleMaster.GetInstance()?.RunPokemonBattle(battlePokemon, battleData);
        InteractionsMaster.GetInstance()?.ExecuteNext(0);
    }

    public PokemonCaughtData SelectRandomEncounter()
    {
        int total = 0;
        List<PokemonEncounter> encounters = GetPosibleEncounters();
        foreach (PokemonEncounter encounterPriority in encounters)
        {
            total += encounterPriority.priority;
        }
        int neededPrioritySum = Random.Range(0, total);
        total = 0;
        foreach (PokemonEncounter encounterPriority in encounters)
        {
            total += encounterPriority.priority;
            if (total >= neededPrioritySum)
                return encounterPriority.GetPokemonCaught();
        }
        return null;
    }

    public List<PokemonEncounter> GetPosibleEncounters()
    {
        List<PokemonEncounter> encounters = new List<PokemonEncounter>();
        TimeOfDayType timeOfDay = WorldMapMaster.GetInstance().GetTimeOfDay();
        foreach (PokemonEncounter encounterPriority in possibleEncounters)
        {
            if (encounterPriority.timeOfDayRequired == TimeOfDayType.Any || encounterPriority.timeOfDayRequired == timeOfDay)
            {
                encounters.Add(encounterPriority);
            }
        }
        return encounters;
    }
}
