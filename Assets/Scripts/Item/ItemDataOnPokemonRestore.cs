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
        int healAmount = GetHealAmount(pokemon);
        pokemon.ChangeHealth(healAmount);
        if (statusClears.Contains(pokemon.statusEffectId))
        {
            pokemon.statusEffectId = StatusEffectId.None;
        }
        base.UseOnPokemon(pokemon);
    }

    public override void UseOnPokemonBattle(PokemonBattleData pokemon, bool isPokemonUsingIt = false)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        base.UseOnPokemonBattle(pokemon, isPokemonUsingIt);
        int healAmount = GetHealAmount(pokemon.GetPokemonCaughtData());
        if (restoreAmount > 0)
        {
            if (restoresPercentage)
            {
                bm.AddPokemonHealEvent(pokemon, new HealSummary(healAmount, HealSource.Item, (int)itemId));
            }
            else
            {
                bm.AddPokemonHealEvent(pokemon, new HealSummary(healAmount, HealSource.Item, (int)itemId));
            }
        }
    }
    public int GetHealAmount(PokemonCaughtData pokemon)
    {
        int healAmount = restoresPercentage ? (int)(pokemon.GetCurrentStats().health * restoreAmount / 100f) : restoreAmount;
        return healAmount;
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
