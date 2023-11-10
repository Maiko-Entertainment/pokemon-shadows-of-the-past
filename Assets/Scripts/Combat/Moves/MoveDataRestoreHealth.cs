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
        HealSummary summary = new HealSummary((int)(target.GetMaxHealth() * restorePercentage), HealSource.Move, battleEvent.move.GetId());
        summary.pokemonSource = battleEvent.pokemon;
        BattleEventPokemonHeal eventHeal = new BattleEventPokemonHeal(target, summary);
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(eventHeal);
    }
}
