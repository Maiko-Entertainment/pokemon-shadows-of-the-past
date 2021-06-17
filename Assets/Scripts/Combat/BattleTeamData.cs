using System.Collections.Generic;
[System.Serializable]
public class BattleTeamData
{
    public List<PokemonBattleData> pokemon = new List<PokemonBattleData>();
    public int moneyPrice = 200;
    // Handles status that affect the whole team such as lightscreen
    public List<Status> teamStatus = new List<Status>();

    public PokemonBattleData GetActivePokemon()
    {
        foreach (PokemonBattleData p in pokemon)
        {
            if (!p.IsFainted())
                return p;
        }
        return null;
    }
}
