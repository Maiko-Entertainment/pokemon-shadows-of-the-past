using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIBattleMovePicker : MonoBehaviour
{
    public Sprite physicalAttackIcon;
    public Sprite specialAttackIcon;
    public Sprite statusAttackIcon;
    public AudioClip onSelectedSound;
    public AudioClip onSubmitSound;

    public UIPokemonMove movePrefab;

    public TextMeshProUGUI moveDescription;
    public TextMeshProUGUI movePower;
    public TextMeshProUGUI moveAccuracy;
    public Image moveCategory;


    public Transform moveList;
    public TransitionFilledImage transition;

    private bool isOpen = false;

    public void Show()
    {
        transition.FadeIn();
        LoadMoves();
        isOpen = true;
    }
    public void Hide()
    {
        transition.FadeOut();
        isOpen = false;
    }

    private void LoadMoves()
    {
        PokemonBattleData activePokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(BattleTeamId.Team1);
        CleanMoves();
        List<MoveEquipped> moves = activePokemon.GetPokemonCaughtData().GetMoves();
        List<Selectable> options = new List<Selectable>();
        int count = 0;
        foreach (MoveEquipped me in moves)
        {
            UIPokemonMove bmp = CreateMove(me, activePokemon, count == 0);
            options.Add(bmp.GetComponent<Selectable>());
            bmp.onClick += (MoveEquipped move, PokemonCaughtData pkmn) => AudioMaster.GetInstance()?.PlaySfx(onSubmitSound);
            count++;
        }
        foreach (Transform option in moveList)
        {
            options.Add(option.GetComponent<Selectable>());
        }
        UtilsMaster.LineSelectables(options);
        UpdateSelected(moves[0]);
    }

    private UIPokemonMove CreateMove(MoveEquipped me, PokemonBattleData pkmn, bool select)
    {
        UIPokemonMove bm = Instantiate(movePrefab, moveList).Load(me, pkmn.GetPokemonCaughtData());
        bm.onClick += UseMove;
        bm.onSelect += (MoveEquipped me, PokemonCaughtData pkmn) => UpdateSelected(me);
        print("Child count " + moveList.childCount);
        if (select)
        {
            UtilsMaster.SetSelected(bm.gameObject);
        }
        return bm;
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
        MoveData m = me.move;
        moveDescription.text = m.description;
        movePower.text = "POW: " + (m.categoryId == MoveCategoryId.status ? "-" : "" + m.GetPower());
        moveAccuracy.text = "ACC: " + (m.alwaysHit ? "-" : "" + m.hitChance * 100 + "%");
        switch (m.GetAttackCategory())
        {
            case MoveCategoryId.physical:
                moveCategory.sprite = physicalAttackIcon;
                break;
            case MoveCategoryId.special:
                moveCategory.sprite = specialAttackIcon;
                break;
            default:
                moveCategory.sprite = statusAttackIcon;
                break;
        }
        AudioMaster.GetInstance()?.PlaySfx(onSelectedSound);
    }

    private void CleanMoves()
    {
        foreach (Transform moves in moveList)
            Destroy(moves.gameObject);
    }
    public void HandleCancel(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (isOpen)
            {
                BattleAnimatorMaster.GetInstance().HidePokemonMoveSelection(true);
            }
        }
    }
}
