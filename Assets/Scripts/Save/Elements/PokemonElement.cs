using System.Collections.Generic;

[System.Serializable]
public class PokemonElement {
    public string pokemonName;
    public PokemonBaseId pokemonId;
    //public string id;
    public int level;
    public int experience = 0;
    public int damageTaken = 0;
    public StatusEffectId statusEffectId = StatusEffectId.None;
    public PokemonNatureId natureId;
    public AbilityId abilityId;
    public List<PokemonMoveElement> moves = new List<PokemonMoveElement>();
    public List<PokemonMoveElement> learnedMoves = new List<PokemonMoveElement>();
    public ItemId equipedItem = ItemId.None;
    public bool isShadow = false;
    public bool isMale = false;
    public float friendship = 0;
}