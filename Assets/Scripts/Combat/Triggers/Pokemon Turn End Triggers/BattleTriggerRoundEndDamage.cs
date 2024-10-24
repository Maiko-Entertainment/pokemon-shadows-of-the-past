﻿public class BattleTriggerRoundEndDamage : BattleTriggerOnPokemonRoundEnd
{
    public DamageSummary damageSummary;
    public BattleTriggerRoundEndDamage(PokemonBattleData pokemon, DamageSummary damageSummary, bool deleteOnLeave=true) :
        base(pokemon, deleteOnLeave)
    {
        this.damageSummary = damageSummary;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            bm.AddDamageDealtEvent(pokemon, damageSummary);
        }
        return base.Execute(battleEvent);
    }

    public override string ToString()
    {
        return base.ToString() + " - " + damageSummary.ToString();
    }
}
