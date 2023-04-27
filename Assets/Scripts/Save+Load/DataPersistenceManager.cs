using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private SaveData saveData;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        //LoadGame();
    }

    public void NewGame()
    {
        this.saveData = new SaveData();
    }

    public void LoadGame()
    {
        //Load data from handler
        this.saveData = dataHandler.Load();

        //Initialize new game if data was not found
        if(this.saveData != null)
        {
            NewGame();
        }

        //Update data in scripts that need it
        if(saveData != null)
            GameData.progressFlags = saveData.progressFlags;
    }

    public void SaveGame()
    {
        //Get most recent progress flags
        saveData.progressFlags = GameData.progressFlags;

        //Save saveData to file
        dataHandler.Save(saveData);
    }
}
