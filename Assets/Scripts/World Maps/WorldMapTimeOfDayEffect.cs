using UnityEngine;
[System.Serializable]
public class WorldMapTimeOfDayEffect
{
    public GameObject UIEffectPrefab;
    public TimeOfDayType timeOfDay;

    public void Initiate()
    {
        TimeOfDayType currentTime = WorldMapMaster.GetInstance().GetTimeOfDay();
        TransitionMaster.GetInstance().ClearDayEffects();
        if (currentTime == timeOfDay)
        {
            TransitionMaster.GetInstance().InitiateDayEffect(UIEffectPrefab);
        }
    }
}
