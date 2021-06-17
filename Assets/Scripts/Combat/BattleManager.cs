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

        PokemonBattleData team1Pokemon = team1.GetActivePokemon();
        PokemonBattleData team2Pokemon = team2.GetActivePokemon();

        team1Pokemon.Initiate();
        team2Pokemon.Initiate();
        BattleAnimatorMaster.GetInstance().LoadBattle(this);

        eventManager.AddEvent(new BattleEventEnterPokemon(team1Pokemon));
        eventManager.AddEvent(new BattleEventEnterPokemon(team2Pokemon));
        eventManager.ResolveAllEventTriggers();

        HandleTurnInput();
        HandleAIInput();
        HandleDesitions();
    }

    public void HandleTurnInput()
    {

    }

    public void HandleAIInput()
    {
        PokemonBattleData team2Pokemon = team2.GetActivePokemon();
        List<MoveEquipped> moves = team2Pokemon.GetPokemonCaughtData().GetAvailableMoves();
        if (moves.Count > 0)
        {
            int randomIndex = Random.Range(0, moves.Count);
            MoveData move = moves[randomIndex].move;
            AddMoveEvent(team2Pokemon, move);
        }
    }

    public PokemonBattleData GetTeamActivePokemon(BattleTeamId teamId)
    {
        return teamId == BattleTeamId.Team1 ? team1.GetActivePokemon() : team2.GetActivePokemon();
    }

    public void HandleDesitions()
    {
        eventManager.ResolveAllEventTriggers();
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

    public void AddStatChangeEvent(PokemonBattleData target, PokemonBattleStats statLevelChange)
    {
        eventManager.AddEvent(new BattleEventPokemonChangeStat(target, statLevelChange));
    }

    public void AddPokemonEnterEvent(PokemonBattleData target)
    {
        eventManager.AddEvent(new BattleEventEnterPokemon(target));
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
        PokemonTypeId moveTypeId = moveMods.moveTypeId;
        MoveCategoryId attackAttackCategory = moveUsed.GetAttackCategory();
        MoveCategoryId targetDefenseCategory = moveUsed.GetAttackCategory();
        PokemonBattleStats attackerStats = attacker.GetBattleStats();
        PokemonBattleStats targetStats = target.GetBattleStats();
        // Formula Variables
        int attackerLevel = attacker.GetPokemonCaughtData().GetLevel();
        int attack =
            attackAttackCategory == MoveCategoryId.physical ? 
            attackerStats.attack : attackerStats.spAttack;
        int defense =
            targetDefenseCategory == MoveCategoryId.physical ?
            targetStats.defense : targetStats.spDefense;
        int movePower = finalEvent.move.GetPower(attacker);
        float randomMultiplier = 0.8f + Random.value * 0.2f;
        float stabBonus = attacker.GetTypeIds().Contains(moveTypeId) ? 1.5f : 1f;
        // Final calculations
        float baseDamage = 2 + (2 * attackerLevel + 10) / 250f * attack / defense * movePower;
        float finalDamage = baseDamage * randomMultiplier * stabBonus;
        DamageSummary damageSummary = new DamageSummary(
            moveTypeId,
            (int) finalDamage,
            DamageSummarySource.Move,
            (int) moveUsed.moveId
            );
        return damageSummary;
    }

    public void AddDamageDealtEvent(PokemonBattleData target, DamageSummary summary)
    {
        eventManager.AddEvent(new BattleEventTakeDamage(target, summary));
    }

    public void ApplyDamage(BattleEventTakeDamage damage)
    {
        damage.pokemon.ChangeHealth(-1 * damage.damageSummary.damageAmount);
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
