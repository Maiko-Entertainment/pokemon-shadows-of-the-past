using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveMaster : MonoBehaviour
{
    public static SaveMaster Instance;

    public List<SaveFile> saveFiles = new List<SaveFile>();

    public SaveFile activeSaveFile;
    public int lastSaveIndex = 0;
    public int maxSaveFiles = 20;

    public bool loadOnStart = false;
    public bool loadHackedData = false;
    public bool startNewGame = false;

    public Dictionary<string, SaveElement> saveElements = new Dictionary<string, SaveElement>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            InstantiateDatabase();
            LoadSaveSlots();
        }
    }

    private void Start()
    {
        if (loadOnStart)
        {
            if (startNewGame)
            {
                StartNewGame();
            }
            else
            {
                LoadSaveFile(0);
            }
        }
        else
        {
            StartCoroutine(GoToMainMenu());
        }
    }

    public void InstantiateDatabase()
    {
        string savePath = ResourceMaster.Instance.GetSaveElementsPath();
        SaveElement[] saveElementsData = Resources.LoadAll<SaveElement>(savePath);
        foreach (SaveElement se in saveElementsData)
        {
            saveElements.Add(se.GetId(), se);
        }
    }

    public IEnumerator GoToMainMenu()
    {
        float delay = TransitionMaster.Instance.TransitionToGameMenu();
        yield return new WaitForSeconds(delay);
        WorldMapMaster.Instance.GoToMap(13, 0);
    }

    public void LoadSaveSlots()
    {
        if (!loadHackedData)
        {
            saveFiles = new List<SaveFile>();
            for (int i=0; i< maxSaveFiles; i++)
            {
                SaveFile file = GetSaveFile(i);
                if (file != null)
                    saveFiles.Add(file);
            }
        }
    }

    public void LoadSaveFile(int saveIndex)
    {
        SaveFile file = saveFiles[saveIndex];
        SaveFile saveFileToUse = file == null ? new SaveFile() : file;
        lastSaveIndex = saveIndex;
        Load(saveFileToUse);
    }

    public void Load(SaveFile save)
    {
        activeSaveFile = save;
        AudioMaster.GetInstance().Load(save);
        PokemonMaster.GetInstance().Load(save);
        PartyMaster.GetInstance().Load(save);
        InventoryMaster.GetInstance().Load(save);
        TacticsMaster.GetInstance().Load(save);
        WorldMapMaster.GetInstance().Load(save);
        BattleMaster.GetInstance().Load(save);
    }


    public void Save(int saveIndex)
    {
        PartyMaster.GetInstance().HandleSave();
        InventoryMaster.GetInstance().HandleSave();
        WorldMapMaster.GetInstance().HandleSave();
        PokemonMaster.GetInstance().HandleSave();
        AudioMaster.GetInstance().HandleSave();
        BattleMaster.GetInstance().HandleSave();
        lastSaveIndex = saveIndex;
        SaveCurrentFile(saveIndex);
        LoadSaveSlots();
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

    public SaveElement GetSaveElementData(string id)
    {
        if (saveElements.ContainsKey(id))
            return saveElements[id];
        return null;
    }

    public PersistedSaveElement GetSaveElementFromSavefile(string id)
    {
        foreach (PersistedSaveElement se in activeSaveFile.persistedElements)
        {
            if (se.GetId() == id)
                return se;
        }
        return null;
    }

    public float GetSaveElementFloat(string id)
    {
        try
        {
            return GetSaveElementValue<float>(id);
        }
        catch
        {
            return 0f;
        }
    }

    public string GetSaveElementString(string id)
    {
        try
        {
            return GetSaveElementValue<string>(id);
        }
        catch
        {
            return "";
        }
    }
    public object GetSaveElement(string id)
    {
        try
        {
            return GetSaveElementValue<object>(id);
        }
        catch
        {
            return null;
        }
    }

    private T GetSaveElementValue<T>(string id)
    {
        // Looks for save element if current savefile
        foreach (PersistedSaveElement se in activeSaveFile.persistedElements)
        {
            if (se.GetId() == id)
                return (T)se.value;
        }
        // Return default value if it's not found
        if (saveElements.ContainsKey(id))
        {
            return (T)saveElements[id].GetValue();
        }
        throw new Exception("Save Variable with id " + id + " not found");
    }

    public void SetSaveElement(object newValue, string id)
    {
        foreach (PersistedSaveElement se in activeSaveFile.persistedElements)
        {
            if (se.GetId() == id)
            {
                se.value = newValue;
                return;
            }
        }
        // If it doesn't exist create a new one
        PersistedSaveElement newElement = new PersistedSaveElement(id, newValue);
        activeSaveFile.persistedElements.Add(newElement);
    }

    public SaveFile GetActiveSave()
    {
        return activeSaveFile;
    }

    public List<SaveFile> GetSaveFiles()
    {
        return saveFiles;
    }

    public void StartNewGame()
    {
        Load(new SaveFile());
    }
}
