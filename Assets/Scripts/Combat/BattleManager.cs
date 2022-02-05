using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleManager
{
    public BattleTeamData team1;
    public BattleTeamData team2;
    public BattleData battleData;
    public Status weather = null;
    public int turnsPassed = 0;
    public BattleEndEvent postBattleEvent;

    public BattleEventManager eventManager = new BattleEventManager();

    private List<PokemonBattleData> participatedPokemon = new List<PokemonBattleData>();

    public MoveData lastUsedMove = null;

    private bool isBattleActive = false;
    private TacticData currentTacticSelected;

    public static int BASE_FRIENDSHIP_GAINED_PER_TAKEDOWN = 10;


    public BattleManager(BattleTeamData player, BattleTeamData opponent, BattleData battleData)
    {
        team1 = player;
        team2 = opponent;
        this.battleData = battleData;
    }

    public void SetOnEndEvent(BattleEndEvent postBattleEvent)
    {
        this.postBattleEvent = postBattleEvent;
    }

    public void StartBattle()
    {
        isBattleActive = true;
        eventManager = new BattleEventManager();
        participatedPokemon = new List<PokemonBattleData>();
        BattleAnimatorMaster.GetInstance()?.SetBackground(battleData.battlebackground);
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTurnStart());
        team1.InitiateTeam();
        team2.InitiateTeam();
        SetTeamActivePokemon(team1.GetFirstAvailabelPokemon());
        SetTeamActivePokemon(team2.GetFirstAvailabelPokemon());

        BattleAnimatorMaster.GetInstance().LoadBattle();

        eventManager.ResolveAllEventTriggers();
        // BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
    }

    public bool IsBattleActive()
    {
        return isBattleActive;
    }

    public void HandleTurnInput(BattleTurnDesition desition)
    {
        BattleTurnDesition AIDesition = HandleAIInput();
        HandleRoundEnd();
        int desitionPriority = desition.priority;
        int tacticPriority = currentTacticSelected ? 4 : -1;
        int aiDesitionPriority = AIDesition.priority;
        int aiTacticPriority = AIDesition.tactic ? 4 : -1;
        for(int priority = 0; priority < 10; priority++)
        {
            if (aiDesitionPriority == priority)
            {
                AIDesition.Execute();
                AddEvent(new BattleEventDestion(AIDesition));
            }
            if (desitionPriority == priority)
            {
                desition.Execute();
                AddEvent(new BattleEventDestion(desition));
            }
            if (aiTacticPriority == priority)
            {
                AddTacticEvent(AIDesition.tactic, BattleTeamId.Team2);
            }
            if (tacticPriority == priority)
            {
                AddTacticEvent(currentTacticSelected, BattleTeamId.Team1);
                GetTeamData(BattleTeamId.Team1).IncreaseTacticGauge(-1 * currentTacticSelected.GetCost());
            }
        }
        HandleDesitions();
        BattleAnimatorMaster.GetInstance().battleOptionsManager.Hide();
    }

    public void HandleRoundEnd()
    {
        eventManager.AddEvent(new BattleEventRoundEnd());
    }

    public BattleTurnDesition HandleAIInput()
    {
        if (team2.brain)
        {
            return team2.brain.GetTurnDesition(this);
        }
        PokemonBattleData team2Pokemon = team2.GetActivePokemon();
        List<MoveEquipped> moves = team2Pokemon.GetPokemonCaughtData().GetAvailableMoves();
        if (moves.Count > 0)
        {
            int randomIndex = Random.Range(0, moves.Count);
            MoveEquipped move = moves[randomIndex];

            return new BattleTurnDesitionPokemonMove(move, team2Pokemon, BattleTeamId.Team2);
        }
        return null;
    }

    public PokemonBattleData GetTeamActivePokemon(BattleTeamId teamId)
    {
        return teamId == BattleTeamId.Team1 ? team1.GetActivePokemon() : team2.GetActivePokemon();
    }
    public PokemonBattleData GetPokemonFromCaughtData(PokemonCaughtData pkmn)
    {
        foreach(PokemonBattleData pbd in GetTeamData(BattleTeamId.Team1).pokemon)
        {
            if (pbd.GetPokemonCaughtData() == pkmn)
                return pbd;
        }
        foreach (PokemonBattleData pbd in GetTeamData(BattleTeamId.Team2).pokemon)
        {
            if (pbd.GetPokemonCaughtData() == pkmn)
                return pbd;
        }
        return null;
    }

    public void SetTeamActivePokemon(PokemonBattleData pokemon)
    {
        BattleTeamId teamId = GetTeamId(pokemon);

        if (teamId == BattleTeamId.Team1)
        {
            team1.SetActivePokemon(pokemon);
            if (!participatedPokemon.Contains(pokemon))
                participatedPokemon.Add(pokemon);
        }
        else
        {
            team2.SetActivePokemon(pokemon);
        }
        AddPokemonEnterEvent(pokemon);
    }

    public BattleTeamId GetTeamId(PokemonBattleData pokemon)
    {
        if (team1.pokemon.Contains(pokemon))
            return BattleTeamId.Team1;
        if (team2.pokemon.Contains(pokemon))
            return BattleTeamId.Team2;
        return BattleTeamId.None;
    }

    public BattleTeamData GetTeamData(BattleTeamId teamId)
    {
        if (teamId == BattleTeamId.Team1)
        {
            return team1;
        }
        else if (teamId == BattleTeamId.Team2)
        {
            return team2;
        }
        return null;
    }

    public void HandleDesitions()
    {
        eventManager.ResolveAllEventTriggers();
        CheckForFainted();
        // BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
    }

    public void CheckForFainted()
    {
        PokemonBattleData pokemon2 = GetTeamActivePokemon(BattleTeamId.Team2);
        PokemonBattleData pokemon = GetTeamActivePokemon(BattleTeamId.Team1);
        if (pokemon.IsFainted() || pokemon2.IsFainted())
        {
            CheckForBattleEnd();
        }
        if (pokemon2.IsFainted())
        {
            HandleExpGain(pokemon2);
            participatedPokemon.Clear();
            PokemonBattleData newPokemon = team2.GetFirstAvailabelPokemon();
            if (newPokemon != null)
                SetTeamActivePokemon(newPokemon);
        }
        if (pokemon.IsFainted())
        {
            PokemonBattleData newPokemon = team1.GetFirstAvailabelPokemon();
            if (newPokemon != null)
                BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPickPokemon());
        }
        else
        {
            if (!participatedPokemon.Contains(pokemon))
                participatedPokemon.Add(pokemon);
            eventManager.ResolveAllEventTriggers();
        }

    }

    public void HandleExpGain(PokemonBattleData defeatedPokemon)
    {
        int totalPokemon = 0;
        int exp = BattleMaster.GetInstance().GetExperienceForDefeating(defeatedPokemon);
        foreach (PokemonBattleData pokemonBattle in participatedPokemon)
        {
            if (!pokemonBattle.IsFainted())
            {
                totalPokemon += 1;
            }
            pokemonBattle.GetPokemonCaughtData().GainFriendship(BASE_FRIENDSHIP_GAINED_PER_TAKEDOWN);
        }
        if (totalPokemon == 0)
            return;
        int expGained = exp / totalPokemon;
        foreach (PokemonBattleData pokemonBattle in participatedPokemon)
        {
            if (!pokemonBattle.IsFainted())
            {
                
                eventManager.AddEvent(new BattleEventPokemonGainExp(pokemonBattle, expGained));
            }
        }
        eventManager.ResolveAllEventTriggers();
    }
    public void CheckForBattleEnd()
    {
        if (team1.GetFirstAvailabelPokemon() == null)
        {
            HandleBattleEnd(BattleTeamId.Team2);
            eventManager.ResolveAllEventTriggers();
        }
        else if (team2.GetFirstAvailabelPokemon() == null)
        {
            HandleBattleEnd(BattleTeamId.Team1);
        }
    }

    public void HandleBattleEnd(BattleTeamId winningTeam, bool endNow = false)
    {
        // Add event for battle end to handle variable saving, end combat dialogue, etc
        isBattleActive = false;
        BattleEventBattleEnd battleEndEvent = new BattleEventBattleEnd(this, winningTeam, postBattleEvent);
        eventManager.AddEvent(battleEndEvent);
        if (endNow)
        {
            eventManager.ResolveAllEventTriggers();
        }
    }

    public void HandlePlayerPokemonEnter(PokemonBattleData pokemon)
    {
        SetTeamActivePokemon(pokemon);
        // BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
    }

    public void AddTrigger(BattleTrigger trigger)
    {
        eventManager.AddBattleTrigger(trigger);
    }

    public void RemoveTrigger(BattleTrigger trigger)
    {
        eventManager.RemoveBattleTrigger(trigger);
    }
    public void AddEvent(BattleEvent be)
    {
        eventManager.AddEvent(be);
    }

    public void AddMoveEvent(PokemonBattleData user, MoveData move)
    {
        eventManager.AddEvent(new BattleEventUseMove(user, move));
    }
    public void AddAbilityEvent(PokemonBattleData user)
    {
        eventManager.AddEvent(new BattleEventPokemonAbility(user));
    }

    public void AddMoveSuccessEvent(BattleEventUseMove battleEvent)
    {
        eventManager.AddEvent(new BattleEventUseMoveSuccess(battleEvent));
    }
    public void AddItemPokemonUseEvent(PokemonBattleData pkmn, ItemDataOnPokemon item, bool isPokemonUsingIt=false)
    {
        eventManager.AddEvent(new BattleEventPokemonUseItem(pkmn, item, isPokemonUsingIt));
    }

    public void AddStatChangeEvent(PokemonBattleData target, PokemonBattleStats statLevelChange)
    {
        eventManager.AddEvent(new BattleEventPokemonChangeStat(target, statLevelChange));
    }

    public void AddStatusEffectEvent(PokemonBattleData target, StatusEffectId statusId, bool isStatus=false)
    {
        eventManager.AddEvent(new BattleEventPokemonStatusAdd(target, statusId, isStatus));
    }

    public void AddPokemonEnterEvent(PokemonBattleData target)
    {
        eventManager.AddEvent(new BattleEventEnterPokemon(target));
    }

    public void AddPokemonHealEvent(PokemonBattleData target, HealSummary healSummary)
    {
        eventManager.AddEvent(new BattleEventPokemonHeal(target, healSummary));
    }
    public void AddTacticEvent(TacticData tactic, BattleTeamId teamId)
    {
        eventManager.AddEvent(new BattleEventTactic(tactic, teamId));
    }

    public void AddTryToRunEvent()
    {
        eventManager.AddEvent(new BattleEventRun());
    }

    // Used for pokemon after fainting, doesnt trigger enemys next turn
    public void AddSwitchInPokemonEvent(PokemonBattleData pokemon, bool isDesition=false)
    {
        PokemonBattleData activePokemon = GetTeamActivePokemon(GetTeamId(pokemon));
        eventManager.AddEvent(new BattleEventPokemonSwitch(activePokemon, pokemon));
        if (!isDesition)
        {
            eventManager.ResolveAllEventTriggers();
            // BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
        }
    }
    public List<TacticData> GetPlayerTactics()
    {
        return GetTeamData(BattleTeamId.Team1).tactics;
    }
    public TacticData GetSelectedTactic()
    {
        return currentTacticSelected;
    }
    public void SetSelectedTactic(TacticData tactic)
    {
        currentTacticSelected = tactic;
    }

    public PokemonBattleData GetTarget(PokemonBattleData pokemon, MoveTarget target)
    {
        PokemonBattleData pokemon1 = team1.GetActivePokemon();
        if (target == MoveTarget.Enemy)
        {
            // Pokemon is on team 1, return team's active pokemon
            if (pokemon1 == pokemon)
                return team2.GetActivePokemon();
            else
                return team1.GetActivePokemon();
        }
        else if (target == MoveTarget.Self)
        {
            // Pokemon is on team 1, return self
            if (pokemon1 == pokemon)
                return team1.GetActivePokemon();
            else
                return team2.GetActivePokemon();
        }
        return null;
    }

    public DamageSummary CalculateMoveDamage(BattleEventUseMove finalEvent)
    {
        PokemonBattleData attacker = finalEvent.pokemon;
        PokemonBattleData target = GetTarget(attacker, finalEvent.move.targetType);
        MoveData moveUsed = finalEvent.move;
        UseMoveMods moveMods = finalEvent.moveMods;
        // Target and Pokemon Types
        PokemonTypeId moveTypeId = moveMods.moveTypeId;
        List<PokemonTypeId> targetTypes = target.inBattleTypes;
        // Categories
        MoveCategoryId attackerAttackCategory = moveUsed.GetAttackCategory();
        MoveCategoryId targetDefenseCategory = moveUsed.GetAttackCategory();
        //Stats
        PokemonBattleStats attackerStats = attacker.GetBattleStats();
        PokemonBattleStats targetStats = target.GetBattleStats();
        // Type Advantages
        float advantageMultiplier = BattleMaster.GetInstance()
            .GetAdvantageMultiplier(moveTypeId, targetTypes);
        // Formula Variables
        int attackerLevel = attacker.GetPokemonCaughtData().GetLevel();
        int attack =
            attackerAttackCategory == MoveCategoryId.physical ? 
            attackerStats.attack : attackerStats.spAttack;
        int defense =
            targetDefenseCategory == MoveCategoryId.physical ?
            targetStats.defense : targetStats.spDefense;
        int movePower = finalEvent.move.GetPower(attacker);
        float randomMultiplier = 0.8f + Random.value * 0.2f;
        float stabBonus = attacker.GetTypeIds().Contains(moveTypeId) ? 1.5f : 1f;
        // Final calculations
        float baseDamage = 2 + (2 * attackerLevel + 10) / 250f * attack / defense * movePower;
        float finalDamage = baseDamage * randomMultiplier * advantageMultiplier * stabBonus * moveMods.powerMultiplier;
        
        DamageSummary damageSummary = new DamageSummary(
            moveTypeId,
            (int) finalDamage,
            DamageSummarySource.Move,
            (int) moveUsed.moveId,
            GetSimpleAdvantageTypeFromMult(advantageMultiplier),
            attacker
            );
        return damageSummary;
    }

    public BattleTypeAdvantageType GetSimpleAdvantageTypeFromMult(float multiplier)
    {
        if (multiplier > 1)
            return BattleTypeAdvantageType.superEffective;
        else if (multiplier < 1)
        {
            if (multiplier > 0)
                return BattleTypeAdvantageType.resists;
            else
                return BattleTypeAdvantageType.inmune;
        }
        return BattleTypeAdvantageType.normal;
    }

    public void AddDamageDealtEvent(PokemonBattleData target, DamageSummary summary)
    {
        eventManager.AddEvent(new BattleEventTakeDamage(target, summary));
    }

    public void AddPokemonFaintEvent(BattleEventTakeDamage damageCauser)
    {
        eventManager.AddEvent(new BattleEventPokemonFaint(damageCauser));
    }

    public int ApplyDamage(BattleEventTakeDamage damage)
    {
        return damage.pokemon.ChangeHealth(-1 * damage.damageSummary.damageAmount);
    }

    public void AddStatusEffect(BattleEventPokemonStatusAdd battleEvent, bool isFromStatusMove = false)
    {
        PokemonBattleData pokemon = battleEvent.pokemon;
        StatusEffectId statusId = battleEvent.statusId;
        Flowchart battleFlowchart = BattleMaster.GetInstance().GetBattleFlowchart();
        StatusEffect status = new StatusEffect(pokemon, battleFlowchart);
        bool typePreventsStatus = false;
        bool alreadyHasPrimaryStatus = pokemon.AlreadyHasPrimaryStatus();
        List<StatusEffect> nonPrimary = pokemon.GetNonPrimaryStatus();
        bool isStatusAlready = false;
        foreach(StatusEffect s in nonPrimary)
        {
            if (s.effectId == statusId)
            {
                isStatusAlready = true;
                break;
            }
        }
        string gainStatusBlockName = "";
        if (!isStatusAlready)
        {
            switch (statusId)
            {
                case StatusEffectId.Poison:
                    status = new StatusEffectPoison(pokemon, battleFlowchart);
                    typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Poison);
                    gainStatusBlockName = "Poison Gain";
                    break;
                case StatusEffectId.Burn:
                    status = new StatusEffectBurn(pokemon, battleFlowchart);
                    typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Fire);
                    gainStatusBlockName = "Burn Gain";
                    break;
                case StatusEffectId.LeechSeed:
                    status = new StatusEffectLeechSeed(pokemon, battleFlowchart);
                    typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Grass);
                    gainStatusBlockName = "Leech Gain";
                    break;
                case StatusEffectId.Charmed:
                    status = new StatusEffectCharm(pokemon, battleFlowchart);
                    gainStatusBlockName = "Charm Gain";
                    break;
                case StatusEffectId.MoveCharge:
                    status = new StatusEffectMoveCharge(pokemon, battleFlowchart, lastUsedMove);
                    break;
                case StatusEffectId.FireVortex:
                    status = new StatusEffectFireVortex(pokemon, battleFlowchart);
                    gainStatusBlockName = "Fire Vortex Gain";
                    break;
                case StatusEffectId.Confused:
                    status = new StatusEffectConfusion(pokemon, battleFlowchart);
                    gainStatusBlockName = "Confusion Gain";
                    break;
            }
        }
        if (isStatusAlready || status.isPrimary && alreadyHasPrimaryStatus) 
        {
            // Cant add status due to type message
            BattleAnimatorMaster.GetInstance()?.AddEventInmuneTextEvent();
        }
        else if (typePreventsStatus && isFromStatusMove)
        {
            // Display cant add message
            BattleAnimatorMaster.GetInstance()?.AddEventInmuneTextEvent();
        }
        else
        {
            pokemon.AddStatusEffect(status);
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonGainStatus(pokemon));
            if (gainStatusBlockName != "")
                BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartPokemonText(gainStatusBlockName, pokemon);
        }
    }

    public int HealPokemon(PokemonBattleData pokemon, HealSummary heal)
    {
        int resultingHealth = pokemon.ChangeHealth(heal.amount);
        return resultingHealth;
    }

    public PokeballResult HandlePokeballUse(ItemDataPokeball pokeball)
    {
        float catchRate = pokeball.GetCaptureRate();
        PokemonBattleData enemy = GetTeamActivePokemon(BattleTeamId.Team2);
        StatusEffect se = enemy.GetCurrentPrimaryStatus();
        int statusBonus = se != null ? se.GetCaptureRateBonus() : 0;
        float captureRate = enemy.GetCaptureRate();
        int max = enemy.GetPokemonHealth();
        int current = enemy.GetPokemonCurrentHealth();
        float a = (3 * max - 2 * current) * captureRate * catchRate / (3 * max) + statusBonus;
        int randomValue = Random.Range(0, 255);
        bool isCaptured = randomValue <= a;
        int shakes = isCaptured ? 3 : Random.Range(1, 3);
        return new PokeballResult(isCaptured, shakes, enemy);
    }

    public void HandleTryToEscape()
    {
        switch (GetBattleData().battleType)
        {
            case BattleType.Trainer:
                BattleAnimatorMaster.GetInstance().ExecuteNoRunningFromTrainerFlowchart();
                break;
            case BattleType.Shadow:
                BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartNoEscape(GetTeamData(BattleTeamId.Team1).trainerTitle);
                break;
            default:
                PokemonBattleData usersPokemon = GetTeamActivePokemon(BattleTeamId.Team1);
                PokemonBattleData enemysPokemon = GetTeamActivePokemon(BattleTeamId.Team2);
                float chance = usersPokemon.GetPokemonCaughtData().GetLevel() / (float)enemysPokemon.GetPokemonCaughtData().GetLevel();
                float random = Random.value;
                if (chance > random)
                {
                    HandleBattleEnd(BattleTeamId.None, true);
                }
                else
                {
                    BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartEscapeFail(GetTeamData(BattleTeamId.Team1).trainerTitle);
                }
                break;
        }
        
    }

    public BattleData GetBattleData()
    {
        return battleData;
    }

    // Turn Cycle
    // Make desition
    // Desition if chosen
    // 1. Items
    // 2. Swap Pokemon
    // 3. Tactics if chosen
    // 4. Moves by priority
    // End of pokemon turn effects
    // End of round effects
}
