[System.Serializable]

public class ObjectElement
{
    public string name;
    public dynamic value;
    
    public ObjectElement(string name, dynamic value)
    {
        this.name = name;
        this.value = value;
    }
}