using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventEnterPokemon : BattleEventPokemon
{
    public BattleEventEnterPokemon(PokemonBattleData pokemon) :
         base(pokemon)
    {
        eventId = BattleEventId.pokemonEnter;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEventPokemonEnterText(pokemon);
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventEnterPokemon(pokemon.Copy()));
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonEnterAnim(pokemon));
        bool isShadowBattle = BattleMaster.GetInstance().GetCurrentBattle().GetBattleData().battleType == BattleType.Shadow;
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPlaySound(pokemon.GetCry(), isShadowBattle ? 0.3f : 1f));
        base.Execute();
    }
}
