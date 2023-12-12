public class BattleAnimationZoomTargetPokemon : BattleAnimationPokemon
{
    public float zoomMultiplier = 1f;
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraZoomPokemon(target, zoomMultiplier);
        return base.Execute(user, target);
    }
}
