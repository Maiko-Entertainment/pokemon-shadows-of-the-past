using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveLearnRequirement : MonoBehaviour
{
    public Image icon;
    public Image background;
    public TextMeshProUGUI moveName;
    public TextMeshProUGUI requirement;

    public delegate void Select(PokemonMoveLearn move);
    public event Select onSelect;
    public delegate void Click(PokemonMoveLearn move);
    public event Click onClick;

    public PokemonMoveLearn moveLearn;

    public UIMoveLearnRequirement Load(PokemonMoveLearn moveLearn)
    {
        this.moveLearn = moveLearn;
        MoveData move = moveLearn.move;
        int levelRequirement = moveLearn.levelRequired;

        TypeData type = move.GetMoveType();
        background.color = type.color;
        icon.sprite = type.icon;
        moveName.text = move.moveName;
        requirement.text = "At Lv: " + levelRequirement;
        return this;
    }

    public void HandleOnSelect()
    {
        onSelect?.Invoke(moveLearn);
    }
}
