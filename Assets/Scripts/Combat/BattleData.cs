using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleData
{
    public AudioClip battleMusic;
    public AudioClip battleWonMusic;
    public GameObject battlebackground;
    public BattleType battleType;
    public List<TacticData> playerExtraTactics = new List<TacticData>();
}
