using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OutOfCombatDamage
{
    public int damagePower;
    public MoveCategoryId moveCategory;
    public PokemonTypeId type = PokemonTypeId.Normal;
}
