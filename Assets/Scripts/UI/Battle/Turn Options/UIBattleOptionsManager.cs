using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIBattleOptionsManager : MonoBehaviour
{
    public AudioClip onSelectSound;
    public TransitionCanvasGroup container;
    public UIBattleMovePicker movesSelector;
    public UIItemsViewer itemSelector;
    public UIPokemonView pokemonSelector;
    public UIBattleOptionsTactics tacticSelector;
    public TextMeshProUGUI selectedTacticText;
    public TextMeshProUGUI tacticGaugeValue;
    public Transform optionsList;
    public Transform subMenuContainer;
    public TransitionCanvasGroup tacticContainer;
    public bool isInSubmenu = false;
    public float tacticsStoryProgressNeeded = 25;

    private UIItemsViewer itemSelectorInstance;
    private UIPokemonView pokemonSelectorInstance;
    private UIBattleOptionsTactics tacticSelectorInstance;

    private void Start()
    {
        List<Selectable> options = new List<Selectable>();
        foreach(Transform option in optionsList)
        {
            options.Add(option.GetComponent<Selectable>());
        }
        UtilsMaster.LineSelectables(options);
    }

    public void PlayOnSelectSound()
    {
        if(BattleMaster.GetInstance().GetCurrentBattle().IsBattleActive())
            AudioMaster.GetInstance().PlaySfx(onSelectSound);
    }

    public void ShowMoveSelector()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        PokemonBattleData pokemon = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        bool hasRepeatForce = false;
        StatusEffectRepeatMove rm = null;
        foreach(StatusEffect se in pokemon.GetNonPrimaryStatus())
        {
            if (se.effectId == StatusEffectId.RepeatMove)
            {
                hasRepeatForce = true;
                rm = (StatusEffectRepeatMove)se;
                break;
            }
        }
        if (hasRepeatForce)
        {
            bm?.HandleTurnInput(
                new BattleTurnDesitionPokemonMove(
                    new MoveEquipped(rm.forceMove),
                    bm.GetTeamActivePokemon(BattleTeamId.Team1),
                    BattleTeamId.Team1
                    )
                );
            BattleAnimatorMaster.GetInstance().HideOptions();
        }
        else
        {
            movesSelector.Show();
            isInSubmenu = true;
        }
    }
    public void HideMoveSelector(bool preSelect = false)
    {
        movesSelector.Hide();
        isInSubmenu = false; 
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(0).gameObject);
    }
    public void ShowTacticSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        tacticSelectorInstance = Instantiate(tacticSelector, subMenuContainer);
        isInSubmenu = true;
    }
    public void HideTacticSelector(bool preSelect = false)
    {
        tacticSelectorInstance.GetComponent<UIMenuPile>().Close();
        isInSubmenu = false;
        UpdateTacticSelectedText();
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(0).gameObject);
    }

    public void ShowItemPokemonSelector(ItemDataOnPokemon item)
    {
        isInSubmenu = true;
    }
    public void ShowPokemonSelector(bool allowClose = false)
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        pokemonSelectorInstance = Instantiate(pokemonSelector, subMenuContainer);
        pokemonSelectorInstance.SetPrefaint(!allowClose);
        isInSubmenu = true;
    }

    public void HidePokemonSelector(bool preSelect = false)
    {
        pokemonSelectorInstance?.GetComponent<UIMenuPile>().Close();
        pokemonSelectorInstance = null;
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(1).gameObject);
        else
        {
            UtilsMaster.SetSelected(null);
        }
        isInSubmenu = false;
    }
    public void ShowItemSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        itemSelectorInstance = Instantiate(itemSelector, subMenuContainer);
        isInSubmenu = true;
    }

    public void HideItemSelector(bool preSelect = false)
    {
        itemSelectorInstance?.GetComponent<UIMenuPile>().Close();
        itemSelectorInstance = null;
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(2).gameObject);
        isInSubmenu = false;
    }

    public void Hide()
    {
        container.FadeOut();
        UtilsMaster.SetSelected(null);
    }

    public void Show()
    {
        container.FadeIn();
        UtilsMaster.SetSelected(optionsList.GetChild(0).gameObject);
        UpdateTacticSelectedText();
        if (IsTacticActive())
        {
            tacticContainer.FadeIn();
        }
        else
        {
            tacticContainer.Hide();
        }
    }

    public void UpdateTacticSelectedText()
    {
        TacticData tactic = BattleMaster.GetInstance().GetCurrentBattle().GetSelectedTactic();
        BattleTeamData data = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1);
        if (tactic == null)
        {
            selectedTacticText.text = "Tactics";
            tacticGaugeValue.text = "" + data.tacticGauge;
        }
        else
        {
            selectedTacticText.text = ""+tactic.GetName();
            tacticGaugeValue.text = "" + (data.tacticGauge - tactic.GetCost());
        }
    }

    public void TryToEscape()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        switch (bm.GetBattleData().battleType)
        {
            case BattleType.Trainer:
                BattleAnimatorMaster.GetInstance().ExecuteNoRunningFromTrainerFlowchart();
                break;
            default:
                bm.HandleTurnInput(new BattleTurnDesitionRun(BattleTeamId.Team1));
                break;

        }
    }

    public void HandleCancel(CallbackContext context)
    {
        bool isBattleActive = BattleMaster.GetInstance().IsBattleActive();
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && isBattleActive)
        {
            if (!isInSubmenu)
            {
                UtilsMaster.SetSelected(optionsList.GetChild(optionsList.childCount - 1).gameObject);
            }
        }
    }
    public void HandleTacticsOpen(CallbackContext context)
    {
        bool isBattleActive = BattleMaster.GetInstance().IsBattleActive();
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && isBattleActive)
        {
            if (tacticSelectorInstance)
            {
                HideTacticSelector(true);
            }
            else if (!isInSubmenu && IsTacticActive())
            {
                ShowTacticSelector();
            }
        }
    }


    public bool IsTacticActive()
    {
        SaveElementNumber storyProgress = (SaveElementNumber)SaveMaster.Instance.GetSaveElement(SaveElementId.storyProgress);
        return ((float)storyProgress.GetValue() >= tacticsStoryProgressNeeded);
    }
}
