using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Modify Stat Changes")]
public class ItemDataPreventStatusDrop : ItemDataOnPokemon
{
    public StatsChangeModifyInstructions instructions = new StatsChangeModifyInstructions();
    public override List<BattleTrigger> InitiateInBattle(PokemonBattleData user)
    {
        BattleManager battle = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTriggerOnPokemonChangeStatsModify trigger = new BattleTriggerOnPokemonChangeStatsModify(user, instructions, false);
        trigger.relatedItem = this;
        battle.AddTrigger(trigger);
        List<BattleTrigger> triggers = base.InitiateInBattle(user);
        triggers.Add(trigger);
        return triggers;
    }

    public override CanUseResult CanUse()
    {
        return new CanUseResult(false, "This item must be equipped");
    }
}
