using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {

    public enum StateName {
        RAIN, RAINWIND, WIND, WINDSUN, SUN, SUNTEUB, TEUB
    }

    [System.Serializable]
    public class TileState {
        public StateName name;
        public Sprite sprite;
        public int value;
    }

    public TileState[] states;
    public int initValue = 100;

    public SpriteRenderer transitionSpriteRenderer;

    private SpriteRenderer spriteRenderer;
    private float value;
    public float Value { get { return value; } }

    private TileState lastState;
    public TileState LastStep { get { return lastState; } }
    private Coroutine runningTransitionCoroutine;

    void Start() {
        value = initValue;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastState = null;
        runningTransitionCoroutine = null;
    }

    //Called by the map manager
    public void UpdateValue(bool increase, float effectMultiplier) {

        //Update Values
        if (increase) {
            value = value + (Time.deltaTime * effectMultiplier);
        } else {
            value = Mathf.Max(0, value - (Time.deltaTime * effectMultiplier));
        }

        //Check for state changing
        int i = 0;
        while (i < states.Length && value > states[i].value) {
            i++;
        }
        if (i == 0)
            i++;

        TileState nextState = states[i - 1];
        if (lastState == null)
            spriteRenderer.sprite = nextState.sprite;
        else if (lastState.sprite != nextState.sprite) {
            ProcessSpriteTransition(lastState.sprite, nextState.sprite);
        }
        lastState = nextState;
    }

    private void ProcessSpriteTransition(Sprite prevSprite, Sprite nextSprite) {
        if (runningTransitionCoroutine != null)
            StopCoroutine(runningTransitionCoroutine);
        runningTransitionCoroutine = StartCoroutine(SpriteTransitionCoroutine(0.5f, prevSprite, nextSprite));
    }

    private IEnumerator SpriteTransitionCoroutine(float lerpDuration, Sprite prevSprite, Sprite nextSprite) {
        Color tsrCol = transitionSpriteRenderer.color;
        Color srCol = spriteRenderer.color;

        transitionSpriteRenderer.color = new Color(tsrCol.r, tsrCol.g, tsrCol.b, srCol.a);
        transitionSpriteRenderer.sprite = prevSprite;

        spriteRenderer.color = new Color(srCol.r, srCol.g, srCol.b, tsrCol.a);
        spriteRenderer.sprite = nextSprite;

        float interpolation = 0;
        while (interpolation < lerpDuration) {
            float alpha = Mathf.Lerp(tsrCol.a, 1, interpolation / lerpDuration);
            spriteRenderer.color = new Color(srCol.r, srCol.g, srCol.b, alpha);
            float talpha = Mathf.Lerp(srCol.a, 0, interpolation / lerpDuration);
            transitionSpriteRenderer.color = new Color(srCol.r, srCol.g, srCol.b, talpha);
            interpolation += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        transitionSpriteRenderer.sprite = null;
    }
}
