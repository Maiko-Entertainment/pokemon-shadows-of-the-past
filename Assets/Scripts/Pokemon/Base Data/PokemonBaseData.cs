using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pokemon/BaseData")]
public class PokemonBaseData : ScriptableObject
{
    public PokemonBaseId pokemonId;
    public string species;
    public Sprite icon;
    public PokemonBaseStats baseStats;
    public List<PokemonTypeId> types;
    public List<PokemonBaseAbility> abilities;
    public float maleChance = 0.5f;
    public bool isGenderless = false;
    public float captureRate;
    public float baseFriendship;
    public List<PokemonBaseEvolution> evolutions;
    public PokemonAnimationController battleAnimation;
}
