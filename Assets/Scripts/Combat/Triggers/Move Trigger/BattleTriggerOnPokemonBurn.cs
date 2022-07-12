public class BattleTriggerOnPokemonBurn : BattleTriggerOnPokemonMove
{
    public MoveCategoryId moveCategory;
    public BattleTriggerOnPokemonBurn(PokemonBattleData pokemon, MoveCategoryId moveCategory): base(pokemon, null, true)
    {
        useMoveMods = new UseMoveMods(PokemonTypeId.Unmodify);
        useMoveMods.powerMultiplier = 0.5f;
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
