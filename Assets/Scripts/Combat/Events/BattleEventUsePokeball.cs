using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventUsePokeball : BattleEvent
{
    public ItemDataPokeball pokeball;
    public BattleEventUsePokeball(ItemDataPokeball pokeball)
    {
        this.pokeball = pokeball;
        eventId = BattleEventId.usePokeball;
    }

    public override void Execute()
    {
        pokeball.UseInBattle();
        base.Execute();
    }
}
