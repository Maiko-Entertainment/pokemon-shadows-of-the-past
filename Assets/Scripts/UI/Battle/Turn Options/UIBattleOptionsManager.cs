using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleOptionsManager : MonoBehaviour
{
    public UIBattleMove movePrefab;

    public TransitionFade container;
    public Transform moveList;

    public void LoadMoves()
    {
        PokemonBattleData activePokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team1);
        CleanMoves();
        foreach(MoveEquipped me in activePokemon.GetPokemonCaughtData().GetAvailableMoves())
        {
            CreateMove(me, activePokemon);
        }
    }

    public void CreateMove(MoveEquipped me, PokemonBattleData pkmn)
    {
        Instantiate(movePrefab.gameObject, moveList).GetComponent<UIBattleMove>().Load(me, pkmn);
    }

    public void CleanMoves()
    {
        foreach (Transform moves in moveList)
            Destroy(moves.gameObject);
    }

    public void Hide()
    {
        container.gameObject.SetActive(false);
    }

    public void Show()
    {
        container.gameObject.SetActive(true);
        container.FadeIn();
    }
}
