public class BattleAnimatorEventPokemonGainStatus : BattleAnimatorEventPokemon
{
    public StatusEffectId status;
    public BattleAnimatorEventPokemonGainStatus(PokemonBattleData pokemon, StatusEffectId status):
        base(pokemon)
    {
        this.status = status;
        eventType = BattleAnimatorEventType.BattleDescriptionText;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.UpdatePokemonStatus(pokemon, status);
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(0.2f);
        base.Execute();
    }

    public override string ToString()
    {
        return base.ToString() + " - " + status.ToString();
    }
}
