using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleMovePicker : MonoBehaviour
{
    public UIBattleMove movePrefab;

    public Transform moveList;
    public TransitionFilledImage transition;

    public void Show()
    {
        transition.FadeIn();
        LoadMoves();
    }
    public void Hide()
    {
        transition.FadeOut();
    }

    private void LoadMoves()
    {
        PokemonBattleData activePokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team1);
        CleanMoves();
        foreach (MoveEquipped me in activePokemon.GetPokemonCaughtData().GetMoves())
        {
            CreateMove(me, activePokemon);
        }
    }

    private void CreateMove(MoveEquipped me, PokemonBattleData pkmn)
    {
        Instantiate(movePrefab.gameObject, moveList).GetComponent<UIBattleMove>().Load(me, pkmn);
    }

    private void CleanMoves()
    {
        foreach (Transform moves in moveList)
            Destroy(moves.gameObject);
    }

}
