public class BattleEventPokemonUseItem : BattleEventPokemon
{
    ItemDataOnPokemon item;
    bool isPokemonUsingIt = false;
    public BattleEventPokemonUseItem(PokemonBattleData pokemon, ItemDataOnPokemon item, bool isPokemonUsingIt = false) :
        base(pokemon)
    {
        this.item = item;
        eventId = BattleEventId.useItemOnPokemon;
        this.isPokemonUsingIt = isPokemonUsingIt;
    }

    public override void Execute()
    {
        item.UseOnPokemonBattle(pokemon, isPokemonUsingIt);
        if (item.unequipAfterBattleUse && isPokemonUsingIt)
        {
            pokemon.UnequipItem();
        }
        if (item.isConsumable && isPokemonUsingIt)
        {
            pokemon.GetPokemonCaughtData().UnequipItem();
        }
    }
}
