using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleTeamData
{
    public string trainerTitle = "Trainer Name";
    public TrainerBrainData brain;
    public List<PokemonBattleData> pokemon = new List<PokemonBattleData>();
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
        foreach(PokemonBattleData p in pokemon)
        {
            pokemonNewInstance.Add(p.Copy());
        }
        BattleTeamData teamInstance = new BattleTeamData(trainerTitle, pokemonNewInstance, moneyPrice);
        teamInstance.teamStatus = teamStatus.DeepClone();
        teamInstance.brain = brain;
        return teamInstance;
    }
    public void InitiateTeam()
    {
        // SetActivePokemon(GetFirstAvailabelPokemon());
        brain?.Initialize(BattleMaster.GetInstance().GetCurrentBattle());
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
        foreach (PokemonBattleData p in pokemon)
        {
            if (!p.IsFainted())
                return p;
        }
        return null;
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
