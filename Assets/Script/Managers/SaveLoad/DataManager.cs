using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SettingsData settingsData;
    public SaveData gameData;
    public string settingsFile = "Settings.conf";
    public string gameSaveFile = "Save1.sav";

    #region Instance
    public static DataManager Instance { get; private set; }//static instance

    private void Awake()
    {
        //make sure there is always only 1 instance
        if (DataManager.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        settingsData ??= new SettingsData();//if settings is null, create data
        gameData ??= new SaveData();
    }
    #endregion

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settingsData); //convert settings data to json
        WriteSettings(settingsFile, json); //stick it in the file
    }
    public void LoadSettings()
    {
        string json = ReadSettings(settingsFile); //read file
        JsonUtility.FromJsonOverwrite(json, settingsData); //replace anything in the data container with what was read from file
    }
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData); //convert settings data to json
        WriteGame(gameSaveFile, json); //stick it in the file
    }
    public void LoadGame()
    {
        string json = ReadGame(gameSaveFile); //read file
        JsonUtility.FromJsonOverwrite(json, gameSaveFile); //replace anything in the data container with what was read from file
    }

    public string ReadSettings(string fileName)
    {
        string path = GetFilePath(fileName, settingsData); //get the file path
        if (File.Exists(path)) //check if file exists
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd(); //read file
                return json; //return data from file
            }
        }
        else
        {
            Debug.LogWarning("File Not Found, auto-generating...");  //not found warning

            //set default values
            settingsData.masterVolume = 1.0f;
            settingsData.musicVolume = 0.5f;
            settingsData.effectsVolume = 0.5f;
            settingsData.resolution = 0;
            settingsData.fullScreen = true;
            settingsData.quality = 0;
            SaveSettings();//save data
        }

        return null;
    }
    public string ReadGame(string fileName)
    {
        string path = GetFilePath(fileName, gameData); //get the file path
        if (File.Exists(path)) //check if file exists
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd(); //read file
                return json; //return data from file
            }
        }
        else
        {
            Debug.LogWarning("File Not Found, auto-generating...");  //not found warning

            SaveGame();//save data
        }

        return null;
    }
    private void WriteSettings(string fileName, string json)
    {
        string path = GetFilePath(fileName, settingsData); //get path
        FileStream filestream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(filestream))
        {
            writer.Write(json); //write file
        }
    }
    //currently the same as settings, this should change to a more secure method at a later date
    private void WriteGame(string fileName, string json)
    {
        string path = GetFilePath(fileName, gameData); //get path
        FileStream filestream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(filestream))
        {
            writer.Write(json); //write file
        }
    }

    private string GetFilePath(string fileName, SettingsData settingsFile)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
    private string GetFilePath(string fileName, SaveData saveFile)
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + "Project Shadowsbane" + "/" + fileName;
    }
}
