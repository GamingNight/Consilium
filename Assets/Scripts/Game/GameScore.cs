using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

	public MapManager mapManager;

	public Text scoreText;
	public Text coefText;

	public int totalScore;
	public float homogeneityCoef;

	void Update() {
		totalScore = (int)mapManager.ComputeScore();
		scoreText.text = totalScore.ToString ();

		// new bonus
		homogeneityCoef = mapManager.TerritoryEqualityBonus();
		coefText.text = "x" + homogeneityCoef.ToString ();

		// old bonus
		// float sdScore = mapManager.ComputeStandardDeviationScore();
		// TileData.TileStep[] steps = mapManager.Map[0][0].GetComponent<TileData>().steps;
		// int maxScore = steps[steps.Length - 1].value + 100;
		// float middleScore = maxScore / 2f;
		// sdScore = Mathf.Min(sdScore, middleScore);
		// //The greater is the standard deviation, the lower is the multiplier
		// homogeneityCoef = 5 * ((1 + (middleScore - sdScore) / middleScore) / 2f);
		// coefText.text = homogeneityCoef.ToString ();
	}
}