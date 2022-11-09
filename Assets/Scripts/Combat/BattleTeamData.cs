using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleTeamData
{
    public string trainerTitle = "Trainer Name";
    public TrainerBrainData brain;
    public List<PokemonBattleData> pokemon = new List<PokemonBattleData>();
    public List<PokemonBattleDataConditional> conditionalPokemon = new List<PokemonBattleDataConditional>();
    public List<PokemonBattleData> allyPokemon = new List<PokemonBattleData>();
    public int moneyPrice = 200;
    // Handles status that affect the whole team such as lightscreen
    public List<Status> teamStatus = new List<Status>();
    public List<TacticData> tactics = new List<TacticData>();
    public int tacticGauge = 1;

    private PokemonBattleData activePokemon;

    public BattleTeamData(string trainerTitle, List<PokemonBattleData> pokemon, int moneyPrice, List<TacticData> tactics = null)
    {
        this.trainerTitle = trainerTitle;
        this.pokemon = pokemon;
        this.moneyPrice = moneyPrice;
        if (tactics != null)
        {
            this.tactics = tactics;
        }
    }

    public BattleTeamData Copy()
    {
        List<PokemonBattleData> pokemonNewInstance = new List<PokemonBattleData>();
        foreach(PokemonBattleData p in GetTeam())
        {
            pokemonNewInstance.Add(p.Copy());
        }
        BattleTeamData teamInstance = new BattleTeamData(trainerTitle, pokemonNewInstance, moneyPrice);
        teamInstance.teamStatus = teamStatus.DeepClone();
        teamInstance.brain = brain;
        return teamInstance;
    }
    public void InitiateTeam(BattleTeamId teamId)
    {
        brain?.Initialize(BattleMaster.GetInstance().GetCurrentBattle());
        int index = 0;
        foreach(PokemonBattleData pkmn in GetTeam())
        {
            pkmn.battleId = (teamId == BattleTeamId.Team1 ? 0 : 100) + index;
            index++;
        }

        index = 0;
        foreach (PokemonBattleData pkmn in allyPokemon)
        {
            pkmn.battleId = (teamId == BattleTeamId.Team1 ? 200 : 300) + index;
            index++;
        }
    }

    public string GetTrainerTitle()
    {
        return trainerTitle;
    }

    public PokemonBattleData GetActivePokemon()
    {
        return activePokemon;
    }

    public PokemonBattleData GetFirstAvailabelPokemon()
    {
        foreach (PokemonBattleData p in GetTeam())
        {
            if (!p.IsFainted())
                return p;
        }
        return null;
    }
    public List<PokemonBattleData> GetAvailablePokemon()
    {
        List<PokemonBattleData> availableList = new List<PokemonBattleData>();
        foreach (PokemonBattleData p in GetTeam())
        {
            if (!p.IsFainted())
                availableList.Add(p);
        }
        return availableList;
    }
    public List<PokemonBattleData> GetTeam()
    {
        List<PokemonBattleData> completeTeam = new List<PokemonBattleData>(pokemon);
        foreach(PokemonBattleDataConditional pokeCondition in conditionalPokemon)
        {
            completeTeam.AddRange(pokeCondition.GetPokemon());
        }
        return completeTeam;
    }

    public void SetActivePokemon(PokemonBattleData pokemon)
    {
        activePokemon = pokemon;
        pokemon?.Initiate();
    }

    public void IncreaseTacticGauge(int increase)
    {
        tacticGauge = Mathf.Clamp(tacticGauge + increase, 0, 10);
    }
}
