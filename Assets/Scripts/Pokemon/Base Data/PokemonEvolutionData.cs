[System.Serializable]
public class PokemonEvolutionData
{
    public PokemonEvolutionType evolutionType;
    // Depending on the evolution type the value will be indicative of something different, such as a level, friendship, item, tradeItem.
    public float value;
}
