[System.Serializable]
public class MoveStatusChance
{
    public StatusEffectData effectData;
    public MoveTarget targetType;
    public float chance = 0.33f;
    public bool removeStatusInstead = false;
}
