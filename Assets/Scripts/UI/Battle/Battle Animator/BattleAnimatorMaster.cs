using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleAnimatorMaster : MonoBehaviour
{
    public static BattleAnimatorMaster Instance;

    public CameraFollow combatCamera;
    public float pokemonZoomValue = 1;
    public Canvas combatCanvas;
    public Transform background;
    public Transform pokemonTeam1Position;
    public Transform pokemonTeam2Position;
    public UIBattleOptionsManager battleOptionsManager;
    public UIBattlePokemonInfoManager battleInfoManager;
    public UIBattlePokemonPickerManager battlePokemonPickerManager;
    public Flowchart battleFlowchart;

    // Sounds Common
    public AudioClip expGainClip;
    public AudioClip levelUpClip;
    public AudioClip pokemonCaughtClip;
    public AudioClip superEffectiveClip;

    public BattleAnimatorManager animatorManager = new BattleAnimatorManager();

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

    public void LoadBattle()
    {
        ClearTeamPokemon(BattleTeamId.Team1);
        ClearTeamPokemon(BattleTeamId.Team2);
    }

    private void ClearTeamPokemon(BattleTeamId teamId)
    {
        if (teamId == BattleTeamId.Team1)
        {
            foreach (Transform p in pokemonTeam1Position)
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
    }

    public void SetBackground(GameObject background)
    {
        foreach (Transform t in this.background)
            Destroy(t.gameObject);
        Instantiate(background, this.background);
    }

    public void SetTeamPokemon(PokemonBattleData pokemon, BattleTeamId teamId)
    {
        ClearTeamPokemon(teamId);
        InstantiatePokemonAnim(pokemon, teamId);
    }

    public void HandlePokemonEnterAnim(PokemonBattleData pokemon)
    {
        Transform teamTransform = GetPokemonTeamTransform(pokemon).parent;
        TransitionSize transition = teamTransform.gameObject.AddComponent<TransitionSize>();
        transition.initialSize = Vector3.zero;
        transition.finalSize = teamTransform.localScale;
        transition.speed = 1.2f;
        transition.FadeIn();
        Destroy(transition, 1 / transition.speed + 0.5f);
    }

    public float HandleCameraZoomPokemon(PokemonBattleData pokemon)
    {
        Transform position = GetPokemonTeamTransform(pokemon).parent;
        combatCamera.SetTarget(position.position);
        combatCamera.SetSizeTarget(pokemonZoomValue);
        return Mathf.Max(combatCamera.time, combatCamera.zoomTime);
    }

    public float HandleCameraReset()
    {
        combatCamera.ResetCamera();
        return Mathf.Max(combatCamera.time, combatCamera.zoomTime);
    }

    public void HandleCameraIdle()
    {
        combatCamera.SetIdle(0f);
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
        if (animatorManager.events.Count == 1)
        {
            GoToNextBattleAnim();
        }
    }

    public void AddEventInmuneTextEvent()
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Inmune"
            ))
        );
    }

    public void AddEventPokemonFaintText(PokemonBattleData pokemon)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Faint",
                new Dictionary<string, string>() { { "pokemon", pokemon .GetName() } }
            ))
        );
    }
    public void AddEventPokemonEnterText(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance()?.GetCurrentBattle();
        if (bm.battleData.battleType == BattleType.Trainer)
        {
            string trainerName = bm.GetTeamData(bm.GetTeamId(pokemon)).trainerTitle;
            AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(
                    battleFlowchart,
                    "Enter Trainer",
                    new Dictionary<string, string>() {
                        { "pokemon", pokemon.GetName() },
                        { "trainer", trainerName },
                    }
                ))
            );
        }
    }

    public void AddEventBattleFlowcartPokemonText(string blockName, PokemonBattleData pokemon)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                blockName,
                new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } }
            ))
        );
    }

    public void AddEventBattleFlowcartTrainerItemText(string trainerTitle, string itemName)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Use Item",
                new Dictionary<string, string>() { 
                    { "trainer", trainerTitle },
                    { "item", itemName },
                }
            ))
        );
    }
    public void AddEventBattleFlowcartGainExpText(int exp)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Exp Gain",
                new Dictionary<string, string>() {
                    { "exp", ""+exp },
                }
            ))
        );
    }
    public void AddEventBattleFlowcartGainLevelText(int level)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Level Up",
                new Dictionary<string, string>() {
                    { "level", ""+level },
                }
            ))
        );
    }

    public void AddEventBattleFlowcartCaptureFailText(string pokemonName)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Capture Fail",
                new Dictionary<string, string>() {
                    { "pokemon", pokemonName },
                }
            ))
        );
    }

    public void AddEventBattleFlowcartCaptureSuccessText(string pokemonName)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Capture Success",
                new Dictionary<string, string>() {
                    { "pokemon", pokemonName },
                }
            ))
        );
    }

    public void ClearEvents()
    {
        animatorManager.events.Clear();
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
    public void ExecuteEnemyTrainerDefeated(string trainerName)
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.SetStringVariable("trainer", trainerName);
        battleFlowchart.ExecuteBlock("Trainer Enemy Defeat");
    }
    public void ExecuteMoveNoUsesLeftFlowchart()
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.ExecuteBlock("Move No Uses");
    }

    public float UpdateHealthBar(PokemonBattleData pokemon, int target)
    {
        return battleInfoManager.UpdateHealthbar(pokemon, target);
    }

    public void UpdatePokemonStatus(PokemonBattleData pokemon, StatusEffectId id)
    {
        battleInfoManager.UpdateStatus(pokemon, GetStatusEffectData(id));
    }

    public float UpdateExpBar(PokemonBattleData pokemon, int target, int max)
    {
        return battleInfoManager.UpdateExpbar(pokemon, target, max);
    }

    public void UpdatePokemonLevel(PokemonBattleData pokemon, int level)
    {
        battleInfoManager.UpdatePokemonLevel(pokemon, level);
    }

    public void UpdatePokemonInfo(PokemonBattleData pokemon)
    {
        battleInfoManager.UpdatePokemonInfo(pokemon);
    }
    public void HidePokemonInfo(BattleTeamId team)
    {
        battleInfoManager.HideTeamInfo(team);
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

    // Moves Animations
    public Vector3 GetPokemonPosition(PokemonBattleData pokemon)
    {
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        if (teamId == BattleTeamId.Team1)
        {
            return pokemonTeam1Position.position + Vector3.up * 0.5f;
        }
        else
        {
            return pokemonTeam2Position.position + Vector3.up * 0.3f;
        }
    }

    public void HandlePokemonMoveAnim(PokemonBattleData pokemon, PokemonBattleData target, BattleAnimation animation)
    {
        float duration = animation.duration;
        Instantiate(animation.gameObject).GetComponent<BattleAnimation>().Execute(pokemon, target);
        GoToNextBattleAnim(duration);
    }

    public Transform GetPokemonTeamTransform(PokemonBattleData pokemon)
    {
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        if (teamId == BattleTeamId.Team1)
        {
            return pokemonTeam1Position.GetChild(0).transform;
        }
        else
        {
            return pokemonTeam2Position.GetChild(0).transform;
        }
    }

    // Menus

    public void ShowTurnOptions()
    {
        battleOptionsManager.Show();
    }

    public void ShowPokemonSelection()
    {
        battlePokemonPickerManager.ShowPokemonPicker();
    }
    public void HidePokemonSelection()
    {
        battlePokemonPickerManager.Hide();
    }

    public void ShowPokemonSelectionData(PokemonBattleData pokemon)
    {
        battlePokemonPickerManager?.ShowPokemonPreview(pokemon);
    }

    public void ShowPokemonMoveSelection()
    {
        battleOptionsManager?.Show();
        battleOptionsManager?.ShowMoveSelector();
    }

    public void HidePokemonMoveSelection()
    {
        battleOptionsManager?.HideMoveSelector();
    }

    public void HideItemSelection()
    {
        battleOptionsManager?.itemSelector.HideItemSelector();
    }

    public void HideOptions()
    {
        HidePokemonMoveSelection();
        HidePokemonSelection();
        HideItemSelection();
    }

    public void HideAll()
    {
        HideOptions();
        combatCanvas?.gameObject.SetActive(false);
    }

    public void ShowAll()
    {
        combatCanvas?.gameObject.SetActive(true);
    }
}
