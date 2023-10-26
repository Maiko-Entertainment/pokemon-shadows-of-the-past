[System.Serializable]
public class PersistedSaveElement
{
    public SaveElementId id;
    protected string idString;
    public object value;

    public PersistedSaveElement(string id, object value)
    {
        idString = id;
        this.value = value;
    }

    public string GetId()
    {
        return string.IsNullOrEmpty(idString) ? id.ToString() : idString;
    }

}
