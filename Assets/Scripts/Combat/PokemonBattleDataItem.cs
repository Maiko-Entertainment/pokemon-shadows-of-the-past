using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonBattleDataItem
{
    public ItemDataOnPokemon equippedItem;
    public List<BattleTrigger> battleTriggers = new List<BattleTrigger>();

    public PokemonBattleDataItem(ItemDataOnPokemon item)
    {
        equippedItem = item;
    }

    public void Initiate(PokemonBattleData pokemon)
    {
        if (equippedItem)
        {
            battleTriggers = equippedItem.InitiateInBattle(pokemon);
        }
    }

    public void Remove()
    {
        foreach(BattleTrigger bt in battleTriggers)
        {
            BattleMaster.GetInstance().GetCurrentBattle().RemoveTrigger(bt);
        }
    }
}
