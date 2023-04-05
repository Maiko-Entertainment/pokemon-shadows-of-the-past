using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonPCSwap : MonoBehaviour
{
    public Image icon;
    
    public PokemonCaughtData pokemon;

    public UIPokemonPCSwap Load(PokemonCaughtData pokemon)
    {
        this.pokemon = pokemon;
        icon.sprite = pokemon.GetPokemonBaseData().icon;
        return this;
    }

    public void SetDestiny(Transform position)
    {
        transform.position = position.position;
    }
}
