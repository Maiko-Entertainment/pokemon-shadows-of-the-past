﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Encounters/Trainer Battle Base")]
public class TrainerCombatData : ScriptableObject
{
    public BattleTeamData battleTeamData;
    public BattleData battleData;
    public AudioClip battleSong;
    public ViewTransition transition;
    
    public AudioClip GetBattleSong()
    {
        return battleSong;
    }

    public BattleTeamData GetTeambattleData()
    {
        return battleTeamData.Copy();
    }
}
