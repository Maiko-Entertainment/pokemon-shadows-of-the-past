public class BattleAnimatorEventPokemonGainStatus : BattleAnimatorEventPokemon
{
    public StatusEffectId status;
    public BattleAnimatorEventPokemonGainStatus(PokemonBattleData pokemon, StatusEffectId status):
        base(pokemon)
    {
        this.status = status;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.UpdatePokemonStatus(pokemon, status);
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(0.2f);
        base.Execute();
    }
}
