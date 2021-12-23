using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBattleMovePicker : MonoBehaviour
{
    public UIPokemonMove movePrefab;

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
        List<MoveEquipped> moves = activePokemon.GetPokemonCaughtData().GetMoves();
        int count = 0;
        foreach (MoveEquipped me in moves)
        {
            CreateMove(me, activePokemon, count == 0);
            count++;
        }
        List<Selectable> options = new List<Selectable>();
        foreach (Transform option in moveList)
        {
            options.Add(option.GetComponent<Selectable>());
        }
        UtilsMaster.LineSelectables(options);
        UpdateSelected(moves[0]);
    }

    private void CreateMove(MoveEquipped me, PokemonBattleData pkmn, bool select)
    {
        UIPokemonMove bm = Instantiate(movePrefab, moveList).Load(me, pkmn.GetPokemonCaughtData());
        bm.onClick += UseMove;
        bm.onSelect += (MoveEquipped me, PokemonCaughtData pkmn) => UpdateSelected(me);
        print("Child count " + moveList.childCount);
        if (select)
        {
            EventSystem eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(bm.gameObject, new BaseEventData(eventSystem));
        }
    }
    public void UseMove(MoveEquipped move, PokemonCaughtData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (move.IsAvailable())
        {
            bm?.HandleTurnInput(
                    new BattleTurnDesitionPokemonMove(
                        move,
                        bm.GetTeamActivePokemon(BattleTeamId.Team1),
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
    public void UpdateSelected(MoveEquipped me)
    {
        foreach (Transform option in moveList)
        {
            option.GetComponent<UIPokemonMove>().UpdateSelectedStatus(me);
        }
    }

    private void CleanMoves()
    {
        foreach (Transform moves in moveList)
            Destroy(moves.gameObject);
    }

}
