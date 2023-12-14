using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Moves/Move Restore Health")]
public class MoveDataRestoreHealth : MoveData
{
    public MoveTarget restoreTarget = MoveTarget.Self;
    public float restorePercentage = 0.5f;

    public override void Execute(BattleEventUseMove battleEvent)
    {
        base.Execute(battleEvent);
        PokemonBattleData target = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(battleEvent.pokemon, restoreTarget);
        int healAmount = GetHealAmount(battleEvent.pokemon);
        HealSummary summary = new HealSummary(healAmount, HealSource.Move, battleEvent.move.GetId());
        summary.pokemonSource = battleEvent.pokemon;
        BattleEventPokemonHeal eventHeal = new BattleEventPokemonHeal(target, summary);
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(eventHeal);
    }

    public int GetHealAmount(PokemonBattleData moveUser)
    {
        float healMultiplier = 1f;
        PokemonBattleData target = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(moveUser, restoreTarget);
        foreach (ConditionalMoveBonus bonus in conditionalBonuses)
        {
            if (bonus.MeetsConditions(target))
            {
                healMultiplier *= bonus.healMultiplier;
            }
        }
        int healAmount = (int)(moveUser.GetMaxHealth() * restorePercentage * healMultiplier);
        return healAmount;
    }
}
