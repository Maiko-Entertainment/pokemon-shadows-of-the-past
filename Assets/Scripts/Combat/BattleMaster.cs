using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMaster : MonoBehaviour
{
    public static BattleMaster Instance { get; set; }
    public Flowchart battleFlowchart;

    public BattleManager currenBattle;
    public bool triggerBattleOnStart = false;

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

    public BattleManager GetCurrentBattle()
    {
        return currenBattle;
    }

    public void RunTestBattle()
    {
        Debug.Log("Started test battle");
        GetCurrentBattle().StartBattle();
    }

    public void RunPokemonBattle(PokemonBattleData pokemon, BattleData battleData)
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        BattleTeamData team1 = new BattleTeamData("Player", GetPokemonBattleDataFromCaught(party), 0);
        BattleTeamData team2 = new BattleTeamData(pokemon.GetName(), new List<PokemonBattleData>() { pokemon }, 0);
        BattleManager newBattle = new BattleManager(team1, team2, battleData);
        currenBattle = newBattle;
        GetCurrentBattle().StartBattle();
        BattleAnimatorMaster.GetInstance().ShowAll();
    }

    public void SetPostBattleEvent(BattleEndEvent postBattleEvent)
    {
        GetCurrentBattle()?.SetOnEndEvent(postBattleEvent);
    }

    public void RunTrainerBattle(TrainerCombatData trainer)
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        BattleTeamData team1 = new BattleTeamData("Player", GetPokemonBattleDataFromCaught(party), 0); ;
        BattleTeamData team2 = trainer.GetTeambattleData();
        BattleManager newBattle = new BattleManager(team1, team2, trainer.battleData);
        currenBattle = newBattle;
        GetCurrentBattle().StartBattle();
        BattleAnimatorMaster.GetInstance().ShowAll();
    }

    public void AddBattleEndFlowchart()
    {

    }

    public static List<PokemonBattleData> GetPokemonBattleDataFromCaught(List<PokemonCaughtData> party)
    {
        List<PokemonBattleData> battleParty = new List<PokemonBattleData>();
        foreach(PokemonCaughtData pokemon in party)
        {
            battleParty.Add(new PokemonBattleData(pokemon));
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
        float baseExp = pokemon.GetPokemonCaughtData().pokemonBase.baseExp;
        int pokemonLevel = pokemon.GetPokemonCaughtData().GetLevel();
        int experienceGained = (int)(baseExp * pokemonLevel / 2);
        return experienceGained;
    }

    public void ClearBattleEvents()
    {
        currenBattle.eventManager.ClearEvents();
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
        float baseDamage = 2 + (2 * pokemon.GetLevel() + 10f) / defense * damage.damagePower;
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
