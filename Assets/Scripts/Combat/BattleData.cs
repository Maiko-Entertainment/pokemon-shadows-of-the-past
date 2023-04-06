using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class BattleData
{
    public AudioClip battleMusic;
    public AudioClip battleWonMusic;
    public GameObject battlebackground;
    public VolumeProfile volumeProfile;
    public BattleType battleType;
    public List<TacticData> playerExtraTactics = new List<TacticData>();
}
