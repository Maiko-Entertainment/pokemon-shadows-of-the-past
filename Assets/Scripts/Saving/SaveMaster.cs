using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveMaster : MonoBehaviour
{
    public static SaveMaster Instance;

    public List<SaveFile> saveFiles = new List<SaveFile>();

    public SaveFile activeSaveFile = new SaveFile();
    public int lastSaveIndex = 0;
    public int maxSaveFiles = 5;

    public bool loadOnStart = false;
    public bool loadHackedData = false;

    public List<SaveElement> saveElements = new List<SaveElement>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            LoadSaveSlots();
        }
    }

    private void Start()
    {
        if (loadOnStart)
            Load(0);
    }

    public void LoadSaveSlots()
    {
        if (!loadHackedData)
        {
            saveFiles = new List<SaveFile>();
            for (int i=0; i< maxSaveFiles; i++)
            {
                SaveFile file = GetSaveFile(i);
                saveFiles.Add(file);
            }
        }
    }

    public void Load(int saveIndex)
    {
        SaveFile file = saveFiles[saveIndex];
        activeSaveFile = file == null ? new SaveFile() : file;
        PartyMaster.GetInstance().Load(activeSaveFile);
        InventoryMaster.GetInstance().Load(activeSaveFile);
        TacticsMaster.GetInstance().Load(activeSaveFile);
        WorldMapMaster.GetInstance().Load(activeSaveFile);
    }

    public void Save(int saveIndex)
    {
        PartyMaster.GetInstance().HandleSave();
        InventoryMaster.GetInstance().HandleSave();
        WorldMapMaster.GetInstance().HandleSave();
        SaveCurrentFile(saveIndex);
    }

    public void SaveCurrentFile(int saveIndex)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string saveDir = GetSavePath();
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }
        string path = GetFilePath(saveIndex);
        FileStream fileStream = File.Create(path);
        formatter.Serialize(fileStream, activeSaveFile);
        fileStream.Close();
    }

    public SaveFile GetSaveFile(int saveIndex)
    {
        if (!File.Exists(GetFilePath(saveIndex)))
        {
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(GetFilePath(saveIndex), FileMode.Open);
        try
        {
            SaveFile file = (SaveFile) formatter.Deserialize(fileStream);
            fileStream.Close();
            return file;
        }
        catch
        {
            print("Error loading savefile " + saveIndex);
        }
        fileStream.Close();
        return null;
    }

    public string GetSavePath()
    {
        return Application.persistentDataPath + "/Saves";
    }
    public string GetFilePath(int index)
    {
        return GetSavePath() + "/Save" + index + ".txt";
    }
    public SaveElement GetSaveElement(SaveElementId id)
    {
        foreach(SaveElement se in saveElements)
        {
            if (se.id == id)
                return se;
        }
        return null;
    }

    public PersistedSaveElement GetSaveElementInner(SaveElementId id)
    {
        foreach (PersistedSaveElement se in activeSaveFile.persistedElements)
        {
            if (se.id == id)
                return se;
        }
        return null;
    }

    public void SetSaveElementInner(object newValue, SaveElementId id)
    {
        foreach (PersistedSaveElement se in activeSaveFile.persistedElements)
        {
            if (se.id == id)
            {
                se.value = newValue;
                return;
            }
        }
        PersistedSaveElement newElement = new PersistedSaveElement();
        newElement.id = id;
        newElement.value = newValue;
        activeSaveFile.persistedElements.Add(newElement);
    }
}
