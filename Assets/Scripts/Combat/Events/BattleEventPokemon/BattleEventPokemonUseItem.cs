public class BattleEventPokemonUseItem : BattleEventPokemon
{
    ItemDataOnPokemon item;
    public BattleEventPokemonUseItem(PokemonBattleData pokemon, ItemDataOnPokemon item):
        base(pokemon)
    {
        this.item = item;
        eventId = BattleEventId.useItemOnPokemon;
    }

    public override void Execute()
    {
        item.UseOnPokemonBattle(pokemon);
    }
}
