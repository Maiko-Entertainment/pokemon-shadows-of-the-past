using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonStatusRemove : BattleEventPokemon
{
    public StatusEffectData statusEffect;
    public BattleEventPokemonStatusRemove(PokemonBattleData pokemon, StatusEffectData se): base(pokemon)
    {
        statusEffect = se;
        eventId = BattleEventId.pokemonRemoveStatus;
    }

    public override void Execute()
    {
        pokemon.RemoveStatusEffect(statusEffect);
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonGainStatus(pokemon));
    }
}
