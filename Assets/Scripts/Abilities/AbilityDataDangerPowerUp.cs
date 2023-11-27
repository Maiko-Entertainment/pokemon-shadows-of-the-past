using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/OnDangerTypePowerUp")]
public class AbilityDataDangerPowerUp : AbilityData
{
    public List<TypeData> powerUpTypes;
    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            UseMoveMods mods = new UseMoveMods(null);
            mods.powerMultiplier = 1.5f;
            bm.AddTrigger(new BattleTriggerOnPokemonMoveDangerTypeMod(pokemon, mods, powerUpTypes, true));
        }
    }
}
