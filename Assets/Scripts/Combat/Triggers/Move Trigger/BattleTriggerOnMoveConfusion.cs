using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnMoveConfusion : BattleTriggerOnPokemonMove
{
    public StatusEffectConfusion status;
    float selfHitChange = 0.9f;
    public BattleTriggerOnMoveConfusion(PokemonBattleData pokemon, StatusEffectConfusion status): base(pokemon, new UseMoveMods(PokemonTypeId.Unmodify), true)
    {
        this.status = status;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        if (pokemon == battleEvent.pokemon)
        {
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(
                status.message,
                "Confusion Warning",
                new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } }))
            );
            float random = Random.value;
            if (random < selfHitChange)
            {
                MoveData move = MovesMaster.Instance.GetMove(MoveId.SelfHit);
                battleEvent.move = move;
            }
        }
        return base.Execute(battleEvent);
    }
}
