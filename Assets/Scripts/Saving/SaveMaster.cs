using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

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
        activeSaveFile = file == null ? new SaveFile() : file;
        lastSaveIndex = saveIndex;
        Load(activeSaveFile);
    }

    public void Load(SaveFile save)
    {
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
        activeSaveFile = new SaveFile();
        Load(activeSaveFile);
    }
}
