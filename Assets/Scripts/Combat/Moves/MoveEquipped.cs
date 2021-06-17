[System.Serializable]
public class MoveEquipped
{
    public MoveData move;
    public int timesUsed = 0;
    public int disabledTurnsLeft = 0;

    public MoveEquipped(MoveData move)
    {
        this.move = move;
    }

    public bool IsAvailable()
    {
        return timesUsed < move.uses && disabledTurnsLeft <= 0;
    }
}
