public class BattleAnimatorEventAnimation : BattleAnimatorEventPokemon
{
    public BattleAnimation animation;
    public PokemonBattleData target;
    public BattleAnimatorEventAnimation(PokemonBattleData user, PokemonBattleData target, BattleAnimationPokemon animation):
        base(user)
    {
        this.animation = animation;
        this.target = target;
        eventType = BattleAnimatorEventType.PokemonMoveAnim;
    }

    public BattleAnimatorEventAnimation(BattleAnimation animation) :
        base(null)
    {
        this.animation = animation;
        eventType = BattleAnimatorEventType.PokemonMoveAnim;
    }

    public override void Execute()
    {
        if (animation is BattleAnimationPokemon)
        {
            BattleAnimatorMaster.GetInstance()?.HandlePokemonMoveAnim(pokemon, target, animation as BattleAnimationPokemon);
        }
        else
        {
            BattleAnimatorMaster.GetInstance()?.HandleAnim(animation);
        }
        base.Execute();
    }
}
