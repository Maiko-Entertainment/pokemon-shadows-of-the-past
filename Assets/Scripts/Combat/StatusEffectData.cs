using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/StatusEffect")]
public class StatusEffectData : ScriptableObject
{
    public StatusEffectId statusId;
    public string statusName;
    public Sprite icon;
    public List<BattleAnimation> hitAnims;
}
