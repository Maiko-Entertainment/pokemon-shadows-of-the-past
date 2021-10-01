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

    public delegate void Hover(MoveEquipped move, PokemonCaughtData pkmn);
    public event Hover onHover;
    public delegate void Click(MoveEquipped move, PokemonCaughtData pkmn);
    public event Click onClick;

    private MoveEquipped move;
    private PokemonCaughtData pokemon;

    public UIPokemonMove Load(MoveEquipped move, PokemonCaughtData pokemon)
    {
        this.move = move;
        this.pokemon = pokemon;

        moveName.text = move.move.moveName;
        uses.text = (move.move.uses - move.timesUsed) + "/" + move.move.uses;
        TypeData type = BattleMaster.GetInstance().GetTypeData(move.move.typeId);
        typingImageBackground.color = type.color;
        moveTypeIcon.sprite = type.icon;
        return this;
    }

    public void HandleClick()
    {
        onClick?.Invoke(move, pokemon);
    }
}
