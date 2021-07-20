public class BattleAnimationCameraZoomUser : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraZoomPokemon(user);
        return base.Execute(user, target);
    }
}