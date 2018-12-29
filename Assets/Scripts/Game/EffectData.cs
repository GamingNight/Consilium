using System;
using UnityEngine;

public class EffectData : MonoBehaviour {

    [Serializable]
    public enum EffectName {
        RAIN, WIND, SUN, TEUB
    }

    public EffectName effectName;
    public float increaseSpeed = 40f;
    public float decreaseSpeed = 5f;
    public ParticleSystem particles;
    public AudioFade audioFade;
}

