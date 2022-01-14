using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonRoundEndHeal : BattleTriggerOnPokemonRoundEnd
{
    public HealSummary summary;
    public BattleTriggerOnPokemonRoundEndHeal(PokemonBattleData pokemon, HealSummary summary): base(pokemon)
    {
        this.summary = summary;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            bm.AddPokemonHealEvent(pokemon, summary);
        }
        return base.Execute(battleEvent);
    }
}
