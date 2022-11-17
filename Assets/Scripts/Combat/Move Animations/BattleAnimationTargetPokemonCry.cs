public class BattleAnimationTargetPokemonCry : BattleAnimation
{
    public float customPitch = 1f;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(target.GetCry(), customPitch);
        return base.Execute(user, target);
    }
}
