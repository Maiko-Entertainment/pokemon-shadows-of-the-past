using System.Collections.Generic;

public class BattleAnimatorEventPokemonGainStatus : BattleAnimatorEventPokemon
{
    public StatusEffectData status;
    public List<StatusEffectData> minors = new List<StatusEffectData>();
    public BattleAnimatorEventPokemonGainStatus(PokemonBattleData pokemon):
        base(pokemon)
    {
        foreach (StatusEffect s in pokemon.GetNonPrimaryStatus())
            minors.Add(s.effectData);
        if (pokemon.GetCurrentPrimaryStatus() != null)
            status = pokemon.GetCurrentPrimaryStatus().effectData;
        eventType = BattleAnimatorEventType.BattleDescriptionText;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.UpdatePokemonInfo(pokemon);
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(0.2f);
        base.Execute();
    }

    public override string ToString()
    {
        return base.ToString() + " - " + status.ToString();
    }
}
