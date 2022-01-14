using System.Collections.Generic;

public class BattleAnimatorEventPokemonGainStatus : BattleAnimatorEventPokemon
{
    public StatusEffectId status;
    public List<StatusEffectId> minors = new List<StatusEffectId>();
    public BattleAnimatorEventPokemonGainStatus(PokemonBattleData pokemon):
        base(pokemon)
    {
        foreach (StatusEffect s in pokemon.GetNonPrimaryStatus())
            minors.Add(s.effectId);
        if (pokemon.GetCurrentPrimaryStatus() != null)
            this.status = pokemon.GetCurrentPrimaryStatus().effectId;
        eventType = BattleAnimatorEventType.BattleDescriptionText;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.UpdatePokemonStatus(pokemon, status, minors);
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(0.2f);
        base.Execute();
    }

    public override string ToString()
    {
        return base.ToString() + " - " + status.ToString();
    }
}
