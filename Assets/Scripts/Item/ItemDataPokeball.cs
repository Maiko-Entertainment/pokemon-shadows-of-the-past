using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Pokeball")]
public class ItemDataPokeball : ItemData
{
    public float baseCaptureRate = 1f;
    public override CanUseResult CanUse()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm!=null)
        {
            if (!bm.isTrainerBattle)
            {
                return new CanUseResult(true, "");
            }
            return new CanUseResult(false, "The pokeball won't have any effect.");
        }
        return new CanUseResult(false, "Can only be used in combat");
    }

    public virtual float GetCaptureRate()
    {
        return baseCaptureRate;
    }
}
