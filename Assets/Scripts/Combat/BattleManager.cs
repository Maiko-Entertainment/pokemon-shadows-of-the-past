using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleManager
{
    public BattleTeamData team1;
    public BattleTeamData team2;
    public bool isTrainerBattle = false;
    public Status weather = null;
    public int turnsPassed = 0;

    public BattleEventManager eventManager = new BattleEventManager();

    public BattleManager(BattleTeamData player, BattleTeamData opponent, bool isTrainerBattle = false)
    {
        team1 = player;
        team2 = opponent;
        this.isTrainerBattle = isTrainerBattle;
    }

    public void StartBattle()
    {
        eventManager = new BattleEventManager();
        team1.InitiateTeam();
        team2.InitiateTeam();

        BattleAnimatorMaster.GetInstance().LoadBattle(this);
        AddPokemonEnterEvent(team1.GetActivePokemon());
        AddPokemonEnterEvent(team2.GetActivePokemon());

        eventManager.ResolveAllEventTriggers();
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
    }

    public void HandleTurnInput(BattleTurnDesition desition)
    {
        BattleTurnDesition AIDesition = HandleAIInput();
        HandleRoundEnd();
        if (desition.priority >= AIDesition.priority)
        {
            AIDesition?.Execute();
            desition.Execute();
        }
        else
        {
            desition.Execute();
            AIDesition?.Execute();
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

    public void SetTeamActivePokemon(PokemonBattleData pokemon)
    {
        BattleTeamId teamId = GetTeamId(pokemon);

        if (teamId == BattleTeamId.Team1)
        {
            team1.SetActivePokemon(pokemon);
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

    public void HandleDesitions()
    {
        eventManager.ResolveAllEventTriggers();
        CheckForFainted();
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
    }

    public void CheckForFainted()
    {
        PokemonBattleData pokemon2 = GetTeamActivePokemon(BattleTeamId.Team2);
        if (pokemon2.IsFainted())
        {
            PokemonBattleData newPokemon = team2.GetFirstAvailabelPokemon();
            SetTeamActivePokemon(newPokemon);
        }
        PokemonBattleData pokemon = GetTeamActivePokemon(BattleTeamId.Team1);
        if (pokemon.IsFainted())
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPickPokemon());
        }
        else
        {
            eventManager.ResolveAllEventTriggers();
        }
    }

    public void HandlePlayerPokemonEnter(PokemonBattleData pokemon)
    {
        SetTeamActivePokemon(pokemon);
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
    }

    public void AddTrigger(BattleTrigger trigger)
    {
        eventManager.AddBattleTrigger(trigger);
    }

    public void RemoveTrigger(BattleTrigger trigger)
    {
        eventManager.RemoveBattleTrigger(trigger);
    }

    public void AddMoveEvent(PokemonBattleData user, MoveData move)
    {
        eventManager.AddEvent(new BattleEventUseMove(user, move));
    }

    public void AddMoveSuccessEvent(BattleEventUseMove battleEvent)
    {
        eventManager.AddEvent(new BattleEventUseMoveSuccess(battleEvent));
    }

    public void AddStatChangeEvent(PokemonBattleData target, PokemonBattleStats statLevelChange)
    {
        eventManager.AddEvent(new BattleEventPokemonChangeStat(target, statLevelChange));
    }

    public void AddStatusEffectEvent(PokemonBattleData target, StatusEffectId statusId)
    {
        eventManager.AddEvent(new BattleEventPokemonStatusAdd(target, statusId));
    }

    public void AddPokemonEnterEvent(PokemonBattleData target)
    {
        eventManager.AddEvent(new BattleEventEnterPokemon(target));
    }

    // Used for pokemon after fainting, doesnt trigger enemys next turn
    public void AddSwitchInPokemonEvent(PokemonBattleData pokemon, bool isDesition=false)
    {
        PokemonBattleData activePokemon = GetTeamActivePokemon(GetTeamId(pokemon));
        eventManager.AddEvent(new BattleEventPokemonSwitch(activePokemon, pokemon));
        if (!isDesition)
        {
            eventManager.ResolveAllEventTriggers();
            BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim();
        }
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
        float finalDamage = baseDamage * randomMultiplier * advantageMultiplier * stabBonus;
        
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

    public void AddStatusEffect(BattleEventPokemonStatusAdd battleEvent)
    {
        PokemonBattleData pokemon = battleEvent.pokemon;
        StatusEffectId statusId = battleEvent.statusId;
        Flowchart battleFlowchart = BattleMaster.GetInstance().GetBattleFlowchart();
        StatusEffect status = new StatusEffect(pokemon, battleFlowchart);
        bool typePreventsStatus = false;
        bool alreadyHasPrimaryStatus = pokemon.AlreadyHasPrimaryStatus();
        string gainStatusBlockName = "";

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
        }
        if (status.isPrimary && alreadyHasPrimaryStatus) 
        {
            // Cant add status due to type message
            BattleAnimatorMaster.GetInstance()?.AddEventInmuneTextEvent();
        }
        else if (typePreventsStatus)
        {
            // Display cant add message
            BattleAnimatorMaster.GetInstance()?.AddEventInmuneTextEvent();
        }
        else
        {
            pokemon.AddStatusEffect(status);
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonGainStatus(pokemon, statusId));
            BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartPokemonText(gainStatusBlockName, pokemon);
        }
    }

    // Turn Cycle
    // Make desition
    // Tactics if chosen
    // Desition if chosen
    // 1. Items
    // 2. Swap Pokemon
    // 3. Moves by priority
    // End of pokemon turn effects
    // End of round effects
}
