public class BattleTriggerOnPokemonMoveCategory : BattleTriggerOnPokemonMove
{
    public MoveCategoryId moveCategory;
    public BattleTriggerOnPokemonMoveCategory(PokemonBattleData pokemon, MoveCategoryId moveCategory, UseMoveMods mods): base(pokemon, mods, true)
    {
        this.moveCategory = moveCategory;
    }

    public override bool Execute(BattleEvent battleEvent)
    {
        BattleEventUseMove be = (BattleEventUseMove)battleEvent;
        if (be.move.GetAttackCategory() == moveCategory && be.pokemon.battleId == pokemon.battleId)
        {
            be.moveMods.Implement(useMoveMods);
        }
        return base.Execute(be);
    }
}
