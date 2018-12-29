using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScenario : MonoBehaviour {

    public enum GameOverState {
        DEZOOM_CAMERA, DISPLAY_STATS, NONE
    }

    public Image gameOverImage;
    public Text scoreText;
    public Text scoreValueText;
    public Text bonusText;
    public Text bonusValueText;
    public Text totalScoreText;
    public Text totalScoreValueText;
    public Image retryImage;
    public Image shopImage;
    public Image quitImage;

    private GameOverState state;

    void Start() {

        state = GameOverState.DEZOOM_CAMERA;
    }

    void Update() {

        if (state == GameOverState.DEZOOM_CAMERA) {
            StartCoroutine(AdjustCameraCoroutine(1f, 8.5f, 5.5f));
            state = GameOverState.NONE;
        } else if (state == GameOverState.DISPLAY_STATS) {
            StartCoroutine(DisplayHUD(1f));
            state = GameOverState.NONE;
        }
    }

    private IEnumerator AdjustCameraCoroutine(float lerpDuration, float targetCameraZoom, float targetXPosition) {

        float initZoom = Camera.main.orthographicSize;
        Vector3 initPos = Camera.main.transform.position;
        float step = 0;
        while (step < lerpDuration) {
            Camera.main.orthographicSize = Mathf.Lerp(initZoom, targetCameraZoom, step / lerpDuration);
            Camera.main.transform.position = new Vector3(Mathf.Lerp(initPos.x, targetXPosition, step / lerpDuration), initPos.y, initPos.z);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        state = GameOverState.DISPLAY_STATS;
    }

    private IEnumerator DisplayHUD(float lerpDuration) {

        float step = 0;
        while (step < lerpDuration) {
            float lerp = Mathf.Lerp(0, 1, step / lerpDuration);
            gameOverImage.color = new Color(1, 1, 1, lerp);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, lerp);
            scoreValueText.color = new Color(scoreValueText.color.r, scoreValueText.color.g, scoreValueText.color.b, lerp);
            bonusText.color = new Color(bonusText.color.r, bonusText.color.g, bonusText.color.b, lerp);
            bonusValueText.color = new Color(bonusValueText.color.r, bonusValueText.color.g, bonusValueText.color.b, lerp);
            totalScoreText.color = new Color(totalScoreText.color.r, totalScoreText.color.g, totalScoreText.color.b, lerp);
            totalScoreValueText.color = new Color(totalScoreValueText.color.r, totalScoreValueText.color.g, totalScoreValueText.color.b, lerp);
            retryImage.color = new Color(1, 1, 1, lerp);
            shopImage.color = new Color(1, 1, 1, lerp);
            quitImage.color = new Color(1, 1, 1, lerp);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
