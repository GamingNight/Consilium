using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class AbstractShopItem : MonoBehaviour {

    public enum Type {
        NEW_EFFECT, DOUBLE_EFFECT, INCREASE_TIME
    }

    //UI
    public Text itemNameText;
    public Text priceText;
    public Button unlockButton;
    public GameObject disablePanel;
    public Text requiredItemsText;

    //Item data
    public Type type;
    public int id;
    public string itemName;
    public int price;
    public int[] requiredItemIds;

    private bool isUnlocked;
    public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = value; } }

    public void Unlock() {

        if (GameManager.Instance.SaveData.globalScore >= price) {
            GameManager.Instance.SaveData.purchasedItems.Add(new SerializableShopItemData().OnSave(this));
            GameManager.Instance.SaveData.globalScore -= price;
            GameOverManager.Instance.UpdateShopCanvas();
        }
    }
}
