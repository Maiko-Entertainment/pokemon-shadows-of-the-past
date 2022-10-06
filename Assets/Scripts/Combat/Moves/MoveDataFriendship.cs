using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Moves/Friendship")]
public class MoveDataFriendship : MoveData
{
    public bool basedOnHate = false;
    public override int GetPower(PokemonBattleData user)
    {
        int power = base.GetPower(user);
        float friendship = user.pokemon.GetFriendship();
        int finalPower = (int) (power * (basedOnHate ? 255 - friendship : friendship) / 250f);
        return finalPower;
    }
}
