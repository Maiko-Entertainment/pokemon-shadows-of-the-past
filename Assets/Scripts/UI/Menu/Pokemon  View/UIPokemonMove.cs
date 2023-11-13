using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonMove : MonoBehaviour
{
    public TextMeshProUGUI moveName;
    public TextMeshProUGUI uses;
    public Image moveTypeIcon;
    public Image typingImageBackground;
    public TransitionFade selectedArrow;

    public delegate void Select(MoveEquipped move, PokemonCaughtData pkmn);
    public event Select onSelect;
    public delegate void Click(MoveEquipped move, PokemonCaughtData pkmn);
    public event Click onClick;

    public MoveEquipped move;
    private PokemonCaughtData pokemon;

    public UIPokemonMove Load(MoveEquipped move, PokemonCaughtData pokemon)
    {
        this.move = move;
        this.pokemon = pokemon;

        moveName.text = move.move.moveName;
        uses.text = (move.move.uses - move.timesUsed) + "/" + move.move.uses;
        TypeData type = BattleMaster.GetInstance().GetTypeData(move.move.GetMoveType().ToString()); // TODO: Update once moves use TypeData
        typingImageBackground.color = type.color;
        moveTypeIcon.sprite = type.icon;
        return this;
    }

    public void HandleClick()
    {
        onClick?.Invoke(move, pokemon);
    }
    public void HandleSelect()
    {
        onSelect?.Invoke(move, pokemon);
    }

    public void UpdateSelectedStatus(MoveEquipped move)
    {
        if (this.move == move)
        {
            selectedArrow?.FadeIn();
        }
        else
        {
            selectedArrow?.FadeOut();
        }
    }
}
