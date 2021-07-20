public class BattleAnimationZoomTargetPokemon : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraZoomPokemon(target);
        return base.Execute(user, target);
    }
}
