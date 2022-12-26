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
    public List<BattleStatsGetter> battleStatsSubscribers = new List<BattleStatsGetter>();

    public MoveData lastUsedMove = null;

    private bool isBattleActive = false;
    private TacticData currentTacticSelected;

    public static int BASE_FRIENDSHIP_GAINED_PER_TAKEDOWN = 2;
    public static float SHADOW_REBEL_CHANCE = 0.25f;
    public static float SHADOW_IGNORE_CHANCE = 0.1f;

    public List<BattleFaintHistory> faintHistory = new List<BattleFaintHistory>();

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
        faintHistory = new List<BattleFaintHistory>();
        BattleAnimatorMaster.GetInstance()?.SetBackground(battleData.battlebackground);
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTurnStart());
        team1.InitiateTeam(BattleTeamId.Team1);
        team2.InitiateTeam(BattleTeamId.Team2);
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

    public void SetBattleActive(bool isActive)
    {
        isBattleActive = isActive;
    }

    public void AddStatGetter(BattleStatsGetter getter)
    {
        battleStatsSubscribers.Add(getter);
    }
    public void RemoveStatGetter(BattleStatsGetter getter)
    {
        battleStatsSubscribers.Remove(getter);
    }

    public PokemonBattleStats GetPokemonStats(PokemonBattleData pokemon)
    {
        PokemonBattleStats statsAccum = pokemon.GetBattleStats();
        foreach(BattleStatsGetter bsg in battleStatsSubscribers)
        {
            statsAccum = bsg.Apply(pokemon, statsAccum);
        }
        return statsAccum;
    }

    public BattleTurnDesition PickNewRandomDesition(BattleTeamId team)
    {
        PokemonBattleData pkmn = GetTeamActivePokemon(team);
        List<MoveEquipped> moves = pkmn.GetPokemonCaughtData().GetAvailableMoves();
        if (moves.Count > 0)
        {
            int randomIndex = Random.Range(0, moves.Count);
            MoveEquipped move = moves[randomIndex];

            return new BattleTurnDesitionPokemonMove(move, pkmn, BattleTeamId.Team2);
        }
        else
        {
            MoveEquipped struggle = new MoveEquipped(MovesMaster.Instance.GetMove(MoveId.Struggle));
            return new BattleTurnDesitionPokemonMove(struggle, pkmn, BattleTeamId.Team2);
        }
    }

    public void HandleTurnInput(BattleTurnDesition desition)
    {
        PokemonBattleData team1Pkmn = GetTeamActivePokemon(BattleTeamId.Team1);
        bool isCurrentPokemonShadow = team1Pkmn.pokemon.isShadow;
        bool willRebel = isCurrentPokemonShadow ? Random.value < SHADOW_REBEL_CHANCE : false;
        bool willIgnore = isCurrentPokemonShadow ? Random.value < SHADOW_IGNORE_CHANCE : false;
        if (willIgnore)
        {
            MoveEquipped struggle = new MoveEquipped(MovesMaster.Instance.GetMove(MoveId.Struggle));
            desition = new BattleTurnDesitionPokemonMove(struggle, team1Pkmn, BattleTeamId.Team2);
            BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartPokemonText("Ignore", team1Pkmn);
        }
        else if (willRebel)
        {
            desition = PickNewRandomDesition(BattleTeamId.Team1);
            BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartPokemonText("Rebel", team1Pkmn);
        }
        BattleTurnDesition AIDesition = HandleAIInput();
        HandleRoundEnd();
        int desitionPriority = desition.priority;
        int tacticPriority = currentTacticSelected ? 9 : -1;
        int aiDesitionPriority = AIDesition.priority;
        int aiTacticPriority = AIDesition.tactic ? 9 : -1;
        for(int priority = 0; priority < 10; priority++)
        {
            if (aiDesitionPriority == priority && aiDesitionPriority == desitionPriority)
            {
                if (AIDesition.GetTiebreakerPriority() <= desition.GetTiebreakerPriority())
                {
                    AIDesition.Execute();
                    AddEvent(new BattleEventDestion(AIDesition));
                    desition.Execute();
                    AddEvent(new BattleEventDestion(desition));
                }
                else
                {
                    desition.Execute();
                    AddEvent(new BattleEventDestion(desition));
                    AIDesition.Execute();
                    AddEvent(new BattleEventDestion(AIDesition));
                }
            }
            else
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
        return PickNewRandomDesition(BattleTeamId.Team2);
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

        pokemon.roundsInCombat = 0;
        if (teamId == BattleTeamId.Team1)
        {
            team1.SetActivePokemon(pokemon);
            if (!participatedPokemon.Contains(pokemon))
                participatedPokemon.Add(pokemon);
        }
        else
        {
            team2.SetActivePokemon(pokemon);
            PokemonMaster.GetInstance().SeePokemon(pokemon.GetPokemonCaughtData().pokemonBase.pokemonId);
        }
        AddPokemonEnterEvent(pokemon);
    }

    public BattleTeamId GetTeamId(PokemonBattleData pokemon)
    {
        foreach(PokemonBattleData p1 in team1.pokemon)
        {
            if (p1.battleId == pokemon.battleId)
            {
                return BattleTeamId.Team1;
            }
        }
        foreach (PokemonBattleData p1 in team1.allyPokemon)
        {
            if (p1.battleId == pokemon.battleId)
            {
                return BattleTeamId.Team1;
            }
        }
        foreach (PokemonBattleData p2 in team2.pokemon)
        {
            if (p2.battleId == pokemon.battleId)
            {
                return BattleTeamId.Team2;
            }
        }
        foreach (PokemonBattleData p2 in team2.allyPokemon)
        {
            if (p2.battleId == pokemon.battleId)
            {
                return BattleTeamId.Team2;
            }
        }
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

    public List<PokemonBattleData> GetAllAvailablePokemon(BattleTeamId teamId)
    {
        List<PokemonBattleData> available = new List<PokemonBattleData>();
        foreach(PokemonBattleData pkmn in teamId == BattleTeamId.Team1 ? team1.pokemon : team2.pokemon)
        {
            if (!pkmn.IsFainted())
            {
                available.Add(pkmn);
            }
        }
        return available;
    }

    public void HandleExpGain(PokemonBattleData defeatedPokemon)
    {
        BattleMaster bm = BattleMaster.GetInstance();
        int totalPokemon = 0;
        int exp = bm.GetExperienceForDefeating(defeatedPokemon);
        List<PokemonBattleData> finalPokemonList = bm.isExpShareOn ? team1.pokemon : participatedPokemon;
        foreach (PokemonBattleData pokemonBattle in finalPokemonList)
        {
            if (!pokemonBattle.IsFainted())
            {
                totalPokemon += 1;
            }
        }
        if (totalPokemon == 0)
            return;
        int expGained = bm.isExpShareOn ? (int)(exp * 0.5f) : (int)(exp / totalPokemon * 1.25f);
        foreach (PokemonBattleData pokemonBattle in finalPokemonList)
        {
            if (!pokemonBattle.IsFainted())
            {
                // Give EXP to participating pokemon but not to active pokemon, put event for active at the end
                if (GetTeamActivePokemon(BattleTeamId.Team1).battleId != pokemonBattle.battleId)
                {
                    eventManager.AddEvent(new BattleEventPokemonGainExp(pokemonBattle, expGained));
                    pokemonBattle.GetPokemonCaughtData().GainFriendship(BASE_FRIENDSHIP_GAINED_PER_TAKEDOWN / 2);
                }
            }
        }
        // Gives active pokemon exp at the end to not mess with animator events order
        if (GetTeamActivePokemon(BattleTeamId.Team1) != null)
        {
            // If exp share is active x2 it to make it 100% exp again
            eventManager.AddEvent(new BattleEventPokemonGainExp(GetTeamActivePokemon(BattleTeamId.Team1), bm.isExpShareOn ? expGained * 2 : expGained));
            GetTeamActivePokemon(BattleTeamId.Team1).GetPokemonCaughtData().GainFriendship(BASE_FRIENDSHIP_GAINED_PER_TAKEDOWN);
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
        BattleTeamId usersTeam = GetTeamId(pokemon);
        if (target == MoveTarget.Enemy)
        {
            // Pokemon is on team 1, return team's active pokemon
            if (usersTeam == BattleTeamId.Team1)
                return team2.GetActivePokemon();
            else
                return team1.GetActivePokemon();
        }
        else if (target == MoveTarget.Self)
        {
            // Pokemon is on team 1, return self
            if (usersTeam == BattleTeamId.Team1)
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
        damageSummary.move = moveUsed;
        return damageSummary;
    }

    public bool CheckForMoveHit(BattleEventUseMove battleEvent)
    {
        PokemonBattleData user = battleEvent.pokemon;
        MoveData move = battleEvent.move;
        float moveHitChance = move.GetAccuracy(user);
        if (move.targetType != MoveTarget.Enemy)
        {
            return true;
        }
        else
        {
            PokemonBattleData enemy = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(user, move.targetType);
            PokemonBattleStats enemyStats = enemy.GetBattleStats();
            float random = Random.value;
            float chanceToDodge = GetMultiplierForAccuracyEvasion(enemyStats.evasion, false);
            float chanceToHit = GetMultiplierForAccuracyEvasion(user.GetBattleStatsChangeLevels().accuracy, false);
            float eventMods = battleEvent.moveMods.accuracyMultiplier;
            float finalChanceToHit = moveHitChance * chanceToHit * chanceToDodge * eventMods;
            return random <= finalChanceToHit;
        }
    }

    public float GetMultiplierForAccuracyEvasion(int stage, bool isAccuracy)
    {
        float multiplier = stage >= 0 ? (3f / (3 + stage)) : ((3 - stage) / 3f);
        return isAccuracy ? 1 - multiplier : multiplier;
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
        StatusEffect status = new StatusEffect(pokemon);
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
                    status = new StatusEffectPoison(pokemon);
                    typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Poison);
                    gainStatusBlockName = "Poison Gain";
                    break;
                case StatusEffectId.Burn:
                    status = new StatusEffectBurn(pokemon);
                     typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Fire);
                    gainStatusBlockName = "Burn Gain";
                    break;
                case StatusEffectId.Frostbite:
                    status = new StatusEffectFrostbite(pokemon);
                    gainStatusBlockName = "Frostbite Gain";
                    typePreventsStatus = pokemon.GetTypeIds().Contains(status.inmuneTypes[0]);
                    break;
                case StatusEffectId.LeechSeed:
                    status = new StatusEffectLeechSeed(pokemon);
                    typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Grass);
                    gainStatusBlockName = "Leech Gain";
                    break;
                case StatusEffectId.Charmed:
                    status = new StatusEffectCharm(pokemon);
                    gainStatusBlockName = "Charm Gain";
                    break;
                case StatusEffectId.MoveCharge:
                    status = new StatusEffectMoveCharge(pokemon, lastUsedMove);
                    break;
                case StatusEffectId.RepeatMove:
                    status = new StatusEffectRepeatMove(pokemon, lastUsedMove);
                    break;
                case StatusEffectId.FireVortex:
                    status = new StatusEffectFireVortex(pokemon);
                    gainStatusBlockName = "Fire Vortex Gain";
                    break;
                case StatusEffectId.Confused:
                    status = new StatusEffectConfusion(pokemon);
                    gainStatusBlockName = "Confusion Gain";
                    break;
                case StatusEffectId.Sleep:
                    status = new StatusEffectSleep(pokemon);
                    gainStatusBlockName = "Sleep Gain";
                    break;
                case StatusEffectId.Paralyzed:
                    status = new StatusEffectParalyzed(pokemon);
                    typePreventsStatus = pokemon.GetTypeIds().Contains(PokemonTypeId.Electric);
                    gainStatusBlockName = "Paralyze Gain";
                    break;
                case StatusEffectId.Charged:
                    status = new StatusEffectCharged(pokemon);
                    break;
                case StatusEffectId.Hopeless:
                    status = new StatusEffectHopeless(pokemon);
                    gainStatusBlockName = "Hopeless Gain";
                    break;
                case StatusEffectId.Flinch:
                    status = new StatusEffectFlinch(pokemon);
                    gainStatusBlockName = "Flinch Gain";
                    break;
            }
        }
        if (isStatusAlready || status.isPrimary && alreadyHasPrimaryStatus) 
        {
            // Cant add status due to type message
            BattleAnimatorMaster.GetInstance()?.AddEventInmuneTextEvent();
        }
        else if (typePreventsStatus)
        {
            if (isFromStatusMove)
            {
                // Display cant add message
                BattleAnimatorMaster.GetInstance()?.AddEventInmuneTextEvent();
            }
        }
        else
        {
            AddEvent(new BattleEventPokemonStatusAddSuccess(battleEvent));
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
        float captureRateBonus = 0;
        StatusEffect se = enemy.GetCurrentPrimaryStatus();
        int statusBonus = se != null ? se.GetCaptureRateBonus() : 0;
        captureRateBonus += statusBonus;
        foreach(StatusEffect mse in enemy.GetNonPrimaryStatus())
        {
            captureRateBonus += mse.captureRateBonus;
        }
        float captureRate = enemy.GetCaptureRate();
        int max = enemy.GetPokemonHealth();
        int current = enemy.GetPokemonCurrentHealth();
        float a = (3 * max - 2 * current) * captureRate * catchRate / (3 * max) + captureRateBonus;
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
