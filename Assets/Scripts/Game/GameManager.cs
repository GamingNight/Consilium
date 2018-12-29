using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private PlayerData saveData;
    public PlayerData SaveData { get { return saveData; } }

    /*
     *DEBUG
     */
    public bool dontSaveOnDebug = false;


    void Awake() {

        if (instance != null) {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        saveData = SaveLoadManager.LoadPlayerData();
    }

    private void OnApplicationQuit() {
        if (!dontSaveOnDebug)
            SaveLoadManager.SavePlayerData(saveData);
    }
}
