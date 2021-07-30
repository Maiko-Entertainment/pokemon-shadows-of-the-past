using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractablePokemonEncounter : WorldInteractable
{
    public List<PokemonEncounter> possibleEncounters = new List<PokemonEncounter>();
    public float activeChance = 0.3f;
    public float retryInterval = 7;
    public GameObject activePrefab;
    public BattleData battleData;
    public ViewTransition transition;

    protected bool isActive = false;
    protected GameObject activeGameoBject;
    protected float timePassed = 0;

    private void Start()
    {
        TryToActivate();
        timePassed = Random.value * retryInterval;
    }

    public void TryToActivate()
    {
        float random = Random.value;
        if (random <= activeChance && !isActive)
        {
            isActive = true;
            StartCoroutine(CreateDelay(0f));
        }
        else
        {
            isActive = false;
            Destroy(activeGameoBject);
        }
        timePassed = 0;
    }

    IEnumerator CreateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeGameoBject = Instantiate(activePrefab, transform);
    }

    public override void OnInteract()
    {
        if (isActive)
        {
            isActive = false;
            base.OnInteract();
            InteractionsMaster.GetInstance().AddEvent(new InteractionEventPokemonBattle(this));
        }
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
        PokemonCaughtData encounterPokemon = SelectRandomEncounter();
        PokemonBattleData battlePokemon = new PokemonBattleData(encounterPokemon);
        BattleMaster.GetInstance()?.RunPokemonBattle(battlePokemon, battleData);
        InteractionsMaster.GetInstance()?.ExecuteNext(0);
        Destroy(activeGameoBject);
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

    private void Update()
    {
        bool interactionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!interactionPlaying)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= retryInterval)
            {
                TryToActivate();
            }
        }
    }
}
