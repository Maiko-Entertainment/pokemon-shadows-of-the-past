using System.Collections.Generic;
[System.Serializable]
public class BattleTeamData
{
    public string trainerTitle = "Trainer Name";
    public List<PokemonBattleData> pokemon = new List<PokemonBattleData>();
    public int moneyPrice = 200;
    // Handles status that affect the whole team such as lightscreen
    public List<Status> teamStatus = new List<Status>();

    private PokemonBattleData activePokemon;

    public void InitiateTeam()
    {
        // SetActivePokemon(GetFirstAvailabelPokemon());
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
