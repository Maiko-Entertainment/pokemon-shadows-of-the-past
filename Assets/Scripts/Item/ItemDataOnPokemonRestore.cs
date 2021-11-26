using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Restore")]
public class ItemDataOnPokemonRestore : ItemDataOnPokemon
{
    public int restoreAmount = 20;
    public bool restoresPercentage = false;
    public List<StatusEffectId> statusClears = new List<StatusEffectId>();

    public override CanUseResult CanUseOnPokemon(PokemonCaughtData pokemon)
    {
        bool isFullHealth = pokemon.GetCurrentStats().health == pokemon.GetCurrentHealth();
        if (restoreAmount > 0)
        {
            if (!statusClears.Contains(pokemon.statusEffectId) && isFullHealth)
            {
                return new CanUseResult(false, "It wouldn't have any effect.");
            }
            return new CanUseResult(true, "");
        }
        if (!statusClears.Contains(pokemon.statusEffectId))
        {
            return new CanUseResult(false, "It wouldn't have any effect.");
        }
        return new CanUseResult(equipable, "");
    }
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
        if (restoreAmount > 0)
        {
            if (restoresPercentage)
            {
                bm.AddPokemonHealEvent(pokemon, new HealSummary((int)(pokemon.GetPokemonHealth() * restoreAmount / 100f), HealSource.Item, (int)itemId));
            }
            else
            {
                bm.AddPokemonHealEvent(pokemon, new HealSummary(restoreAmount, HealSource.Item, (int)itemId));
            }
        }
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
