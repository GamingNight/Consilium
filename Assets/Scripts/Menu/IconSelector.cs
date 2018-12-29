using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelector : MonoBehaviour {

    public EffectData.EffectName effectName;
    public Sprite selected;
    public Sprite unselected;

    public Image image;

    public void Select() {
        image.sprite = selected;
    }

    public void Unselect() {
        image.sprite = unselected;
    }
}
