using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class PlayerData {

    public int globalScore;
    public List<SerializableShopItemData> purchasedItems;
}


public static class SaveLoadManager {

    public readonly static string PLAYER_SAVE_FILE_PATH = Application.persistentDataPath + "/player.bin";

    public static void SavePlayerData(PlayerData playerData) {

        Debug.Log("Saving data on file...");
        Debug.Log("Saving global Score = " + playerData.globalScore);
        foreach (SerializableShopItemData shopItem in playerData.purchasedItems) {
            Debug.Log("Saving shop item #" + shopItem.id);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(PLAYER_SAVE_FILE_PATH, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();

        Debug.Log("done.");
    }

    public static PlayerData LoadPlayerData() {

        Debug.Log("Loading data from file " + PLAYER_SAVE_FILE_PATH + "...");

        PlayerData playerData = null;
        if (File.Exists(PLAYER_SAVE_FILE_PATH)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PLAYER_SAVE_FILE_PATH, FileMode.Open);

            playerData = formatter.Deserialize(stream) as PlayerData;
        } else {
            playerData = new PlayerData();
            playerData.globalScore = 0;
            playerData.purchasedItems = new List<SerializableShopItemData>();
        }

        Debug.Log("Loaded global Score = " + playerData.globalScore);
        foreach (SerializableShopItemData shopItem in playerData.purchasedItems) {
            Debug.Log("Loaded shop item #" + shopItem.id);
        }
        Debug.Log("done.");

        return playerData;
    }
}
