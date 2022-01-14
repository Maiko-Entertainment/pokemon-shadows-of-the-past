using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Tactics/Befriend")]
public class TacticDataBefriend : TacticData
{
    public List<PokemonTypeId> workingTypes = new List<PokemonTypeId>();
    public string failBlockName;

    public override void HandlePostExecute(BattleTeamId teamId, PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTriggerOnPokemonMove trigger = new BattleTriggerOnPokemonMove(pokemon, powerMoveMod, true);
        trigger.maxTriggers = 1;
        bm.AddTrigger(trigger);
        List<PokemonTypeId> types = pokemon.GetTypeIds();
        bool contains = false;
        foreach (PokemonTypeId type in types)
        {
            if (workingTypes.Contains(type))
            {
                contains = true;
                break;
            }
        }
        if (blockName != "")
        {
            string trainerName = bm.GetTeamData(teamId).trainerTitle;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(
                    TacticsMaster.GetInstance().tacticsFlochart,
                    contains ? blockName : failBlockName,
                    new Dictionary<string, string>()
                    {
                        {"trainer", trainerName },
                        {"pokemon", pokemon.GetName() },
                    }
                )
            ));
            if (contains)
            {
                foreach (MoveStatusChance msc in statusAdds)
                {
                    PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
                    float random = Random.value;
                    if (random < msc.chance)
                    {
                        bm.AddStatusEffectEvent(pokemonTarget, msc.effectId);
                    }
                }
            }
        }
    }
}
