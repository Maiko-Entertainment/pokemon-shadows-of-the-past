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
    public MoveEquipped(PersistedPokemonMove move)
    {
        this.move = MovesMaster.Instance.GetMove(move.GetId());
        timesUsed = move.uses;
    }

    public MoveEquipped(MoveData move, int timesUsed, int disabledTurnsLeft)
    {
        this.move = move;
        this.timesUsed = timesUsed;
        this.disabledTurnsLeft = disabledTurnsLeft;
    }

    public PersistedPokemonMove GetSave()
    {
        PersistedPokemonMove pe = new PersistedPokemonMove();
        pe.moveId = move.GetId();
        pe.uses = timesUsed;
        return pe;
    }

    public bool IsAvailable()
    {
        return timesUsed < move.uses && disabledTurnsLeft <= 0;
    }

    public void ChangeTimesUsed(int uses)
    {
        timesUsed += uses;
    }

    public MoveEquipped Copy()
    {
        return new MoveEquipped(move, timesUsed, disabledTurnsLeft);
    }
}
