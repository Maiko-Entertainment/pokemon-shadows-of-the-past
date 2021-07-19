using System.Collections.Generic;
[System.Serializable]
public class BattleTeamData
{
    public string trainerTitle = "Trainer Name";
    public TrainerBrainData brain;
    public List<PokemonBattleData> pokemon = new List<PokemonBattleData>();
    public int moneyPrice = 200;
    // Handles status that affect the whole team such as lightscreen
    public List<Status> teamStatus = new List<Status>();

    private PokemonBattleData activePokemon;

    public BattleTeamData(string trainerTitle, List<PokemonBattleData> pokemon, int moneyPrice)
    {
        this.trainerTitle = trainerTitle;
        this.pokemon = pokemon;
        this.moneyPrice = moneyPrice;
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
}
