[System.Serializable]
public class PersistedPokemonMove
{
    public MoveId id;
    public string moveId;
    public int uses;

    public string GetId()
    {
        return string.IsNullOrEmpty(moveId) ? id.ToString() : moveId;
    }
}
