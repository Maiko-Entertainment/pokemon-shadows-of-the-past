using System.Collections.Generic;

[System.Serializable]
public class PersistedPokemon
{
    public string pokemonName;
    public PokemonBaseId pokemonId;
    public string id;
    public int level;
    public int experience = 0;
    public int damageTaken = 0;
    public StatusEffectId statusEffectId = StatusEffectId.None;
    public PokemonNatureId natureId;
    public AbilityId abilityId;
    public string abilityIdString;
    public List<PersistedPokemonMove> moves = new List<PersistedPokemonMove>();
    public List<PersistedPokemonMove> learnedMoves = new List<PersistedPokemonMove>();
    public string equipedItem = "";
    public bool isShadow = false;
    public bool isMale = false;
    public float friendship = 0;

    public string GetId()
    {
        return string.IsNullOrEmpty(id) ? pokemonId.ToString() : id;
    }

    public string GetAbilityId()
    {
        return string.IsNullOrEmpty(abilityIdString) ? abilityId.ToString() : abilityIdString;
    }

}
