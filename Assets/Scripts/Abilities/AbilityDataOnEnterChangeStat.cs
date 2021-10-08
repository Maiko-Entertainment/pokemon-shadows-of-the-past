using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/OnEnterStatChange")]
public class AbilityDataOnEnterChangeStat : AbilityData
{
    public PokemonBattleStats levelChange;
    public MoveTarget targetType;
    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm!=null)
        {
            bm.AddTrigger(new BattleTriggerOnPokemonEnterChangeStats(pokemon, levelChange, targetType));
            bm.AddAbilityEvent(pokemon);
        }
    }
}
