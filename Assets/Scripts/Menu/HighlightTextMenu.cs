using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTextMenu : MonoBehaviour {

    public Sprite selectedSprite;
    public Sprite unselectedSprite;

    private bool selected;
    public bool Selected { get { return selected; } }

    private SpriteRenderer spriteRenderer;

    private void Start() {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter() {
        spriteRenderer.sprite = selectedSprite;
        selected = true;
    }

    private void OnMouseExit() {
        spriteRenderer.sprite = unselectedSprite;
        selected = false;
    }
}
