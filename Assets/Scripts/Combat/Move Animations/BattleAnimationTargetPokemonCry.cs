public class BattleAnimationTargetPokemonCry : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(target.GetCry());
        return base.Execute(user, target);
    }
}
