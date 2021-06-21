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
    //public TextMeshProUGUI usesTotal;

    private MoveEquipped move;
    private PokemonBattleData pokemon;

    public void Load(MoveEquipped move, PokemonBattleData pokemon)
    {
        this.move = move;
        this.pokemon = pokemon;

        moveName.text = move.move.moveName;
        usesLeft.text = (move.move.uses - move.timesUsed) + "/" + move.move.uses;
        TypeData type = BattleMaster.GetInstance().GetTypeData(move.move.typeId);
        typingImageBackground.color = type.color;
        typingIcon.sprite = type.icon;
        // usesLeft.text = "" + move.move.uses;
    }

    public void UseMove()
    {
        // TO DO
        BattleMaster.GetInstance()
            ?.GetCurrentBattle()
            ?.HandleTurnInput(
                new BattleTurnDesitionPokemonMove(
                    move, 
                    pokemon, 
                    BattleTeamId.Team1
                    )
                );
    }
}
