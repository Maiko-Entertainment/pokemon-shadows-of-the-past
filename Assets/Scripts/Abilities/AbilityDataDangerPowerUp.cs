using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/OnDangerTypePowerUp")]
public class AbilityDataDangerPowerUp : AbilityData
{
    public PokemonTypeId powerUpType;
    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            UseMoveMods mods = new UseMoveMods(PokemonTypeId.Unmodify);
            mods.powerMultiplier = 1.5f;
            bm.AddTrigger(new BattleTriggerOnPokemonMoveDangerTypeMod(pokemon, mods, powerUpType, true));
        }
    }
}
