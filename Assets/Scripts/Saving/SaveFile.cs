using System.Collections.Generic;
[System.Serializable]
public class SaveFile
{
    public List<PersistedSaveElement> persistedElements = new List<PersistedSaveElement>();
    public List<PersistedItem> persistedItems = new List<PersistedItem>();
    public List<PersistedPokemon> persistedParty = new List<PersistedPokemon>();
    public List<PersistedPokemon> persistedBox = new List<PersistedPokemon>();
}
