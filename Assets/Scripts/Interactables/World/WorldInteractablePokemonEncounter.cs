using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractablePokemonEncounter : WorldInteractable
{
    public List<PokemonEncounter> possibleEncounters = new List<PokemonEncounter>();
    public float activeChance = 0.3f;
    public bool ignoreDistanceRange = false;
    public float retryInterval = 7;
    public float spawnWorldEntityChance = 0.25f;
    public Vector3 spawnWorldOffset = Vector3.zero;
    public float spawnYRenderOffset = 0f;
    public MoveBrainDirection spawnDirection = MoveBrainDirection.Bottom;
    public bool showActivePrefabWithWorldEntity = false;
    public GameObject activePrefab;
    public BattleData battleData;
    public ViewTransition transition;

    protected bool isActive = false;
    protected GameObject activeGameObject;
    protected WorldInteractableWorldBrainPokemon worldEntity;
    protected PokemonEncounter currentEncounter;
    protected float timePassed = 0;

    private void Start()
    {
        TryToActivate();
        timePassed = Random.value * retryInterval;
    }

    public void TryToActivate()
    {
        float random = Random.value;
        isActive = false;
        ClearEncounter();
        if (random <= activeChance && !isActive && IsWithinPlayerDistance())
        {
            isActive = true;
            StartCoroutine(CreateDelay(0f));
        }
        timePassed = 0;
    }

    IEnumerator CreateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        float random = Random.value;
        currentEncounter = SelectRandomEncounter();
        PokemonCaughtData pokemon = currentEncounter.GetPokemonCaught();
        WorldInteractableWorldBrainPokemon pokemonWorld = WorldMapMaster.GetInstance().pokeFollowerPrefab;
        if (currentEncounter != null)
        {
            if (random < spawnWorldEntityChance && pokemonWorld)
            {
                worldEntity = Instantiate(pokemonWorld, transform).Load(pokemon);
                worldEntity.transform.localPosition = spawnWorldOffset;
                worldEntity.AddDirection(new MoveBrainDirectionData(spawnDirection, true));
                worldEntity.followMode = false;
                worldEntity.heightOffset = spawnYRenderOffset;
                SpriteRenderer renderer = worldEntity.GetComponentInChildren<SpriteRenderer>();
                if (currentEncounter.entityMaterial && renderer)
                {
                    renderer.material = currentEncounter.entityMaterial;
                }
                TransitionSize transition = worldEntity.gameObject.AddComponent<TransitionSize>();
                transition.speed = 2f;
                transition.FadeIn();
                if (showActivePrefabWithWorldEntity)
                    activeGameObject = Instantiate(activePrefab, transform);
            }
            else
            {
                activeGameObject = Instantiate(activePrefab, transform);
            }
        }
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
        if (currentEncounter != null)
        {
            PokemonBattleData battlePokemon = new PokemonBattleData(currentEncounter.GetPokemonCaught(), 100);
            BattleMaster.GetInstance()?.RunPokemonBattle(battlePokemon, battleData);
            InteractionsMaster.GetInstance()?.ExecuteNext(0);
        }
        ClearEncounter();
    }

    public void ClearEncounter()
    {
        if (activeGameObject)
        {
            TransitionSize transition = activeGameObject.AddComponent<TransitionSize>();
            transition.speed = 2f;
            transition.FadeOut();
            Destroy(activeGameObject, 0.5f);
        }
        if (worldEntity)
        {
            TransitionSize transition = worldEntity.gameObject.GetComponent<TransitionSize>();
            transition.FadeOut();
            Destroy(worldEntity.gameObject, 1 / 2f);
        }
        currentEncounter = null;
    }

    public PokemonEncounter SelectRandomEncounter()
    {
        TimeOfDayType time = WorldMapMaster.GetInstance().GetTimeOfDay();
        int total = 0;
        List<PokemonEncounter> encounters = new List<PokemonEncounter>();
        foreach (PokemonEncounter pe in possibleEncounters)
        {
            if (pe.timeOfDayRequired == TimeOfDayType.Any || pe.timeOfDayRequired == time)
                encounters.Add(pe);
        }
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
                return encounterPriority;
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

    public bool IsWithinPlayerDistance()
    {
        bool isMainMenu = WorldMapMaster.GetInstance().GetCurrentMap().isMainMenu;
        if (isMainMenu) return true;
        PlayerController player = WorldMapMaster.GetInstance().GetPlayer();
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return ignoreDistanceRange || 2f < distance && distance < 15f;
    }
}
