using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPokemonMovesViewer : MonoBehaviour
{
    public Sprite physicalAttackIcon;
    public Sprite specialAttackIcon;
    public Sprite statusAttackIcon;
    public UIPokemonMove movePrefab;
    public UIMenuPile swapMovePrefab;

    public Transform moveContainer;

    public TransitionBase moveInspector;
    public TextMeshProUGUI moveName;
    public TextMeshProUGUI moveDescription;
    public TextMeshProUGUI movePower;
    public TextMeshProUGUI moveAccuracy;
    public Image moveCategory;

    private PokemonCaughtData pokemon;
    private List<UIPokemonMove> movesInstanced = new List<UIPokemonMove>();
    private MoveEquipped currentMove;
    private bool isInsideSection = false;

    public void Load(PokemonCaughtData pokemon)
    {
        this.pokemon = pokemon;
        LoadEquippedMoves();
    }

    public void LoadEquippedMoves()
    {
        List<MoveEquipped> moves = pokemon.GetMoves();
        foreach(UIPokemonMove m in movesInstanced)
        {
            m.onSelect -= ViewMove;
            m.onClick -= HandleSwapMenu;
            Destroy(m.gameObject);
        }
        movesInstanced = new List<UIPokemonMove>();
        foreach (MoveEquipped m in moves)
        {
            UIPokemonMove uiMove = Instantiate(movePrefab, moveContainer).GetComponent<UIPokemonMove>().Load(m, pokemon);
            movesInstanced.Add(uiMove);
            uiMove.onSelect += ViewMove;
            uiMove.onClick += HandleSwapMenu;
        }
        UtilsMaster.LineSelectables(new List<Selectable>(moveContainer.GetComponentsInChildren<Selectable>()));
        UtilsMaster.SetSelected(movesInstanced[0].gameObject);
    }

    public void ViewMove(MoveEquipped move, PokemonCaughtData pkmn)
    {
        currentMove = move;
        moveInspector?.FadeIn();
        MoveData m = move.move;
        moveName.text = m.moveName;
        moveDescription.text = m.description;
        movePower.text = "POW: "+(m.categoryId == MoveCategoryId.status ? "-" : ""+m.GetPower());
        moveAccuracy.text = "ACC: " + (m.alwaysHit ? "-" : ""+m.hitChance * 100);
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
    }

    public void HandleSwapMenu(MoveEquipped move, PokemonCaughtData pkmn)
    {
        UIPauseMenuMaster.GetInstance().OpenMenu(swapMovePrefab, true).GetComponent<UIPokemonMoveViewSwap>().LoadPokemon(pkmn,move, this);
    }
}
