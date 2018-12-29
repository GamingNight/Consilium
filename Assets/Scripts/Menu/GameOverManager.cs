using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverManager : MonoBehaviour {

    private static GameOverManager instance;
    public static GameOverManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Text rawScoreText;
    public Text bonusText;
    public Text totalScoreText;
    public GameObject gameOverTilePrefab;
    public GameObject finalMap;
    public float tileHeight = 4;
    public float tileWidth = 5;
    public GameObject gameOverCanvasObject;
    public GameObject shopCanvasObject;
    public Text shopPointText;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    void Start() {

        int intRawScore = (int)EndGameStats.RAW_SCORE;
        rawScoreText.text = intRawScore.ToString();
        float roundedMultiplier = ((int)(10 * EndGameStats.HOMOGENEITY_MULTIPLIER)) / 10f;
        bonusText.text = "x" + roundedMultiplier;
        totalScoreText.text = (intRawScore * roundedMultiplier).ToString();

        GenerateSpriteMap();
        UpdateShopCanvas();
    }

    private void GenerateSpriteMap() {

        if (EndGameStats.SPRITES == null)
            return;

        float initPosX = 0;
        float initPosY = tileHeight * (EndGameStats.SPRITES.Length - 1) / 2;
        for (int i = 0; i < EndGameStats.SPRITES.Length; i++) {
            for (int j = 0; j < EndGameStats.SPRITES[i].Length; j++) {
                GameObject tileClone = Instantiate<GameObject>(gameOverTilePrefab);
                tileClone.gameObject.name = "Tile" + i.ToString() + j.ToString();
                tileClone.transform.parent = finalMap.transform;
                tileClone.transform.position = new Vector3(initPosX - i * tileWidth / 2 + j * tileWidth / 2, initPosY - i * tileHeight / 2 - j * tileHeight / 2, 0);
                tileClone.GetComponent<SpriteRenderer>().sprite = EndGameStats.SPRITES[i][j];
                tileClone.GetComponent<SpriteRenderer>().sortingOrder = 2 + (i * EndGameStats.SPRITES.Length + j);
            }
        }
    }

    public void UpdateShopCanvas() {

        AbstractShopItem[] allShopItems = shopCanvasObject.GetComponentsInChildren<AbstractShopItem>();


        //Update various features
        for (int i = 0; i < allShopItems.Length; i++) {
            //Item names
            allShopItems[i].itemNameText.text = allShopItems[i].itemName;
            //Price
            allShopItems[i].priceText.text = allShopItems[i].price.ToString();
            Color priceTextColor = allShopItems[i].priceText.color;
            if (allShopItems[i].price > GameManager.Instance.SaveData.globalScore) {
                allShopItems[i].priceText.color = new Color(255, priceTextColor.g, priceTextColor.b);
                allShopItems[i].disablePanel.SetActive(true);
                allShopItems[i].unlockButton.gameObject.SetActive(false);
            } else {
                allShopItems[i].priceText.color = new Color(priceTextColor.r, priceTextColor.g, priceTextColor.b);
                allShopItems[i].disablePanel.SetActive(false);
                allShopItems[i].unlockButton.gameObject.SetActive(true);
            }
            //Purchased
            allShopItems[i].IsUnlocked = false;
            int j = 0;
            while (j < GameManager.Instance.SaveData.purchasedItems.Count && !allShopItems[i].IsUnlocked) {
                if (GameManager.Instance.SaveData.purchasedItems[j].id == allShopItems[i].id) {
                    allShopItems[i].IsUnlocked = true;
                    allShopItems[i].disablePanel.SetActive(true);
                    Image disablePanelImage = allShopItems[i].disablePanel.GetComponent<Image>();
                    disablePanelImage.color = new Color(disablePanelImage.color.r, 255, disablePanelImage.color.b, disablePanelImage.color.a);
                    allShopItems[i].unlockButton.gameObject.SetActive(false);
                    allShopItems[i].priceText.text = "Purchased";
                }
                j++;
            }
        }

        //If item is locked, check required items
        for (int i = 0; i < allShopItems.Length; i++) {
            Debug.Log("item " + allShopItems[i].itemName);
            if (!allShopItems[i].IsUnlocked) {
                Debug.Log("is not unlocked");
                int[] requiredItemIds = allShopItems[i].requiredItemIds;
                allShopItems[i].requiredItemsText.text = "";
                bool first = true;
                for (int j = 0; j < requiredItemIds.Length; j++) {
                    Debug.Log("requires item #" + requiredItemIds[j]);
                    bool isPurchased = false;
                    int k = 0;
                    string requiredItemName = "";
                    while (k < allShopItems.Length && requiredItemName == "") {
                        if (allShopItems[k].id == requiredItemIds[j]) {
                            Debug.Log("found in shop list");
                            isPurchased = allShopItems[k].IsUnlocked;
                            requiredItemName = allShopItems[k].itemName;
                        }
                        k++;
                    }
                    if (!isPurchased) {
                        Debug.Log("not purchased");
                        if (first) {
                            allShopItems[i].requiredItemsText.text = "Required: ";
                            allShopItems[i].disablePanel.SetActive(true);
                            allShopItems[i].requiredItemsText.gameObject.SetActive(true);
                            allShopItems[i].unlockButton.gameObject.SetActive(false);
                            first = false;
                        } else {
                            allShopItems[i].requiredItemsText.text += ", ";
                        }
                        allShopItems[i].requiredItemsText.text += "\"";
                        allShopItems[i].requiredItemsText.text += requiredItemName;
                        allShopItems[i].requiredItemsText.text += "\"";
                    }
                }
            }
        }

        //Update shop point text
        shopPointText.text = "Your points: " + GameManager.Instance.SaveData.globalScore;
    }

    public void LoadMainScene() {
        SceneManager.LoadScene("main");
    }

    public void Quit() {
        Application.Quit();
    }

    public void LoadShop() {

        gameOverCanvasObject.SetActive(false);
        finalMap.SetActive(false);
        shopPointText.text = "Your points: " + GameManager.Instance.SaveData.globalScore;
        shopCanvasObject.SetActive(true);
    }

    public void QuitShop() {
        shopCanvasObject.SetActive(false);
        gameOverCanvasObject.SetActive(true);
        finalMap.SetActive(true);
    }
}
