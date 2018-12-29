using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneScenario : MonoBehaviour {

    public enum MenuState {
        LOGO_FADE_IN, LOGO_WAIT, LOGO_FADE_OUT, BLACK_SCREEN_FADE_OUT, INTRO_WAIT, INTRO_FADE_OUT, MENU, NONE
    }

    public GameObject logo;
    public GameObject blackScreen;
    public GameObject menu;
    public GameObject intro;
    public GameObject greyScreen;

    private Coroutine currentRoutine;
    private MenuState currentState;

    void Start() {
        currentState = MenuState.LOGO_FADE_IN;
    }

    void Update() {

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            StopCoroutine(currentRoutine);
            menu.SetActive(true);
            currentState = MenuState.MENU;
        }

        if (currentState == MenuState.LOGO_FADE_IN) {
            currentRoutine = StartCoroutine(LogoFadeInCoroutine(2f));
            currentState = MenuState.NONE;
        } else if (currentState == MenuState.LOGO_WAIT) {
            currentRoutine = StartCoroutine(LogoWaitCoroutine(0.5f));
            currentState = MenuState.NONE;
        } else if (currentState == MenuState.LOGO_FADE_OUT) {
            currentRoutine = StartCoroutine(LogoFadeOutCoroutine(2f));
            currentState = MenuState.NONE;
        } else if (currentState == MenuState.BLACK_SCREEN_FADE_OUT) {
            currentRoutine = StartCoroutine(BlackScreenFadeOutCoroutine(0.5f));
            currentState = MenuState.NONE;
        } else if (currentState == MenuState.INTRO_WAIT) {
            currentRoutine = StartCoroutine(IntroWaitCoroutine(15f));
            currentState = MenuState.NONE;
        } else if (currentState == MenuState.INTRO_FADE_OUT) {
            currentRoutine = StartCoroutine(IntroFadeOutCoroutine(0.5f));
            currentState = MenuState.NONE;
        } else if (currentState == MenuState.MENU) {
            logo.SetActive(false);
            blackScreen.SetActive(false);
            intro.SetActive(false);
            greyScreen.SetActive(false);
            currentState = MenuState.NONE;
        }
    }

    private IEnumerator LogoFadeInCoroutine(float lerpDuration) {

        Image logoImage = logo.GetComponent<Image>();
        float step = 0;
        while (step < lerpDuration) {
            float lerp = Mathf.Lerp(0, 1, step / lerpDuration);
            logoImage.color = new Color(logoImage.color.r, logoImage.color.g, logoImage.color.b, lerp);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        currentState = MenuState.LOGO_WAIT;
    }

    private IEnumerator LogoWaitCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        currentState = MenuState.LOGO_FADE_OUT;
    }

    private IEnumerator LogoFadeOutCoroutine(float lerpDuration) {

        Image logoImage = logo.GetComponent<Image>();
        float step = 0;
        while (step < lerpDuration) {
            float lerp = Mathf.Lerp(1, 0, step / lerpDuration);
            logoImage.color = new Color(logoImage.color.r, logoImage.color.g, logoImage.color.b, lerp);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        currentState = MenuState.BLACK_SCREEN_FADE_OUT;
    }

    private IEnumerator BlackScreenFadeOutCoroutine(float lerpDuration) {

        Image blackScreenImage = blackScreen.GetComponent<Image>();
        menu.SetActive(true);
        float step = 0;
        while (step < lerpDuration) {
            float lerp = Mathf.Lerp(1, 0, step / lerpDuration);
            blackScreenImage.color = new Color(blackScreenImage.color.r, blackScreenImage.color.g, blackScreenImage.color.b, lerp);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        currentState = MenuState.INTRO_WAIT;
    }

    private IEnumerator IntroWaitCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        currentState = MenuState.INTRO_FADE_OUT;
    }

    private IEnumerator IntroFadeOutCoroutine(float lerpDuration) {

        Image introImage = intro.GetComponent<Image>();
        Image greyImage = greyScreen.GetComponent<Image>();
        menu.SetActive(true);
        float step = 0;
        while (step < lerpDuration) {
            float lerp = Mathf.Lerp(1, 0, step / lerpDuration);
            introImage.color = new Color(introImage.color.r, introImage.color.g, introImage.color.b, lerp);
            greyImage.color = new Color(greyImage.color.r, greyImage.color.g, greyImage.color.b, lerp);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        currentState = MenuState.MENU;
    }
}
