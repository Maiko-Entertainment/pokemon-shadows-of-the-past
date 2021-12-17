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
            m.onClick -= ViewMove;
            Destroy(m.gameObject);
        }
        foreach(MoveEquipped m in moves)
        {
            UIPokemonMove uiMove = Instantiate(movePrefab, moveContainer).GetComponent<UIPokemonMove>().Load(m, pokemon);
            movesInstanced.Add(uiMove);
            uiMove.onSelect += ViewMove;
        }
    }

    public void HandleMoveView()
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(movesInstanced[0].gameObject, new BaseEventData(eventSystem));
        ViewMove(pokemon.GetMoves()[0], pokemon);
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
        foreach (UIPokemonMove listMove in movesInstanced)
        {
            listMove.UpdateSelectedStatus(currentMove);
        }
    }
}
