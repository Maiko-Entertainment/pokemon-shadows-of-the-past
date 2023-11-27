using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTriggerOnPokemonStatusGiveBack : BattleTriggerOnPokemonGainStatusEffectSuccess
{
    public List<StatusEffectData> applicableStatus = new List<StatusEffectData>();
    bool showAbility = false;
    public BattleTriggerOnPokemonStatusGiveBack(PokemonBattleData pokemon, List<StatusEffectData> applicableStatus, bool showAbility = false) :
        base(pokemon)
    {
        this.showAbility = showAbility;
        this.applicableStatus = applicableStatus;
    }

    public override bool Execute(BattleEventPokemonStatusAddSuccess battleEvent)
    {
        if (
            pokemon.battleId == battleEvent.statusEvent.pokemon.battleId &&
            maxTriggers > 0 &&
            applicableStatus.Contains(battleEvent.statusEvent.status)
           )
        {
            BattleManager bm = BattleMaster.GetInstance()?.GetCurrentBattle();
            PokemonBattleData enemy = bm.GetTarget(pokemon, MoveTarget.Enemy);
            BattleMaster.GetInstance()?.GetCurrentBattle()?.AddEvent(new BattleEventPokemonStatusAdd(
                    enemy,
                    battleEvent.statusEvent.status,
                    true
                ));
            if (showAbility)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(pokemon));
            }
            return false;
        }
        maxTriggers++;
        return base.Execute(battleEvent);
    }
}
