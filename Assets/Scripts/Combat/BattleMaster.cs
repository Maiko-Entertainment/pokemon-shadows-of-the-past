using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMaster : MonoBehaviour
{
    public static BattleMaster Instance { get; set; }
    public Flowchart battleFlowchart;

    public BattleManager currentBattle;
    public bool triggerBattleOnStart = false;
    public bool isExpShareOn = true;

    public BattleTypeAdvantageManager advantageManager = new BattleTypeAdvantageManager();


    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (triggerBattleOnStart)
        {
            RunTestBattle();
        }
    }

    public static BattleMaster GetInstance()
    {
        return Instance;
    }
    public void HandleSave()
    {
        SaveMaster.Instance.SetSaveElementInner(isExpShareOn ? 1f : 0f, SaveElementId.expShare);
    }

    public BattleManager GetCurrentBattle()
    {
        return currentBattle;
    }

    public bool IsBattleActive()
    {
        if (currentBattle != null)
        {
            return currentBattle.IsBattleActive();
        }
        return false;
    }

    public void RunTestBattle()
    {
        Debug.Log("Started test battle");
        GetCurrentBattle().StartBattle();
    }

    public void RunPokemonBattle(PokemonBattleData pokemon, BattleData battleData)
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        List<TacticData> finalTactics = new List<TacticData>();
        finalTactics.AddRange(TacticsMaster.GetInstance().GetEquippedTactics());
        finalTactics.AddRange(battleData.playerExtraTactics);
        BattleTeamData team1 = new BattleTeamData(
            (string) SaveMaster.Instance.GetSaveElement(SaveElementId.playerName).GetValue(),
            GetPokemonBattleDataFromCaught(party, BattleTeamId.Team1), 0, finalTactics);
        BattleTeamData team2 = new BattleTeamData(pokemon.GetName(), new List<PokemonBattleData>() { pokemon }, 0);
        BattleManager newBattle = new BattleManager(team1, team2, battleData);
        currentBattle = newBattle;
        currentBattle.SetOnEndEvent(new BattleEndEvent(
            battleFlowchart,
            "",
            "Lose Wild Battle",
            ""
        ));
        if (battleData.volumeProfile)
            TransitionMaster.GetInstance().SetBattleCameraProfile(battleData.volumeProfile);
        GetCurrentBattle().StartBattle();
        BattleAnimatorMaster.GetInstance().ShowAll();
    }

    public void Load(SaveFile save)
    {
        PersistedSaveElement expShare = save.persistedElements.Find((se) => se.id == SaveElementId.expShare);
        if (expShare != null)
        {
            isExpShareOn = (float)expShare.value != 0f;
        }
        else
        {
            isExpShareOn = true;
        }
    }

    public void SetPostBattleEvent(BattleEndEvent postBattleEvent)
    {
        GetCurrentBattle()?.SetOnEndEvent(postBattleEvent);
    }

    public void RunTrainerBattle(TrainerCombatData trainer)
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        List<TacticData> finalTactics = new List<TacticData>(TacticsMaster.GetInstance().GetEquippedTactics());
        finalTactics.AddRange(trainer.battleData.playerExtraTactics);
        BattleTeamData team1 = new BattleTeamData(
            (string)SaveMaster.Instance.GetSaveElement(SaveElementId.playerName).GetValue(),
            GetPokemonBattleDataFromCaught(party, BattleTeamId.Team1), 0, finalTactics);
        team1.allyPokemon = trainer.team1PokemonAllies;
        BattleTeamData team2 = trainer.GetTeambattleData();
        BattleManager newBattle = new BattleManager(team1, team2, trainer.battleData);
        currentBattle = newBattle;
        TransitionMaster.GetInstance().SetBattleCameraProfile(trainer.battleData.volumeProfile);
        GetCurrentBattle().StartBattle();
        BattleAnimatorMaster.GetInstance().ShowAll();
    }

    public void AddBattleEndFlowchart()
    {

    }

    public static List<PokemonBattleData> GetPokemonBattleDataFromCaught(List<PokemonCaughtData> party, BattleTeamId teamId)
    {
        List<PokemonBattleData> battleParty = new List<PokemonBattleData>();
        int id = teamId == BattleTeamId.Team1 ? 0 : 100;
        foreach(PokemonCaughtData pokemon in party)
        {
            battleParty.Add(new PokemonBattleData(pokemon, id));
            id++;
        }
        return battleParty;
    }

    public float GetAdvantageMultiplier(PokemonTypeId damageType, List<PokemonTypeId> targetTypes)
    {
        float multiplier = 1;
        foreach(PokemonTypeId targetType in targetTypes)
        {
            BattleTypeAdvantageType adv = advantageManager.GetTypeRelation(damageType, targetType);
            multiplier *= advantageManager.GetAdvantageMultiplier(adv);
        }
        return multiplier;
    }

    public TypeData GetTypeData(PokemonTypeId type)
    {
        return advantageManager.GetTypeData(type);
    }

    public Flowchart GetBattleFlowchart()
    {
        return battleFlowchart;
    }

    public int GetExperienceForDefeating(PokemonBattleData pokemon)
    {
        int baseExp = 15; //pokemon.GetPokemonCaughtData().pokemonBase.baseExp;
        int pokemonLevel = pokemon.GetPokemonCaughtData().GetLevel();
        int experienceGained = (int)Mathf.Pow(baseExp * pokemonLevel * 0.75f, 2.5f) / 2000 + 2 * baseExp;
        return experienceGained;
    }

    public void ClearBattleEvents()
    {
        currentBattle.eventManager.ClearEvents();
    }

    public DamageSummary CalculateOutOfBattleDamage(PokemonCaughtData pokemon, OutOfCombatDamage damage)
    {
        List<PokemonTypeId> targetTypes = pokemon.GetTypes();
        // Effectiveness
        float advantageMultiplier = GetAdvantageMultiplier(damage.type, targetTypes);
        // Stats
        PokemonBaseStats targetStats = pokemon.GetCurrentStats();
        int defense =
            damage.moveCategory == MoveCategoryId.physical ?
            targetStats.defense : targetStats.spDefense;
        // Damage Calc
        float randomMultiplier = 0.8f + Random.value * 0.2f;
        // Damage scales with pkmn level
        float baseDamage = 2 + (2 * pokemon.GetLevel() + 10f) / defense / 4f * damage.damagePower;
        float finalDamage = baseDamage * randomMultiplier * advantageMultiplier;

        DamageSummary damageSummary = new DamageSummary(
           0,
           (int)finalDamage,
           DamageSummarySource.Move,
           0,
           GetSimpleAdvantageTypeFromMult(advantageMultiplier),
           null
           );
        return damageSummary;
    }
    public BattleTypeAdvantageType GetSimpleAdvantageTypeFromMult(float multiplier)
    {
        if (multiplier > 1)
            return BattleTypeAdvantageType.superEffective;
        else if (multiplier < 1)
        {
            if (multiplier > 0)
                return BattleTypeAdvantageType.resists;
            else
                return BattleTypeAdvantageType.inmune;
        }
        return BattleTypeAdvantageType.normal;
    }
}
