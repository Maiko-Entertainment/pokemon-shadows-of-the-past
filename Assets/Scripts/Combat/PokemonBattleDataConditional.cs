using System.Collections.Generic;
[System.Serializable]
public class PokemonBattleDataConditional
{
    public List<PokemonBattleData> pokemon = new List<PokemonBattleData>();
    public List<SpawnConditionDataSaveValue> conditions = new List<SpawnConditionDataSaveValue>();

    protected bool MeetsConditions()
    {
        foreach (SpawnConditionDataSaveValue con in conditions)
        {
            if (!con.IsTrue())
                return false;
        }
        return true;
    }

    public List<PokemonBattleData> GetPokemon()
    {
        if (MeetsConditions())
        {
            return pokemon;
        }
        return new List<PokemonBattleData>();
    }
}
