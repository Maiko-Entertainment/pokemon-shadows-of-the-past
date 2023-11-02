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
    
	private void Awake()
    {
        Debug.Log("????");
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
        StartCoroutine(GoToMainMenu());
    }

    public IEnumerator GoToMainMenu()
    {
        float delay = TransitionMaster.Instance.TransitionToGameMenu();
        yield return new WaitForSeconds(delay);
        WorldMapMaster.Instance.GoToMap(13, 0);
    }
    
    public void LoadSaveSlots()
    {
        saveFiles = new List<SaveFile>();
        for (int i=0; i< maxSaveFiles; i++)
        {
            SaveFile file = GetSaveFile(i);
            if (file != null)
                saveFiles.Add(file);
        }
    }

    public void LoadSaveFile(int saveIndex)
    {
        SaveFile file = saveFiles[saveIndex];
        activeSaveFile = file == null ? new SaveFile() : file;
        lastSaveIndex = saveIndex;
        Load(activeSaveFile);
    }

    public void Load(SaveFile save) {
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
        return GetSavePath() + "/Save" + index + ".bin";
    }

    public void SaveElement(string elementName, dynamic value) {
        foreach (ObjectElement o in activeSaveFile.elements) {
            if (o.name == elementName) {
                o.value = value;
                return;
            }
        }
        Debug.Log("new element " + elementName + ":" + value);
        activeSaveFile.elements.Add(new ObjectElement(elementName, value));
    }

    public string GetElementAsString(string elementName) {
        dynamic value = GetElement(elementName);
        if (value == null) {
            return "";
        }
        return value;
       // return (string)GetElementAsObject(elementName);
    }
    
    public bool GetElementAsBoolean(string elementName) {
        //return (bool)GetElementAsObject(elementName);
        dynamic value = GetElement(elementName);
        if (value == null) {
            return false;
        }
        return value;
    }
    
    public int GetElementAsInt(string elementName) {
        dynamic value = GetElement(elementName);
        if (value == null) {
            return 0;
        }
        return value;
        // return (int)GetElement(elementName);
    }
    
    public float GetElementAsFloat(string elementName) {
        dynamic value = GetElement(elementName);
        //Debug.Log(value);
        if (value == null) {
            return 0;
        }
        return value;
    }

    public dynamic GetElement(string elementName) {
        foreach (ObjectElement o in activeSaveFile.elements) {
            if (o.name == elementName) {
                Debug.Log("get element " + elementName + ":" + o.value);
                return o.value;
            }
        }

        foreach (ObjectElement o in activeSaveFile.elements) {
            Debug.Log("saved element " + o.name + ":" + o.value);
        }
        Debug.Log("elements size " + activeSaveFile.elements.Count);

        Debug.Log("empty element " + elementName);
        return null;
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
        print("NEW SAVE");
        activeSaveFile = new SaveFile();
        Load(activeSaveFile);
    }
}