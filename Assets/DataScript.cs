using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataScript : MonoBehaviour
{
    public static DataScript instance;

    [Serializable]
    public class SaveClass
    {
        public List<LevelSaveData> leveldata;
        public OptionData optiondata;
    }
    [Serializable]
    public class LevelSaveData
    {
        public int ID;
        public float PBTime;
        public int PBTimeStop;
        public int PBMoves;
    }
    [Serializable]
    public class OptionData
    {
        public float MasterVol = 1.0f;
        public float SFXVol = 1.0f;
        public float MusicVol = 1.0f;
        public float Sensitivity = 1.0f;
    }

    [SerializeField] private SaveClass Save;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        LoadSave();
    }
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "FirstScene")
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }


    private void LoadSave()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName = "Save.json";
        string fullPath = Path.Combine(path, fileName);

        try
        {
            string json = File.ReadAllText(fullPath);
            if (json != null)
            {
                Save = JsonUtility.FromJson<SaveClass>(json);
            }
        }
        catch
        {
            Debug.Log("creating new Save data");
            Save = new SaveClass();
            Save.leveldata = new List<LevelSaveData>();
            Save.optiondata = new OptionData();
            SaveData();
        }
    }

    public LevelSaveData GetLevelData(int levelID)
    {
        foreach (LevelSaveData Data in Save.leveldata)
        {
            if (Data.ID == levelID)
            {
                return Data;
            }
        }
        return null;
    }

    public void SaveLevelData(LevelSaveData NewlevelData)
    {
        for (int i = 0; i < Save.leveldata.Count; i++)
        {
            LevelSaveData Data = Save.leveldata[i];
            if (Data.ID == NewlevelData.ID)
            {
                Save.leveldata[i] = NewlevelData;
                SaveData();
                return;
            }
        }
        // level data wit the same ID was not found;
        Save.leveldata.Add(NewlevelData);
        SaveData();
    }
    public void SaveData()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName = "Save.json";
        string fullPath = Path.Combine(path, fileName);
        string json = JsonUtility.ToJson(Save, true);

        try
        {
            File.WriteAllText(fullPath, json);
            Debug.Log($"Save Saved : {fullPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error when saving save : {e.Message}");
        }
    }
}
