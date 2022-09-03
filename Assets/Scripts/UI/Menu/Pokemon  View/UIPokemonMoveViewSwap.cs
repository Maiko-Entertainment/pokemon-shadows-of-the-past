using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIPokemonMoveViewSwap : MonoBehaviour
{
    public UIPokemonMove movePrefab;
    public Transform otherMovesList;
    public ScrollRect scrollRect;

    public Sprite categoryPhysical;
    public Sprite categorySpecial;
    public Sprite categoryStatus;

    public TextMeshProUGUI swapMoveTitle;
    public TextMeshProUGUI swapMoveDescription;
    public TextMeshProUGUI swapMovePow;
    public TextMeshProUGUI swapMoveAcc;
    public Image swapMoveCategory;

    public TextMeshProUGUI currentMoveTitle;
    public TextMeshProUGUI currentMoveDescription;
    public TextMeshProUGUI currentMovePow;
    public TextMeshProUGUI currentMoveAcc;
    public Image currentMoveCategory;

    protected MoveEquipped currentMove = null;
    protected UIPokemonMovesViewer instance = null;
    protected PokemonCaughtData pokemon;
    protected List<Selectable> buttons;

    public void LoadPokemon(PokemonCaughtData pokemon, MoveEquipped currentMove, UIPokemonMovesViewer instance)
    {
        List<MoveEquipped> otherMoves = new List<MoveEquipped>();
        foreach (MoveEquipped move in pokemon.GetLearnedMoves())
        {
            if (!pokemon.GetMoves().Contains(move))
            {
                otherMoves.Add(move);
            }
        }
        foreach (MoveEquipped move in otherMoves)
        {
            UIPokemonMove uiMove = Instantiate(movePrefab, otherMovesList).Load(move, pokemon);
            uiMove.onSelect += OnViewMove;
            uiMove.onClick += HandleSwap;
        }
        buttons = new List<Selectable>(otherMovesList.GetComponentsInChildren<Selectable>());
        UtilsMaster.LineSelectables(buttons);
        if (buttons.Count > 0)
        {
            UtilsMaster.SetSelected(buttons[0].gameObject);
        }
        else
        {
            UtilsMaster.SetSelected(null);
        }
        LoadCurrentMove(currentMove);
        this.currentMove = currentMove;
        this.instance = instance;
        this.pokemon = pokemon;
    }

    public void OnViewMove(MoveEquipped move, PokemonCaughtData pokemon)
    {
        swapMoveTitle.text = move.move.moveName;
        swapMoveDescription.text = move.move.description;
        swapMovePow.text = "Pow: " + move.move.power;
        swapMoveAcc.text = "Acc: " + (move.move.hitChance * 100).ToString()+"%";
        if (move.move.GetAttackCategory() == MoveCategoryId.physical)
        {
            swapMoveCategory.sprite = categoryPhysical;
        }
        else if (move.move.GetAttackCategory() == MoveCategoryId.special)
        {
            swapMoveCategory.sprite = categorySpecial;
        }
        else
        {
            swapMoveCategory.sprite = categoryStatus;
        }
        foreach (RectTransform moveRect in otherMovesList)
        {
            UIPokemonMove pokemonSelected = moveRect.GetComponent<UIPokemonMove>();
            if (pokemonSelected.move == move)
                UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, moveRect);
        }
    }

    public void LoadCurrentMove(MoveEquipped move)
    {
        currentMoveTitle.text = move.move.moveName;
        currentMoveDescription.text = move.move.description;
        currentMovePow.text = "Pow: " + move.move.power;
        currentMoveAcc.text = "Acc: " + (move.move.hitChance * 100).ToString() + "%";
        currentMoveAcc.text = (move.move.hitChance * 100).ToString();
        if (move.move.GetAttackCategory() == MoveCategoryId.physical)
        {
            currentMoveCategory.sprite = categoryPhysical;
        }
        else if (move.move.GetAttackCategory() == MoveCategoryId.special)
        {
            currentMoveCategory.sprite = categorySpecial;
        }
        else
        {
            currentMoveCategory.sprite = categoryStatus;
        }
    }

    public void HandleSwap(MoveEquipped move, PokemonCaughtData pokemon)
    {
        pokemon.EquipMove(move, currentMove);
        HandleClose();
        instance.Load(pokemon);
    }

    public void HandleClose()
    {
        if (UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>())
        {
            UIPauseMenuMaster.GetInstance()?.CloseCurrentMenu();
            instance.Load(pokemon);
        }
    }

    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            HandleClose();
        }
    }
}
