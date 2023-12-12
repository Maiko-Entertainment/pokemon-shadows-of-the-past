public class BattleAnimationCameraZoomUser : BattleAnimationPokemon
{
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraZoomPokemon(user);
        return base.Execute(user, target);
    }
}