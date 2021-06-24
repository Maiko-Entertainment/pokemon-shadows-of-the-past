using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleAnimatorMaster : MonoBehaviour
{
    public static BattleAnimatorMaster Instance;

    public Transform background;
    public Transform pokemonTeam1Position;
    public Transform pokemonTeam2Position;
    public UIBattleOptionsManager battleOptionsManager;
    public UIBattlePokemonInfoManager battleInfoManager;
    public UIBattlePokemonPickerManager battlePokemonPickerManager;
    public Flowchart battleFlowchart;

    public BattleAnimatorManager animatorManager = new BattleAnimatorManager();
    public BattleManager currentBattle;

    public List<StatusEffectData> statusEffectData = new List<StatusEffectData>();

    public void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static BattleAnimatorMaster GetInstance() { return Instance; }

    public void LoadBattle(BattleManager battleState)
    {
        battleOptionsManager?.LoadMoves();
        //battleInfoManager?.UpdateInfo(battleState);
    }

    public void SetTeamPokemon(PokemonBattleData pokemon, BattleTeamId teamId)
    {
        if (teamId == BattleTeamId.Team1)
        {
            foreach(Transform p in pokemonTeam1Position)
            {
                Destroy(p.gameObject);
            }
            
        }
        else if (teamId == BattleTeamId.Team2)
        {
            foreach (Transform p in pokemonTeam2Position)
            {
                Destroy(p.gameObject);
            }
        }
        InstantiatePokemonAnim(pokemon, teamId);
    }

    public void LoadPokemonsInfo(PokemonBattleData pokemon, int health, StatusEffect status)
    {
        battleInfoManager.UpdatePokemonData(pokemon, health, GetStatusEffectData(status!=null ? status.effectId : StatusEffectId.None));
    }

    public PokemonAnimationController InstantiatePokemonAnim(PokemonBattleData pokemon, BattleTeamId teamId)
    {
        PokemonAnimationController pkmnInstance = Instantiate(
            pokemon.GetPokemonCaughtData().GetPokemonBaseData()
            .battleAnimation.gameObject, teamId == BattleTeamId.Team1 ? pokemonTeam1Position : pokemonTeam2Position)
            .GetComponent<PokemonAnimationController>();
        if (teamId == BattleTeamId.Team1)
        {
            pkmnInstance.TriggerBack();
        }

        return pkmnInstance;
    }

    public void UpdatePokemonHealthbarInfo(PokemonBattleData pokemon)
    {

    }

    public void GoToNextBattleAnim(float seconds=0)
    {
        StartCoroutine(TriggerNextEvent(seconds));
    }
    IEnumerator TriggerNextEvent(float seconds=0)
    {
        yield return new WaitForSeconds(seconds);
        animatorManager.TriggerNextEvent();
    }

    public void AddEvent(BattleAnimatorEvent newEvent)
    {
        animatorManager.AddEvent(newEvent);
    }

    public void AddEventInmuneTextEvent()
    {
        animatorManager.AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Inmune"
            ))
        );
    }

    public void AddEventPokemonFaintText(PokemonBattleData pokemon)
    {
        animatorManager.AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Faint",
                new Dictionary<string, string>() { { "pokemon", pokemon .GetName() } }
            ))
        );
    }

    public void AddEventBattleFlowcartPokemonText(string blockName, PokemonBattleData pokemon)
    {
        animatorManager.AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                blockName,
                new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } }
            ))
        );
    }


    // Must only be used by Anim events when they are executed
    public void ExecuteMoveFlowchart(BattleEventUseMove moveEvent)
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.SetStringVariable("pokemon", moveEvent.pokemon.GetName());
        battleFlowchart.SetStringVariable("move", moveEvent.move.moveName);
        battleFlowchart.ExecuteBlock("Move Use");
    }

    public void ExecuteEffectivenessFlowchart(BattleTypeAdvantageType advantageType)
    {
        switch (advantageType)
        {
            case BattleTypeAdvantageType.superEffective:
                battleFlowchart.ExecuteBlock("Super Effective");
                break;
            case BattleTypeAdvantageType.resists:
                battleFlowchart.ExecuteBlock("Resistant");
                break;
            case BattleTypeAdvantageType.inmune:
                battleFlowchart.ExecuteBlock("Inmune");
                break;
            default:
                GoToNextBattleAnim();
                break;
        }
    }

    public float UpdateHealthBar(PokemonBattleData pokemon, int target)
    {
        return battleInfoManager.UpdateHealthbar(pokemon, target);
    }

    public void UpdatePokemonStatus(PokemonBattleData pokemon, StatusEffectId id)
    {
        battleInfoManager.UpdateStatus(pokemon, GetStatusEffectData(id));
    }

    public StatusEffectData GetStatusEffectData(StatusEffectId id)
    {
        foreach (StatusEffectData se in statusEffectData)
        {
            if (se.statusId == id)
            {
                return se;
            }
        }
        return null;
    }

    public void ShowTurnOptions()
    {
        battleOptionsManager.Show();
    }


    public void ShowPokemonSelection()
    {
        battlePokemonPickerManager.ShowPokemonPicker();
    }
    public void ClosePokemonSelection()
    {
        battlePokemonPickerManager.Hide();
    }

    public void ShowPokemonSelectionData(PokemonBattleData pokemon)
    {
        battlePokemonPickerManager?.ShowPokemonPreview(pokemon);
    }

    public void ShowPokemonMoveSelection()
    {
        battleOptionsManager?.LoadMoves();
        battleOptionsManager?.ShowMoveSelector();
    }

    public void HidePokemonMoveSelection()
    {
        battleOptionsManager?.HideMoveSelector();
    }

    public void HideOptions()
    {
        HidePokemonMoveSelection();
        ClosePokemonSelection();
    }
}
