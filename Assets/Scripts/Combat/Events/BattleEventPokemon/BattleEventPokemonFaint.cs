using UnityEngine;

public class BattleEventPokemonFaint : BattleEventPokemon
{
    public BattleEventTakeDamage eventCauser;
    public BattleEventPokemonFaint(BattleEventTakeDamage eventCauser) :
        base(eventCauser.pokemon)
    {
        eventId = BattleEventId.pokemonFaint;
        this.eventCauser = eventCauser;
    }

    public override void Execute()
    {
        pokemon.RemoveAllStatusEffects();
        BattleAnimatorMaster.GetInstance()?.AddEventPokemonFaintText(pokemon);

        bool isShadow = BattleMaster.GetInstance().GetCurrentBattle().GetBattleData().battleType == BattleType.Shadow;
        BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventPlaySound(pokemon.GetCry(), isShadow ? 0.2f : 0.75f));
        AudioClip faintSound = BattleAnimatorMaster.GetInstance().pokemonFaintClip;
        AudioOptions options = new AudioOptions(faintSound, isShadow ? 0.5f : 1f);
        options.volumeModifier = 1f;
        BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventPlaySound(options, true));

        BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventPokemonFaint(pokemon)
            );
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        bm?.faintHistory.Add(new BattleFaintHistory(pokemon, bm.turnsPassed));
        base.Execute();
    }
}
