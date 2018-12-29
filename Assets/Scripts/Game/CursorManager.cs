using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    public MapManager mapManager;

    public GameObject[] allEffectPrefabs;
    public IconSelector[] allIconSelectors;
    public GameObject[] defaultAvailableEffects;

    private List<GameObject> availableEffects;
    private List<IconSelector> availableIconSelectors;
    int activeEffectIndex;
    EffectData activeEffectData;

    void Start() {
        availableEffects = new List<GameObject>();
        availableIconSelectors = new List<IconSelector>();
        for (int i = 0; i < defaultAvailableEffects.Length; i++) {
            availableEffects.Add(defaultAvailableEffects[i]);
            allIconSelectors[i].gameObject.SetActive(true);
            availableIconSelectors.Add(allIconSelectors[i]);
        }
        UpdateAvailableEffects();

        activeEffectIndex = 0;//default effect is the first in the list
        GameObject activeEffect = Instantiate<GameObject>(availableEffects[activeEffectIndex]);
        activeEffect.transform.parent = transform;
        activeEffect.transform.localPosition = Vector3.zero;
        activeEffectData = activeEffect.GetComponent<EffectData>();
        activeEffectData.particles.Stop();
    }

    private void UpdateAvailableEffects() {

        List<SerializableShopItemData> purchasedItems = GameManager.Instance.SaveData.purchasedItems;
        for (int i = 0; i < purchasedItems.Count; i++) {
            if (purchasedItems[i].type == AbstractShopItem.Type.NEW_EFFECT) {
                bool found = false;
                int j = 0;
                while (j < allEffectPrefabs.Length && !found) {
                    if (allEffectPrefabs[j].GetComponent<EffectData>().effectName == purchasedItems[i].effectName) {
                        if (!availableEffects.Contains(allEffectPrefabs[j])) {
                            availableEffects.Add(allEffectPrefabs[j]);
                            allIconSelectors[j].gameObject.SetActive(true);
                            availableIconSelectors.Add(allIconSelectors[j]);
                        }
                        found = true;
                    }
                    j++;
                }

            }
        }
    }

    void Update() {
        //Change the selected effect as soon as player right click or press space
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) {
            activeEffectData.particles.Stop();
            activeEffectData.audioFade.StopWithFadeOut();
            Destroy(activeEffectData.gameObject);
            availableIconSelectors[activeEffectIndex].Unselect();
            //Update index
            activeEffectIndex = (activeEffectIndex + 1) % availableEffects.Count;
            GameObject newActiveEffect = Instantiate<GameObject>(availableEffects[activeEffectIndex]);
            newActiveEffect.transform.parent = transform;
            newActiveEffect.transform.localPosition = Vector3.zero;
            activeEffectData = newActiveEffect.GetComponent<EffectData>();
            activeEffectData.particles.Stop();
            availableIconSelectors[activeEffectIndex].Select();
        }

        if (mapManager.SelectedTile != null) {
            transform.position = mapManager.SelectedTile.transform.position;
        }

        //Activate particle effect if player presses mouse left button
        if (mapManager.SelectedTile != null && Input.GetMouseButton(0)) {
            if (!activeEffectData.particles.isPlaying)
                activeEffectData.particles.Play();
            if (!activeEffectData.audioFade.isPlaying || activeEffectData.audioFade.isFadingOut)
                activeEffectData.audioFade.PlayWithFadeIn();
        } else {
            activeEffectData.particles.Stop();
            if (activeEffectData.audioFade.isPlaying && !activeEffectData.audioFade.isFadingOut)
                activeEffectData.audioFade.StopWithFadeOut();
        }

        //Update tile values
        bool effectCompatibleWithSelectedTile = false;
        if (mapManager.SelectedTile != null)
            effectCompatibleWithSelectedTile = mapManager.SelectedTile.GetComponent<TileData>().LastStep.name.ToString().Contains(activeEffectData.effectName.ToString());
        GameObject[][] map = mapManager.Map;
        for (int i = 0; i < map.Length; i++) {
            for (int j = 0; j < map[i].Length; j++) {
                //Left click + selectedTile + compatible effect = increase value
                if (Input.GetMouseButton(0) && map[i][j] == mapManager.SelectedTile && effectCompatibleWithSelectedTile) {
                    //increase value = the defaut speed of the effect * the bonus unlocked by the player in the shop
                    float value = activeEffectData.increaseSpeed * GetActiveEffectMultiplier();
                    map[i][j].GetComponent<TileData>().UpdateValue(true, value);
                } else {
                    map[i][j].GetComponent<TileData>().UpdateValue(false, activeEffectData.decreaseSpeed);
                }
            }
        }
    }

    private int GetActiveEffectMultiplier() {

        int multiplier = 1;
        List<SerializableShopItemData> purchasedItems = GameManager.Instance.SaveData.purchasedItems;
        for (int i = 0; i < purchasedItems.Count; i++) {
            if (purchasedItems[i].type == AbstractShopItem.Type.DOUBLE_EFFECT) {
                if (purchasedItems[i].effectName == activeEffectData.effectName) {
                    multiplier *= 2;
                }
            }
        }
        return multiplier;
    }
}
