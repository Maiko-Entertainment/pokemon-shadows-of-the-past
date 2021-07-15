﻿using System.Collections;
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

    private MoveEquipped move;
    private PokemonBattleData pokemon;

    public UIBattleMove Load(MoveEquipped move, PokemonBattleData pokemon)
    {
        this.move = move;
        this.pokemon = pokemon;

        moveName.text = move.move.moveName;
        usesLeft.text = (move.move.uses - move.timesUsed) + "/" + move.move.uses;
        TypeData type = BattleMaster.GetInstance().GetTypeData(move.move.typeId);
        typingImageBackground.color = type.color;
        typingIcon.sprite = type.icon;
        return this;
    }

    public void UseMove()
    {
        if (!seeOnly)
        {
            if (move.IsAvailable())
            {
                BattleMaster.GetInstance()
                    ?.GetCurrentBattle()
                    ?.HandleTurnInput(
                        new BattleTurnDesitionPokemonMove(
                            move, 
                            pokemon, 
                            BattleTeamId.Team1
                            )
                        );
                BattleAnimatorMaster.GetInstance().HideOptions();
            }
            else
            {
                BattleAnimatorMaster.GetInstance()?.ExecuteMoveNoUsesLeftFlowchart();
            }
        }
    }
}