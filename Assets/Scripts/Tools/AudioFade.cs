using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFade : MonoBehaviour {

    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    private AudioSource audioSource;
    public AudioClip clip { get { return audioSource.clip; } set { audioSource.clip = value; } }
    public bool isPlaying { get { return audioSource.isPlaying; } }

    private float initVolume;
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    private bool isFadeInRunning;
    private bool isFadeOutRunning;
    public bool isFadingOut { get { return isFadeOutRunning; } }
    public bool isFadingIn { get { return isFadeInRunning; } }

    private void Awake() {

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        initVolume = audioSource.volume;
        fadeInCoroutine = null;
        fadeOutCoroutine = null;
        isFadeInRunning = false;
        isFadeOutRunning = false;
    }

    public void PlayWithFadeIn() {

        bool fromFadeOut = false;
        if (isFadeOutRunning) {
            //Debug.Log("Interrupt Fade out");
            StopCoroutine(fadeOutCoroutine);
            isFadeOutRunning = false;
            fromFadeOut = true;
        }
        fadeInCoroutine = StartCoroutine(FadeInCoroutine(fromFadeOut));
    }

    public void StopWithFadeOut() {

        bool fromFadeIn = false;
        if (isFadeInRunning) {
            //Debug.Log("Interrupt Fade in");
            StopCoroutine(fadeInCoroutine);
            isFadeInRunning = false;
            fromFadeIn = true;
        }
        fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(fromFadeIn));
    }

    private IEnumerator FadeInCoroutine(bool fromFadeOut) {

        //Debug.Log("Start Fade in");
        isFadeInRunning = true;
        float step = 0;
        audioSource.Play();
        while (step < fadeInDuration) {
            audioSource.volume = Mathf.Lerp(fromFadeOut ? audioSource.volume : 0, initVolume, step / fadeInDuration);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        isFadeInRunning = false;
        //Debug.Log("End Fade in");
    }

    private IEnumerator FadeOutCoroutine(bool fromFadeIn) {

        //Debug.Log("Start Fade out");
        isFadeOutRunning = true;
        float step = 0;
        while (step < fadeInDuration) {
            audioSource.volume = Mathf.Lerp(fromFadeIn ? audioSource.volume : initVolume, 0, step / fadeInDuration);
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        audioSource.Stop();
        audioSource.volume = initVolume;
        isFadeOutRunning = false;
        //Debug.Log("End Fade out");
    }

    public void Play() {
        audioSource.Play();
    }

    public void Stop() {
        audioSource.Stop();
    }
}
