using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokedexSectionMoves : MonoBehaviour
{
    public Sprite physicalAttackIcon;
    public Sprite specialAttackIcon;
    public Sprite statusAttackIcon;

    public UIMoveLearnRequirement moveLearnPrefab;
    public TextMeshProUGUI helpText;

    public TextMeshProUGUI moveName;
    public TextMeshProUGUI moveDescription;
    public TextMeshProUGUI movePower;
    public TextMeshProUGUI moveAccuracy;
    public Image category;

    public Transform moveList;
    public ScrollRect scrollRect;
    public GameObject moveData;

    private List<UIMoveLearnRequirement> movesInstanced = new List<UIMoveLearnRequirement>();
    public UIPokedexSectionMoves Load(PokedexPokemonData pokemonData)
    {
        PokemonBaseData pokemon = pokemonData.pokemon;
        foreach (Transform stat in moveList)
        {
            Destroy(stat.gameObject);
        }
        if (pokemonData.caughtAmount > 0)
        {
            moveData.SetActive(true);
            scrollRect.gameObject.SetActive(true);
            helpText.text = "";
            movesInstanced = new List<UIMoveLearnRequirement>();

            foreach (PokemonMoveLearn m in pokemon.levelUpMoves)
            {
                UIMoveLearnRequirement uiMove = Instantiate(moveLearnPrefab, moveList).Load(m);
                movesInstanced.Add(uiMove);
                uiMove.onSelect += ViewMove;
            }
            UtilsMaster.LineSelectables(new List<Selectable>(moveList.GetComponentsInChildren<Selectable>()));
            StartCoroutine(UtilsMaster.SetSelectedNextFrame(movesInstanced[0].gameObject));
        }
        else
        {
            helpText.text = "Catch this pokemon to learn more about it.";
            moveData.SetActive(false);
            scrollRect.gameObject.SetActive(false);
        }
        return this;
    }

    public void ViewMove(PokemonMoveLearn learnMove)
    {
        foreach (RectTransform uiMove in moveList)
        {
            if (uiMove.GetComponent<UIMoveLearnRequirement>().moveLearn == learnMove)
            {
                UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, uiMove);
            }
        }
        MoveData m = learnMove.move;
        moveName.text = m.moveName;
        moveDescription.text = m.description;
        movePower.text = "POW: " + (m.categoryId == MoveCategoryId.status ? "-" : "" + m.GetPower());
        moveAccuracy.text = "ACC: " + (m.alwaysHit ? "-" : "" + m.hitChance * 100 + "%");
        switch (m.GetAttackCategory())
        {
            case MoveCategoryId.physical:
                category.sprite = physicalAttackIcon;
                break;
            case MoveCategoryId.special:
                category.sprite = specialAttackIcon;
                break;
            default:
                category.sprite = statusAttackIcon;
                break;
        }
    }
}
