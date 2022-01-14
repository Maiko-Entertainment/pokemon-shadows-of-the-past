using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Tactics/Tactic")]
public class TacticData : ScriptableObject
{
    public TacticId tacticId;
    public MoveTarget targets;
    public int cost = 2;
    public UseMoveMods powerMoveMod = new UseMoveMods(PokemonTypeId.Unmodify);
    public List<StatusEffectId> statusClears = new List<StatusEffectId>();
    public List<MoveStatusChance> statusAdds = new List<MoveStatusChance>();

    public string blockName;
    public string tacticName;
    public string tacticDescription;
    public Sprite tacticIcon;

    public virtual void Execute(BattleTeamId teamId)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        // Default to target self
        BattleTeamId finalTarget = teamId;
        if (targets == MoveTarget.Enemy)
        {
            finalTarget = teamId == BattleTeamId.Team1 ? BattleTeamId.Team2 : BattleTeamId.Team1;
        }
        PokemonBattleData pokemon = bm.GetTeamActivePokemon(finalTarget);
        HandlePostExecute(teamId, pokemon);
    }

    public virtual void HandlePostExecute(BattleTeamId teamId, PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTriggerOnPokemonMove trigger = new BattleTriggerOnPokemonMove(pokemon, powerMoveMod, true);
        trigger.maxTriggers = 1;
        bm.AddTrigger(trigger);
        if (blockName != "")
        {
            string trainerName = bm.GetTeamData(teamId).trainerTitle;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(
                    TacticsMaster.GetInstance().tacticsFlochart,
                    blockName,
                    new Dictionary<string, string>()
                    {
                        {"trainer", trainerName },
                        {"pokemon", pokemon.GetName() },
                    }
                )
            ));
        }
        foreach(MoveStatusChance msc in statusAdds)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.chance)
            {
                bm.AddStatusEffectEvent(pokemonTarget, msc.effectId);
            }
        }
    }

    public int GetCost()
    {
        return cost;
    }

    public string GetName()
    {
        return tacticName;
    }
}
