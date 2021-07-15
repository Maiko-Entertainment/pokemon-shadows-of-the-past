using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleOptionsManager : MonoBehaviour
{

    public TransitionFilledImage container;
    public UIBattleMovePicker movesSelector;
    public UIBattleItemPickerManager itemSelector;


    public void ShowMoveSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        movesSelector.Show();
    }
    public void HideMoveSelector()
    {
        movesSelector.Hide();
    }

    public void ShowItemPokemonSelector(ItemDataOnPokemon item)
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        itemSelector.ShowPokemonList(item);
    }
    public void ShowItemSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        itemSelector.ShowItemSelector();
    }

    public void HideItemSelector()
    {
        itemSelector.HideItemSelector();
    }

    public void Hide()
    {
        container.FadeOut();
    }

    public void Show()
    {
        container.FadeIn();
    }
}
