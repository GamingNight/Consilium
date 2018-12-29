using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

    public int mapSize = 5;
    public float tileHeight = 4;
    public float tileWidth = 5;
    public GameObject tilePrefab;
    public GameObject selectorPrefab;

    private GameObject[][] map;
    public GameObject[][] Map { get { return map; } }
    private GameObject selector;
    private GameObject selectedTile;
    public GameObject SelectedTile { get { return selectedTile; } }

    void Start() {

        GenerateMap();
        selector = Instantiate<GameObject>(selectorPrefab);
        selector.SetActive(false);
        selectedTile = null;
    }

    private void GenerateMap() {

        map = new GameObject[mapSize][];

        float initPosX = 0;
        float initPosY = tileHeight * (mapSize - 1) / 2;
        for (int i = 0; i < mapSize; i++) {
            map[i] = new GameObject[mapSize];
            for (int j = 0; j < mapSize; j++) {
                GameObject tileClone = Instantiate<GameObject>(tilePrefab);
                tileClone.transform.position = new Vector3(initPosX - i * tileWidth / 2 + j * tileWidth / 2, initPosY - i * tileHeight / 2 - j * tileHeight / 2, 0);
                tileClone.transform.parent = gameObject.transform;
                tileClone.name = tilePrefab.name + "" + i + "" + j;
                tileClone.GetComponent<SpriteRenderer>().sortingOrder = 2 + (i * mapSize + j);
                map[i][j] = tileClone;
            }
        }

        InitTileData();
    }

    private void InitTileData() {

        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                map[i][j].GetComponent<TileData>().initValue = 0;
            }
        }

    }

    void Update() {

        //Update selector depending on mouse position
        int i = 0;
        selectedTile = null;
        Vector2 selectedTileIndices = new Vector2(-1, -1);
        while (i < mapSize && selectedTile == null) {
            int j = 0;
            while (j < mapSize && selectedTile == null) {
                TileMouseDetector mouseDetector = map[i][j].GetComponent<TileMouseDetector>();
                if (mouseDetector.MouseIsOver) {
                    selectedTile = map[i][j];
                    selector.transform.position = map[i][j].transform.position;
                    selector.SetActive(true);
                    selectedTileIndices.x = i;
                    selectedTileIndices.y = j;
                }
                j++;
            }
            i++;
        }

        //Disable selector if mouse is away
        if (selectedTile == null) {
            selector.SetActive(false);
        }
    }

    public void HideAllTiles() {
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                map[i][j].SetActive(false);
            }
        }
    }

    public void ShowTile(int i, int j) {

        map[i][j].SetActive(true);
    }

    public void SetTileTransparency(int i, int j, float alpha) {
        Color tileColor = map[i][j].GetComponent<SpriteRenderer>().color;
        map[i][j].GetComponent<SpriteRenderer>().color = new Color(tileColor.r, tileColor.g, tileColor.b, alpha);
    }

    public float ComputeScore() {

        float score = 0;
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                score += map[i][j].GetComponent<TileData>().Value;
            }
        }
        return score;
    }

    public float ComputeMeanScore() {

        float mean = 0;
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                mean += map[i][j].GetComponent<TileData>().Value;
            }
        }
        if (mapSize != 0) {
            mean /= (mapSize * mapSize);
        }
        return mean;
    }

    public float ComputeStandardDeviationScore() {

        float sd = 0;
        float mean = ComputeMeanScore();
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                float value = map[i][j].GetComponent<TileData>().Value;
                sd += Mathf.Pow(mean - value, 2);
            }
        }
        if (mapSize != 0) {
            sd = Mathf.Sqrt(sd / (mapSize * mapSize));
        }
        return sd;
    }

    public float TerritoryEqualityBonus() {
        // Fixme 12 is the number of step
        // please update if you update scoreSteps and scoreSteps_ref
        int nbSteps = 11;
        int[] scoreSteps = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] scoreSteps_ref = new int[] { 0, 50, 100, 200, 300, 320, 400, 550, 700, 720, 850, 1000 };

        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                float score = map[i][j].GetComponent<TileData>().Value;
                int steps = 0;
                for (int k = 0; k < nbSteps; k++) {
                    if (score > scoreSteps_ref[k])
                        steps = k;
                }
                scoreSteps[steps] = scoreSteps[steps] + 1;
            }
        }
        int bonus = 1;

        // Do not count empty tile -> start at 1
        for (int i = 1; i < nbSteps; i++) {
            if (bonus < scoreSteps[i]) {
                bonus = scoreSteps[i];
            }
        }
        return (float)bonus;

    }


    public void PrintScores() {

        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                Debug.Log(i + "_" + j + " = " + map[i][j].GetComponent<TileData>().Value);
            }
        }
    }
}
