public class BattleAnimationZoomTargetPokemon : BattleAnimation
{
    public float zoomMultiplier = 1f;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraZoomPokemon(target, zoomMultiplier);
        return base.Execute(user, target);
    }
}
