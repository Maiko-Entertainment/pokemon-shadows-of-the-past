[System.Serializable]
public class PokemonBaseEvolution
{
    public PokemonBaseId pokemonId;
    public PokemonEvolutionData evolutionData;

    public bool CanEvolve(PokemonCaughtData pokemon)
    {
        switch (evolutionData.evolutionType)
        {
            default:
                if (pokemon.GetLevel() >= evolutionData.value)
                {
                    return true;
                }
                break;
            case PokemonEvolutionType.friendship:
                if (pokemon.GetFriendship() >= evolutionData.value)
                {
                    return true;
                }
                break;
        }
        return false;
    }
}
