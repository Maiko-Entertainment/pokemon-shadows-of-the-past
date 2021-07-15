using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Restore")]
public class ItemDataOnPokemonRestore : ItemDataOnPokemon
{
    public int restoreAmount = 20;
    public List<StatusEffectId> statusClears = new List<StatusEffectId>();

    public override void UseOnPokemon(PokemonCaughtData pokemon)
    {
        pokemon.ChangeHealth(restoreAmount);
        if (statusClears.Contains(pokemon.statusEffectId))
        {
            pokemon.statusEffectId = StatusEffectId.None;
        }
        base.UseOnPokemon(pokemon);
    }

    public override void UseOnPokemonBattle(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        base.UseOnPokemonBattle(pokemon);
        bm.AddPokemonHealEvent(pokemon, new HealSummary(restoreAmount, HealSource.Item, (int)itemId));
    }

    public override void Use()
    {
        if (BattleMaster.GetInstance().GetCurrentBattle() != null)
        {
            BattleAnimatorMaster.GetInstance().battleOptionsManager.ShowItemPokemonSelector(this);
        }
        base.Use();
    }
}
