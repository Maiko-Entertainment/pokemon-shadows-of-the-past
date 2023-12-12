public class BattleAnimationTargetPokemonCry : BattleAnimationPokemon
{
    public float customPitch = 1f;
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(target.GetCry(), customPitch);
        return base.Execute(user, target);
    }
}
