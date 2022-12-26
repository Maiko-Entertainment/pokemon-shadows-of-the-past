using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonFaintNumberLeftDialog : BattleTriggerOnPokemonFaint
{
    public BattleTriggerMessageData messageData;
    public BattleTeamId teamId;
    public int pokemonLeft = 0;
    public int priority = 0;
    public BattleTriggerOnPokemonFaintNumberLeftDialog(BattleTriggerMessageData messageData, BattleTeamId teamId, int pokemonLeft) :
        base(null, true)
    {
        this.messageData = messageData;
        this.pokemonLeft = pokemonLeft;
        this.teamId = teamId;
        maxTriggers = 1;
    }
    public override bool Execute(BattleEventPokemonFaint battleEvent)
    {
        PokemonBattleData faintedPokemon = battleEvent.pokemon;
        BattleTeamId faintedTeamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(faintedPokemon);
        int pokemonLeft = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(teamId).GetAvailablePokemon().Count;
        if (faintedTeamId == teamId)
        {
            if (maxTriggers > 0)
            {
                if (this.pokemonLeft == pokemonLeft)
                {
                    DamageSummary summary = battleEvent.eventCauser.damageSummary;
                    PokemonBattleData pkmn = summary.pokemonSource;
                    if (pkmn != null)
                        messageData.variables.Add("pokemonFainter", pkmn.GetName());
                    messageData.variables.Add("pokemon", faintedPokemon.GetName());
                    BattleAnimatorEventNarrative ae =
                        new BattleAnimatorEventNarrative(
                                messageData
                        );
                    ae.priority = priority;
                    BattleAnimatorMaster.GetInstance().AddEvent(ae);
                }
                else
                {
                    maxTriggers++;
                }
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}
