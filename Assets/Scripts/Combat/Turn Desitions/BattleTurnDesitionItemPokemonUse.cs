using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionItemPokemonUse : BattleTurnDesitionPokemon
{
    ItemDataOnPokemon item;
    public BattleTurnDesitionItemPokemonUse(
        PokemonBattleData pkmn, 
        ItemDataOnPokemon item, 
        BattleTeamId teamId):
        base(pkmn, teamId)
    {
        this.item = item;
        priority = 3;
    }

    public override void Execute()
    {
        base.Execute();
        BattleMaster.GetInstance().GetCurrentBattle()?
            .AddItemPokemonUseEvent(pokemon, item);
    }
}