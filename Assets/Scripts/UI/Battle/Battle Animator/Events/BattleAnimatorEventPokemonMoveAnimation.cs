public class BattleAnimatorEventPokemonMoveAnimation : BattleAnimatorEventPokemon
{
    public BattleAnimation animation;
    public PokemonBattleData target;
    public BattleAnimatorEventPokemonMoveAnimation(PokemonBattleData user, PokemonBattleData target, BattleAnimation animation):
        base(user)
    {
        this.animation = animation;
        this.target = target;
        eventType = BattleAnimatorEventType.PokemonMoveAnim;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.HandlePokemonMoveAnim(pokemon, target, animation);
        base.Execute();
    }
}
