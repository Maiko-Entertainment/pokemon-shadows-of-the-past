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
    public Flowchart battleFlowchart;

    // Sounds Common
    public AudioClip expGainClip;
    public AudioClip levelUpClip;
    public AudioClip pokemonCaughtClip;
    public AudioClip pokemonFaintClip;
    public AudioClip pokemonExitPokeballClip;
    public AudioClip superEffectiveClip;
    public AudioClip weakClip;
    public AudioClip normalClip;
    public AudioClip pokeballThrowClip;

    //Anims Common
    public GameObject pokeballFrontReleaseAnimPrefab;
    public GameObject pokeballBackReleaseAnimPrefab;
    public GameObject pokeballGlow;
    public List<BattleAnimation> statDown = new List<BattleAnimation>();
    public List<BattleAnimation> statUp = new List<BattleAnimation>();

    // Utils
    public Material shadowMaterial;

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
            HideAll();
        }
    }

    public static BattleAnimatorMaster GetInstance() { return Instance; }

    public void LoadBattle()
    {
        ClearTeamPokemon(BattleTeamId.Team1);
        ClearTeamPokemon(BattleTeamId.Team2);
        combatCanvas.GetComponent<CanvasGroup>().interactable = true;
        combatCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void ClearTeamPokemon(BattleTeamId teamId, bool instant = false)
    {
        if (teamId == BattleTeamId.Team1)
        {
            foreach (Transform p in pokemonTeam1Position)
            {
                Destroy(p.gameObject);
            }
            HidePokemonInfo(BattleTeamId.Team1, instant);
        }
        else if (teamId == BattleTeamId.Team2)
        {
            foreach (Transform p in pokemonTeam2Position)
            {
                Destroy(p.gameObject);
            }
            HidePokemonInfo(BattleTeamId.Team2, instant);
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

    public float HandlePokemonEnterAnim(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleData bd = bm.GetBattleData();
        Transform teamTransform = GetPokemonTeamTransform(pokemon).parent;
        if(bd.battleType == BattleType.Pokemon && bm.GetTeamId(pokemon) == BattleTeamId.Team2)
        {
            TransitionSpriteChangeColor colorTrans = teamTransform.gameObject.AddComponent<TransitionSpriteChangeColor>();
            colorTrans.initialColor = Color.black;
            colorTrans.finalColor = Color.white;
            colorTrans.sprite = teamTransform.GetComponentInChildren<PokemonAnimationController>().sprite;
            colorTrans.sprite.color = colorTrans.initialColor;
            colorTrans.initialDelay = 1f;
            colorTrans.FadeIn();
            float time = 1 / colorTrans.speed + colorTrans.initialDelay;
            HandleCameraZoomPokemon(pokemon);
            HandleCameraReset(time);
            Destroy(colorTrans, time);
            return colorTrans.initialDelay;
        }
        else if (bd.battleType == BattleType.Shadow && bm.GetTeamId(pokemon) == BattleTeamId.Team2)
        {
            return 1f;
        }
        else
        {
            float time = 1 / 1.2f + 0.1f;
            StartCoroutine(HandlePokeballReleasePokemon(pokemon));
            if (bd.battleType == BattleType.Trainer || bm.GetTeamId(pokemon) == BattleTeamId.Team1)
            {
                time += 1.5f;
            }
            return time;
        }
    }

    private IEnumerator HandlePokeballReleasePokemon(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleData bd = bm.GetBattleData();
        Transform teamTransform = GetPokemonTeamTransform(pokemon).parent;
        BattleTeamId pokemonTeam = bm.GetTeamId(pokemon);

        TransitionSize pokemonIncreaseSize = teamTransform.gameObject.AddComponent<TransitionSize>();
        pokemonIncreaseSize.initialSize = Vector3.zero;
        pokemonIncreaseSize.finalSize = teamTransform.localScale;
        pokemonIncreaseSize.speed = 1.4f;
        teamTransform.localScale = Vector3.zero;

        if (bd.battleType == BattleType.Trainer || pokemonTeam == BattleTeamId.Team1)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject pokeballAnim;
            AudioMaster.GetInstance().PlaySfx(new AudioOptions(pokeballThrowClip));
            if (bm.GetTeamId(pokemon) == BattleTeamId.Team2)
            {
                pokeballAnim = Instantiate(pokeballFrontReleaseAnimPrefab);
            }
            else
            {
                pokeballAnim = Instantiate(pokeballBackReleaseAnimPrefab);
            }
            pokeballAnim.transform.position = teamTransform.position + Vector3.up * (pokemonTeam == BattleTeamId.Team1 ? 1.5f : 0.5f);
            TransitionSpriteFadeIn pokeballFade = pokeballAnim.AddComponent<TransitionSpriteFadeIn>();
            pokeballFade.spriteRenderer = pokeballAnim.GetComponent<SpriteRenderer>();
            pokeballFade.spriteRenderer.color = new Color(255, 255, 255, 0);
            pokeballFade.speed = 3f;
            // Add position Change to pokemon so it starts up
            TransitionMoveGameObject transitionMoveUpDown = teamTransform.gameObject.AddComponent<TransitionMoveGameObject>();
            transitionMoveUpDown.initialPosition = teamTransform.localPosition + Vector3.up * (pokemonTeam == BattleTeamId.Team1 ? 1f : 0.0f);
            transitionMoveUpDown.finalPosition = teamTransform.localPosition;
            transitionMoveUpDown.speed = 2f;
            teamTransform.localPosition = transitionMoveUpDown.initialPosition;
            pokeballFade.FadeIn();
            yield return new WaitForSeconds(1f);
            GameObject glow = Instantiate(pokeballGlow);
            glow.transform.position = pokeballFade.transform.position;
            pokeballFade.FadeOut();
            AudioMaster.GetInstance().PlaySfx(new AudioOptions(pokemonExitPokeballClip));
            yield return new WaitForSeconds(.25f);
            pokemonIncreaseSize.FadeIn();
            yield return new WaitForSeconds(.75f);
            Destroy(pokeballAnim);
            transitionMoveUpDown.FadeIn();
            yield return new WaitForSeconds(0.5f);
            Destroy(glow, 1f);
        }
        else
        {
            pokemonIncreaseSize.FadeIn();
        }
        Destroy(pokemonIncreaseSize, 1 / pokemonIncreaseSize.speed + 0.1f);
    }

    public float HandleCameraZoomPokemon(PokemonBattleData pokemon)
    {
        BattleTeamId pokemonTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        Transform position = GetPokemonTeamTransform(pokemon).parent;
        combatCamera.SetTarget(position.position + Vector3.up * (pokemonTeam == BattleTeamId.Team1 ? 1f : 0.6f));
        combatCamera.SetSizeTarget(pokemonTeam == BattleTeamId.Team1 ? pokemonZoomValue : pokemonZoomValue * 0.7f);
        return Mathf.Max(combatCamera.time, combatCamera.zoomTime);
    }

    public float HandleCameraZoomPokemon(PokemonBattleData pokemon, float sizeMultiplier)
    {
        BattleTeamId pokemonTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        Transform position = GetPokemonTeamTransform(pokemon).parent;
        combatCamera.SetTarget(position.position + Vector3.up * (pokemonTeam == BattleTeamId.Team1 ? 1f : 0.6f) * sizeMultiplier);
        combatCamera.SetSizeTarget((pokemonTeam == BattleTeamId.Team1 ? pokemonZoomValue : pokemonZoomValue * 0.7f) * sizeMultiplier);
        return Mathf.Max(combatCamera.time, combatCamera.zoomTime);
    }

    public float HandleCameraReset(float delay = 0)
    {
        StartCoroutine(HandleCameraResetEnum(delay));
        return Mathf.Max(combatCamera.time, combatCamera.zoomTime);
    }

    IEnumerator HandleCameraResetEnum(float delay=0)
    {
        yield return new WaitForSeconds(delay);
        combatCamera.ResetCamera();
    }

    public void HandleCameraIdle()
    {
        combatCamera.SetIdle(0f);
    }

    public void LoadPokemonsInfo(PokemonBattleData pokemon, int health)
    {
        List<StatusEffectData> minorData = new List<StatusEffectData>();
        foreach (StatusEffect s in pokemon.GetNonPrimaryStatus())
        {
            minorData.Add(GetStatusEffectData(s.effectId));
        }
        StatusEffect status = pokemon.GetCurrentPrimaryStatus();
        battleInfoManager.UpdatePokemonData(pokemon, health, GetStatusEffectData(status!=null ? status.effectId : StatusEffectId.None), minorData);
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
        BattleType battleType = BattleMaster.GetInstance().GetCurrentBattle().GetBattleData().battleType;
        float showDelay = battleType == BattleType.Trainer || teamId == BattleTeamId.Team1 ? 2f : 1f;
        bool isPokemonShadow = pokemon.GetPokemonCaughtData().isShadow;
        if (isPokemonShadow)
            pkmnInstance.GetComponentInChildren<SpriteRenderer>().material = BattleMaster.GetInstance().glitchMaterial;
        StartCoroutine(ShowPokemonInfoAfter(pokemon, showDelay));
        return pkmnInstance;
    }

    IEnumerator ShowPokemonInfoAfter(PokemonBattleData pokemon, float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdatePokemonInfo(pokemon, true);
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

    public void AddStatusChangeEvent(PokemonBattleData pokemon, int change, int priority = 0)
    {
        if (change > 0)
        {
            foreach(BattleAnimation ba in statUp)
            {
                BattleAnimatorEventPokemonMoveAnimation anim = new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, ba);
                anim.priority = priority;
                AddEvent(anim);
            }
        }
        else if (change < 0)
        {
            foreach (BattleAnimation ba in statDown)
            {
                BattleAnimatorEventPokemonMoveAnimation anim = new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, ba);
                anim.priority = priority;
                AddEvent(anim);
            }
        }
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

    public void AddEventBattleFlowcartPokemonText(string blockName, PokemonBattleData pokemon, Dictionary<string, string> additionalVariables = null, int priority = 0)
    {
        Dictionary<string, string> variables= new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } };
        if (additionalVariables != null)
        {
            foreach (string key in additionalVariables.Keys)
            {
                if (!variables.ContainsKey(key))
                {
                    variables.Add(key, additionalVariables[key]);
                }
            }
        }
        BattleAnimatorEventNarrative e = new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                blockName,
                variables
            ));
        e.priority = priority;
        AddEvent(e);
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
    public void AddEventBattleFlowcartGainLevelText(string pokemonName, int level)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Level Up",
                new Dictionary<string, string>() {
                    { "pokemon", ""+pokemonName },
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
    public void AddEventBattleFlowcartPokemonBoxSent(string pokemonName)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Pokemon Box Sent",
                new Dictionary<string, string>() {
                    { "pokemon", pokemonName },
                }
            ))
        );
    }
    public void AddEventBattleFlowcartEscapeFail(string trainer)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Battle Escape Fail",
                new Dictionary<string, string>() {
                    { "trainer", trainer },
                }
            ))
        );
    }
    public void AddEventBattleFlowcartNoEscape(string trainer)
    {
        AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlowchart,
                "Battle No Escape",
                new Dictionary<string, string>() {
                    { "trainer", trainer },
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
    public void ExecuteMissMoveFlowchart(BattleEventUseMove moveEvent)
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.SetStringVariable("pokemon", moveEvent.pokemon.GetName());
        battleFlowchart.SetStringVariable("move", moveEvent.move.moveName);
        battleFlowchart.ExecuteBlock("Move Miss");
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
    public void ExecuteBattleEscape(string trainerName)
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.SetStringVariable("trainer", trainerName);
        battleFlowchart.ExecuteBlock("Battle Escape");
    }
    public void ExecuteMoveNoUsesLeftFlowchart()
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.ExecuteBlock("Move No Uses");
    }
    public void ExecuteNoRunningFromTrainerFlowchart()
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.ExecuteBlock("Battle Escape Trainer");
    }

    public float UpdateHealthBar(PokemonBattleData pokemon, int target)
    {
        return battleInfoManager.UpdateHealthbar(pokemon, target);
    }

    public void UpdatePokemonStatus(PokemonBattleData pokemon, StatusEffectId primary, List<StatusEffectId> minor)
    {
        List<StatusEffectData> minorData = new List<StatusEffectData>();
        foreach(StatusEffectId m in minor)
        {
            minorData.Add(GetStatusEffectData(m));
        }
        battleInfoManager.UpdateStatus(pokemon, GetStatusEffectData(primary), minorData);
    }

    public float UpdateExpBar(PokemonBattleData pokemon, int target, int max)
    {
        return battleInfoManager.UpdateExpbar(pokemon, target, max);
    }

    public void UpdatePokemonLevel(PokemonBattleData pokemon, int level)
    {
        battleInfoManager.UpdatePokemonLevel(pokemon, level);
    }

    public void UpdatePokemonInfo(PokemonBattleData pokemon, bool showIfHidden = false)
    {
        battleInfoManager.UpdatePokemonInfo(pokemon, showIfHidden);
    }
    public void HidePokemonInfo(BattleTeamId team, bool instant = false)
    {
        battleInfoManager.HideTeamInfo(team, instant);
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

    public float ShowPokemonAbility(PokemonBattleData pokemon)
    {
        return battleInfoManager.ShowPokemonAbility(pokemon);
    }

    // Menus

    public void ShowTurnOptions()
    {
        battleOptionsManager.Show();
    }

    public void ShowPokemonSelection(bool allowClose = false)
    {
        battleOptionsManager.ShowPokemonSelector(allowClose);
        if (allowClose)
        {
            battleOptionsManager.isInSubmenu = true;
        }
    }
    public void HidePokemonSelection(bool preSelect = false)
    {
        battleOptionsManager.HidePokemonSelector(preSelect);
    }
    public void HideTacticSelection(bool preSelect = false)
    {
        battleOptionsManager.HideTacticSelector(preSelect);
    }

    public void ShowPokemonMoveSelection()
    {
        battleOptionsManager?.Show();
        battleOptionsManager?.ShowMoveSelector();
    }

    public void HidePokemonMoveSelection(bool preSelect = false)
    {
        battleOptionsManager?.HideMoveSelector(preSelect);
    }

    public void HideItemSelection(bool preSelect = false)
    {
       battleOptionsManager?.HideItemSelector(preSelect);
    }

    public void HideOptions()
    {
        HidePokemonSelection();
        HideItemSelection();
        HidePokemonMoveSelection();
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
