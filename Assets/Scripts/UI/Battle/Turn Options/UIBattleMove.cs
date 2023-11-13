using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleMove : MonoBehaviour
{
    public TextMeshProUGUI moveName;
    public TextMeshProUGUI usesLeft;
    public Image typingImageBackground;
    public Image typingIcon;
    public bool seeOnly;

    public delegate void Select(PokemonBattleData pkmn, MoveEquipped move);
    public event Select onSelect;
    public delegate void Click(PokemonBattleData pkmn, MoveEquipped move);
    public event Click onClick;

    private MoveEquipped move;
    private PokemonBattleData pokemon;

    public UIBattleMove Load(MoveEquipped move, PokemonBattleData pokemon)
    {
        this.move = move;
        this.pokemon = pokemon;

        moveName.text = move.move.moveName;
        usesLeft.text = (move.move.uses - move.timesUsed) + "/" + move.move.uses;
        TypeData type = BattleMaster.GetInstance().GetTypeData(move.move.GetMoveType().ToString()); // TODO: change to typeData
        typingImageBackground.color = type.color;
        typingIcon.sprite = type.icon;
        return this;
    }

    public void HandleClick()
    {
        onClick?.Invoke(pokemon, move);
    }
    public void HandleSelect()
    {
        onSelect?.Invoke(pokemon, move);
    }

}
