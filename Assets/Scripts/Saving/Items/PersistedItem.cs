[System.Serializable]
public class PersistedItem
{
    public string itemId;
    public ItemId id;
    public int amount = 0;

    public string GetId()
    {
        return string.IsNullOrEmpty(itemId) ? id.ToString() : itemId;
    }
}
