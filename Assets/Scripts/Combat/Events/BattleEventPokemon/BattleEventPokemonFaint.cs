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
        BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventPlaySound(pokemon.GetCry(), 0.75f));
        AudioClip faintSound = BattleAnimatorMaster.GetInstance().pokemonFaintClip;
        AudioOptions options = new AudioOptions(faintSound, 1f);
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
