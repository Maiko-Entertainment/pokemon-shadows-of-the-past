using System.Collections.Generic;

[System.Serializable]
public class PersistedPokemon
{
    public string pokemonName;
    public PokemonBaseId pokemonId;
    public int level;
    public int experience = 0;
    public int damageTaken = 0;
    public StatusEffectId statusEffectId = StatusEffectId.None;
    public PokemonNatureId natureId;
    public AbilityId abilityId;
    public List<PersistedPokemonMove> moves = new List<PersistedPokemonMove>();
    public List<PersistedPokemonMove> learnedMoves = new List<PersistedPokemonMove>();


}
